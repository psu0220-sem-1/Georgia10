using System;
using System.Collections.Generic;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Server.Controllers;
using Server;
using Server.Models;
using System.Reflection;

namespace Test
{
    [TestFixture]
    public class VolumeControllerTests
    {
        private List<Volume> volumes;

        [SetUp]
        public void Setup()
        {
            volumes = new List<Volume>
            {
                new Volume
                {
                    MaterialId = 1,
                    CurrentLocationId =1,
                    HomeLocationId =2
                },
                  new Volume
                {
                    MaterialId = 2,
                    CurrentLocationId =1,
                    HomeLocationId =2
                },
                    new Volume
                {
                    MaterialId = 3,
                    CurrentLocationId = 1,
                    HomeLocationId = 2
                },
            };
        }

        [Test]
        public void InsertInsertsCorrectly()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            var options = new DbContextOptionsBuilder<GTLContext>().UseInMemoryDatabase(methodName).Options;
            var materialId = 1;
            var homeLocationId = 1;
            var currentLocationId = 1;
            using (var context = new GTLContext(options))
            {
                var volumeController = ControllerFactory.CreateVolumeController(context);
                var volume = new Volume { MaterialId = materialId, HomeLocationId = homeLocationId, CurrentLocationId = currentLocationId };
                volumeController.Insert(volume);
            }

            using (var context = new GTLContext(options))
            {
                var insertedVolume = context.Volumes.FirstOrDefault(v => v.MaterialId == materialId && v.CurrentLocationId == currentLocationId && v.HomeLocationId == homeLocationId);
                Assert.That(insertedVolume, Has
                    .Property(nameof(Volume.MaterialId))
                    .EqualTo(materialId)
                    .And
                    .Property(nameof(Volume.HomeLocationId))
                    .EqualTo(homeLocationId)
                    .And
                    .Property(nameof(Volume.CurrentLocationId))
                    .EqualTo(currentLocationId)
                    );
            }
        }

        [Test]
        public void CreateWithNonExistingDataThrowsArgumentException()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            var options = new DbContextOptionsBuilder<GTLContext>().UseInMemoryDatabase(methodName).Options;

            // There is no material and address with the following IDs in the DB
            var materialId = 1;
            var homeLocationId = 1;
            var currentLocationId = 1;

            using (var context = new GTLContext(options))
            {
                var volumeController = ControllerFactory.CreateVolumeController(context);

                // Since there is no address and material in the DB, creating a volume with those
                // references should throw and exception
                Assert.Throws<ArgumentException>(() => volumeController.Create(materialId, homeLocationId, currentLocationId));
            }
        }

        [Test]
        public void DeleteDeletesCorrectly()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            var options = new DbContextOptionsBuilder<GTLContext>().UseInMemoryDatabase(methodName).Options;

            // add volumes to db
            using (var context = new GTLContext(options))
            {
                context.AddRange(volumes);
                context.SaveChanges();
            }

            // delete volume
            using (var context = new GTLContext(options))
            {

                var volumeController = ControllerFactory.CreateVolumeController(context);

                var volumeToDelete = volumes[1];

                volumeController.Delete(volumeToDelete);

                var fetchedVolume = context.Volumes.Find(volumeToDelete.VolumeId);
                var fetchedVolumes = context.Volumes.ToList();

                Assert.Multiple(() =>
                {
                    // assert that the fetched volume is null - deleted from db
                    Assert.That(fetchedVolume, Is.Null);
                    // assert that number of volumes decreased by 1
                    Assert.AreEqual(fetchedVolumes.Count, volumes.Count - 1);
                });
            }
        }

        [Test]
        public void CreateCreatesCorrectly()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            var options = new DbContextOptionsBuilder<GTLContext>().UseInMemoryDatabase(methodName).Options;

            // setup
            using (var context = new GTLContext(options))
            {
                var zip = new ZipCode { City = "Aalborg", Code = 9000 };
                // zip has to be saved in db in order for the address to be created
                context.Add(zip);
                context.SaveChanges();
                var homeAddress = new Address { Street = "Main road 4", AdditionalInfo = "4.floor, 10", Zip = zip };
                var currentAddress = new Address { Street = "Library road 5", AdditionalInfo = "1.floor", Zip = zip };
                var material = new Material { Isbn = "1234321", Title = "Mat title", Description = "A description", Language = "English", Lendable = true };
                context.Add(homeAddress);
                context.Add(currentAddress);
                context.Add(material);
                context.SaveChanges();

                var volumeController = ControllerFactory.CreateVolumeController(context);

                var matid = material.MaterialId;
                var createdVolume = volumeController.Create(materialId: material.MaterialId, homeLocationId: homeAddress.AddressId, currentLocationId: currentAddress.AddressId);

                Assert.Multiple(() =>
                {
                    Assert.AreEqual(createdVolume.Material.MaterialId, material.MaterialId);
                    Assert.AreEqual(createdVolume.HomeLocation.AddressId, homeAddress.AddressId);
                    Assert.AreEqual(createdVolume.CurrentLocation.AddressId, currentAddress.AddressId);
                });
            }
        }
    }
}

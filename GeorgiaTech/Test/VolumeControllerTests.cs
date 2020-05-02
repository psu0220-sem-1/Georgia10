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
        public void CreateWithNonExistingDataThrowsInvalidOperationException()
        {
            var methodName = MethodBase.GetCurrentMethod().Name;
            var options = new DbContextOptionsBuilder<GTLContext>().UseInMemoryDatabase(methodName).Options;

            var materialId = 1;
            var homeLocationId = 1;
            var currentLocationId = 1;

            using (var context = new GTLContext(options))
            {
                var volumeController = ControllerFactory.CreateVolumeController(context);

                Assert.Throws<InvalidOperationException>(() => volumeController.Create(materialId, homeLocationId, currentLocationId));

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

                // assert that the fetched volume is null - deleted from db
                Assert.That(fetchedVolume, Is.Null);
                // assert that number of volumes decreased by 1
                Assert.AreEqual(fetchedVolumes.Count, volumes.Count - 1);

            }
        }
    }
}

using System;
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
        [SetUp]
        public void Setup()
        {

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
                var volume = new Volume { MaterialId = materialId, HomeLocationId = homeLocationId, CurrentLocationId = currentLocationId};
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
    }
}

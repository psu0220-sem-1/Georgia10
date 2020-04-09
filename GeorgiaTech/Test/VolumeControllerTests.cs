using System;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Server.Controllers;
using Server;
using Server.Models;

namespace Test
{
    [TestFixture]
    public class VolumeControllerTests
    {
        public void Setup()
        {

        }
        [Test]
        public void InsertInsertsCorrectly()
        {
            var options = new DbContextOptionsBuilder<GTLContext>().UseInMemoryDatabase("InsertInsertsCorrectly").Options;
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
                    .Property("MaterialId")
                    .EqualTo(materialId)
                    .And
                    .Property("HomeLocationId")
                    .EqualTo(homeLocationId)
                    .And
                    .Property("CurrentLocationId")
                    .EqualTo(currentLocationId)
                    );
            }
        }
    }
}

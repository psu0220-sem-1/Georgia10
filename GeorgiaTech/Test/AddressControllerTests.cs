using System;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Server;
using Server.Controllers;
using Server.Models;

namespace Test
{
    [TestFixture]
    [Author("Nikola Anastasov Velichkov", "federlizer@gmail.com")]
    public class AddressControllerTests
    {
        private string street;
        private string additionalInfo;
        private ZipCode zip;

        [SetUp]
        public void Setup()
        {
            street = "Spobjergvej 26";
            additionalInfo = "1.4";
            zip = new ZipCode {City = "Aarhus", Code = 8000};
        }

        [Test]
        public void CreateCreatesCorrectAddressInstance()
        {
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateCreatesCorrectAddressInstance")
                .Options;

            using (var context = new GTLContext(options))
            {
                context.Add(zip);
                context.SaveChanges();
            }

            // action
            using (var context = new GTLContext(options))
            {
                var addressController = ControllerFactory.CreateAddressController(context);
                var address = addressController.Create(street, additionalInfo, zip.Code);

                // assertion
                Assert.That(address, Has
                    .Property("Street").EqualTo(street).And
                    .Property("AdditionalInfo").EqualTo(additionalInfo).And
                    .Property("Zip"));
            }
        }

        [Test]
        public void CreateThrowsAnArgumentOutOfRangeExceptionWithEmptyStreetParam()
        {
            // setup
            street = "";
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateThrowsAnArgumentOutOfRangeExceptionWithEmptyStreetParam")
                .Options;

            using (var context = new GTLContext(options))
            {
                context.Add(zip);
                context.SaveChanges();
            }

            // action
            using (var context = new GTLContext(options))
            {
                var addressController = ControllerFactory.CreateAddressController(context);

                // assertion
                Assert.Throws<ArgumentOutOfRangeException>(() =>
                    addressController.Create(street, additionalInfo, zip.Code));
            }
        }

        [Test]
        public void CreateThrowsAnArgumentOutOfRangeExceptionWithEmptyAdditionalInfoParam()
        {
            // setup
            additionalInfo = "";
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateThrowsAnArgumentOutOfRangeExceptionWithEmptyAdditionalInfoParam")
                .Options;

            using (var context = new GTLContext(options))
            {
                context.Add(zip);
                context.SaveChanges();
            }

            // action
            using (var context = new GTLContext(options))
            {
                var addressController = ControllerFactory.CreateAddressController(context);

                // assertion
                Assert.Throws<ArgumentOutOfRangeException>(() =>
                    addressController.Create(street, additionalInfo, zip.Code));
            }
        }

        [Test]
        public void CreateThrowsAnArgumentExceptionWithAZipThatDoesNotExist()
        {
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateThrowsAnArgumentExceptionWithAZipThatDoesNotExist")
                .Options;

            // action
            using var context = new GTLContext(options);
            var addressController = ControllerFactory.CreateAddressController(context);

            // assertion
            Assert.Throws<ArgumentException>(() =>
                addressController.Create(street, additionalInfo, zip.Code));
        }

        [Test]
        public void FindByIdFindsAnAddressInstance()
        {
            // setup
            int addressId;
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("FindByIdFindsAnAddressInstance")
                .Options;

            using (var context = new GTLContext(options))
            {
                var address = new Address
                {
                    AdditionalInfo = additionalInfo,
                    Street = street,
                    Zip = zip,
                };

                context.Add(address);
                context.SaveChanges();

                addressId = address.AddressId;
            }

            // action
            using (var context = new GTLContext(options))
            {
                var addressController = ControllerFactory.CreateAddressController(context);
                var address = addressController.FindByID(addressId);

                // assertion
                Assert.That(address, Has
                    .Property("Street").EqualTo(street).And
                    .Property("AdditionalInfo").EqualTo(additionalInfo).And
                    .Property("Zip"));
            }
        }

        [Test]
        public void FindByIdDoesNotFindAnInstance()
        {
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("FindByIdDoesNotFindAnInstance")
                .Options;

            // action
            using var context = new GTLContext(options);
            var addressController = ControllerFactory.CreateAddressController(context);
            // some random number, doesn't matter what, DB should be empty
            var address = addressController.FindByID(42);

            Assert.That(address, Is.Null);
        }
    }
}
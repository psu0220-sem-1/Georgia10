using System;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Server;
using Server.Controllers;
using Server.Models;

namespace Test
{
    public class AddressControllerTests
    {
        [Test]
        public void CreateCreatesCorrectAddressInstance()
        {
            const string street = "Spobjergvej 26";
            const string additionalInfo = "1.4";
            const int zipCode = 8000;
            const string city = "Aarhus";

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateCreatesCorrectAddressInstance")
                .Options;

            using var context = new GTLContext(options);

            // make sure to have the zip code you need to run the test
            context.Add(new ZipCode {City = city, Code = zipCode});
            context.SaveChanges();

            var addressController = ControllerFactory.CreateAddressController(context);
            var address = addressController.Create(street, additionalInfo, zipCode);

            Assert.That(address, Has
                .Property("Street").EqualTo(street).And
                .Property("AdditionalInfo").EqualTo(additionalInfo).And
                .Property("Zip"));
        }

        [Test]
        public void CreateThrowsAnArgumentOutOfRangeExceptionWithEmptyStreetParam()
        {
            const string street = "";
            const string additionalInfo = "1.4";
            const int zipCode = 8000;
            const string city = "Aarhus";

            Assert.IsTrue(street.Length == 0);

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateThrowsAnArgumentOutOfRangeExceptionWithEmptyStreetParam")
                .Options;

            using var context = new GTLContext(options);

            // make sure to have the zip code you need to run the test
            context.Add(new ZipCode {City = city, Code = zipCode});
            context.SaveChanges();

            var addressController = ControllerFactory.CreateAddressController(context);

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                addressController.Create(street, additionalInfo, zipCode));
        }

        [Test]
        public void CreateThrowsAnArgumentOutOfRangeExceptionWithEmptyAdditionalInfoParam()
        {
            const string street = "Spobjervej 26";
            const string additionalInfo = "";
            const int zipCode = 8000;
            const string city = "Aarhus";

            Assert.IsTrue(additionalInfo.Length == 0);

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateThrowsAnArgumentOutOfRangeExceptionWithEmptyAdditionalInfoParam")
                .Options;

            using var context = new GTLContext(options);

            // make sure to have the zip code you need to run the test
            context.Add(new ZipCode {City = city, Code = zipCode});
            context.SaveChanges();

            var addressController = ControllerFactory.CreateAddressController(context);

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                addressController.Create(street, additionalInfo, zipCode));
        }

        [Test]
        public void CreateThrowsAnArgumentExceptionWithAZipThatDoesNotExist()
        {
            const string street = "Spobjervej 26";
            const string additionalInfo = "1.4";
            // don't create zip in this test
            const int zipCode = 8000;
            const string city = "Aarhus";

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateThrowsAnArgumentExceptionWithAZipThatDoesNotExist")
                .Options;

            using var context = new GTLContext(options);
            var addressController = ControllerFactory.CreateAddressController(context);

            Assert.Throws<ArgumentException>(() =>
                addressController.Create(street, additionalInfo, zipCode));
        }

        [Test]
        public void FindByIdFindsAnAddressInstance()
        {
            const string street = "Spobjervej 26";
            const string additionalInfo = "1.4";
            const int zipCode = 8000;
            const string city = "Aarhus";


            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("FindByIdFindsAnAddressInstance")
                .Options;

            using var context = new GTLContext(options);
            var addressController = ControllerFactory.CreateAddressController(context);

            // insert zip code
            context.ZipCodes.Add(new ZipCode {Code = zipCode, City = city});
            // insert address
            var insertedAddress = context.Addresses.Add(new Address
            {
                AdditionalInfo = additionalInfo,
                Street = street,
                ZipCode = zipCode,
            });
            context.SaveChanges();

            var address = addressController.FindByID(insertedAddress.Entity.AddressId);

            Assert.That(address, Has
                .Property("Street").EqualTo(street).And
                .Property("AdditionalInfo").EqualTo(additionalInfo).And
                .Property("Zip"));
        }

        [Test]
        public void FindByIdDoesNotFindAnInstance()
        {
            const string street = "Spobjervej 26";
            const string additionalInfo = "1.4";
            const int zipCode = 8000;
            const string city = "Aarhus";


            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("FindByIdDoesNotFindAnInstance")
                .Options;

            using var context = new GTLContext(options);
            var addressController = ControllerFactory.CreateAddressController(context);

            // insert zip code
            context.ZipCodes.Add(new ZipCode {Code = zipCode, City = city});
            // insert address
            var insertedAddress = context.Addresses.Add(new Address
            {
                AdditionalInfo = additionalInfo,
                Street = street,
                ZipCode = zipCode,
            });
            context.SaveChanges();

            // find an entity with an incremented ID (make sure there doesn't exist such an entity)
            var address = addressController.FindByID(insertedAddress.Entity.AddressId + 1);

            Assert.IsNull(address);
        }
    }
}
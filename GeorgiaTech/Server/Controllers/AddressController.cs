using System;
using System.Collections.Generic;
using System.Linq;
using Server.Models;

namespace Server.Controllers
{
    public class AddressController: IAddressController
    {
        private readonly GTLContext _db;

        public AddressController(GTLContext context)
        {
            _db = context;
        }

        public Address Insert(Address t)
        {
            throw new System.NotImplementedException();
        }

        public Address FindByID(int ID)
        {
            throw new System.NotImplementedException();
        }

        public Address FindByType(Address t)
        {
            throw new System.NotImplementedException();
        }

        public List<Address> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public Address Update(Address t)
        {
            throw new System.NotImplementedException();
        }

        public int Delete(Address t)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Creates an address instance based on the information passed.
        /// The zipCode will be found in the database and the object will be assigned to
        /// the Zip property of the entity. If such a zip code doesn't exist in the database
        /// the method will throw an ArgumentException
        /// </summary>
        /// <param name="street">The street of the address, including the street number</param>
        /// <param name="additionalInfo">The floor and apartment of the address</param>
        /// <param name="zipCode">The zip code as an integer</param>
        /// <returns>An address entity including its Zip navigational property</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Address Create(string street, string additionalInfo, int zipCode)
        {
            if (street.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(street), "Street can't be empty");

            if (additionalInfo.Length == 0)
                throw new ArgumentOutOfRangeException(nameof(additionalInfo), "Additional info can't be empty");

            var zip = _db.ZipCodes.Find(zipCode);

            if (zip == null)
                throw new ArgumentException("Zip code provided must already exist in the database", nameof(zipCode));

            var address = new Address
            {
                Street = street,
                AdditionalInfo = additionalInfo,
                Zip = zip,
            };

            return address;
        }
    }
}
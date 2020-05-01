using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Inserts an address entity in the database
        /// </summary>
        /// <param name="address">The address entity to be inserted</param>
        /// <returns>The same address entity with its new ID assigned</returns>
        /// <exception cref="ArgumentNullException">If address parameter is null</exception>
        /// <remarks>Not tested</remarks>
        public int Insert(Address address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address), "Address object can't be null");
            }

            _db.Add(address);
            var changedRows = _db.SaveChanges();

            return changedRows;
        }

        /// <summary>
        /// Finds an address by its ID. If no address is found, null is returned
        /// </summary>
        /// <param name="ID">The ID of the address</param>
        /// <returns>The found address or null</returns>
        /// <remarks>Not tested</remarks>
        public Address FindByID(int ID)
        {
            var address = _db.Addresses.Find(ID);
            return address;
        }

        /// <summary>
        /// Returns a list of all addresses saved on the database
        /// </summary>
        /// <returns>A list of all addresses saved on the database</returns>
        /// <remarks>Not tested</remarks>
        public List<Address> FindAll()
        {
            return _db.Addresses.ToList();
        }

        /// <summary>
        /// Saves any changes to the context's entities. WARNING: this will save ALL changes made
        /// to the current context, not only to the entity that is being passed.
        /// </summary>
        /// <param name="address">An address instance that will be updated</param>
        /// <returns>The address instance passed if changes are saved, null otherwise</returns>
        /// <remarks>Not tested</remarks>
        public int Update(Address address)
        {
            if (!_db.ChangeTracker.HasChanges())
                return 0;

            return _db.SaveChanges();
        }

        /// <summary>
        /// Deletes an address entity from the context and removes it from the database as well.
        /// WARNING: Calling this method WILL save any other changes made to the same context
        /// passed to the controller!!!
        /// </summary>
        /// <param name="address">The address entity to be deleted</param>
        /// <returns>The number of changed rows</returns>
        /// <remarks>Not tested</remarks>
        public int Delete(Address address)
        {
            _db.Remove(address);
            return _db.SaveChanges();
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
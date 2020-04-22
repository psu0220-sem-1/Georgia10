using System.Collections.Generic;
using Server.Models;

namespace Server.Controllers
{
    public class AddressController: IAddressController
    {
        public Address Insert(Address t)
        {
            throw new System.NotImplementedException();
        }

        public Address FindByID(int ID)
        {
            throw new System.NotImplementedException();
        }

        public List<Address> FindAll()
        {
            throw new System.NotImplementedException();
        }

        public int Update(Address t)
        {
            throw new System.NotImplementedException();
        }

        public int Delete(Address t)
        {
            throw new System.NotImplementedException();
        }

        public Address Create(string street, string additionalInfo, int zip)
        {
            throw new System.NotImplementedException();
        }
    }
}
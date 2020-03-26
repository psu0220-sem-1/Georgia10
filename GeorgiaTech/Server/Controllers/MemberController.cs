using System;
using System.Collections.Generic;
using System.Text;
using Server.Models;
namespace Server.Controllers
{
    class MemberController : IMemberController
    {
        static GTLContext _context;
        IAddressController addressController = ControllerFactory.CreateAddressController(_context);

        public MemberController(GTLContext context)
        {
            _context = context;
        }
        
        public void Create(string SSN, string fName, string lName, string homeAddress, string campusAddress, int zip, string homeAddressAdditionalInfo)
        {
            Member member = new Member();
            member.Ssn = SSN;
            member.FName = fName;
            member.LName = lName;
            //decide if this should be extrapolated into a specific method.
            member.HomeAddress = addressController.Create(homeAddress, homeAddressAdditionalInfo, zip);
            //Campus address is set based on a pre-existing list. 
            //TODO
            member.Card = null;            
            throw new NotImplementedException();
        }
        


        public void Delete(Member t)
        {
            throw new NotImplementedException();
            /*Search for new Member. Find new Member. Delete new Member.
             */
        }

        public List<Member> FindAll()
        {
            throw new NotImplementedException();
        }

        public Member FindByID(int ID)
        {
            throw new NotImplementedException();
        }

        public Member FindByType(Member t)
        {
            throw new NotImplementedException();
        }

        public Member Insert(Member t)
        {
            throw new NotImplementedException();
        }

        public void Read(Member t)
        {
            throw new NotImplementedException();
            //return chosen member based on parameter.
        }

        public void Update(Member t)
        {
            throw new NotImplementedException();
            //find chosen member. Update chosen member parameter. Update chosen member in database. 
        }

        int IController<Member>.Delete(Member t)
        {
            throw new NotImplementedException();
        }

        Member IController<Member>.Update(Member t)
        {
            throw new NotImplementedException();
        }
    }
}

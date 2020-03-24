using System;
using System.Collections.Generic;
using System.Text;
using Server.Models;

namespace Server.Controllers
{
    class MemberController : IGenerics<Member>
    {
        public void Create()
        {
            //currently not being implemented, due to CreateMember existing. Will get removed.
            throw new NotImplementedException();
            //Take data for new Member. Add data to new member. Insert member into Database. 
        }
        public void CreateMember(string SSN, string fName, string lName, string homeAddress, string campusAddress)
        {
            Member member = new Member();
            member.Ssn = SSN;
            member.FName = fName;
            member.LName = lName;
            //member.HomeAddress = homeAddress;
            //member.CampusAddress = campusAddress;
            //address requires Address controller to be done.
            //This should be an Enum as well of some kind. 
            //member.MemberTypeAssignment

            throw new NotImplementedException();

        }
        

        public void Delete(Member t)
        {
            throw new NotImplementedException();
            /*Search for new Member. Find new Member. Delete new Member.
             */
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
    }
}

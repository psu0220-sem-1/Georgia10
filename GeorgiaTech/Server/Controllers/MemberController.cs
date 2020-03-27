using System;
using System.Collections.Generic;
using System.Text;
using Server.Models;
namespace Server.Controllers
{
    class MemberController : IMemberController
    {
        static GTLContext context;
        IAddressController addressController = ControllerFactory.CreateAddressController(context);
        
        public MemberController(GTLContext context)
        {
            MemberController.context = context;
        }

        public void Create(string SSN, string fName, string lName, string homeAddress, string campusAddress, int zip, string homeAddressAdditionalInfo)
        {
            Member member = new Member
            {
                Ssn = SSN,
                FName = fName,
                LName = lName,
                //decide if this should be extrapolated into a specific method.
                HomeAddress = addressController.Create(homeAddress, homeAddressAdditionalInfo, zip),
                //Campus address is set based on a pre-existing list. 
                //this is set from the Card controller. Have set it to Null for now.
                //TODO when interface is set up.
                Card = null,
                Loan = null
            };

            throw new NotImplementedException();
        }
       public Member setMemberType(Member member)
        {
            
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
            
            context.Add(t);
            context.SaveChanges();
            //TODO
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

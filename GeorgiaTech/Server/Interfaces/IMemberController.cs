using Server.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Controllers
{
    public interface IMemberController : IController<Member>
    {
        public Member Create(string SSN, string fName, string lName, string homeAddress, string campusAddress, int zip, string homeAddressAdditionalInfo, List<MemberType> memberTypes);
        


    }
}

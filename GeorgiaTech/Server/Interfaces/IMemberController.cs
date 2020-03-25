using Server.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Controllers
{
    interface IMemberController : IController<Member>
    {
        public void Create(string SSN, string fName, string lName, string homeAddress, string campusAddress);

    }
}

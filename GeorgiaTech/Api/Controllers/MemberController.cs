using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Controllers;
using Server;
using System.Collections.Generic;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberController controller;

        public MemberController(GTLContext context)
        {
            controller = new Server.Controllers.MemberController(context);
        }
        [HttpPost]
        public int PostMember([FromBody] JsonElement content)
        {

            var FName = content.GetProperty("firstName").GetString();
            var LName = content.GetProperty("lastName").GetString();
            var Ssn = content.GetProperty("Ssn").GetString();
            string homeAddress = content.GetProperty("omeaddress").GetString();
            string campusAddress = content.GetProperty("campusAddress").GetString();
            int zip = content.GetProperty("zip").GetInt16();
            string homeaAddressAdditionalInfo = content.GetProperty("homeAddressAdditionalInfo").GetString();
            List<MemberType> membertypes = new List<MemberType>();
            var member = controller.Create(Ssn, FName, LName, homeAddress, campusAddress, (int)zip, homeaAddressAdditionalInfo, membertypes);
            return controller.Insert(member);
        }
        [HttpGet("{name}")]
        public Member Get()
        {
            return null;
        }
    }
}
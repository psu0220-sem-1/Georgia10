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
        private readonly IMemberController mController;

        public MemberController(GTLContext context)
        {
            mController = ControllerFactory.CreateMemberController(context);
        }
        //Insert method.
        public Member PostMember(string SSN, string fName, string lName, string homeAddress, string campusAddress, int zip, string homeAddressAdditionalInfo, List<MemberType> memberTypes)
        {

            var member = mController.Create(SSN, fName, lName, homeAddress, campusAddress, zip, homeAddressAdditionalInfo, memberTypes);
            return member;
        }
        public Member PostMember([FromBody]Member member)
        {

            
            return member;
        }
        /// <summary>
        /// returns member if found on it's ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        //Find method
        public IActionResult GetMemberByID(int id)
        {
            var member = mController.FindByID(id);
            if (member == null)
            {
                return new NotFoundResult();
            }
            return new JsonResult(member);
        }
        /// <summary>
        /// returns member by name if found.
        /// </summary>
        /// <param name="name">name of searched member</param>
        /// <returns></returns>
        public IActionResult GetMemberByName(string name)
        {
            var member = mController.FindByName(name);
            if (member == null)
            {
                return new NotFoundResult();
            }
            return new JsonResult(member);
        }
        //FindAll - needs to be changed
        public IEnumerable<Member> FindAllByType(List<MemberType> types)
        {
            List<Member> members = mController.FindAllByType(types);
            return members;
        }
        //Update method
        public int UpdateMember(Member member)
        {
            return mController.Update(member);   
        }

        public int DeleteMember(Member member)
        {
            return mController.Delete(member);
        }
        
    }
}
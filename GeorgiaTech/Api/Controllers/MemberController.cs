using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Api.Models;
using Server.Controllers;
using Server;
using System.Collections.Generic;
using System;

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

        //Converter from Servermodel to APImodel. Will be used when returning a servermodel.

        private Api.Models.Member Build_API_Member(Server.Models.Member sMember)
        {
            Member member = new Member()
            {
                SSN = sMember.SSN,
                FName = sMember.FName,
                LName = sMember.LName,
                HomeAddress = sMember.HomeAddress.Street,
                HomeAddressZip = sMember.HomeAddress.ZipCode
            };
            //Logic for making MemberTypes from memberShips.
            foreach (var type in sMember.Memberships)
            {
                MemberType memberType = new MemberType()
                {
                    TypeId = type.MemberType.TypeId,
                    TypeName = type.MemberType.TypeName
                };
                member.MemberTypes.Add(memberType);
            }
            return member;
        }
        private Server.Models.Member BuildServerMember(Member aMember)
        {
            List<Server.Models.MemberType> memberTypes = new List<Server.Models.MemberType>();
            foreach (var type in aMember.MemberTypes)
            {
                Server.Models.MemberType mType = new Server.Models.MemberType()
                {
                    TypeId = type.TypeId,
                    TypeName = type.TypeName
                };
                memberTypes.Add(mType);
            }
            
            Server.Models.Member sMember = mController.Create(aMember.SSN, aMember.FName, aMember.LName, aMember.HomeAddress, aMember.CampusAddress, aMember.HomeAddressZip, aMember.HomeAddressAdditionalInfo, memberTypes);
            return sMember;
        }


        //Frontending API methods. Should be called with /api/{methodname}?[var]=[value]
        [HttpPost]
        public IActionResult PostMember([FromBody]Member member) 
        {
            //Returns an errorcode of 500 if no changes was made, to indicate a problem when 
            int changes = mController.Insert(BuildServerMember(member)); ;
            if (changes == 0)
            {
                return StatusCode(500);
            }
            return new JsonResult(changes);
        }

        /// <summary>
        /// returns member if found on it's ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        //Find method
        [HttpGet("id:int")]
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
        ///
        [HttpGet("name:string")]
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
       
        //Update method
        [HttpPut]
        public int UpdateMember(Member member)
        {
            return mController.Update(BuildServerMember(member));
        }
        [HttpDelete]
        public int DeleteMember(Member member)
        {
            return mController.Delete(BuildServerMember(member));
        }
        [HttpGet]
        public List<Member> GetMembers()
        {
            List<Member> members = new List<Member>();
            foreach (var member in mController.FindAll())
            {
                members.Add(Build_API_Member(member));
            }
            return members;
        }

    }
}
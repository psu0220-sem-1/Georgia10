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

        private Api.Models.Member BuildMember(Server.Models.Member sMember)
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
        //Insert method.
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
        public IEnumerable<Member> FindAllByType([FromBody]List<MemberType> types)
        {
            //first make list of Server Membertypes from the list of Membertypes
            List<Server.Models.MemberType> mTypes = new List<Server.Models.MemberType>();
            foreach (var type in types)
            {
                Server.Models.MemberType mType = new Server.Models.MemberType()
                {
                    TypeId = type.TypeId,
                    TypeName = type.TypeName
                };
                mTypes.Add(mType);
            }
            //then get back a list of members corresponding to the types.
            var sMembers = mController.FindAllByType(mTypes);

            //convert the Server Members to the API members.
            List<Member> aMembers = new List<Member>();
            foreach (var member in sMembers)
            {
                aMembers.Add(BuildMember(member));
            }
            
            return aMembers;
        }
        //Update method
        public int UpdateMember(Member member)
        {
            return mController.Update(BuildServerMember(member));
        }

        public int DeleteMember(Member member)
        {
            return mController.Delete(BuildServerMember(member));
        }

    }
}
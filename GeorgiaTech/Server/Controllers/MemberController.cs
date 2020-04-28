using System;
using System.Collections.Generic;
using System.Text;
using Server.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Server.Controllers
{
    public class MemberController : IMemberController
    {
        static GTLContext db;
        IAddressController addressController;
        public MemberController(GTLContext context)
        {
            db = context;
            addressController = ControllerFactory.CreateAddressController(db);

        }

        public Member Create(string SSN, string fName, string lName, string homeAddress, string campusAddress, int zip, string homeAddressAdditionalInfo, List<MemberType> memberTypes)
        {
            Member member = new Member
            {
                Ssn = SSN,
                FName = fName,
                LName = lName,
                HomeAddress = addressController.Create(homeAddress, homeAddressAdditionalInfo, zip),
                //Campus address is set based on a pre-existing list. 
                //this is set from the Card controller. Have set it to Null for now.
                //TODO when interface is set up.
                Cards = null,
                Loans = null,
            };

            //not sure if this will be the best implementation
            member.Memberships = UpdateMembershipsOnMember(member, memberTypes);
            return member;
        }
        public List<Membership> UpdateMembershipsOnMember(Member member, List<MemberType> memberTypes)
        {
            if (memberTypes.Count == 0)
            {
                throw new ArgumentNullException(nameof(member), "Member must have a membertype chosen");
            }
            if (member.Memberships.GroupBy(x => x.MemberType).Any(g => g.Count() > 1))
            {
                throw new Exception("Member already has the designated membertype");
            }
            List<Membership> memberships = new List<Membership>();
            foreach (var type in memberTypes)
            {
                Membership membership = new Membership
                {
                    MemberId = member.MemberId,
                    TypeId = type.TypeId,
                    Member = member,
                    MemberType = type
                };
                memberships.Add(membership);
            }
            
            return memberships;
        }
        public int Delete(Member t)
        {
            //doesn't work due a lack of Cascading deletes.
            db.Remove(t);
            return db.SaveChanges();
        }
        public List<MemberType> GetMemberTypes()
        {
            //is used in the frontend to get a list of each membertype in the database, with its TypeID and TypeName.
            return db.MemberTypes.ToList<MemberType>();
        }
        public List<Member> FindAll()
        {
            //Dont see where thhis would ever be used, but here it is.
            return db.Members.ToList<Member>();
        }

        public Member FindByID(int ID)
        {
            return db.Members.FirstOrDefault(m => m.MemberId == ID);
        }

        [Obsolete("use FindAllByType that returns a list")]
        public Member FindByType(Member t)
        {
            throw new NotImplementedException();
        }
        public List<Member> FindAllByType(Member t)
        {

            List<Member> members = new List<Member>();
            foreach (var type in t.Memberships)
            {
                int typeID = type.TypeId;
                List<Member> membersList = db.Members.Where(x => x.Memberships.Any(y => y.TypeId == typeID)).ToList();
                members.AddRange(membersList);
            }
            return members;
        }

        public int Insert(Member t)
        {
            if (t == null)
            {
                throw new ArgumentNullException(nameof(t), "member object can't be null");
            }
            db.Add(t);
            return db.SaveChanges();
        }


        public int Update(Member member)
        {

            int changes = 0;
            db.Update(member);
            //SaveChanges is transactional by nature.
            changes = db.SaveChanges();
            return changes;
        }
        /// <summary>
        /// Returns the member or null if no member is found.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Member FindByName(string name)
        {
            if (name == "")
            {
                throw new Exception("No name passed as argument, or name was empty");
            }
            return db.Members.FirstOrDefault(m => m.FName == name);

        }
    }
}

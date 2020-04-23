using System;
using System.Collections.Generic;
using System.Text;
using Server.Models;
using System.Linq;
namespace Server.Controllers
{
    public class MemberController : IMemberController
    {
        static GTLContext db;
        IAddressController addressController = ControllerFactory.CreateAddressController(db);

        public MemberController(GTLContext context)
        {
            db = context;
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
            member.Memberships = AddMembershipToMember(memberTypes, member);
            return member;
        }
        public List<Membership> AddMembershipToMember(List<MemberType> memberTypes, Member member)
        {
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
            db.Members.Remove(t);

            
            return db.SaveChanges();
        }
        public List<MemberType> GetMemberTypes()
        {
            //is used in the frontend to get a list of each membertype in the database, with its TypeID and TypeName.
            return db.MemberTypes.ToList<MemberType>();
        }
        public List<Member> FindAll()
        {
            return db.Members.ToList<Member>();
        }

        public Member FindByID(int ID)
        {
            
            return db.Members.FirstOrDefault(m => m.MemberId == ID);
        }
        [Obsolete ("use FindAllByType that returns a list")]
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
            db.Add(t);
            return db.SaveChanges();
        }

        
        public int Update(Member member)
        {
            
            int changes = 0;
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    db.Update(member);
                    changes = db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    //TODO: handle failure somehow.
                    throw e;
                }
            }
            
            return changes;
        }
    }
}

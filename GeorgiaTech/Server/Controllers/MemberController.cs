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
                //decide if this should be extrapolated into a specific method.
                HomeAddress = addressController.Create(homeAddress, homeAddressAdditionalInfo, zip),
                //Campus address is set based on a pre-existing list. 
                //this is set from the Card controller. Have set it to Null for now.
                //TODO when interface is set up.
                Cards = null,
                Loans = null,
            };
            //not sure if this will be the best implementation
            member.Memberships = AddMembership(memberTypes, member);
            return member;
        }
        private List<Membership> AddMembership(List<MemberType> memberTypes, Member member)
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
            //should just look in the db for the given member, removing it.
            db.Members.Remove(t);

            //returns number of changed rows. If it's zero, that means there wasn't a member with that information in the DB.
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
            //returns the given member if it exists, otherwise returns null.
            return db.Members.FirstOrDefault(m => m.MemberId == ID);
        }
        [Obsolete ("use FindAllByType that returns a list")]
        public Member FindByType(Member t)
        {
            throw new NotImplementedException();
        }
        public List<Member> FindAllByType(Member t)
        {
            //lets sake for posterity that the TypeID on the Memberships field is the type. This will return the Member based on the type. 
            //first i think i need to find the type enum from the DB.
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

        [Obsolete("Use Update that returns int instead")]
        public Member Update(Member member)
        {
            throw new NotImplementedException();
        }
        public int Update(Member member)
        {
            //Fairly certain this will first find the entity, then update the entity with the new data.
            //Is likely quite slow, as context doesn't know what updated, so will update everything.
            //Uses transactions in case anything goes wrong.
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
            //fairly certain this is useless. Would prefer returning int with amount of changed rows instead. 
            return changes;
        }
    }
}

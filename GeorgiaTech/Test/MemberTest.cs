using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Server;
using Server.Controllers;
using Server.Models;
using System.Diagnostics;
using System.Reflection;

namespace Test
{

    [TestFixture]
    public class MemberTest
    {
        string fName = "Anders";
        string lName = "And";
        string ssn = "250-57-9705";
        string homeAddres = "Annemosevej";
        string campusAddress = "Sofiendalsvej 60";
        string homeAddressAdditionalInfo = "None available";
        int zip = 30002;
        List<MemberType> mTypes = new List<MemberType>();

        [OneTimeSetUp]
        public void SetupOnce()
        {
            //MemberTypes:
            MemberType student = new MemberType()
            {
                TypeName = "Student"
            };
            MemberType staff = new MemberType()
            {
                TypeName = "Staff"
            };
            MemberType faculty = new MemberType()
            {
                TypeName = "Faculty"
            };

            mTypes.Add(student);
            mTypes.Add(staff);
            mTypes.Add(faculty);
        }

        [SetUp]
        public void Setup()
        {
        }
        public DbContextOptions<GTLContext> SetupInMemoryDatabase()
        {
            // Get call stack - this has info on the calling method's name.
            StackTrace stackTrace = new StackTrace();
            // Get calling method name through the stacktrace method. 
            string callingMethod = stackTrace.GetFrame(0).GetMethod().Name;
            //setup the options for the DbContext.
            DbContextOptions<GTLContext> options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase(callingMethod).EnableSensitiveDataLogging(true)
                .Options;

            return options;
        }
        
        private void InsertDummyData(GTLContext context)
        {

            //this should be passed by reference, and as such work with differnet objects without carrying the data over.
            ZipCode zipCode = new ZipCode
            {
                City = "Georgia",
                Code = zip
            };
            context.ZipCodes.Add(zipCode);
            context.MemberTypes.AddRange(mTypes);
            context.SaveChanges();
        }
        [Test]
        public void InsertMemberIntoDatabase()
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            DbContextOptions<GTLContext> options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase(method.Name).EnableSensitiveDataLogging(true)
                .Options;
            using (var context = new GTLContext(options))
            {
                IMemberController mController = ControllerFactory.CreateMemberController(context);
                //Currently only inserting for the first method calling it.
                InsertDummyData(context);

                var member = mController.Create(ssn, fName, lName, homeAddres, campusAddress, zip, homeAddressAdditionalInfo, mTypes);
                mController.Insert(member);

                Assert.That(member, Has.Property("FName").EqualTo(fName).And.Property("LName").EqualTo(lName)
                );
            }
        }
        [Test]
        public void FindByName()
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            DbContextOptions<GTLContext> options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase(method.Name).EnableSensitiveDataLogging(true)
                .Options;
            using (var context = new GTLContext(options))
            {
                //as long as dbContext is needed for initializing controller. this can't easily be extracted - so it must be in each method. 
                IMemberController mController = ControllerFactory.CreateMemberController(context);
                InsertDummyData(context);

                var member = mController.Create(ssn, fName, lName, homeAddres, campusAddress, zip, homeAddressAdditionalInfo, mTypes);
                string firstName = "Bodacious";
                string lastName = "Booty";
                member.FName = firstName;
                member.LName = lastName;
                //currently not inserting data for some reason.
                //action
                mController.Insert(member);
                //Assertation
                Assert.That(member, Has.
                    Property("FName").EqualTo(firstName)
                    .And.Property("LName").EqualTo(lastName));
               var foundMember = mController.FindByName(member.FName);
                Assert.That(member, Is.EqualTo(foundMember));
            };
        }
        [Test]
        public void UpdateMemberWithNewAddress()
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            DbContextOptions<GTLContext> options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase(method.Name).EnableSensitiveDataLogging(true)
                .Options;
            using (var context = new GTLContext(options))
            {
                //setup
                IMemberController mController = ControllerFactory.CreateMemberController(context);
                InsertDummyData(context);
                
                var member = mController.Create(ssn, fName, lName, homeAddres, campusAddress, zip, homeAddressAdditionalInfo, mTypes);
                IAddressController aController = ControllerFactory.CreateAddressController(context);

                var newAddress = aController.Create("Blaviken street", "Be careful of the Dog", zip);
                //Action
                mController.Insert(member);
                //--update the member with new info, find it again make sure they are actually updated. 
                var updatedMember = mController.FindByName(member.FName);
                updatedMember.HomeAddress = newAddress;
                mController.Update(updatedMember);
                member = mController.FindByName(member.FName);
                Assert.That(member, Is.EqualTo(updatedMember));
            }

        }
        [Test]
        public void UpdateMembertypeOnMember()
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            DbContextOptions<GTLContext> options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase(method.Name).EnableSensitiveDataLogging(true)
                .Options;
            MemberType student = new MemberType()
            {
                TypeName = "Student"
            };
            MemberType staff = new MemberType()
            {
                TypeName = "Staff"
            };
            List<MemberType> memberTypes = new List<MemberType>();
            memberTypes.Add(student);

            using (var context = new GTLContext(options))
            {
                //Basic environment.
                IMemberController mController = ControllerFactory.CreateMemberController(context);
                InsertDummyData(context);
                var member = mController.Create(ssn, fName, lName, homeAddres, campusAddress, zip, homeAddressAdditionalInfo, memberTypes);

                mController.Insert(member);
                memberTypes.Add(staff);
                //Actions
                mController.UpdateMembershipsOnMember(member, memberTypes);
                mController.Update(member);
                var updatedMember = mController.FindByName(member.FName);
                //Assertations!
                Assert.That(member, Is.EqualTo(updatedMember));
            }
        }
        [Test]
        public void DeleteMemberFromDatabase()
        {
            MethodBase method = MethodBase.GetCurrentMethod();
            DbContextOptions<GTLContext> options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase(method.Name).EnableSensitiveDataLogging(true)
                .Options;
            using (var context = new GTLContext(options))
            {
                IMemberController mController = ControllerFactory.CreateMemberController(context);
                InsertDummyData(context);
                var member = mController.Create(ssn, fName, lName, homeAddres, campusAddress, zip, homeAddressAdditionalInfo, mTypes);
                mController.Insert(member);

                //remove the member again. This crurrently doesn't work due to a lack of cascacing action.
                mController.Delete(member);
                var foundMember = mController.FindByName(member.FName);
                //A null should have been returned as the member doesn't exist.
                Assert.IsNull(foundMember);
            }

        }
        [Test]
        public void AddMemberShipToMember()
        {
            throw new NotImplementedException();

        }
        [Test]
        public void GetMembershipsOfMember()
        {
           throw new NotImplementedException();

        }
        [Test]
        public void FindAllMembersByType()
        {
            throw new NotImplementedException();

        }

    }
}

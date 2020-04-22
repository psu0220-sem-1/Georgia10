using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Server;
using Server.Controllers;
using Server.Models;
using System.Diagnostics;


namespace Test
{
    //TestFixture marks the file as containing tests.
    [TestFixture]
    public class MemberTest
    {
        string fName = "";
        string lName = "";
        string ssn = "250-57-9705";
        string homeAddres = "Annemosevej";
        string campusAddress = "Sofiendalsvej 60";
        string homeAddressAdditionalInfo = "";
        int zip = 8858;
        List<MemberType> mTypes = new List<MemberType>();
        Member member;
        //To avoid setting these things up each time.
        
        [SetUp]
        public void Setup()
        {
            //gonna need to assign dbContext somewhere / somehow.
            //MemberTypes:
            MemberType student = new MemberType()
            {
                TypeId = 0,
                TypeName = "Student"
            };
            MemberType staff = new MemberType()
            {
                TypeId = 1,
                TypeName = "Staff"
            };
            MemberType faculty = new MemberType()
            {
                TypeId = 2,
                TypeName = "Faculty"
            };
            List<MemberType> memberTypes = new List<MemberType>();
            mTypes.Add(student);
            mTypes.Add(staff);
            mTypes.Add(faculty);
            

        }
        public DbContextOptions<GTLContext> SetupInMemoryDatabase()
        {
            // Get call stack - this has info on the calling method's name.
            StackTrace stackTrace = new StackTrace();

            // Get calling method name through the stacktrace method. 
            string callingMethod = stackTrace.GetFrame(1).GetMethod().Name;
            
            //setup the options for the DbContext.
            //Hopefully it wont be closed when this method ends, but rather when the using statement that called it ends.
            DbContextOptions<GTLContext> options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase(callingMethod)
                .Options;
            //type to be returned.
            return options;
        }
        [Test]
        public void InsertMemberIntoDatabase()
        {
            //Call this each test. Dont worry about TearDown, it happens when the method has finished executing.
            using var context = new GTLContext(SetupInMemoryDatabase());

            //as long as dbContext is needed for initializing controller. this can't easily be extracted - so it must be in each method. 
            IMemberController mController = ControllerFactory.CreateMemberController(context);
            var member = mController.Create(ssn, fName, lName, homeAddres, campusAddress, zip, homeAddressAdditionalInfo, mTypes);

            //action
            mController.Insert(member);
            //Assertation:
            Assert.That(member, Has.Property("FirstName").EqualTo(member.FName).And.Property("LastName").EqualTo(member.LName)
                );

        }
    }
}

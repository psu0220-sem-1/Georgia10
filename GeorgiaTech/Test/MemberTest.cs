using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Server;
using Server.Controllers;
using Server.Models;

namespace Test
{
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
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("MemberTestDatabase")
                .Options;
            using (var context = new GTLContext(options))
            {
                IMemberController membercontroller = ControllerFactory.CreateMemberController(context);
                var member = membercontroller.Create(ssn, fName, lName, homeAddres, campusAddress, zip, homeAddressAdditionalInfo, mTypes);
            }
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}

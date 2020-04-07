using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Server;
using Server.Controllers;

namespace Test
{
    [TestFixture]
    public class MemberTest
    {
        
        [SetUp]
        public void Setup()
        {
            //gonna need to assign dbContext somewhere / somehow.
            GTLContext dbContext;
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("InsertThrowsArgumentNullExceptionWithNullAuthorParam")
                .Options;
            
            IMemberController membercontroller = ControllerFactory.CreateMemberController(dbContext);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Server;
using Server.Controllers;
using Server.Models;

namespace Test
{
    [TestFixture]
    public class LoanControllerTests
    {
        private Member _member;
        private Volume _volume;

        [SetUp]
        public void SetUp()
        {
            var material = new Material
            {
                Isbn = "978-0-201-61622-4",
                Title = "The Pragmatic Programmer: From Journeyman to Master",
                Language = "English",
                Lendable = true,
                Description = "Some description in here",
                Type = new MaterialType {Type = "Book"}
            };

            var zip = new ZipCode
            {
                Code = 9000,
                City = "Aalborg",
            };

            var campusAddress = new Address
            {
                Street = "Sofiendalsvej 60",
                AdditionalInfo = "3.20.1",
                Zip = zip,
            };

            var memberAddress = new Address
            {
                Street = "Kjellerupsgade 14",
                AdditionalInfo = "4.11",
                Zip = zip,
            };

            _volume = new Volume
            {
                Material = material,
                CurrentLocation = campusAddress,
                HomeLocation = campusAddress,
            };

            _member = new Member
            {
                Ssn = "1234567891",
                FName = "Nikola",
                LName = "Velichkov",
                HomeAddress = memberAddress,
                CampusAddress = campusAddress,
            };

            var memberships = new List<Membership>
            {
                new Membership
                {
                    Member = _member,
                    MemberType = new MemberType {TypeName = "Student"},
                },
            };

            _member.Memberships = memberships;
        }

        [Test]
        public void CreateCreatesCorrectly()
        {
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name)
                .Options;

            int memberId;
            int volumeId;
            using (var context = new GTLContext(options))
            {
                context.Add(_member);
                context.Add(_volume);

                context.SaveChanges();

                memberId = _member.MemberId;
                volumeId = _volume.VolumeId;
            }

            // action
            using (var context = new GTLContext(options))
            {
                var member = context.Members.Find(memberId);
                var volume = context.Volumes.Find(volumeId);

                var controller = ControllerFactory.CreateLoanController(context);
                var loan = controller.Create(member, volume);

                // assertion
                var today = DateTime.Today;
                var dueDate = DateTime.Today.AddDays(LoanController.LendingPeriod);

                Assert.That(loan, Has
                    .Property(nameof(Loan.Member)).Not.Null.And
                    .Property(nameof(Loan.Volume)).Not.Null.And
                    .Property(nameof(Loan.LoanDate)).EqualTo(today).And
                    .Property(nameof(Loan.DueDate)).EqualTo(dueDate).And
                    .Property(nameof(Loan.ReturnedDate)).Null.And
                    .Property(nameof(Loan.Extensions)).EqualTo(0)
                );
            }
        }
    }
}

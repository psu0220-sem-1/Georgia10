using System;
using System.Collections.Generic;
using System.Linq;
using Server.Models;
using Microsoft.EntityFrameworkCore;


namespace Server.Controllers
{
    public class LoanController : ILoanController
    {
        // in days
        public const int LendingPeriod = 21;
        public const int GracePeriod = 7;
        public const int ProfessorLendingPeriod = 90;
        public const int ProfessorGracePeriod = 14;

        private readonly GTLContext _context;

        public LoanController(GTLContext context)
        {
            _context = context;
        }

        public Loan Create(Member member, Volume volume)
        {
            var today = DateTime.Today;
            var dueDate = today;

            foreach (var membership in member.Memberships)
            {
                if (membership.MemberType.TypeName == "Professor")
                    dueDate = today.AddDays(ProfessorLendingPeriod);
            }

            if (dueDate == today)
                dueDate = today.AddDays(LendingPeriod);


            var loan = new Loan
            {
                Member = member,
                Volume = volume,
                LoanDate = today,
                DueDate = dueDate,
                ReturnedDate = null,
                Extensions = 0,
            };

            return loan;
        }

        public int Delete(Loan t)
        {
            throw new NotImplementedException();
        }

        public List<Loan> FindAll()
        {
            return _context.Loans
                .Include(l => l.Member)
                .Include(l => l.Volume)
                    .ThenInclude(v => v.Material)
                .ToList();
        }

        public Loan FindByID(int ID)
        {
            return _context.Loans.Find(ID);
        }

        public int Insert(Loan t)
        {
            throw new NotImplementedException();
        }

        public int Update(Loan t)
        {
            throw new NotImplementedException();
        }

        public List<Loan> FindForMember(int memberId)
        {
            return _context.Loans.Where(l => l.MemberId.Equals(memberId))
                .Include(l => l.Member)
                .Include(l => l.Volume)
                .ToList();
        }

    }
}

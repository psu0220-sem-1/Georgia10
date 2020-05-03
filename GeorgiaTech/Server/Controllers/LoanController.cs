using System;
using System.Collections.Generic;
using System.Linq;
using Server.Models;
using Microsoft.EntityFrameworkCore;


namespace Server.Controllers
{
    public class LoanController : ILoanController
    {
        private readonly GTLContext _context;

        public LoanController(GTLContext context)
        {
            _context = context;
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

using System;
using System.Collections.Generic;
using System.Linq;
using Server.Models;

namespace Server.Controllers
{
    public class LoanController: ILoanController
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
            throw new NotImplementedException();
        }

        public Loan FindByID(int ID)
        {
            throw new NotImplementedException();
        }

        public int Insert(Loan t)
        {
            throw new NotImplementedException();
        }

        public int Update(Loan t)
        {
            throw new NotImplementedException();
        }
    }
}

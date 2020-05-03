using System.Collections.Generic;
using Server.Models;

namespace Server.Controllers
{
    public interface ILoanController: IController<Loan>
    {
        public List<Loan> FindForMember(int memberId);
    }
}

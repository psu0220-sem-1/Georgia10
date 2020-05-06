using System.Collections.Generic;
using Server.Models;

namespace Server.Controllers
{
    public interface ILoanController: IController<Loan>
    {
        public Loan Create(Member member, Volume volume);
        public List<Loan> FindForMember(int memberId);
    }
}

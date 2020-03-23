using System;
using System.Collections.Generic;

namespace Server.Models
{
    public partial class Loan
    {
        public int LoanId { get; set; }
        public int MemberId { get; set; }
        public int VolumeId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public int Extensions { get; set; }
        public DateTime? ReturnedDate { get; set; }

        public virtual Member Member { get; set; }
        public virtual Volume Volume { get; set; }
    }
}

namespace Server.Models
{
    public partial class Acquire
    {
        public int MaterialId { get; set; }
        public string AdditionalInfo { get; set; }
        public int ReasonId { get; set; }

        public virtual Material Material { get; set; }
        public virtual AcquireReason Reason { get; set; }
    }
}

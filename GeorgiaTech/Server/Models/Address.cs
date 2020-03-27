namespace Server.Models
{
    public partial class Address
    {
        public int AddressId { get; set; }
        public string Street { get; set; }
        public string AdditionalInfo { get; set; }
        public int ZipCode { get; set; }

        public virtual ZipCode Zip { get; set; }
    }
}

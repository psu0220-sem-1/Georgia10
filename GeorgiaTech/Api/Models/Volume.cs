using System;
namespace Api.Models
{
    public class Volume
    {
        public int VolumeId { get; set; }
        public Address HomeLocation { get; set; }
        public Address CurrentLocation { get; set; }
        public Material Material { get; set; }
    }
}

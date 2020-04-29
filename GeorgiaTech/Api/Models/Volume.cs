using System;
namespace Api.Models
{
    public class Volume
    {
        public int VolumeId { get; set; }
        public int MaterialId { get; set; }
        public int HomeLocationId { get; set; }
        public int CurrentLocationId { get; set; }

        public Address HomeLocation { get; set; }
        public Address CurrentLocation { get; set; }
        public Material Material { get; set; }
    }
}

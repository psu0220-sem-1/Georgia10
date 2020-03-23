using System;
using System.Collections.Generic;

namespace Server.Models
{
    public partial class MaterialType
    {
        public MaterialType()
        {
            Material = new HashSet<Material>();
        }

        public int TypeId { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Material> Material { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Api.Models
{
    public class Material
    {
        public int MaterialId { get; set; }
        public string Isbn { get; set; }
        public string Language { get; set; }
        public bool Lendable { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public List<Author> Authors { get; set; }
        public List<MaterialSubject> MaterialSubjects { get; set; }
    }
}

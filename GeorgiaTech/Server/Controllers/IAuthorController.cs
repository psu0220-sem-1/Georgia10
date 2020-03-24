using System.Collections.Generic;
using Server.Models;

namespace Server.Controllers
{
    public interface IAuthorController: IController<Author>
    {
        public Author Create(string firstName, string lastName);
        public List<Material> FindMaterials(Author author);
    }
}
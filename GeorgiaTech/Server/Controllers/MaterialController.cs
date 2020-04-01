using System;
using System.Collections.Generic;
using System.Linq;
using Server.Models;

namespace Server.Controllers
{
    public class MaterialController: IMaterialController
    {
        private readonly GTLContext _context;

        public MaterialController(GTLContext context)
        {
            _context = context;
        }

        public int Delete(Material t)
        {
            throw new NotImplementedException();
        }

        public List<Material> FindAll()
        {
            throw new NotImplementedException();
        }

        public Material FindByID(int ID)
        {
            return _context.Materials.Single(m => m.MaterialId == ID);
        }

        public Material FindByType(Material t)
        {
            throw new NotImplementedException();
        }

        public Material Insert(Material t)
        {
            throw new NotImplementedException();
        }

        public Material Update(Material t)
        {
            throw new NotImplementedException();
        }
    }
}

﻿using System.Collections.Generic;
using Server.Models;

namespace Server.Controllers
{
    public interface IMaterialController: IController<Material>
    {
        public Material Create(
            string isbn,
            string title,
            string language,
            bool lendable,
            string description,
            MaterialType type,
            List<MaterialSubject> subjects,
            List<Author> authors
        );

        public int Update(int id, Material newMaterial);
        public MaterialType FindMaterialTypeById(int id);
        public IEnumerable<MaterialType> GetMaterialTypes();
        public MaterialSubject FindMaterialSubjectById(int id);
        public IEnumerable<MaterialSubject> GetMaterialSubjects();
    }
}

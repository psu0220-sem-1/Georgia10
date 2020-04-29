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

        /// <summary>
        /// Deletes the material entity passed from the database
        /// </summary>
        /// <param name="material">The material to be deleted</param>
        /// <returns>Number of rows that have changed</returns>
        public int Delete(Material material)
        {
            _context.Remove(material);
            return _context.SaveChanges();
        }

        /// <summary>
        /// Creates a material entity based on the parameters passed
        /// </summary>
        /// <param name="isbn">The ISBN number of the material</param>
        /// <param name="title">The title of the material</param>
        /// <param name="language">The language the material is written in</param>
        /// <param name="lendable">Whether the material is lendable</param>
        /// <param name="description">A description of the material</param>
        /// <param name="type">The material type</param>
        /// <param name="subjects">The subjects the material covers</param>
        /// <param name="authors">A list of authors that have written the book</param>
        /// <returns>A material entity object</returns>
        /// <remarks>NOT TESTED!!!</remarks>
        public Material Create(string isbn, string title, string language, bool lendable, string description, MaterialType type,
            List<MaterialSubject> subjects, List<Author> authors)
        {
            var material = new Material
            {
                Isbn = isbn,
                Title = title,
                Language = language,
                Lendable = lendable,
                Description = description,
                Type = type,
            };

            var materialSubjects = subjects
                .Select(subject => new MaterialSubjects {MaterialSubject = subject, Material = material})
                .ToList();
            var materialAuthors = authors
                .Select(author => new MaterialAuthor {Author = author, Material = material})
                .ToList();

            material.MaterialSubjects = materialSubjects;
            material.MaterialAuthors = materialAuthors;

            return material;
        }

        /// <summary>
        /// Fetches all saved materials on the database
        /// </summary>
        /// <returns>A list of all materials saved on the database</returns>
        public List<Material> FindAll()
        {
            return _context.Materials.ToList();
        }

        /// <summary>
        /// Finds a single material by its ID
        /// </summary>
        /// <param name="ID">The ID of the material</param>
        /// <returns>The material or null if not found</returns>
        public Material FindByID(int ID)
        {
            return _context.Materials.Find(ID);
        }

        /// <summary>
        /// Saves a new material entity to the database
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public int Insert(Material material)
        {
            _context.Add(material);
            return _context.SaveChanges();
        }

        /// <summary>
        /// Saves all changes to a the database. If no changes are tracked, then nothing will happen.
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public int Update(Material material)
        {
            if (!_context.ChangeTracker.HasChanges())
                return 0;

            return _context.SaveChanges();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Server.Models;

namespace Server.Controllers
{
    public class MaterialController: IMaterialController
    {
        private readonly GTLContext _context;
        private readonly IAuthorController _authorController;

        public MaterialController(GTLContext context)
        {
            _context = context;
            _authorController = ControllerFactory.CreateAuthorController(context);
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
        /// <param name="isbn">The ISBN of the material</param>
        /// <param name="title">The title of the material</param>
        /// <param name="language">The language the material is written in</param>
        /// <param name="lendable">Whether the material is lendable or not</param>
        /// <param name="description">A description of the material</param>
        /// <param name="type">The type of the material</param>
        /// <param name="subjects">The subjects the material covers</param>
        /// <param name="authors">The authors of the material</param>
        /// <returns>A material object</returns>
        /// <exception cref="ArgumentNullException">Thrown if one of isbn, title, language or description is empty or null</exception>
        /// <exception cref="ArgumentException">Thrown if MaterialType, MaterialSubject or Author doesn't exist in the database</exception>
        public Material Create(string isbn, string title, string language, bool lendable, string description,
            MaterialType type, List<MaterialSubject> subjects, List<Author> authors)
        {
            // validate material data
            if (isbn.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(isbn), "ISBN can't be null or empty");
            if (title.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(title), "Title can't be null or empty");
            if (language.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(language), "Language can't be null or empty");
            if (description.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(description), "Description can't be null or empty");

            var materialType = _context.MaterialTypes.Find(type.TypeId);
            if (materialType == null)
                throw new ArgumentException("MaterialType must already exist", nameof(type));

            // create material
            var material = new Material
            {
                Isbn = isbn,
                Title = title,
                Language = language,
                Lendable = lendable,
                Description = description,
                Type = materialType,
            };

            // validate extra data
            var materialSubjects = subjects
                .Select(subject =>
                {
                    var s = _context.MaterialSubjects.Find(subject.SubjectId);
                    if (s == null)
                        throw new ArgumentException("All MaterialSubjects must already exist", nameof(subjects));

                    return new MaterialSubjects {MaterialSubject = s, Material = material};
                }).ToList();

            var materialAuthors = authors
                .Select(author =>
                {
                    var a = _authorController.FindByID(author.AuthorId);
                    if (a == null)
                        throw new ArgumentException("All Authors must already exist", nameof(authors));

                    return new MaterialAuthor {Author = a, Material = material};
                }).ToList();

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


        /// <summary>
        /// Updates a material found by id to match newMaterial. Will validate all the data passed. Expects a full
        /// Material object to update by. Fields that are to remain the same, should still be passed holding the
        /// original value.
        /// </summary>
        /// <param name="id">The ID of the material</param>
        /// <param name="newMaterial">The new data wrapped in a Material object</param>
        /// <returns>The number of rows that were changed</returns>
        ///
        /// <exception cref="ArgumentNullException">
        /// If any of the Isbn, Title, Language or Description properties
        /// are null or empty
        /// </exception>
        ///
        /// <exception cref="ArgumentException">
        /// If MaterialType, any of MaterialSubjects or any of MaterialAuthors can't
        /// be found in the database
        /// </exception>
        ///
        /// <remarks>NOT TESTED</remarks>
        public int Update(int id, Material newMaterial)
        {
            var material = FindByID(id);

            if (material == null)
                return 0;

            // validate material data
            if (newMaterial.Isbn.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(newMaterial.Isbn), "ISBN can't be null or empty");
            if (newMaterial.Title.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(newMaterial.Title), "Title can't be null or empty");
            if (newMaterial.Language.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(newMaterial.Language), "Language can't be null or empty");
            if (newMaterial.Description.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(newMaterial.Description), "Description can't be null or empty");

            var materialType = _context.MaterialTypes.Find(newMaterial.Type.TypeId);
            if (materialType == null)
                throw new ArgumentException("MaterialType must already exist", nameof(newMaterial.Type));

            material.Isbn = newMaterial.Isbn;
            material.Title = newMaterial.Title;
            material.Language = newMaterial.Language;
            material.Lendable = newMaterial.Lendable;
            material.Description = newMaterial.Description;
            material.Type = materialType;

            var materialSubjects = newMaterial.MaterialSubjects
                .Select(ms =>
                {
                    var subject = _context.MaterialSubjects.Find(ms.SubjectId);
                    if (subject == null)
                        throw new ArgumentException("MaterialSubjects must already exist", nameof(newMaterial.MaterialSubjects));

                    return new MaterialSubjects {MaterialSubject = subject, Material = material};
                }).ToList();

            var materialAuthors = newMaterial.MaterialAuthors
                .Select(ma =>
                {
                    var author = _authorController.FindByID(ma.AuthorId);
                    if (author == null)
                        throw new ArgumentException("Authors must already exist", nameof(newMaterial.MaterialAuthors));

                    return new MaterialAuthor {Author = author, Material = material};
                }).ToList();

            material.MaterialSubjects = materialSubjects;
            material.MaterialAuthors = materialAuthors;

            return _context.SaveChanges();
        }

        /// <summary>
        /// Finds and returns a MaterialType by its ID
        /// </summary>
        /// <param name="id">the ID of the materialType</param>
        /// <returns>The material type or null if not found</returns>
        /// <remarks>NOT TESTED</remarks>
        public MaterialType FindMaterialTypeById(int id)
        {
            return _context.MaterialTypes.Find(id);
        }

        /// <summary>
        /// Returns an IEnumerable containing all material types saved on the database
        /// </summary>
        /// <returns>An IEnumerable object containing all material types</returns>
        public IEnumerable<MaterialType> GetMaterialTypes()
        {
            return _context.MaterialTypes.ToList();
        }

        /// <summary>
        /// Finds and returns a MaterialSubject by its ID
        /// </summary>
        /// <param name="id">The ID of the material subject</param>
        /// <returns>The MaterialSubject or null if not found</returns>
        /// <remarks>NOT TESTED</remarks>
        public MaterialSubject FindMaterialSubjectById(int id)
        {
            return _context.MaterialSubjects.Find(id);
        }
    }
}

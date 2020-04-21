using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers
{
    public class AuthorController: IAuthorController
    {
        private readonly GTLContext _db;

        public AuthorController(GTLContext context)
        {
            _db = context;
        }

        /// <summary>
        /// Inserts an author entity in the database.
        /// </summary>
        /// <param name="author">An author entity instance</param>
        /// <returns>The updated author entity</returns>
        /// <exception cref="ArgumentNullException">If the author entity instance is null</exception>
        public Author Insert(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author), "Author object can't be null");
            }

            _db.Add(author);
            _db.SaveChanges();

            return author;
        }

        /// <summary>
        /// Finds an author entity by ID
        /// </summary>
        /// <param name="ID">The ID to query by</param>
        /// <returns>The found author entity or null</returns>
        public Author FindByID(int ID)
        {
            var author = _db.Authors.Find(ID);
            return author;
        }

        [Obsolete]
        public Author FindByType(Author t)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Fetches all authors that exist on the database
        /// </summary>
        /// <returns>A list of all authors on the database</returns>
        /// <remarks>NOT TESTED</remarks>
        public List<Author> FindAll()
        {
            return _db.Authors.ToList();
        }

        /// <summary>
        /// Saves any changes made to the context's entities. If no changes are made before the method call
        /// the method won't do any operations and return null signifying such.
        /// </summary>
        /// <param name="author">An author instance that has been changed</param>
        /// <returns>The author instance passed if changes are saved, null otherwise</returns>
        /// <remarks>NOT TESTED</remarks>
        public Author Update(Author author)
        {
            if (!_db.ChangeTracker.HasChanges())
                return null;

            int changedRows;
            using var transaction = _db.Database.BeginTransaction();

            try
            {
                // TODO: return changed rows once IController's Update method signature has been updated
                changedRows = _db.SaveChanges();
                transaction.Commit();
            }
            catch (DbUpdateException)
            {
                transaction.Rollback();
                throw;
            }

            return author;
        }

        /// <summary>
        /// Deletes an author entity from the context and applies the removal to the database.
        /// </summary>
        /// <param name="author">The author to be removed</param>
        /// <returns>The number of rows affected on the database</returns>
        /// <remarks>NOT TESTED</remarks>
        public int Delete(Author author)
        {
            int changedRows;
            using var transaction = _db.Database.BeginTransaction();

            try
            {
                _db.Remove(author);
                changedRows = _db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                transaction.Rollback();
                throw; // rethrow
            }

            return changedRows;
        }

        /// <summary>
        /// Creates an Author entity instance and returns it. The firstName and lastName arguments
        /// can't be empty or longer than 50 characters each.
        /// </summary>
        /// <param name="firstName">The first name of the author</param>
        /// <param name="lastName">The last name of the author</param>
        /// <returns>A new Author instance</returns>
        /// <exception cref="ArgumentException">Thrown if either the firstName or lastName arguments are empty</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if either the firstName or lastName are longer than 50 characters</exception>
        public Author Create(string firstName, string lastName)
        {
            if (firstName.Equals(""))
            {
                throw new ArgumentException("First name can't be empty");
            }

            if (lastName.Equals(""))
            {
                throw new ArgumentException("Last name can't be empty");
            }

            if (firstName.Length > 50)
            {
                throw new ArgumentOutOfRangeException(nameof(firstName), "First name can't be more than 50 characters");
            }

            if (lastName.Length > 50)
            {
                throw new ArgumentOutOfRangeException(nameof(lastName), "Last name can't be longer than 50 characters");
            }

            var author = new Author
            {
                FirstName = firstName,
                LastName = lastName
            };

            return author;
        }

        /// <summary>
        /// Fetches all materials found in the database that are written by the author passed as a parameter
        /// </summary>
        /// <param name="author">The author to search by. Must already be inserted in the database</param>
        /// <returns>A list of all materials found that are written by the author (could be an empty list)</returns>
        public List<Material> FindMaterials(Author author)
        {
            return _db.Materials
                .Where(m => m.MaterialAuthors
                    .Any(ma => ma.AuthorId == author.AuthorId))
                .ToList();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Author FindByType(Author t)
        {
            throw new System.NotImplementedException();
        }

        public List<Author> FindAll()
        {
            return _db.Authors.Where(a => true).ToList();
        }

        public Author Update(Author t)
        {
            throw new System.NotImplementedException();
        }

        public int Delete(Author t)
        {
            throw new System.NotImplementedException();
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

        public List<Material> FindMaterials(Author author)
        {
            throw new System.NotImplementedException();
        }
    }
}
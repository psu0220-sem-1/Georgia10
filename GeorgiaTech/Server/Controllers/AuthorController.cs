using System;
using System.Collections.Generic;
using System.Linq;
using Server.Models;

namespace Server.Controllers
{
    public class AuthorController: IAuthorController
    {
        private readonly GTLContext _context;

        public AuthorController(GTLContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Insert method will insert (i.e. save or persist) the author passed.
        /// </summary>
        /// <param name="author">The author object to be saved</param>
        /// <returns>The same author object but with its ID</returns>
        public Author Insert(Author author)
        {
            _context.Authors.Add(author);
            _context.SaveChanges();

            return author;
        }

        public Author FindByID(int ID)
        {
            throw new System.NotImplementedException();
        }

        public Author FindByType(Author t)
        {
            throw new System.NotImplementedException();
        }

        public List<Author> FindAll()
        {
            return _context.Authors.Where(a => true).ToList();
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
        /// Create method creates an Author object and returns it.
        ///
        /// Note: The created author won't be saved to the DB, hence no ID will be assigned to the object.
        /// </summary>
        /// <param name="firstName">The first name of the author</param>
        /// <param name="lastName">The last name of the author</param>
        /// <returns>The constructed Author object</returns>
        public Author Create(string firstName, string lastName)
        {
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
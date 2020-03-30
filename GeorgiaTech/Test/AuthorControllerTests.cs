using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Server;
using Server.Controllers;
using Server.Models;

namespace Test
{
    public class AuthorControllerTests
    {
        [Test]
        public void AuthorControllerCreateCreatesCorrectAuthorInstance()
        {
            const string firstName = "Nikola";
            const string lastName = "Velichkov";

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("AuthorControllerCreateCreatesCorrectAuthorInstance")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);

            var author = authorController.Create(firstName, lastName);

            Assert.IsInstanceOf(typeof(Author), author);
            Assert.AreEqual(firstName, author.FirstName);
            Assert.AreEqual(lastName, author.LastName);
        }

        [Test]
        public void AuthorControllerCreateThrowsArgumentExceptionWithFirstNameLongerThan50()
        {
            const string firstName = "123456789012345678901234567890123456789012345678901";
            const string lastName = "Velichkov";
            Assert.IsTrue(firstName.Length > 50);

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("AuthorControllerCreateThrowsArgumentExceptionWithFirstNameLongerThan50")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);
            Assert.Throws<ArgumentOutOfRangeException>(() => authorController.Create(firstName, lastName));
        }


        [Test]
        public void AuthorControllerCreateThrowsArgumentExceptionWithEmptyFirstNameArgument()
        {
            const string firstName = "";
            const string lastName = "Velichkov";

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("AuthorControllerCreateThrowsArgumentExceptionWithEmptyFirstNameArgument")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);
            Assert.Throws<ArgumentException>(() => { authorController.Create(firstName, lastName); });
        }

        [Test]
        public void AuthorControllerCreateThrowsArgumentExceptionWithLastNameLongerThan50()
        {
            const string firstName = "Nikola";
            const string lastName = "123456789012345678901234567890123456789012345678901";
            Assert.IsTrue(lastName.Length > 50);

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("AuthorControllerCreateThrowsArgumentExceptionWithLastNameLongerThan50")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);
            Assert.Throws<ArgumentOutOfRangeException>(() => authorController.Create(firstName, lastName));
        }

        [Test]
        public void AuthorControllerCreateThrowsArgumentExceptionWithEmptyLastNameArgument()
        {
            const string firstName = "Nikola";
            const string lastName = "";

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("AuthorControllerCreateThrowsArgumentExceptionWithEmptyLastNameArgument")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);

            Assert.Throws<ArgumentException>(() => { authorController.Create(firstName, lastName); });
        }

        [Test]
        public void AuthorControllerInsertInsertsCorrectly()
        {
            const string firstName = "Nikola";
            const string lastName = "Velichkov";

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("AuthorControllerInsertInsertsCorrectly")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);

            var author = authorController.Create(firstName, lastName);
            Assert.AreEqual(0, author.AuthorId);

            authorController.Insert(author);

            var insertedAuthor = context.Authors
                .FirstOrDefault(a => a.FirstName == firstName && a.LastName == lastName);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(insertedAuthor);
                Assert.AreEqual(author.AuthorId, insertedAuthor.AuthorId);
                Assert.AreEqual(author.FirstName, insertedAuthor.FirstName);
                Assert.AreEqual(author.LastName, insertedAuthor.LastName);
            });
        }

        [Test]
        public void AuthorControllerInsertThrowsArgumentNullExceptionWithNullAuthorParam()
        {
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("AuthorControllerInsertThrowsArgumentNullExceptionWithNullAuthorParam")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);

            Assert.Throws<ArgumentNullException>(() => authorController.Insert(null));
            Assert.AreEqual(0, context.Authors.Count());
        }

        [Test]
        public void AuthorControllerFindByIdFindsAnExistingAuthor()
        {
            const string firstName = "Nikola";
            const string lastName = "Velichkov";

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("AuthorControllerFindByIdFindsAnExistingAuthor")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);
            var insertedAuthor = authorController.Create(firstName, lastName);
            Assert.NotNull(insertedAuthor);
            authorController.Insert(insertedAuthor);

            var author = authorController.FindByID(insertedAuthor.AuthorId);
            Assert.Multiple(() =>
            {
                Assert.NotNull(author);
                Assert.AreEqual(insertedAuthor.AuthorId, author.AuthorId);
                Assert.AreEqual(firstName, author.FirstName);
                Assert.AreEqual(lastName, author.LastName);
            });
        }

        [Test]
        public void AuthorControllerFindByIdDoesNotFindAnExistingAuthor()
        {
            const string firstName = "Nikola";
            const string lastName = "Velichkov";

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("AuthorControllerFindByIdDoesNotFindAnExistingAuthor")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);
            // create an author to use their ID (incremented) to be sure to try to get a non existing entity
            var insertedAuthor = authorController.Create(firstName, lastName);
            authorController.Insert(insertedAuthor);

            var author = authorController.FindByID(insertedAuthor.AuthorId + 1);
            Assert.IsNull(author);
        }
    }
}
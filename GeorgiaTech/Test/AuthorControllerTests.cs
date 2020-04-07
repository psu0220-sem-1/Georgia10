using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Server;
using Server.Controllers;
using Server.Models;

namespace Test
{
    [TestFixture]
    [Author("Nikola Anastasov Velichkov", "federlizer@gmail.com")]
    public class AuthorControllerTests
    {
        private string authorFirstName;
        private string authorLastName;
        private List<Material> materials;

        [SetUp]
        public void Setup()
        {
            authorFirstName = "Nikola";
            authorLastName = "Velichkov";

            materials = new List<Material>
            {
                new Material
                {
                    Isbn = "9780135957059",
                    Title = "Pragmatic Programmer special 2nd",
                    Language = "English",
                    Lendable = true,
                    Description = "Some description here",
                },
                new Material
                {
                    Isbn = "9780132350884",
                    Title = "Clean Code: A Handbook of Agile Software Craftsmanship",
                    Language = "English",
                    Lendable = true,
                    Description = "Some description here",
                },
                new Material
                {
                    Isbn = "9780062301253",
                    Title = "Elon Musk: Tesla, SpaceX, and the Quest for a Fantastic Future",
                    Language = "English",
                    Lendable = true,
                    Description = "Some description here",
                },
            };
        }

        [Test]
        public void CreateCreatesCorrectAuthorInstance()
        {
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateCreatesCorrectAuthorInstance")
                .Options;
            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);

            // action
            var author = authorController.Create(authorFirstName, authorLastName);

            // assertion
            Assert.That(author, Has
                .Property("FirstName").EqualTo(authorFirstName).And
                .Property("LastName").EqualTo(authorLastName));
        }

        [Test]
        public void CreateThrowsArgumentOutOfRangeExceptionWithFirstNameLongerThan50()
        {
            // setup
            authorFirstName = "123456789012345678901234567890123456789012345678901";
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateThrowsArgumentOutOfRangeExceptionWithFirstNameLongerThan50")
                .Options;
            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);

            // assertion
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                authorController.Create(authorFirstName, authorLastName));
        }


        [Test]
        public void CreateThrowsArgumentExceptionWithEmptyFirstNameArgument()
        {
            // setup
            authorFirstName = "";
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateThrowsArgumentExceptionWithEmptyFirstNameArgument")
                .Options;
            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);

            // assertion
            Assert.Throws<ArgumentException>(() =>
                authorController.Create(authorFirstName, authorLastName));
        }

        [Test]
        public void CreateThrowsArgumentOutOfRangeExceptionWithLastNameLongerThan50()
        {
            // setup
            authorLastName = "123456789012345678901234567890123456789012345678901";
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateThrowsArgumentOutOfRangeExceptionWithLastNameLongerThan50")
                .Options;
            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);

            // assertion
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                authorController.Create(authorFirstName, authorLastName));
        }

        [Test]
        public void CreateThrowsArgumentExceptionWithEmptyLastNameArgument()
        {
            // setup
            authorLastName = "";
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateThrowsArgumentExceptionWithEmptyLastNameArgument")
                .Options;
            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);

            // assertion
            Assert.Throws<ArgumentException>(() =>
                authorController.Create(authorFirstName, authorLastName));
        }

        [Test]
        public void InsertInsertsCorrectly()
        {
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("InsertInsertsCorrectly")
                .Options;

            // action
            using (var context = new GTLContext(options))
            {
                var authorController = ControllerFactory.CreateAuthorController(context);
                var author = new Author {FirstName = authorFirstName, LastName = authorLastName};

                authorController.Insert(author);
            }

            // assertion
            using (var context = new GTLContext(options))
            {
                var fetchedAuthor = context.Authors
                    .FirstOrDefault(a => a.FirstName == authorFirstName && a.LastName == authorLastName);

                Assert.That(fetchedAuthor, Has
                    .Property("FirstName").EqualTo(authorFirstName).And
                    .Property("LastName").EqualTo(authorLastName));
            }
        }

        [Test]
        public void InsertThrowsArgumentNullExceptionWithNullAuthorParam()
        {
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("InsertThrowsArgumentNullExceptionWithNullAuthorParam")
                .Options;
            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);

            // assertion
            Assert.Throws<ArgumentNullException>(() => authorController.Insert(null));
        }

        [Test]
        public void InsertDoesNotInsertWithNullAuthorParam()
        {
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("InsertDoesNotInsertWithNullAuthorParam")
                .Options;

            // action
            using (var context = new GTLContext(options))
            {
                var authorController = ControllerFactory.CreateAuthorController(context);

                // silently catch the error that's supposed to be thrown by the Insert method
                // in order to test the after effect
                try { authorController.Insert(null); }
                catch (ArgumentNullException) { }
            }

            // assertion
            using (var context = new GTLContext(options))
            {
                Assert.That(context.Authors.Count(), Is.EqualTo(0));
            }
        }

        [Test]
        public void FindByIdFindsAnExistingAuthor()
        {
            // setup
            int authorId;
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("FindByIdFindsAnExistingAuthor")
                .Options;

            // action
            using (var context = new GTLContext(options))
            {
                var author = new Author {FirstName = authorFirstName, LastName = authorLastName};
                context.Add(author);
                context.SaveChanges();
                authorId = author.AuthorId;
            }

            // assertion
            using (var context = new GTLContext(options))
            {
                var authorController = ControllerFactory.CreateAuthorController(context);
                var fetchedAuthor = authorController.FindByID(authorId);

                Assert.That(fetchedAuthor, Has
                    .Property("FirstName").EqualTo(authorFirstName).And
                    .Property("LastName").EqualTo(authorLastName));
            }
        }

        [Test]
        public void FindByIdReturnsNullOnANonExistingAuthor()
        {
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("FindByIdReturnsNullOnANonExistingAuthor")
                .Options;

            // action
            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);
            // some random number, doesn't matter what
            var fetchedAuthor = authorController.FindByID(11);

            // assertion
            Assert.That(fetchedAuthor, Is.Null);
        }

        [Test]
        public void FindMaterialsFetchesMaterialsWithASingleAuthor()
        {
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("FindMaterialsFetchesMaterialsWithASingleAuthor")
                .UseLazyLoadingProxies()
                .Options;

            // action
            var author = new Author { FirstName = authorFirstName, LastName = authorLastName };

            // add authors to materials
            materials.ForEach(material =>
            {
                material.MaterialAuthors = new List<MaterialAuthor>
                {
                    new MaterialAuthor {Author = author, Material = material}
                };
            });

            using (var context = new GTLContext(options))
            {
                context.AddRange(materials);
                context.SaveChanges();
            }

            // assertion
            using (var context = new GTLContext(options))
            {
                var authorController = ControllerFactory.CreateAuthorController(context);
                var fetchedAuthor = context.Authors.FirstOrDefault(a => a.FirstName == authorFirstName);
                var fetchedMaterials = authorController.FindMaterials(fetchedAuthor);

                Assert.That(fetchedMaterials, Has.Exactly(materials.Count).Items);
            }
        }

        [Test]
        public void FindMaterialsFetchesMaterialsWithMultipleAuthors()
        {
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("FindMaterialsFetchesMaterialsWithMultipleAuthors")
                .UseLazyLoadingProxies()
                .Options;

            var authorOne = new Author { FirstName = authorFirstName, LastName = authorLastName };
            var authorTwo = new Author { FirstName = "Gergana", LastName = "Petkova" };

            // action
            // add authors to materials
            materials.ForEach(material =>
            {
                material.MaterialAuthors = new List<MaterialAuthor>
                {
                    new MaterialAuthor {Author = authorOne, Material = material},
                    new MaterialAuthor {Author = authorTwo, Material = material}
                };
            });

            using (var context = new GTLContext(options))
            {
                context.AddRange(materials);
                context.SaveChanges();
            }

            // assertion
            using (var context = new GTLContext(options))
            {
                var authorController = ControllerFactory.CreateAuthorController(context);
                var fetchedAuthor = context.Authors.FirstOrDefault(a => a.FirstName == authorOne.FirstName);

                var fetchedMaterials = authorController.FindMaterials(fetchedAuthor);
                Assert.That(fetchedMaterials, Has.Exactly(materials.Count).Items);
            }
        }

        [Test]
        public void FindMaterialsDoesNotFetchAnyMaterialsWithOneAuthorAndZeroMaterials()
        {
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("FindMaterialsDoesNotFetchAnyMaterialsWithOneAuthorAndZeroMaterials")
                .UseLazyLoadingProxies()
                .Options;

            var authorOne = new Author { FirstName = authorFirstName, LastName = authorLastName };
            var authorTwo = new Author { FirstName = "Gergana", LastName = "Petkova" };

            // action
            // add authors to materials
            materials.ForEach(material =>
            {
                material.MaterialAuthors = new List<MaterialAuthor>
                {
                    new MaterialAuthor {Author = authorTwo, Material = material}
                };
            });

            using (var context = new GTLContext(options))
            {
                context.Add(authorOne);
                context.AddRange(materials);
                context.SaveChanges();
            }

            // assertion
            using (var context = new GTLContext(options))
            {
                var authorController = ControllerFactory.CreateAuthorController(context);
                var fetchedAuthor = context.Authors.FirstOrDefault(a => a.FirstName == authorFirstName);

                var fetchedMaterials = authorController.FindMaterials(fetchedAuthor);

                Assert.That(fetchedMaterials, Is.Empty.And.Not.Null);
            }
        }
    }
}
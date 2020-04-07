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
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateCreatesCorrectAuthorInstance")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);

            var author = authorController.Create(authorFirstName, authorLastName);

            Assert.IsInstanceOf(typeof(Author), author);
            Assert.AreEqual(authorFirstName, author.FirstName);
            Assert.AreEqual(authorLastName, author.LastName);
        }

        [Test]
        public void CreateThrowsArgumentOutOfRangeExceptionWithFirstNameLongerThan50()
        {
            authorFirstName = "123456789012345678901234567890123456789012345678901";
            Assert.IsTrue(authorFirstName.Length > 50);

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateThrowsArgumentOutOfRangeExceptionWithFirstNameLongerThan50")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);
            Assert.Throws<ArgumentOutOfRangeException>(() => authorController.Create(authorFirstName, authorLastName));
        }


        [Test]
        public void CreateThrowsArgumentExceptionWithEmptyFirstNameArgument()
        {
            authorFirstName = "";

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateThrowsArgumentExceptionWithEmptyFirstNameArgument")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);
            Assert.Throws<ArgumentException>(() => { authorController.Create(authorFirstName, authorLastName); });
        }

        [Test]
        public void CreateThrowsArgumentOutOfRangeExceptionWithLastNameLongerThan50()
        {
            authorLastName = "123456789012345678901234567890123456789012345678901";
            Assert.IsTrue(authorLastName.Length > 50);

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateThrowsArgumentOutOfRangeExceptionWithLastNameLongerThan50")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);
            Assert.Throws<ArgumentOutOfRangeException>(() => authorController.Create(authorFirstName, authorLastName));
        }

        [Test]
        public void CreateThrowsArgumentExceptionWithEmptyLastNameArgument()
        {
            authorLastName = "";

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateThrowsArgumentExceptionWithEmptyLastNameArgument")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);

            Assert.Throws<ArgumentException>(() => { authorController.Create(authorFirstName, authorLastName); });
        }

        [Test]
        public void InsertInsertsCorrectly()
        {
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("InsertInsertsCorrectly")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);

            var author = authorController.Create(authorFirstName, authorLastName);
            Assert.AreEqual(0, author.AuthorId);

            authorController.Insert(author);

            var insertedAuthor = context.Authors
                .FirstOrDefault(a => a.FirstName == authorFirstName && a.LastName == authorLastName);

            Assert.Multiple(() =>
            {
                Assert.IsNotNull(insertedAuthor);
                Assert.AreEqual(author.AuthorId, insertedAuthor.AuthorId);
                Assert.AreEqual(author.FirstName, insertedAuthor.FirstName);
                Assert.AreEqual(author.LastName, insertedAuthor.LastName);
            });
        }

        [Test]
        public void InsertThrowsArgumentNullExceptionWithNullAuthorParam()
        {
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("InsertThrowsArgumentNullExceptionWithNullAuthorParam")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);

            Assert.Throws<ArgumentNullException>(() => authorController.Insert(null));
        }

        [Test]
        public void InsertDoesNotInsertWithNullAuthorParam()
        {
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("InsertThrowsArgumentNullExceptionWithNullAuthorParam")
                .Options;

            // action
            using (var context = new GTLContext(options))
            {
                var authorController = ControllerFactory.CreateAuthorController(context);

                // silently catch the error that's supposed to be thrown by the Insert method
                // in order to test the after effect
                try { authorController.Insert(null); }
                catch (ArgumentNullException e) { }
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
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("FindByIdFindsAnExistingAuthor")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);
            var insertedAuthor = authorController.Create(authorFirstName, authorLastName);
            Assert.NotNull(insertedAuthor);
            authorController.Insert(insertedAuthor);

            var author = authorController.FindByID(insertedAuthor.AuthorId);
            Assert.Multiple(() =>
            {
                Assert.NotNull(author);
                Assert.AreEqual(insertedAuthor.AuthorId, author.AuthorId);
                Assert.AreEqual(authorFirstName, author.FirstName);
                Assert.AreEqual(authorLastName, author.LastName);
            });
        }

        [Test]
        public void FindByIdDoesNotFindANonExistingAuthor()
        {
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("FindByIdDoesNotFindANonExistingAuthor")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);
            // create an author to use their ID (incremented) to be sure to try to get a non existing entity
            var insertedAuthor = authorController.Create(authorFirstName, authorLastName);
            authorController.Insert(insertedAuthor);

            var author = authorController.FindByID(insertedAuthor.AuthorId + 1);
            Assert.IsNull(author);
        }

        [Test]
        public void FindMaterialsFetchesMaterialsWithASingleAuthor()
        {
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("FindMaterialsFetchesMaterialsWithASingleAuthor")
                .UseLazyLoadingProxies()
                .Options;

            // setup
            var author = new Author { FirstName = authorFirstName, LastName = authorLastName };

            // add authors to materials
            materials.ForEach(material =>
            {
                material.MaterialAuthors = new List<MaterialAuthor>
                {
                    new MaterialAuthor
                    {
                        Author = author,
                        Material = material,
                    }
                };
            });

            using (var context = new GTLContext(options))
            {
                context.AddRange(materials);
                context.SaveChanges();
            }

            // action
            using (var context = new GTLContext(options))
            {
                var authorController = ControllerFactory.CreateAuthorController(context);
                var fetchedAuthor = context.Authors.FirstOrDefault(a => a.FirstName == authorFirstName);
                Assert.IsNotNull(fetchedAuthor);

                var fetchedMaterials = authorController.FindMaterials(fetchedAuthor);

                // assert material authors
                fetchedMaterials.ForEach(material =>
                {
                    Assert.Multiple(() =>
                    {
                        Assert.AreEqual(1, material.MaterialAuthors.Count);
                        Assert.IsNotNull(material.MaterialAuthors.FirstOrDefault(ma =>
                            ma.Author.FirstName == authorFirstName && ma.Author.LastName == authorLastName));
                    });
                });
            }
        }

        [Test]
        public void FindMaterialsFetchesMaterialsWithMultipleAuthors()
        {
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("FindMaterialsFetchesMaterialsWithMultipleAuthors")
                .UseLazyLoadingProxies()
                .Options;

            // setup
            var authorOne = new Author { FirstName = authorFirstName, LastName = authorLastName };
            var authorTwo = new Author { FirstName = "Gergana", LastName = "Petkova" };

            // add authors to materials
            materials.ForEach(material =>
            {
                material.MaterialAuthors = new List<MaterialAuthor>
                {
                    new MaterialAuthor
                    {
                        Author = authorOne,
                        Material = material,
                    },
                    new MaterialAuthor
                    {
                        Author = authorTwo,
                        Material = material,
                    }
                };
            });

            using (var context = new GTLContext(options))
            {
                context.AddRange(materials);
                context.SaveChanges();
            }

            using (var context = new GTLContext(options))
            {
                var authorController = ControllerFactory.CreateAuthorController(context);
                var fetchedAuthor = context.Authors.FirstOrDefault(a => a.FirstName == authorOne.FirstName);
                Assert.IsNotNull(fetchedAuthor);

                var fetchedMaterials = authorController.FindMaterials(fetchedAuthor);

                // assert material authors
                fetchedMaterials.ForEach(material =>
                {
                    Assert.Multiple(() =>
                    {
                        Assert.AreEqual(2, material.MaterialAuthors.Count);
                        Assert.IsNotNull(material.MaterialAuthors.FirstOrDefault(ma =>
                            ma.Author.FirstName == authorFirstName && ma.Author.LastName == authorLastName));
                    });
                });
            }
        }

        [Test]
        public void FindMaterialsDoesNotFetchAnyMaterialsWithOneAuthorAndZeroMaterials()
        {
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("FindMaterialsDoesNotFetchAnyMaterialsWithOneAuthorAndZeroMaterials")
                .UseLazyLoadingProxies()
                .Options;

            // setup

            var authorOne = new Author { FirstName = authorFirstName, LastName = authorLastName };
            var authorTwo = new Author { FirstName = "Gergana", LastName = "Petkova" };

            // add authors to materials
            materials.ForEach(material =>
            {
                material.MaterialAuthors = new List<MaterialAuthor>
                {
                    new MaterialAuthor
                    {
                        Author = authorTwo,
                        Material = material,
                    }
                };
            });

            using (var context = new GTLContext(options))
            {
                context.Add(authorOne);
                context.AddRange(materials);
                context.SaveChanges();
            }

            using (var context = new GTLContext(options))
            {
                var authorController = ControllerFactory.CreateAuthorController(context);
                var fetchedAuthor = context.Authors.FirstOrDefault(a => a.FirstName == authorFirstName);
                Assert.IsNotNull(fetchedAuthor);

                var fetchedMaterials = authorController.FindMaterials(fetchedAuthor);

                // assert materials
                Assert.IsNotNull(fetchedMaterials);
                Assert.AreEqual(0, fetchedMaterials.Count);
            }
        }
    }
}
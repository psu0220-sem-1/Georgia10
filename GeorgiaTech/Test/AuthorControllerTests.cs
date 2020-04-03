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
    public class AuthorControllerTests
    {
        [Test]
        public void CreateCreatesCorrectAuthorInstance()
        {
            const string firstName = "Nikola";
            const string lastName = "Velichkov";

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateCreatesCorrectAuthorInstance")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);

            var author = authorController.Create(firstName, lastName);

            Assert.IsInstanceOf(typeof(Author), author);
            Assert.AreEqual(firstName, author.FirstName);
            Assert.AreEqual(lastName, author.LastName);
        }

        [Test]
        public void CreateThrowsArgumentOutOfRangeExceptionWithFirstNameLongerThan50()
        {
            const string firstName = "123456789012345678901234567890123456789012345678901";
            const string lastName = "Velichkov";
            Assert.IsTrue(firstName.Length > 50);

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateThrowsArgumentOutOfRangeExceptionWithFirstNameLongerThan50")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);
            Assert.Throws<ArgumentOutOfRangeException>(() => authorController.Create(firstName, lastName));
        }


        [Test]
        public void CreateThrowsArgumentExceptionWithEmptyFirstNameArgument()
        {
            const string firstName = "";
            const string lastName = "Velichkov";

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateThrowsArgumentExceptionWithEmptyFirstNameArgument")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);
            Assert.Throws<ArgumentException>(() => { authorController.Create(firstName, lastName); });
        }

        [Test]
        public void CreateThrowsArgumentOutOfRangeExceptionWithLastNameLongerThan50()
        {
            const string firstName = "Nikola";
            const string lastName = "123456789012345678901234567890123456789012345678901";
            Assert.IsTrue(lastName.Length > 50);

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateThrowsArgumentOutOfRangeExceptionWithLastNameLongerThan50")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);
            Assert.Throws<ArgumentOutOfRangeException>(() => authorController.Create(firstName, lastName));
        }

        [Test]
        public void CreateThrowsArgumentExceptionWithEmptyLastNameArgument()
        {
            const string firstName = "Nikola";
            const string lastName = "";

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("CreateThrowsArgumentExceptionWithEmptyLastNameArgument")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);

            Assert.Throws<ArgumentException>(() => { authorController.Create(firstName, lastName); });
        }

        [Test]
        public void InsertInsertsCorrectly()
        {
            const string firstName = "Nikola";
            const string lastName = "Velichkov";

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("InsertInsertsCorrectly")
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
        public void InsertThrowsArgumentNullExceptionWithNullAuthorParam()
        {
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("InsertThrowsArgumentNullExceptionWithNullAuthorParam")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);

            Assert.Throws<ArgumentNullException>(() => authorController.Insert(null));
            Assert.AreEqual(0, context.Authors.Count());
        }

        [Test]
        public void FindByIdFindsAnExistingAuthor()
        {
            const string firstName = "Nikola";
            const string lastName = "Velichkov";

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("FindByIdFindsAnExistingAuthor")
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
        public void FindByIdDoesNotFindANonExistingAuthor()
        {
            const string firstName = "Nikola";
            const string lastName = "Velichkov";

            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase("FindByIdDoesNotFindANonExistingAuthor")
                .Options;

            using var context = new GTLContext(options);
            var authorController = ControllerFactory.CreateAuthorController(context);
            // create an author to use their ID (incremented) to be sure to try to get a non existing entity
            var insertedAuthor = authorController.Create(firstName, lastName);
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
            const string materialOneTitle = "Pragmatic Programmer special 2nd";
            const string materialTwoTitle = "Clean Code: A Handbook of Agile Software Craftsmanship";
            const string materialThreeTitle = "Elon Musk: Tesla, SpaceX, and the Quest for a Fantastic Future";

            var insertedAuthor = new Author
            {
                FirstName = "Nikola",
                LastName = "Velichkov",
            };

            var insertedMaterials = new List<Material>
            {
                new Material
                {
                    Isbn = "9780135957059",
                    Title = materialOneTitle,
                    Language = "English",
                    Lendable = true,
                    Description = "Some description here",
                },
                new Material
                {
                    Isbn = "9780132350884",
                    Title = materialTwoTitle,
                    Language = "English",
                    Lendable = true,
                    Description = "Some description here",
                },
                new Material
                {
                    Isbn = "9780062301253",
                    Title = materialThreeTitle,
                    Language = "English",
                    Lendable = true,
                    Description = "Some description here",
                },
            };

            // add authors to materials
            insertedMaterials.ForEach(material =>
            {
                material.MaterialAuthors = new List<MaterialAuthor>
                {
                    new MaterialAuthor
                    {
                        Author = insertedAuthor,
                        Material = material,
                    }
                };
            });

            using (var context = new GTLContext(options))
            {
                context.AddRange(insertedMaterials);
                context.SaveChanges();
            }

            // action
            using (var context = new GTLContext(options))
            {
                var authorController = ControllerFactory.CreateAuthorController(context);
                var author = context.Authors.FirstOrDefault(a => a.FirstName == insertedAuthor.FirstName);
                Assert.IsNotNull(author);

                var materials = authorController.FindMaterials(author);

                // assert materials
                Assert.Multiple(() =>
                {
                    Assert.IsNotNull(materials);
                    Assert.AreEqual(3, materials.Count);
                    Assert.IsTrue(materials.Any(m => m.Title == materialOneTitle));
                    Assert.IsTrue(materials.Any(m => m.Title == materialTwoTitle));
                    Assert.IsTrue(materials.Any(m => m.Title == materialThreeTitle));
                });

                // assert material authors
                materials.ForEach(material =>
                {
                    Assert.Multiple(() =>
                    {
                        Assert.AreEqual(1, material.MaterialAuthors.Count);
                        Assert.IsNotNull(material.MaterialAuthors.FirstOrDefault(ma =>
                            ma.Author.FirstName == insertedAuthor.FirstName && ma.Author.LastName == insertedAuthor.LastName));
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
            const string materialOneTitle = "Pragmatic Programmer special 2nd";
            const string materialTwoTitle = "Clean Code: A Handbook of Agile Software Craftsmanship";
            const string materialThreeTitle = "Elon Musk: Tesla, SpaceX, and the Quest for a Fantastic Future";

            var insertedAuthor = new Author
            {
                FirstName = "Nikola",
                LastName = "Velichkov",
            };

            var insertedAuthorTwo = new Author
            {
                FirstName = "Gergana",
                LastName = "Petkova",
            };

            var insertedMaterials = new List<Material>
            {
                new Material
                {
                    Isbn = "9780135957059",
                    Title = materialOneTitle,
                    Language = "English",
                    Lendable = true,
                    Description = "Some description here",
                },
                new Material
                {
                    Isbn = "9780132350884",
                    Title = materialTwoTitle,
                    Language = "English",
                    Lendable = true,
                    Description = "Some description here",
                },
                new Material
                {
                    Isbn = "9780062301253",
                    Title = materialThreeTitle,
                    Language = "English",
                    Lendable = true,
                    Description = "Some description here",
                },
            };

            // add authors to materials
            insertedMaterials.ForEach(material =>
            {
                material.MaterialAuthors = new List<MaterialAuthor>
                {
                    new MaterialAuthor
                    {
                        Author = insertedAuthor,
                        Material = material,
                    },
                    new MaterialAuthor
                    {
                        Author = insertedAuthorTwo,
                        Material = material,
                    }
                };
            });

            using (var context = new GTLContext(options))
            {
                context.AddRange(insertedMaterials);
                context.SaveChanges();
            }

            using (var context = new GTLContext(options))
            {
                var authorController = ControllerFactory.CreateAuthorController(context);
                var author = context.Authors.FirstOrDefault(a => a.FirstName == insertedAuthor.FirstName);
                Assert.IsNotNull(author);

                var materials = authorController.FindMaterials(author);

                // assert materials
                Assert.Multiple(() =>
                {
                    Assert.IsNotNull(materials);
                    Assert.AreEqual(3, materials.Count);
                    Assert.IsTrue(materials.Any(m => m.Title == materialOneTitle));
                    Assert.IsTrue(materials.Any(m => m.Title == materialTwoTitle));
                    Assert.IsTrue(materials.Any(m => m.Title == materialThreeTitle));
                });

                // assert material authors
                materials.ForEach(material =>
                {
                    Assert.Multiple(() =>
                    {
                        Assert.AreEqual(2, material.MaterialAuthors.Count);
                        Assert.IsNotNull(material.MaterialAuthors.FirstOrDefault(ma =>
                            ma.Author.FirstName == insertedAuthor.FirstName && ma.Author.LastName == insertedAuthor.LastName));
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
            const string materialOneTitle = "Pragmatic Programmer special 2nd";
            const string materialTwoTitle = "Clean Code: A Handbook of Agile Software Craftsmanship";
            const string materialThreeTitle = "Elon Musk: Tesla, SpaceX, and the Quest for a Fantastic Future";

            var insertedAuthor = new Author
            {
                FirstName = "Nikola",
                LastName = "Velichkov",
            };

            var insertedAuthorTwo = new Author
            {
                FirstName = "Gergana",
                LastName = "Petkova",
            };

            var insertedMaterials = new List<Material>
            {
                new Material
                {
                    Isbn = "9780135957059",
                    Title = materialOneTitle,
                    Language = "English",
                    Lendable = true,
                    Description = "Some description here",
                },
                new Material
                {
                    Isbn = "9780132350884",
                    Title = materialTwoTitle,
                    Language = "English",
                    Lendable = true,
                    Description = "Some description here",
                },
                new Material
                {
                    Isbn = "9780062301253",
                    Title = materialThreeTitle,
                    Language = "English",
                    Lendable = true,
                    Description = "Some description here",
                },
            };

            // add authors to materials
            insertedMaterials.ForEach(material =>
            {
                material.MaterialAuthors = new List<MaterialAuthor>
                {
                    new MaterialAuthor
                    {
                        Author = insertedAuthorTwo,
                        Material = material,
                    }
                };
            });

            using (var context = new GTLContext(options))
            {
                context.Add(insertedAuthor);
                context.AddRange(insertedMaterials);
                context.SaveChanges();
            }

            using (var context = new GTLContext(options))
            {
                var authorController = ControllerFactory.CreateAuthorController(context);
                var author = context.Authors.FirstOrDefault(a => a.FirstName == insertedAuthor.FirstName);
                Assert.IsNotNull(author);

                var materials = authorController.FindMaterials(author);

                // assert materials
                Assert.IsNotNull(materials);
                Assert.AreEqual(0, materials.Count);
            }
        }
    }
}
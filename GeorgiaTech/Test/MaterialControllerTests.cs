using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Server;
using Server.Controllers;
using Server.Models;

namespace Test
{
    [TestFixture]
    public class MaterialControllerTests
    {
        private MaterialType _materialType;
        private string _isbn;
        private string _title;
        private string _language;
        private bool _lendable;
        private string _description;

        [SetUp]
        public void Setup()
        {
            _materialType = new MaterialType {Type = "book"};
            _isbn = "978-0-201-61622-4";
            _title = "The Pragmatic Programmer: From Journeyman to Master";
            _language =  "English";
            _lendable = true;
            _description = "Some description in here";
        }

        [Test]
        public void InsertInsertsCorrectly()
        {
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name)
                .Options;

            // action
            int materialId;
            using (var context = new GTLContext(options))
            {
                var controller = ControllerFactory.CreateMaterialController(context);
                var material = new Material
                {
                    Isbn = _isbn,
                    Title = _title,
                    Language = _language,
                    Lendable = _lendable,
                    Description = _description,
                    Type = _materialType,
                };
                controller.Insert(material);
                materialId = material.MaterialId;
            }

            // assertion
            using (var context = new GTLContext(options))
            {
                var material = context.Materials.Find(materialId);
                Assert.That(material, Has
                    .Property(nameof(Material.Isbn)).EqualTo(_isbn).And
                    .Property(nameof(Material.Title)).EqualTo(_title).And
                    .Property(nameof(Material.Language)).EqualTo(_language).And
                    .Property(nameof(Material.Lendable)).EqualTo(_lendable).And
                    .Property(nameof(Material.Description)).EqualTo(_description).And
                    .Property(nameof(Material.Type)));
            }
        }

        [Test]
        public void InsertThrowsArgumentNullExceptionWithNullParameter()
        {
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name)
                .Options;

            // action
            using var context = new GTLContext(options);
            var controller = ControllerFactory.CreateMaterialController(context);

            // assertion
            Assert.Throws<ArgumentNullException>(() => controller.Insert(null));
        }

        [Test]
        public void InsertDoesNotInsertWithNullParameter()
        {
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name)
                .Options;

            // action
            using (var context = new GTLContext(options))
            {
                var controller = ControllerFactory.CreateMaterialController(context);

                // silently catch the error that's supposed to be thrown by the Insert method
                // in order to test the after effect
                try { controller.Insert(null); }
                catch (ArgumentNullException) { }
            }

            // assertion
            using (var context = new GTLContext(options))
            {
                Assert.That(context.Materials.Count(), Is.EqualTo(0));
            }
        }

        [Test]
        public void FindByIdFindsAnExistingMaterial()
        {
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name)
                .Options;

            // action
            int materialId;
            using (var context = new GTLContext(options))
            {
                var material = new Material
                {
                    Isbn = _isbn,
                    Title = _title,
                    Language = _language,
                    Lendable = _lendable,
                    Description = _description,
                    Type = _materialType,
                };
                context.Add(material);
                context.SaveChanges();
                materialId = material.MaterialId;
            }

            // assertion
            using (var context = new GTLContext(options))
            {
                var controller = ControllerFactory.CreateMaterialController(context);
                var material = controller.FindByID(materialId);

                Assert.That(material, Has
                    .Property(nameof(Material.Isbn)).EqualTo(_isbn).And
                    .Property(nameof(Material.Title)).EqualTo(_title).And
                    .Property(nameof(Material.Language)).EqualTo(_language).And
                    .Property(nameof(Material.Lendable)).EqualTo(_lendable).And
                    .Property(nameof(Material.Description)).EqualTo(_description).And
                    .Property(nameof(Material.Type)));
            }
        }

        [Test]
        public void FindByIdReturnsNullWhenMaterialNotFound()
        {
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name)
                .Options;

            // action
            using var context = new GTLContext(options);
            var controller = ControllerFactory.CreateMaterialController(context);
            var material = controller.FindByID(42);

            Assert.That(material, Is.Null);
        }

        [Test]
        public void DeleteDeletesCorrectly()
        {
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name)
                .Options;

            int materialId;
            using (var context = new GTLContext(options))
            {
                var material = new Material
                {
                    Isbn = _isbn,
                    Title = _title,
                    Language = _language,
                    Lendable = _lendable,
                    Description = _description,
                    Type = _materialType,
                };

                context.Add(material);
                context.SaveChanges();
                materialId = material.MaterialId;
            }

            // action
            using (var context = new GTLContext(options))
            {
                var controller = ControllerFactory.CreateMaterialController(context);
                var material = context.Materials.Find(materialId);
                controller.Delete(material);
            }

            // assertion
            using (var context = new GTLContext(options))
            {
                var material = context.Materials.Find(materialId);
                Assert.That(material, Is.Null);
            }
        }

        [Test]
        public void UpdateUpdatesCorrectly()
        {
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name)
                .Options;

            int materialId;
            var newTitle = "Some other title";
            var newLanguage = "Danish";
            using (var context = new GTLContext(options))
            {
                var material = new Material
                {
                    Isbn = _isbn,
                    Title = _title,
                    Language = _language,
                    Lendable = _lendable,
                    Description = _description,
                    Type = _materialType,
                };

                context.Add(material);
                context.SaveChanges();
                materialId = material.MaterialId;
            }

            // action
            using (var context = new GTLContext(options))
            {
                var controller = ControllerFactory.CreateMaterialController(context);
                var material = context.Materials.Find(materialId);
                material.Title = newTitle;
                material.Language = newLanguage;

                controller.Update(material);
            }

            // assertion
            using (var context = new GTLContext(options))
            {
                var material = context.Materials.Find(materialId);
                Assert.That(material, Has
                    .Property(nameof(Material.Isbn)).EqualTo(_isbn).And
                    .Property(nameof(Material.Title)).EqualTo(newTitle).And
                    .Property(nameof(Material.Language)).EqualTo(newLanguage).And
                    .Property(nameof(Material.Lendable)).EqualTo(_lendable).And
                    .Property(nameof(Material.Description)).EqualTo(_description).And
                    .Property(nameof(Material.Type))
                );
            }
        }

        [Test]
        public void FindAllReturnsAPopulatedList()
        {
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name)
                .Options;

            using (var context = new GTLContext(options))
            {
                var material = new Material
                {
                    Isbn = _isbn,
                    Title = _title,
                    Description = _description,
                    Lendable = _lendable,
                    Language = _language,
                    Type = _materialType,
                };

                context.Materials.Add(material);
                context.SaveChanges();
            }

            // action
            using (var context = new GTLContext(options))
            {
                var controller = ControllerFactory.CreateMaterialController(context);
                var materialList = controller.FindAll();

                Assert.That(materialList, Is.Not.Empty);
            }
        }

        [Test]
        public void FindAllReturnsAnEmptyList()
        {
            // setup
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name)
                .Options;

            // don't add any materials
            using var context = new GTLContext(options);
            var controller = ControllerFactory.CreateMaterialController(context);
            var materialList = controller.FindAll();

            Assert.That(materialList, Is.Empty);
        }

        [Test]
        public void CreateCreatesCorrectly()
        {
            // setup
            var bookType = new MaterialType {Type = "Book"};
            var programmingSubject = new MaterialSubject {SubjectName = "Programming"};
            var author = new Author {FirstName = "Nikola", LastName = "Velichkov"};
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name)
                .Options;

            using (var context = new GTLContext(options))
            {
                context.Add(bookType);
                context.Add(programmingSubject);
                context.Add(author);

                context.SaveChanges();
            }

            // action
            using (var context = new GTLContext(options))
            {
                var controller = ControllerFactory.CreateMaterialController(context);
                var material = controller.Create(
                    _isbn,
                    _title,
                    _language,
                    _lendable,
                    _description,
                    bookType,
                    new List<MaterialSubject> {programmingSubject},
                    new List<Author> {author}
                );
            // assertion

            Assert.That(material, Has
                .Property(nameof(Material.Isbn)).EqualTo(_isbn).And
                .Property(nameof(Material.Title)).EqualTo(_title).And
                .Property(nameof(Material.Language)).EqualTo(_language).And
                .Property(nameof(Material.Lendable)).EqualTo(_lendable).And
                .Property(nameof(Material.Description)).EqualTo(_description).And
                .Property(nameof(Material.Type)).And
                .Property(nameof(Material.MaterialSubjects)).And
                .Property(nameof(Material.MaterialAuthors)));
            }
        }

        [Test]
        // should be more tests with the same conditions for different properties of Material
        // (Isbn, Title, Language, Description, Lendable?)
        public void CreateThrowsAnArgumentNullExceptionOnNullOrEmptyTitle()
        {
            // setup
            var bookType = new MaterialType {Type = "Book"};
            var programmingSubject = new MaterialSubject {SubjectName = "Programming"};
            var author = new Author {FirstName = "Nikola", LastName = "Velichkov"};
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name)
                .Options;

            using (var context = new GTLContext(options))
            {
                context.Add(bookType);
                context.Add(programmingSubject);
                context.Add(author);

                context.SaveChanges();
            }

            // assertion
            using (var context = new GTLContext(options))
            {
                var controller = ControllerFactory.CreateMaterialController(context);
                Assert.Throws<ArgumentNullException>(() =>
                    controller.Create(
                        _isbn,
                        "",
                        _language,
                        _lendable,
                        _description,
                        bookType,
                        new List<MaterialSubject> {programmingSubject},
                        new List<Author> {author}
                    ));
            }
        }

        [Test]
        public void CreateThrowsAnArgumentExceptionOnNonExistingMaterialType()
        {
            // setup
            var bookType = new MaterialType {Type = "Book"};
            var programmingSubject = new MaterialSubject {SubjectName = "Programming"};
            var author = new Author {FirstName = "Nikola", LastName = "Velichkov"};
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name)
                .Options;

            using (var context = new GTLContext(options))
            {
                context.Add(programmingSubject);
                context.Add(author);

                context.SaveChanges();
            }

            using (var context = new GTLContext(options))
            {
                var controller = ControllerFactory.CreateMaterialController(context);
                Assert.Throws<ArgumentException>(() =>
                    controller.Create(
                        _isbn,
                        _title,
                        _language,
                        _lendable,
                        _description,
                        bookType,
                        new List<MaterialSubject> {programmingSubject},
                        new List<Author> {author}
                    ));
            }
        }

        [Test]
        public void CreateThrowsAnArgumentExceptionOnNonExistingMaterialSubject()
        {
            // setup
            var bookType = new MaterialType {Type = "Book"};
            var programmingSubject = new MaterialSubject {SubjectName = "Programming"};
            var author = new Author {FirstName = "Nikola", LastName = "Velichkov"};
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name)
                .Options;

            using (var context = new GTLContext(options))
            {
                context.Add(bookType);
                context.Add(author);

                context.SaveChanges();
            }

            using (var context = new GTLContext(options))
            {
                var controller = ControllerFactory.CreateMaterialController(context);
                Assert.Throws<ArgumentException>(() =>
                    controller.Create(
                        _isbn,
                        _title,
                        _language,
                        _lendable,
                        _description,
                        bookType,
                        new List<MaterialSubject> {programmingSubject},
                        new List<Author> {author}
                    ));
            }
        }

        [Test]
        public void CreateThrowsAnArgumentExceptionOnNonExistingAuthor()
        {
            // setup
            var bookType = new MaterialType {Type = "Book"};
            var programmingSubject = new MaterialSubject {SubjectName = "Programming"};
            var author = new Author {FirstName = "Nikola", LastName = "Velichkov"};
            var options = new DbContextOptionsBuilder<GTLContext>()
                .UseInMemoryDatabase(MethodBase.GetCurrentMethod().Name)
                .Options;

            using (var context = new GTLContext(options))
            {
                context.Add(bookType);
                context.Add(programmingSubject);

                context.SaveChanges();
            }

            using (var context = new GTLContext(options))
            {
                var controller = ControllerFactory.CreateMaterialController(context);
                Assert.Throws<ArgumentException>(() =>
                    controller.Create(
                        _isbn,
                        _title,
                        _language,
                        _lendable,
                        _description,
                        bookType,
                        new List<MaterialSubject> {programmingSubject},
                        new List<Author> {author}
                    ));
            }
        }
    }
}
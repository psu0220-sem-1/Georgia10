using System;
using System.Collections.Generic;
using System.Linq;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Server;
using Server.Controllers;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/material")]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialController _controller;

        public MaterialController(GTLContext context)
        {
            _controller = ControllerFactory.CreateMaterialController(context);
        }

        private Material BuildMaterial(Server.Models.Material material)
        {
            var authors = material.MaterialAuthors
                .Select(ma => new Author
                {
                    AuthorId = ma.Author.AuthorId,
                    FirstName = ma.Author.FirstName,
                    LastName = ma.Author.LastName,
                })
                .ToList();

            var subjects = material.MaterialSubjects
                .Select(ms => new MaterialSubject
                {
                    SubjectId = ms.MaterialSubject.SubjectId,
                    Subject = ms.MaterialSubject.SubjectName,
                })
                .ToList();

            return new Material
            {
                MaterialId = material.MaterialId,
                Isbn = material.Isbn,
                Title = material.Title,
                Language =  material.Language,
                Description = material.Description,
                Type = material.Type,
                Authors = authors,
                MaterialSubjects = subjects,
            };
        }

        [HttpGet]
        public IEnumerable<Material> GetAll()
        {
            var materials = _controller.FindAll();
            return materials.Select(BuildMaterial).ToList();
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var material = _controller.FindByID(id);
            if (material == null)
                return new NotFoundResult();

            return new JsonResult(BuildMaterial(material));
        }

        [HttpPost]
        public IActionResult CreateMaterial([FromBody] Material materialData)
        {
            // build server data
            var materialType = new
                Server.Models.MaterialType { TypeId = materialData.Type.TypeId, Type = materialData.Type.Type };

            var materialSubjects = materialData.MaterialSubjects
                .Select(subject => new Server.Models.MaterialSubject
                    { SubjectId = subject.SubjectId, SubjectName = subject.Subject })
                .ToList();

            var authors = materialData.Authors
                .Select(author => new Server.Models.Author
                    {AuthorId = author.AuthorId, FirstName = author.FirstName, LastName = author.LastName})
                .ToList();

            // create & insert
            try
            {
                var material = _controller.Create(
                    materialData.Isbn,
                    materialData.Title,
                    materialData.Language,
                    materialData.Lendable,
                    materialData.Description,
                    materialType,
                    materialSubjects,
                    authors
                );

                _controller.Insert(material);
                return new JsonResult(BuildMaterial(material));
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateMaterial(int id, [FromBody] Material newMaterial)
        {
            var material = _controller.FindByID(id);
            if (material == null)
                return new NotFoundResult();


            // build server data
            var materialType = new Server.Models.MaterialType
                { TypeId = newMaterial.Type.TypeId, Type = newMaterial.Type.Type };

            var materialSubjects = newMaterial.MaterialSubjects
                .Select(subject => new Server.Models.MaterialSubjects
                    {
                        MaterialSubject = new Server.Models.MaterialSubject
                            {SubjectName = subject.Subject, SubjectId = subject.SubjectId},
                        Material = material
                    })
                .ToList();

            var materialAuthors = newMaterial.Authors
                .Select(author => new Server.Models.MaterialAuthor
                {
                    Material = material,
                    Author = new Server.Models.Author
                        {FirstName = author.FirstName, LastName = author.LastName}
                })
                .ToList();

            var newServerMaterial = new Server.Models.Material
            {
                Isbn = newMaterial.Isbn,
                Title = newMaterial.Title,
                Language = newMaterial.Language,
                Description = newMaterial.Description,
                Lendable = newMaterial.Lendable,
                Type = materialType,
                MaterialAuthors = materialAuthors,
                MaterialSubjects = materialSubjects,
            };

            try
            {
                var changedRows = _controller.Update(id, newServerMaterial);
                // TODO: Add if statement that checks for the right amount of changedRows
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            material = _controller.FindByID(id);
            return new JsonResult(BuildMaterial(material));
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteById(int id)
        {
            var material = _controller.FindByID(id);
            if (material == null)
                return new NotFoundResult();

            var changedRows = _controller.Delete(material);
            // TODO: Add if statement that checks for the right amount of changedRows
            return new OkResult();
        }
    }
}
using System;
using System.Linq;
using Api.Models;
using Castle.Core.Internal;
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
        private readonly GTLContext _context;

        public MaterialController(GTLContext context)
        {
            _context = context;
            _controller = ControllerFactory.CreateMaterialController(context);
        }

        private Api.Models.Material BuildMaterial(Server.Models.Material material)
        {
            var authors = material.MaterialAuthors
                .Select(ma => new Api.Models.Author
                {
                    AuthorId = ma.Author.AuthorId,
                    FirstName = ma.Author.FirstName,
                    LastName = ma.Author.LastName,
                })
                .ToList();

            var subjects = material.MaterialSubjects
                .Select(ms => new Api.Models.MaterialSubject
                {
                    SubjectId = ms.MaterialSubject.SubjectId,
                    Subject = ms.MaterialSubject.SubjectName,
                })
                .ToList();

            return new Api.Models.Material
            {
                MaterialId = material.MaterialId,
                Isbn = material.Isbn,
                Language =  material.Language,
                Description = material.Description,
                Type = material.Type,
                Authors = authors,
                MaterialSubjects = subjects,
            };
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var materials = _controller.FindAll();

            return new JsonResult(materials.Select(BuildMaterial).ToList());
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
                    {FirstName = author.FirstName, LastName = author.LastName})
                .ToList();

            // create & insert
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

        [HttpPut("{id:int}")]
        public IActionResult UpdateMaterial(int id, [FromBody] Material newMaterial)
        {
            throw new NotImplementedException();
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
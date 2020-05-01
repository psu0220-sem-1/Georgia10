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
            // validate string data
            if (materialData.Isbn.IsNullOrEmpty() ||
                materialData.Title.IsNullOrEmpty() ||
                materialData.Language.IsNullOrEmpty() ||
                materialData.Description.IsNullOrEmpty())
            {
                return new BadRequestResult();
            }

            // validate MaterialType
            var materialType = _controller.FindMaterialTypeById(materialData.Type.TypeId);
            if (materialType == null)
                return new BadRequestResult();

            // validate Authors
            var authorController = ControllerFactory.CreateAuthorController(_context);
            var authors = materialData.Authors
                .Select(author => authorController.FindByID(author.AuthorId))
                .ToList();
            if (authors.Any(author => author == null))
                return new BadRequestResult();

            // validate Subjects
            var subjects = materialData.MaterialSubjects
                .Select(subject => _controller.FindMaterialSubjectById(subject.SubjectId))
                .ToList();
            if (subjects.Any(subject => subject == null))
                return new BadRequestResult();

            // create & insert
            var material = _controller.Create(
                materialData.Isbn,
                materialData.Title,
                materialData.Language,
                materialData.Lendable,
                materialData.Description,
                materialType,
                subjects,
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
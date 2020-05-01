using System;
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
        public IActionResult CreateMaterial([FromBody] Material material)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateMaterial(int id, [FromBody] Material newMaterial)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
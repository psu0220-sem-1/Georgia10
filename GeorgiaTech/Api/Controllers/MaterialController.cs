using System;
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

        [HttpGet]
        public IActionResult GetAll()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            throw new NotImplementedException();
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
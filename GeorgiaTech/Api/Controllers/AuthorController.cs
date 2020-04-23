using System;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;
using Server;
using Server.Controllers;
using Server.Models;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/author")]
    public class AuthorController: ControllerBase
    {
        private readonly IAuthorController _controller;

        public AuthorController(GTLContext context)
        {
            _controller = ControllerFactory.CreateAuthorController(context);
        }

        [HttpGet]
        public IActionResult GetAuthors()
        {
            var authors = _controller.FindAll();
            return new JsonResult(authors);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetAuthor(int id)
        {
            var author = _controller.FindByID(id);

            if (author == null)
                return new NotFoundResult();

            return new JsonResult(author);
        }

        [HttpPost]
        public IActionResult CreateAuthor([FromBody] Author authorData)
        {
            if (authorData.FirstName.IsNullOrEmpty() || authorData.LastName.IsNullOrEmpty())
                return new BadRequestResult();

            var author = _controller.Create(authorData.FirstName, authorData.LastName);
            _controller.Insert(author);

            var uri = new Uri($"http://{Request.Host}/api/author/{author.AuthorId}");

            return new CreatedResult(uri, author);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateAuthor(int id, [FromBody] Author authorData)
        {
            var author = _controller.FindByID(id);
            if (author == null)
                return new NotFoundResult();

            if (!authorData.FirstName.IsNullOrEmpty())
                author.FirstName = authorData.FirstName;

            if (!authorData.LastName.IsNullOrEmpty())
                author.LastName = authorData.LastName;

            var rowsChanged = _controller.Update(author);
            if (rowsChanged != 1)
            {
                // log error, maybe even stop the program as it's in an inconsistent state?
                return new StatusCodeResult(500);
            }

            return new JsonResult(author);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteAuthor(int id)
        {
            var author = _controller.FindByID(id);
            if (author == null)
                return new NotFoundResult();

            var rowsChanged = _controller.Delete(author);
            if (rowsChanged != 1)
            {
                // log error, maybe even stop the program as it's in an inconsistent state?
                return new StatusCodeResult(500);
            }

            return new OkResult();
        }
    }
}
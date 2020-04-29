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

        /// <summary>
        /// Returns all authors saved on the system
        /// </summary>
        /// <returns>A JSON representation of the authors</returns>
        [HttpGet]
        public IActionResult GetAuthors()
        {
            var authors = _controller.FindAll();
            return new JsonResult(authors);
        }

        /// <summary>
        /// Returns a single author identified by its ID
        /// </summary>
        /// <param name="id">The author's ID</param>
        /// <returns>JSON representation of the author</returns>
        [HttpGet("{id:int}")]
        public IActionResult GetAuthor(int id)
        {
            var author = _controller.FindByID(id);

            if (author == null)
                return new NotFoundResult();

            return new JsonResult(author);
        }

        /// <summary>
        /// Creates and saves an author
        /// </summary>
        /// <param name="authorData">Author's data</param>
        /// <returns>A JSON representation of the created author</returns>
        [HttpPost]
        public IActionResult CreateAuthor([FromBody] Author authorData)
        {
            if (authorData.FirstName.IsNullOrEmpty() || authorData.LastName.IsNullOrEmpty())
                return new BadRequestResult();

            var author = _controller.Create(authorData.FirstName, authorData.LastName);
            var rowsChanged = _controller.Insert(author);
            if (rowsChanged != 1)
            {
                // log error, maybe even stop the program as it's in an inconsistent state?
                return new StatusCodeResult(500);
            }

            var uri = new Uri($"http://{Request.Host}/api/author/{author.AuthorId}");
            return new CreatedResult(uri, author);
        }

        /// <summary>
        /// Updates an author
        /// </summary>
        /// <param name="id">The author's current ID</param>
        /// <param name="authorData">The author's new data. No fields are required. Skipped fields won't be updated</param>
        /// <returns>A JSON representation of the updated author</returns>
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

        /// <summary>
        /// Deletes an author
        /// </summary>
        /// <param name="id">The author's ID</param>
        /// <returns>200 OK if deletion was successful or 404 if author doesn't exist</returns>
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

        /// <summary>
        /// Fetches a list of all materials by a specific author
        /// </summary>
        /// <param name="authorId">The author's ID</param>
        /// <returns>A JSON representation of a list of all materials by that author</returns>
        [HttpGet("{authorId:int}/materials")]
        public IActionResult GetAuthorMaterials(int authorId)
        {
            var author = _controller.FindByID(authorId);
            if (author == null)
                return new NotFoundResult();

            var materials = _controller.FindMaterials(author);
            return new JsonResult(materials);
        }
    }
}
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Server;
using Server.Controllers;
using Server.Models;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController: ControllerBase
    {
        private readonly IAuthorController _controller;

        public AuthorController(GTLContext context)
        {
            _controller = new Server.Controllers.AuthorController(context);
        }

        [HttpPost]
        public Author Post([FromBody] JsonElement content)
        {
            var firstName = content.GetProperty("firstName").GetString();
            var lastName = content.GetProperty("lastName").GetString();


            var author = _controller.Create(firstName, lastName);
            _controller.Insert(author);

            return author;
        }
    }
}
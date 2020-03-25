using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Api.Models;
using Server.Controllers;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("[controller]")]
    public class VolumeController : Controller
    {
        private readonly Server.Controllers.VolumeController volumeController;
        public VolumeController(GTLContext context)
        {
            volumeController = new Server.Controllers.VolumeController(context);
        }
        // GET: api/values
        [HttpGet]
        public List<Models.Volume> Get()
        {
            var volumes = volumeController.FindAll();
            var models = volumes.Select(volume => BuildVolume(volume)).ToList();

            return models;

        }

        public Models.Volume BuildVolume(Server.Models.Volume volume)
        {

            var homeAddress = new Models.Address
            {
                AddressId = volume.HomeLocation.AddressId,
                Street = volume.HomeLocation.Street,
                AdditionalInfo = volume.HomeLocation.AdditionalInfo,
                Zip = volume.HomeLocation.Zip,
                City = volume.HomeLocation.ZipNavigation.City
            };

            var currentAddress = new Models.Address
            {
                AddressId = volume.CurrentLocation.AddressId,
                Street = volume.CurrentLocation.Street,
                AdditionalInfo = volume.CurrentLocation.AdditionalInfo,
                Zip = volume.CurrentLocation.Zip,
                City = volume.CurrentLocation.ZipNavigation.City
            };

            var volumeMaterial = new Models.Material
            {
                MaterialId = volume.MaterialId,
                Isbn = volume.Material.Isbn,
                Language = volume.Material.Language,
                Lendable = volume.Material.Lendable,
                Description = volume.Material.Description,
                Authors = volume.Material.MaterialAuthor.Select(e => new Models.Author
                {
                    AuthorId = e.AuthorId,
                    FirstName = e.Author.FirstName,
                    LastName = e.Author.LastName
                })
                .ToList(),
                MaterialSubjects = volume.Material.MaterialSubjectAssignment.Select(e => new Models.MaterialSubject
                {
                    SubjectId = e.SubjectId,
                    Subject = e.Subject.Subject
                }).ToList()
            };
            var modelVolume = new Models.Volume
            {
                VolumeId = volume.VolumeId,
                HomeLocation = homeAddress,
                CurrentLocation = currentAddress,
                Material = volumeMaterial
            };
            return modelVolume;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server;
using Api.Models;
using Server.Controllers;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("[controller]")]
    public class VolumeController : Controller
    {
        private readonly IVolumeController volumeController;
        public VolumeController(GTLContext context)
        {
            volumeController = ControllerFactory.CreateVolumeController(context);
        }
        // GET: /volume
        [HttpGet]
        public IActionResult Get(int materialId)
        {
            Console.WriteLine(materialId);
            try
            {
                List<Server.Models.Volume> volumes; ;
                if (materialId > 0)
                {
                    volumes = volumeController.FindVolumesForMaterial(materialId);

                } else
                {
                    volumes = volumeController.FindAll();
                }
                var volumeModels = volumes.Select(volume => BuildVolume(volume)).ToList();
                return Ok(volumeModels);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
          
        }

        public Models.Volume BuildVolume(Server.Models.Volume volume)
        {
            var modelVolume = new Volume();
            modelVolume.VolumeId = volume.VolumeId;

            if (volume.HomeLocation != null)
            {
                var homeAddress = new Models.Address
                {
                    AddressId = volume.HomeLocation.AddressId,
                    Street = volume.HomeLocation.Street,
                    AdditionalInfo = volume.HomeLocation.AdditionalInfo,
                    Zip = volume.HomeLocation.ZipCode,
                    City = volume.HomeLocation.Zip.City
                };
                modelVolume.HomeLocation = homeAddress;
                modelVolume.HomeLocationId = volume.HomeLocationId;
            }
       
            if (volume.CurrentLocation != null)
            {
                var currentAddress = new Models.Address
                {
                    AddressId = volume.CurrentLocation.AddressId,
                    Street = volume.CurrentLocation.Street,
                    AdditionalInfo = volume.CurrentLocation.AdditionalInfo,
                    Zip = volume.CurrentLocation.ZipCode,
                    City = volume.CurrentLocation.Zip.City
                };
                modelVolume.CurrentLocation = currentAddress;
                modelVolume.CurrentLocationId = volume.CurrentLocationId;
            }
         
            if (volume.Material != null)
            {
                var volumeMaterial = new Models.Material
                {
                    MaterialId = volume.MaterialId,
                    Isbn = volume.Material.Isbn,
                    Language = volume.Material.Language,
                    Lendable = volume.Material.Lendable,
                    Description = volume.Material.Description,
                   
                };
                if (volume.Material.MaterialAuthors != null)
                {
                    var authors = volume.Material.MaterialAuthors.Select(e => new Models.Author
                    {
                        AuthorId = e.Author.AuthorId,
                        FirstName = e.Author.FirstName,
                        LastName = e.Author.LastName
                    }).ToList();
                    volumeMaterial.Authors = authors;
          
                }
                if (volume.Material.MaterialSubjects != null)
                {
                    var materialSubjects = volume.Material.MaterialSubjects.Select(e => new Models.MaterialSubject
                    {
                        SubjectId = e.MaterialSubject.SubjectId,
                        Subject = e.MaterialSubject.SubjectName
                    }).ToList();
                    volumeMaterial.MaterialSubjects = materialSubjects;
                }
                modelVolume.Material = volumeMaterial;
                modelVolume.MaterialId = volume.MaterialId;
              
            }
         
         
            return modelVolume;
        }

        // GET /volume/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var volume = volumeController.FindByID(id);
                var volumeModel = BuildVolume(volume);
                return Ok(volumeModel);
            }
            catch (Exception ex)
            {
               return NotFound(ex.Message);
            }
        
        }

        // POST /volume
        [HttpPost]
        public IActionResult Post([FromBody] Volume volume)
        {
            try
            {
                var newVolume = volumeController.Create(materialId: volume.MaterialId, currentLocationId: volume.CurrentLocationId, homeLocationId: volume.HomeLocationId);
                volumeController.Insert(newVolume);
                var modelVolume = BuildVolume(newVolume);
                return Ok(modelVolume);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
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

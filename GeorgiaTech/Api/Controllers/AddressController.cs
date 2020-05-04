using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Server.Controllers;
using Server;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/address")]
    public class AddressController : Controller
    {
        private readonly IAddressController addressController;

        public AddressController(GTLContext context)
        {
            addressController = ControllerFactory.CreateAddressController(context);
        }

        // GET: /address
        [HttpGet]
        public IEnumerable<Api.Models.Address> GetAddresses()
        {
            var addresses = addressController.FindAll();
            return addresses.Select(a => BuildAddress(a));
        }

        // GET: /address/:id
        [HttpGet("{id}")]
        public IActionResult GetAddress(int id)
        {
            var address = addressController.FindByID(id);
            if (address == null)
            {
                return NotFound();
            }
            return Json(BuildAddress(address));
        }

        // POST: /address
        [HttpPost]
        public IActionResult CreateAddress([FromBody] Api.Models.Address addressData)
        {
            try
            {
                var address = addressController.Create(street: addressData.Street, additionalInfo: addressData.AdditionalInfo, zip: addressData.Zip);
                var rowsChanged = addressController.Insert(address);

                if (rowsChanged != 1)
                    return StatusCode(500);

                return Json(BuildAddress(address));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: /address/:id
        [HttpDelete("{id}")]
        public IActionResult DeleteAddress(int id)
        {
            var address = addressController.FindByID(id);
            if (address == null)
                return NotFound();

            var rowsChanged = addressController.Delete(address);
            if (rowsChanged != 1)
                return StatusCode(500);

            return Ok();
        }

        private Api.Models.Address BuildAddress(Server.Models.Address addressEntity)
        {
            return new Models.Address
            {
                AddressId = addressEntity.AddressId,
                Street = addressEntity.Street,
                AdditionalInfo = addressEntity.AdditionalInfo,
                Zip = addressEntity.ZipCode,
                City = addressEntity.Zip.City
            };
        }
    }
}

using System;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetAddresses()
        {
            var addresses = addressController.FindAll();
            return Json(addresses);
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
            return Json(address);
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

                return Json(address);

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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Controllers;
using Server;
using Server.Models;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpPost]
        public IActionResult CreateAddress([FromBody] Api.Models.Address addressData)
        {
            try
            {
                var address = addressController.Create(street: addressData.Street, additionalInfo: addressData.AdditionalInfo, zip: addressData.Zip);
                var rowsChanged = addressController.Insert(address);
                return Json(address);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAddress(int id)
        {
            var address = addressController.FindByID(id);
            if (address == null)
                return NotFound();

            var rowsChanged = addressController.Delete(address);
            if (rowsChanged != 1)
            {
                return StatusCode(500);
            }

            return Ok("Address deleted");
        }

    }
}

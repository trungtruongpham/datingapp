using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Generic;
using DatingApp.API.Models;
using DatingApp.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/value/[action]")]
    public class ValueController : ControllerBase
    {
        private IValueService valueService;

        public ValueController(IValueService valueService)
        {
            this.valueService = valueService;
        }
        
        [HttpGet]
        public IEnumerable<Value> GetValues()
        {
            return valueService.GetAll();
        }

        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody] Value newValue)
        {
            if (ModelState.IsValid)
            {
                var result = await valueService.InsertAsync(newValue);

                if (result)
                    return Ok();
                else
                    return NotFound();
            }

            return NotFound();
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleManager.Dto.Filters;
using PeopleManager.Dto.Requests;
using PeopleManager.Services;
using Vives.Services.Model;

namespace PeopleManager.RestApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly PersonService _personService;

        public PeopleController(PersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> FindAsync([FromQuery]Paging paging, [FromQuery]PersonFilter filter)
        {
            var result = await _personService.FindAsync(paging, filter);
            return Ok(result);
        }

        [HttpGet("{id:int}", Name = "GetPersonRoute")]
        public async Task<IActionResult> GetAsync([FromRoute]int id)
        {
            var person = await _personService.GetAsync(id);
            
            return Ok(person);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]PersonRequest model)
        {
            var serviceResult = await _personService.CreateAsync(model);
            if (!serviceResult.IsSuccess || serviceResult.Result is null)
            {
                return Ok(serviceResult);
            }
            return CreatedAtRoute("GetPersonRoute", new {id = serviceResult.Result.Id}, serviceResult);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditAsync([FromRoute]int id, [FromBody]PersonRequest model)
        {
            var serviceResult = await _personService.UpdateAsync(id, model);

            return Ok(serviceResult);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]int id)
        {
            var serviceResult = await _personService.DeleteAsync(id);

            return Ok(serviceResult);
        }
    }
}

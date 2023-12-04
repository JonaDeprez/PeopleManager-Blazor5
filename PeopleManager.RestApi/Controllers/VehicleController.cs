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
    public class VehiclesController : ControllerBase
    {
        private readonly VehicleService _vehicleService;

        public VehiclesController(VehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<IActionResult> FindAsync([FromQuery]Paging paging, [FromQuery]VehicleFilter filter)
        {
            var result = await _vehicleService.FindAsync(paging, filter);
            return Ok(result);
        }

        [HttpGet("{id:int}", Name = "GetVehicleRoute")]
        public async Task<IActionResult> GetAsync([FromRoute]int id)
        {
            var vehicles = await _vehicleService.GetAsync(id);
            return Ok(vehicles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]VehicleRequest model)
        {
            var serviceResult = await _vehicleService.CreateAsync(model);
            if (!serviceResult.IsSuccess || serviceResult.Result is null)
            {
                return Ok(serviceResult);
            }
            return CreatedAtRoute("GetVehicleRoute", new {id = serviceResult.Result.Id}, serviceResult);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditAsync([FromRoute]int id, [FromBody]VehicleRequest model)
        {
            var serviceResult = await _vehicleService.UpdateAsync(id, model);
            return Ok(serviceResult);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]int id)
        {
            var serviceResult = await _vehicleService.DeleteAsync(id);

            return Ok(serviceResult);
        }
    }
}

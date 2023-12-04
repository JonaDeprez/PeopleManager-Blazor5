using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using PeopleManager.Dto.Filters;
using PeopleManager.Dto.Requests;
using PeopleManager.Sdk;
using Vives.Services.Model;

namespace PeopleManager.Ui.Mvc.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly VehicleApiService _vehicleApiService;
        private readonly PersonApiService _personApiService;

        public VehiclesController(VehicleApiService vehicleApiService, PersonApiService personApiService)
        {
            _vehicleApiService = vehicleApiService;
            _personApiService = personApiService;
        }


        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]Paging paging, [FromQuery]VehicleFilter filter)
        {
            paging.PageSize = 100;
            var result = await _vehicleApiService.Find(paging, filter);

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return await CreateEditView("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleRequest vehicle)
        {
            if (!ModelState.IsValid)
            {
                return await CreateEditView("Create", vehicle);
            }

            var serviceResult = await _vehicleApiService.Create(vehicle);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var vehicle = await _vehicleApiService.Get(id);

            if (vehicle is null)
            {
                return RedirectToAction("Index");
            }

            var vehicleRequest = new VehicleRequest
            {
                LicensePlate = vehicle.LicensePlate,
                Brand = vehicle.Brand,
                Type = vehicle.Type,
                ResponsiblePersonId = vehicle.ResponsiblePersonId
            };

            return await CreateEditView("Edit", vehicleRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id, [FromForm]VehicleRequest vehicle)
        {
            if (!ModelState.IsValid)
            {
                return await CreateEditView("Edit", vehicle);
            }

            await _vehicleApiService.Update(id, vehicle);

            return RedirectToAction("Index");
        }
        
        private async Task<IActionResult> CreateEditView([AspMvcView]string viewName, VehicleRequest? vehicle = null)
        {
            var people = await _personApiService.FindAsync();

            ViewBag.People = people;

            return View(viewName, vehicle);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _vehicleApiService.Get(id);

            if (vehicle is null)
            {
                return RedirectToAction("Index");
            }

            return View(vehicle);
        }

        [HttpPost("Vehicles/Delete/{id:int?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _vehicleApiService.Delete(id);

            return RedirectToAction("Index");
        }
    }
}

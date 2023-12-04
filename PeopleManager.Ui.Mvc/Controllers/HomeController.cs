using Microsoft.AspNetCore.Mvc;
using PeopleManager.Ui.Mvc.Models;
using System.Diagnostics;
using PeopleManager.Sdk;

namespace PeopleManager.Ui.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly PersonApiService _personApiService;

        public HomeController(PersonApiService personApiService)
        {
            _personApiService = personApiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var people = await _personApiService.FindAsync();
            return View(people.Data);
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var person = await _personApiService.GetAsync(id);

            if (person is null)
            {
                return RedirectToAction("Index");
            }

            return View("PersonDetail", person);
        }
    }
}
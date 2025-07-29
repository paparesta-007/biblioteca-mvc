using Biblioteca.Core.Models;
using Biblioteca.Data;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly PrestitiRepository _prestitirepo;
        public HomeController(IConfiguration configuration)
        {
            string? connStr = configuration.GetConnectionString("DefaultConnection");
            _prestitirepo = new PrestitiRepository(connStr);
        }
        public IActionResult Index()
        {
            ViewBag.Title = "Home Page";
            var prestiti = _prestitirepo.GetPrestitiDettagliati();

            return View(prestiti); 
        }
    }
}

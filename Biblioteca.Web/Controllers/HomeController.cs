using Biblioteca.Data;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly PrestitiRepository _repo;
        public HomeController(IConfiguration configuration)
        {
            string? connStr = configuration.GetConnectionString("DefaultConnection");
            _repo = new PrestitiRepository(connStr);
        }
        public IActionResult Index()
        {
            ViewBag.Title="Home Page";
            var prestiti = _repo.GetAll();
            if (prestiti.Count > 0)
            {
                return View(prestiti);
            }
            else
            {
                return View();
            }
        }
    }
}

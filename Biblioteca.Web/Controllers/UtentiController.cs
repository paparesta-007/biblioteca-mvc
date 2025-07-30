using Biblioteca.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Biblioteca.Data;
namespace Biblioteca.Web.Controllers;

public class UtentiController : Controller
{
    private readonly UtenteRepository _utenteRepository;
    public UtentiController(IConfiguration configuration)
    {
        string? connStr = configuration.GetConnectionString("DefaultConnection");
        _utenteRepository = new UtenteRepository(connStr);
    }
    public IActionResult Index()
    {
        ViewBag.Title = "Tutti gli utenti";
        var utenti = _utenteRepository.GetAll();
        if (utenti != null)
        {
            return View(utenti);
        }
        return View();
    }
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Title = "Crea un nuovo utente";
        return View();
    }

    [HttpPost]
    public IActionResult Create(Utenti utenti)
    {
        if (ModelState.IsValid)
        {
            _utenteRepository.Add(utenti);
            TempData["Message"] = "Utente creato correttamente";
            return RedirectToAction("Index");
        }
        return View(utenti);
    }
    
    [HttpPost]
    public IActionResult Delete(int id)
    {
        _utenteRepository.Delete(id);
        TempData["Message"] = "Utente eliminato correttamente";
        return Ok();
    }

}
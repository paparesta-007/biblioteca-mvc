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
    public IActionResult Create(Utente utente)
    {
        if (ModelState.IsValid)
        {
            _utenteRepository.Add(utente);
            TempData["Message"] = "Utente creato correttamente";
            return RedirectToAction("Index");
        }
        return View(utente);
    }
    
    [HttpPost]
    public IActionResult Delete(string allId)
    {
        var idList = allId
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(id => int.Parse(id)) 
            .ToList();

        foreach (var id in idList)
        {
            _utenteRepository.Delete(id);
        }
        TempData["Message"] = $"{idList.Count} utente/i eliminato/i correttamente.";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Edit(string idEdit)
    {
        ViewBag.Title = "Modifica un utente";
    
        if (!int.TryParse(idEdit, out int id))
        {
            return NotFound("ID non valido");
        }

        var utente = _utenteRepository.GetById(id);

        if (utente == null)
        {
            return NotFound("Utente non trovato");
        }

        return View(utente);
    }

    
    
    [HttpPost]
    public IActionResult Edit(Utente utente)
    {
        if (ModelState.IsValid)
        {
            _utenteRepository.Update(utente);
            TempData["Message"] = "Utente modificato correttamente";
            return RedirectToAction("Index");
        }
        return View(utente);
    }
}
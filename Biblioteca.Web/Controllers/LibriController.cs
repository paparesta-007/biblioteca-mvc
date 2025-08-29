using Biblioteca.Core.Models;
using Biblioteca.Data;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers;

public class LibriController : Controller
{
    private readonly LibroRepository _libroRepository;
    private readonly AutoreRepository _autoreRepository;
    private readonly PrestitiRepository _prestitiRepository;
    private readonly PrenotazioneRepository _prenotazioneRepository;
    private readonly NazioniRepository _nazioniRepository;
    private readonly LinguaRepository _linguaRepository;
    private readonly GeneriRepository _generiRepository;
    public LibriController(IConfiguration configuration)
    {
        string? connStr = configuration.GetConnectionString("DefaultConnection");
        _libroRepository = new LibroRepository(connStr);
        _autoreRepository = new AutoreRepository(connStr);
        _prestitiRepository = new PrestitiRepository(connStr);
        _prenotazioneRepository = new PrenotazioneRepository(connStr);
        _nazioniRepository = new NazioniRepository(connStr);
        _linguaRepository = new LinguaRepository(connStr);
        _generiRepository = new GeneriRepository(connStr);
    }
    public IActionResult Index()
    {
        ViewBag.Title = "Tutti i libri";
        var libri = _libroRepository.GetAll();
        var prestiti = _prestitiRepository.GetAll();
        var prenotazioni = _prenotazioneRepository.GetAll();
        if (libri != null)
        {
            foreach (var libro in libri)
            {
                libro.IsInPrestito = prestiti.Any(p => p.IDL == libro.IdLibro);
                libro.IsPrenotato = prenotazioni.Any(p =>p.IdLibro == libro.IdLibro);
            }
            return View(libri);
        }
        return View();
    }

    
    public IActionResult Delete(string allId)
    {
        var idList = allId
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(id => int.Parse(id)) 
            .ToList();

        foreach (var id in idList)
        {
            _prestitiRepository.Delete(id);
            _prenotazioneRepository.Delete(id);
            _libroRepository.Delete(id);
        }
        TempData["Message"] = $"{idList.Count} libro/i eliminato/i correttamente.";
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public IActionResult Edit(string idEdit)
    {
        ViewBag.Title = "Modifica un libro";
    
        if (!int.TryParse(idEdit, out int id))
        {
            return NotFound("ID non valido");
        }
        var libro = _libroRepository.GetById(id);
        var nazioni= _nazioniRepository.GetAll();
        var lingue = _linguaRepository.GetAll();
        var generi = _generiRepository.GetAll();
        var autori= _autoreRepository.GetAll();
        if (libro == null)
        {
            return NotFound("Libro non trovato");
        }
        return View((libro, nazioni,lingue,generi,autori));
    }

    [HttpPost]
    [HttpPost]
    public IActionResult Edit(Libro libro)
    {
        if (ModelState.IsValid)
        {
            _libroRepository.Update(libro);
            TempData["Message"] = "Libro modificato correttamente";
            return RedirectToAction("Index");
        }

        // Se non è valido, ricarico i dati per ripopolare la view
        var nazioni = _nazioniRepository.GetAll();
        var lingue = _linguaRepository.GetAll();
        var generi = _generiRepository.GetAll();
        var autori = _autoreRepository.GetAll();

        return View((libro, nazioni, lingue, generi, autori));
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Title = "Crea un nuovo libro";
        var nazioni = _nazioniRepository.GetAll();
        var lingue = _linguaRepository.GetAll();
        var generi = _generiRepository.GetAll();
        var autori = _autoreRepository.GetAll();

        return View(( nazioni, lingue, generi, autori));
        
    }
    
    [HttpPost]
    public IActionResult Create(Libro libro)
    {
        if (ModelState.IsValid)
        {
            _libroRepository.Add(libro);
            TempData["Message"] = "Libro creato correttamente";
            return RedirectToAction("Index");
        }
        // Se non è valido, ricarico i dati per ripopolare la view
        var nazioni = _nazioniRepository.GetAll();
        var lingue = _linguaRepository.GetAll();
        var generi = _generiRepository.GetAll();
        var autori = _autoreRepository.GetAll();

        return View((nazioni, lingue, generi, autori));
    }
}
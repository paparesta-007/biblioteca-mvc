using Biblioteca.Core.Models;
using Biblioteca.Data;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers;

public class GestioneScambiController : Controller
{
    private readonly LibroRepository _libroRepository;
    private readonly PrenotazioneRepository _prenotazioneRepository;
    private readonly PrestitiRepository _prestitiRepository;
    private readonly UtenteRepository _utenteRepository;

    public GestioneScambiController(IConfiguration configuration)
    {
        _prestitiRepository = new PrestitiRepository(configuration.GetConnectionString("DefaultConnection"));
        _prenotazioneRepository = new PrenotazioneRepository(configuration.GetConnectionString("DefaultConnection"));
        _libroRepository = new LibroRepository(configuration.GetConnectionString("DefaultConnection"));
        _utenteRepository = new UtenteRepository(configuration.GetConnectionString("DefaultConnection"));
    }

    public IActionResult Index()
    {
        var utenti = _utenteRepository.GetAll();
        var libri = _libroRepository.GetAll();
        var prestiti = _prestitiRepository.GetAll();
        var prenotazioni = _prenotazioneRepository.GetAll();
        return View();
    }

    [HttpGet]
    public IActionResult AggiungiPrestito()
    {
        var libri = _libroRepository.GetAll();
        var utenti = _utenteRepository.GetAll();
        return View((libri, utenti));
        ;
    }

    [HttpPost]
    public IActionResult AggiungiPrestito(int idLibro, int idUtente)
    {
        // Check if the book is already booked
        
        var prestito = _prestitiRepository.GetById(idLibro);
        if (prestito != null)
        {
            var prenotato = _prenotazioneRepository.GetById(idLibro);
            if (prenotato != null) return Content("Libro in prestito e già prenotato");

            var prenotazione = new Prenotazioni
            {
                IdLibro = idLibro,
                IdUtente = idUtente,
                DataPrenotazione = DateTime.Now
            };
            _prenotazioneRepository.Add(prenotazione);
            return Content("Libro aggiungi prenotato");
        }
        else
        {
            var prenotato = _prenotazioneRepository.GetById(idLibro);
            if (prenotato != null) return Content("Libro già prenotato");

            var totalePrestiti = _prestitiRepository.TotalePrestiti(idUtente);
            var totalePrenotazioni = _prenotazioneRepository.TotalePrenotazioni(idUtente);
            if (totalePrenotazioni + totalePrestiti >= 3) return Content("Limite di prenotazioni raggiunto");

            var newPrestito = new Prestiti
            {
                IDU = idUtente,
                IDL = idLibro,
                DataPrestito = DateTime.Now
            };
            _prestitiRepository.Add(newPrestito);
            return Content("Prestito aggiunto");
        }
    }
}
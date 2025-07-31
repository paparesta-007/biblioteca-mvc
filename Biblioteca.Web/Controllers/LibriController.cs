using Biblioteca.Data;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers;

public class LibriController : Controller
{
    private readonly LibroRepository _libroRepository;
    private readonly AutoreRepository _autoreRepository;
    public LibriController(IConfiguration configuration)
    {
        string? connStr = configuration.GetConnectionString("DefaultConnection");
        _libroRepository = new LibroRepository(connStr);
        _autoreRepository = new AutoreRepository(connStr);
    }
    public IActionResult Index()
    {
        ViewBag.Title = "Tutti gli utenti";
        var libri = _libroRepository.GetAll();
        var autori = _autoreRepository.GetAll();
        if (libri != null)
        {
            return View(libri);
        }
        return View();
    }
}
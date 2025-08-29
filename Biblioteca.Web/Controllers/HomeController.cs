using Biblioteca.Core.Models;
using Biblioteca.Data;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers;

public class HomeController : Controller
{
    private readonly PrestitiRepository _prestitirepo;
    private readonly PrenotazioneRepository _prenotazioniRepository;

    public HomeController(IConfiguration configuration)
    {
        var connStr = configuration.GetConnectionString("DefaultConnection");
        _prestitirepo = new PrestitiRepository(connStr);
        _prenotazioniRepository = new PrenotazioneRepository(connStr);
    }

    public IActionResult Index()
    {
        ViewBag.Title = "Home Page";
        var today = DateTime.Now;
        var prestiti = _prestitirepo.GetPrestitiDettagliati();
        var eliminati = new List<PrestitiViewModel>();
        foreach (var pre in prestiti)
            if (today.Date >= pre.DataPrestito.Date.AddDays(90))
            {
                _prestitirepo.Delete(pre.IdPrestito);
                eliminati.Add(pre);
            }
        var prenotazioni = _prenotazioniRepository.GetPrenotazioniDettagliati();
        if (eliminati.Any()) GeneraReportWord(eliminati);
        return View((prenotazioni, prestiti));
    }

    private void GeneraReportWord(List<PrestitiViewModel> eliminati)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "PrestitiEliminati.docx");

        using (var wordDoc = WordprocessingDocument.Create(
                   filePath, WordprocessingDocumentType.Document))
        {
            var mainPart = wordDoc.AddMainDocumentPart();
            mainPart.Document = new Document(new Body());

            var body = mainPart.Document.Body;

            // Titolo
            body.Append(new Paragraph(new Run(new Text("Report Prestiti Eliminati"))));

            foreach (var pre in eliminati)
            {
                var riga =
                    $"ID: {pre.IdLibro} - Libro: {pre.TitoloLibro} - Utente: {pre.CognomeUtente} {pre.NomeUtente} - Data Prestito: {pre.DataPrestito:dd/MM/yyyy}";
                body.Append(new Paragraph(new Run(new Text(riga))));
            }

            mainPart.Document.Save();
        }
    }
}
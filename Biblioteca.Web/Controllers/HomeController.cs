using Biblioteca.Core.Models;
using Biblioteca.Data;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
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
            var today = DateTime.Now;
            var prestiti = _prestitirepo.GetPrestitiDettagliati();
            var eliminati = new List<PrestitiViewModel>();
            foreach (var pre in prestiti)
            {
                if (today.Date >= pre.DataPrestito.Date.AddDays(90))
                {
                    _prestitirepo.Delete(pre.IdPrestito);
                    eliminati.Add(pre);
                }
            }

            if (eliminati.Any())
            {
                GeneraReportWord(eliminati);
            }
            return View(prestiti); 
            
            
        }
        private void GeneraReportWord(List<PrestitiViewModel> eliminati)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "PrestitiEliminati.docx");

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(
                       filePath, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new Document(new Body());

                Body body = mainPart.Document.Body;

                // Titolo
                body.Append(new Paragraph(new Run(new Text("Report Prestiti Eliminati"))));

                foreach (var pre in eliminati)
                {
                    string riga = $"ID: {pre.IdLibro} - Libro: {pre.TitoloLibro} - Utente: {pre.CognomeUtente} {pre.NomeUtente} - Data Prestito: {pre.DataPrestito:dd/MM/yyyy}";
                    body.Append(new Paragraph(new Run(new Text(riga))));
                }

                mainPart.Document.Save();
            }
        }
    }
}

namespace Biblioteca.Core.Models;

public class PrenotazioniViewModel
{
    public int IdPrenotazione { get; set; }
    public DateTime DataPrenotazione { get; set; }
    public string NomeUtente { get; set; }
    public string CognomeUtente { get; set; }
    public int IdUtente { get; set; }
    public string TitoloLibro { get; set; }
    public int IdLibro { get; set; }
}
namespace Biblioteca.Core.Models;

public class PrestitiViewModel
{
    public int IdPrestito { get; set; }
    public DateTime DataPrestito { get; set; }
    public string NomeUtente { get; set; }
    public string CognomeUtente { get; set; }
    public int IdUtente { get; set; }
    public string TitoloLibro { get; set; }
    public int IdLibro { get; set; }
}
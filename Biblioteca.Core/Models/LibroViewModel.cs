namespace Biblioteca.Core.Models;

public class LibroViewModel
{
    public int IdLibro { get; set; }
    public string Titolo { get; set; }
    public int IdAutore { get; set; }
    public int Anno { get; set; }
    public int IdPaese { get; set; }
    public int IdLingua { get; set; }
    public int IdGenere { get; set; }
    public decimal CostoLibro { get; set; }
    public int Pagine { get; set; }
    public string NomeAutore { get; set; }
    
    public string CognomeAutore { get; set; }
    public string NomePaese { get; set; }
    public string NomeLingua { get; set; }
    public string NomeGenere { get; set; }
    public string NomeNazione { get; set; }
    public bool IsPrenotato { get; set; }
    public bool IsInPrestito { get; set; }
    
}
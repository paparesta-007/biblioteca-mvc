namespace Biblioteca.Core.Models;

public class Libro
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
}
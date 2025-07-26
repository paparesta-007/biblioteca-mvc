namespace Biblioteca.Core.Models;

public class Prestiti
{
    public int IdPrestiti { get; set; }
    public int IdUtente { get; set; }
    public int IdLibro  { get; set; }
    public string Data { get; set; }
}
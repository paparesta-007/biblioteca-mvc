using System.Collections;

namespace Biblioteca.Core.Models;

public class Utente
{
    public int IdUtente { get; set; }
    public DateTime DataNascita { get; set; }
    public string Nome { get; set; }
    public string Cognome { get; set; }
    public string Email { get; set; }


}
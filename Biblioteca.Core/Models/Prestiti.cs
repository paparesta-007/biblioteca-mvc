using System.Collections;

namespace Biblioteca.Core.Models;

public class Prestiti : IEnumerable
{
    public int IDP { get; set; }
    public int IDU { get; set; }
    public int IDL  { get; set; }
    public DateTime DataPrestito { get; set; }
   
}
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Biblioteca.Core;
using Biblioteca.Core.Models;

namespace Biblioteca.Data;

public class AutoreRepository(string? connectionString)
{
    private Database _database = new(connectionString);

    public List<Autore> GetAll()
    {
        var autori= new List<Autore>();
        string query = "SELECT * FROM AUTORI";
        using var reader = _database.GetExecuteReader(query);
        while (reader.Read())
        {
            autori.Add(new Autore
            {
                IdAutore = reader.GetInt32(0),
                Nome = reader.GetString(1),
                Cognome = reader.GetString(2)
            });
        }
        return autori;
    }

    public Autore? GetById(int id)
    {
        string query = "SELECT * FROM AUTORI WHERE IDa = @id";
        var parameters=new [] { new SqlParameter("@id", id) };
        using var reader = _database.GetExecuteReader(query, parameters);
        if (reader.Read())
        {
            return new Autore
            {
                IdAutore = reader.GetInt32(0),
                Nome = reader.GetString(1),
                Cognome = reader.GetString(2)
            };
        }
        return null;
    }
    public int Add(Autore autore)
    {
        string query = "INSERT INTO Autori (Nome,Cognome) VALUES ('George','Orwell')";
        var parameters = new[] { new SqlParameter("@nome", autore.Nome), new SqlParameter("@cognome",autore.Cognome) };

        return _database.ExecuteNonQuery(query,parameters);
    }

    public int Update(Autore autore)
    {
        string query="UPDATE Autori SET Nome = 'Italo', Cognome = 'Calvino' WHERE IDa = '1'";
        var parameters = new[] { new SqlParameter("@nome", autore.Nome), new SqlParameter("@cognome", autore.Cognome), new SqlParameter("@id", autore.IdAutore) };
        return _database.ExecuteNonQuery(query, parameters);
    }

    public int Delete(int id)
    {
        string query="DELETE FROM Autori WHERE IDa = @id";
        var parameters = new[] { new SqlParameter("@id", id) };
        return _database.ExecuteNonQuery(query, parameters);
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Biblioteca.Core;
using Biblioteca.Core.Models;

namespace Biblioteca.Data;

public class GeneriRepository(string? connectionString)
{
    private readonly Database _database = new(connectionString);

    public List<Genere> GetAll()
    {
        var genere = new List<Genere>();

        string query = "SELECT * FROM Genere ";
        using var reader = _database.GetExecuteReader(query);
        while (reader.Read())
        {
            genere.Add(new Genere
            {
                IdGenere = reader.GetInt32(0),
                Descrizione = reader.GetString(1),
                
            });
        }
        
        return genere;
    }


    public Genere? GetById(int id)
    {
        string query = "SELECT * FROM Genere WHERE IDG = @id";
        var parameters = new[] { new SqlParameter("@id", id) };
        using var reader = _database.GetExecuteReader(query, parameters);
        if (reader.Read())
        {
            return new Genere
            {
                IdGenere = reader.GetInt32(0),
                Descrizione = reader.GetString(1),
            };
        }
        
        return null;
    }

    public int Add(Genere genere)
    {
        string query = "INSERT INTO Genere (Descrizione) VALUES (@Descrizione)";
       
        var parameters = new[]
        {
            new SqlParameter("@Descrizione", genere.Descrizione),
        };
        return _database.ExecuteNonQuery(query, parameters);
    }

    public int Update(Genere genere)
    {
        string query = "UPDATE Genere SET Descrizione = @Descrizione WHERE IDG = @IdGenere";
        var parameters = new[]
        {
            new SqlParameter("@Descrizione", genere.Descrizione),
            new SqlParameter("@Id_Paese", genere.IdGenere)
        };
        return _database.ExecuteNonQuery(query, parameters);
    }

    public int Delete(int id)
    {
        string query = "DELETE FROM Genere WHERE IdGenere = @id";
        var parameters = new[] { new SqlParameter("@id", id) };
        return _database.ExecuteNonQuery(query, parameters);
    }
}

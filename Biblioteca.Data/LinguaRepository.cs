using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Biblioteca.Core;
using Biblioteca.Core.Models;

namespace Biblioteca.Data;

public class LinguaRepository(string? connectionString)
{
    private readonly Database _database = new(connectionString);

    public List<Lingua> GetAll()
    {
        var lingua = new List<Lingua>();

        string query = "SELECT * FROM Lingua ";
        using var reader = _database.GetExecuteReader(query);
        while (reader.Read())
        {
            lingua.Add(new Lingua
            {
                IdLingua = reader.GetInt32(0),
                Nome = reader.GetString(1),
                
            });
        }
        
        return lingua;
    }


    public Lingua? GetById(int id)
    {
        string query = "SELECT * FROM Lingua WHERE IDl = @id";
        var parameters = new[] { new SqlParameter("@id", id) };
        using var reader = _database.GetExecuteReader(query, parameters);
        if (reader.Read())
        {
            return new Lingua
            {
                IdLingua = reader.GetInt32(0),
                Nome = reader.GetString(1),
            };
        }
        
        return null;
    }

    public int Add(Lingua lingua)
    {
        string query = "INSERT INTO Lingua (Nome) VALUES (@Nome)";
       
        var parameters = new[]
        {
            new SqlParameter("@Nome", lingua.Nome),
        };
        return _database.ExecuteNonQuery(query, parameters);
    }

    public int Update(Lingua lingua)
    {
        string query = "UPDATE Lingua SET Nome = @Nome WHERE IDl = @IdLingua";
        var parameters = new[]
        {
            new SqlParameter("@IDU", lingua.Nome),
            new SqlParameter("@IdLingua", lingua.IdLingua)
        };
        return _database.ExecuteNonQuery(query, parameters);
    }

    public int Delete(int id)
    {
        string query = "DELETE FROM Lingua WHERE IDl = @id";
        var parameters = new[] { new SqlParameter("@id", id) };
        return _database.ExecuteNonQuery(query, parameters);
    }
}

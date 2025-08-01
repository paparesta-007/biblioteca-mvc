using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Biblioteca.Core;
using Biblioteca.Core.Models;

namespace Biblioteca.Data;

public class NazioniRepository(string connectionString)
{
    private readonly Database _database = new(connectionString);

    public List<Nazioni> GetAll()
    {
        var nazioni = new List<Nazioni>();

        string query = "SELECT * FROM Nazioni ";
        using var reader = _database.GetExecuteReader(query);
        while (reader.Read())
        {
            nazioni.Add(new Nazioni
            {
                IdPaese = reader.GetInt32(0),
                Nome = reader.GetString(1),
                
            });
        }
        
        return nazioni;
    }


    public Nazioni? GetById(int id)
    {
        string query = "SELECT * FROM Nazioni WHERE Id_Paese = @id";
        var parameters = new[] { new SqlParameter("@id", id) };
        using var reader = _database.GetExecuteReader(query, parameters);
        if (reader.Read())
        {
            return new Nazioni
            {
                IdPaese = reader.GetInt32(0),
                Nome = reader.GetString(1),
            };
        }
        
        return null;
    }

    public int Add(Nazioni nazioni)
    {
        string query = "INSERT INTO Nazioni (Nome) VALUES (@Nome)";
       
        var parameters = new[]
        {
            new SqlParameter("@Nome", nazioni.Nome),
        };
        return _database.ExecuteNonQuery(query, parameters);
    }

    public int Update(Nazioni nazioni)
    {
        string query = "UPDATE Nazioni SET Nome = @Nome WHERE Id_Paese = @Id_Paese";
        var parameters = new[]
        {
            new SqlParameter("@IDU", nazioni.Nome),
            new SqlParameter("@Id_Paese", nazioni.IdPaese)
        };
        return _database.ExecuteNonQuery(query, parameters);
    }

    public int Delete(int id)
    {
        string query = "DELETE FROM Nazioni WHERE Id_Paese = @id";
        var parameters = new[] { new SqlParameter("@id", id) };
        return _database.ExecuteNonQuery(query, parameters);
    }
}

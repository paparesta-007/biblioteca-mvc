using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Biblioteca.Core;
using Biblioteca.Core.Models;

namespace Biblioteca.Data;

public class PrestitiRepository(string connectionString)
{
    private readonly Database _database = new(connectionString);

    public List<Prestiti> GetAll()
    {
        var prestiti = new List<Prestiti>();
        string query = "SELECT * FROM Prestiti";
        using var reader = _database.GetExecuteReader(query);
        while (reader.Read())
        {
            prestiti.Add(new Prestiti
            {
                IDP = reader.GetInt32(0),
                IDU = reader.GetInt32(1),
                IDL = reader.GetInt32(2),
                DataPrestito = reader.GetDateTime(3)
            });
        }
        return prestiti;
    }

    public Prestiti? GetById(int id)
    {
        string query = "SELECT * FROM Prestiti WHERE IDP = @id";
        var parameters = new[] { new SqlParameter("@id", id) };
        using var reader = _database.GetExecuteReader(query, parameters);
        if (reader.Read())
        {
            return new Prestiti
            {
                IDP = reader.GetInt32(0),
                IDU = reader.GetInt32(1),
                IDL = reader.GetInt32(2),
                DataPrestito = reader.GetDateTime(3)
            };
        }
        return null;
    }

    public int Add(Prestiti prestiti)
    {
        string query = "INSERT INTO Prestiti (IDU, IDL, DataPrestito) VALUES (@IDU, @IDL, @DataPrestito)";
       
        var parameters = new[]
        {
            new SqlParameter("@IDU", prestiti.IDU),
            new SqlParameter("@IDL", prestiti.IDL),
            new SqlParameter("@DataPrestito", DateTime.Now) // <-- così va bene!
        };
        return _database.ExecuteNonQuery(query, parameters);
    }

    public int Update(Prestiti prestiti)
    {
        string query = "UPDATE Prestiti SET IDU = @IDU, IDL = @IDL, DataPrestito = @DataPrestito WHERE IDP = @IDP";
        var parameters = new[]
        {
            new SqlParameter("@IDU", prestiti.IDU),
            new SqlParameter("@IDL", prestiti.IDL),
            new SqlParameter("@DataPrestito", prestiti.DataPrestito),
            new SqlParameter("@IDP", prestiti.IDP)
        };
        return _database.ExecuteNonQuery(query, parameters);
    }

    public int Delete(int id)
    {
        string query = "DELETE FROM Prestiti WHERE IDP = @id";
        var parameters = new[] { new SqlParameter("@id", id) };
        return _database.ExecuteNonQuery(query, parameters);
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Biblioteca.Core;
using Biblioteca.Core.Models;

namespace Biblioteca.Data;

public class PrestitiRepository(string? connectionString)
{
    private readonly Database _database = new(connectionString);

    public List<Prestiti> GetAll()
    {
        var prestiti = new List<Prestiti>();
        var libri = new List<Libro>();
        var utenti = new List<Utente>();
        string query = "SELECT * FROM Prestiti ";
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
    public int TotalePrestiti(int idUtente)
    {
        string query = "SELECT COUNT(*) FROM Prestiti WHERE IDU = @id";
        var parameters = new[] { new SqlParameter("@id", idUtente) };
        using var reader = _database.GetExecuteReader(query, parameters);
        if (reader.Read())
        {
            return reader.GetInt32(0);
        }
        return 0;
    }
    public List<PrestitiViewModel> GetPrestitiDettagliati()
    {
        var lista = new List<PrestitiViewModel>();
        const string query = @"
        SELECT p.IDP, p.DataPrestito, u.Nome, u.Cognome, l.Titolo, l.IdLibro, u.ID
        FROM Prestiti p
        JOIN Utenti u ON p.IDU = u.ID
        JOIN Libri l ON p.IDL = l.IdLibro";

        using var reader = _database.GetExecuteReader(query);
        while (reader.Read())
        {
            lista.Add(new PrestitiViewModel
            {
                IdPrestito = reader.GetInt32(0),
                DataPrestito = reader.GetDateTime(1),
                NomeUtente = reader.GetString(2),
                
                CognomeUtente = reader.GetString(3),
                TitoloLibro = reader.GetString(4),
                IdUtente = reader.GetInt32(5),
                IdLibro = reader.GetInt32(6)
            });
        }

        return lista;
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
            new SqlParameter("@DataPrestito", prestiti.DataPrestito) // <-- così va bene!
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

using System;
using System.Collections.Generic;
using Biblioteca.Core.Models;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Data;

public class PrenotazioneRepository(string? connectionString)
{
    private readonly Database _database = new(connectionString);

    public List<Prenotazioni> GetAll()
    {
        var prenotazioni = new List<Prenotazioni>();

        string query = "SELECT * FROM Prenotazioni ";
        using var reader = _database.GetExecuteReader(query);
        while (reader.Read())
        {
            prenotazioni.Add(new Prenotazioni
            {
                IdPrenotazioni = reader.GetInt32(0),
                IdUtente = reader.GetInt32(1),
                IdLibro = reader.GetInt32(2),
                DataPrenotazione = reader.GetDateTime(3)
                
            });
        }
        
        return prenotazioni;
    }
   

    public Prenotazioni? GetById(int id)
    {
        string query = "SELECT * FROM Prenotazioni WHERE IDL = @id";
        var parameters = new[] { new SqlParameter("@id", id) };
        using var reader = _database.GetExecuteReader(query, parameters);
        if (reader.Read())
        {
            return new Prenotazioni
            {
                IdPrenotazioni = reader.GetInt32(0),
                IdUtente = reader.GetInt32(1),
                IdLibro = reader.GetInt32(2),
                DataPrenotazione = reader.GetDateTime(3)
            };
        }
        
        return null;
    }
    
    public Prenotazioni? GetByIdLibro(int id)
    {
        string query = "SELECT * FROM Prenotazioni WHERE IDU = @id";
        var parameters = new[] { new SqlParameter("@id", id) };
        using var reader = _database.GetExecuteReader(query, parameters);
        if (reader.Read())
        {
            return new Prenotazioni
            {
                IdPrenotazioni = reader.GetInt32(0),
                IdUtente = reader.GetInt32(1),
                IdLibro = reader.GetInt32(2),
                DataPrenotazione = reader.GetDateTime(3)
            };
        }
        
        return null;
    }
    public List<Prenotazioni> GetByIdUtente(int id)
    {
        string query = "SELECT * FROM Prenotazioni WHERE IDU = @id";
        var parameters = new[] { new SqlParameter("@id", id) };
        var prenotazioni = new List<Prenotazioni>();

        using var reader = _database.GetExecuteReader(query, parameters);
        while (reader.Read())
        {
            prenotazioni.Add(new Prenotazioni
            {
                IdPrenotazioni = reader.GetInt32(0),
                IdUtente = reader.GetInt32(1),
                IdLibro = reader.GetInt32(2),
                DataPrenotazione = reader.GetDateTime(3)
            });
        }

        return prenotazioni;
    }

    public int TotalePrenotazioni(int idUtente)
    {
        string query = "SELECT COUNT(*) FROM Prenotazioni WHERE IDU = @id";
        var parameters = new[] { new SqlParameter("@id", idUtente) };
        using var reader = _database.GetExecuteReader(query, parameters);
        if (reader.Read())
        {
            return reader.GetInt32(0);
        }
        return 0;
    }
    public int Add(Prenotazioni prenotazioni)
    {
        string query = "INSERT INTO Prenotazioni (IDU, IDL, DataPrenot) VALUES (@IDU, @IDL, @DataPrenot)";
       
        var parameters = new[]
        {
            new SqlParameter("@IDU", prenotazioni.IdUtente),
            new SqlParameter("@IDL", prenotazioni.IdLibro),
            new SqlParameter("@DataPrenot", DateTime.Now) // <-- così va bene!
        };
        return _database.ExecuteNonQuery(query, parameters);
    }

    public int Update(Prenotazioni prenotazioni)
    {
        string query = "UPDATE Prenotazioni SET IDU = @IDU, IDL = @IDL, DataPrestito = @DataPrestito WHERE IDP = @IDP";
        var parameters = new[]
        {
            new SqlParameter("@IDU", prenotazioni.IdUtente),
            new SqlParameter("@IDL", prenotazioni.IdLibro),
            new SqlParameter("@DataPrestito", prenotazioni.DataPrenotazione),
            new SqlParameter("@IDP", prenotazioni.IdPrenotazioni)
        };
        return _database.ExecuteNonQuery(query, parameters);
    }

    public int Delete(int id)
    {
        string query = "DELETE FROM Prenotazioni WHERE IDL = @id";
        var parameters = new[] { new SqlParameter("@id", id) };
        return _database.ExecuteNonQuery(query, parameters);
    }
}
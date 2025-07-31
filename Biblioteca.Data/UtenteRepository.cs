using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Biblioteca.Core;
using Biblioteca.Core.Models;

namespace Biblioteca.Data;

public class UtenteRepository(string connectionString)
{
    private readonly Database _database = new(connectionString);

    public List<Utente> GetAll()
    {
        var utenti = new List<Utente>();
        string query = "SELECT * FROM Utenti";
        using var reader = _database.GetExecuteReader(query);
        while (reader.Read())
        {
            utenti.Add(new Utente
            {
                IdUtente = reader.GetInt32(0),
                DataNascita = reader.GetDateTime(1),
                Nome = reader.GetString(2),
                Cognome = reader.GetString(3),
                Email = reader.GetString(4)
                
            });
        }
        
        return utenti;
    }

    public Utente? GetById(int id)
    {
        string query = "SELECT * FROM Utenti WHERE ID = @id";
        var parameters = new[] { new SqlParameter("@id", id) };
        using var reader = _database.GetExecuteReader(query, parameters);
        if (reader.Read())
        {
            return new Utente
            {
                IdUtente = reader.GetInt32(0),
                DataNascita = reader.GetDateTime(1),
                Nome = reader.GetString(2),
                Cognome = reader.GetString(3),
                Email = reader.GetString(4)
            };
        }
        
        return null;
    }

    public int Add(Utente utente)
    {
        string query = "INSERT INTO Utenti (DataNascita, Nome, Cognome,Email) VALUES (@DataNascita, @Nome, @Cognome, @Email)";
       
        var parameters = new[]
        {
            new SqlParameter("@DataNascita", utente.DataNascita), 
            new SqlParameter("@Nome", utente.Nome),
            new SqlParameter("@Cognome", utente.Cognome),
            new SqlParameter("@Email", utente.Email),
            
        };
        return _database.ExecuteNonQuery(query, parameters);
    }

    public int Update(Utente utente)
    {
        string query = "UPDATE Utenti SET DataNascita = @DataNascita, Nome = @Nome, Cognome = @Cognome, Email = @Email WHERE ID = @ID";
        var parameters = new[]
        {
            new SqlParameter("@DataNascita", utente.DataNascita),
            new SqlParameter("@Nome", utente.Nome),
            new SqlParameter("@Cognome", utente.Cognome),
            new SqlParameter("@Email", utente.Email),
            new SqlParameter("@ID", utente.IdUtente)
        };
        return _database.ExecuteNonQuery(query, parameters);
    }

    public int Delete(int id)
    {
        string query = "DELETE FROM Utenti WHERE ID = @id";
        var parameters = new[] { new SqlParameter("@id", id) };
        return _database.ExecuteNonQuery(query, parameters);
    }
}

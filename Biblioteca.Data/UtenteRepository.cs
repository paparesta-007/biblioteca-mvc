using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Biblioteca.Core;
using Biblioteca.Core.Models;

namespace Biblioteca.Data;

public class UtenteRepository(string connectionString)
{
    private readonly Database _database = new(connectionString);

    public List<Utenti> GetAll()
    {
        var utenti = new List<Utenti>();
        string query = "SELECT * FROM Utenti";
        using var reader = _database.GetExecuteReader(query);
        while (reader.Read())
        {
            utenti.Add(new Utenti
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

    public Utenti? GetById(int id)
    {
        string query = "SELECT * FROM Utenti WHERE ID = @id";
        var parameters = new[] { new SqlParameter("@id", id) };
        using var reader = _database.GetExecuteReader(query, parameters);
        if (reader.Read())
        {
            return new Utenti
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

    public int Add(Utenti utenti)
    {
        string query = "INSERT INTO Utenti (DataNascita, Nome, Cognome,Email) VALUES (@DataNascita, @Nome, @Cognome, @Email)";
       
        var parameters = new[]
        {
            new SqlParameter("@DataNascita", utenti.DataNascita), 
            new SqlParameter("@Nome", utenti.Nome),
            new SqlParameter("@Cognome", utenti.Cognome),
            new SqlParameter("@Email", utenti.Email),
            
        };
        return _database.ExecuteNonQuery(query, parameters);
    }

    public int Update(Utenti utenti)
    {
        var idU = utenti.IdUtente;
        string query = "UPDATE Utenti SET IDU = @DataNascita, IDL = @Nome, Cognome = @Cognome, Email=@Email,  WHERE ID =idU ";
        var parameters = new[]
        {
            new SqlParameter("@DataNascita", utenti.DataNascita),
            new SqlParameter("@Nome", utenti.Nome),
            new SqlParameter("@Cognome", utenti.Cognome),
            new SqlParameter("@Email", utenti.Email),

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

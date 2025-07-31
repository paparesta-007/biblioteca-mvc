using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Biblioteca.Core.Models;

namespace Biblioteca.Data;

public class LibroRepository(string connectionString)
{
    private readonly Database _database = new(connectionString);

    public List<LibroViewModel> GetAll()
    {
        var libri = new List<LibroViewModel>();
        const string query = "SELECT * FROM Libri JOIN Autori ON Libri.IdAutore = Autori.IDa JOIN Lingua ON Libri.IdLingua = Lingua.IDl JOIN Genere ON Libri.IdGenere = Genere.IDG JOIN dbo.Nazioni N on Libri.IdPaese = N.Id_Paese";

        using var reader = _database.GetExecuteReader(query);
        while (reader.Read())
        {
            libri.Add(new LibroViewModel
            {
                IdLibro = reader.GetInt32(0),
                Titolo = reader.GetString(1),
                IdAutore = reader.GetInt32(2),
                Anno = reader.GetInt32(3),
                IdPaese = reader.GetInt32(4),
                IdLingua = reader.GetInt32(5),
                IdGenere = reader.GetInt32(6),
                CostoLibro = reader.GetDecimal(7),
                Pagine = reader.GetInt32(8),
                NomeAutore = reader.GetString(10),
                CognomeAutore = reader.GetString(11),
                NomeLingua = reader.GetString(13),
                NomeGenere = reader.GetString(15),
                NomeNazione = reader.GetString(17),
            });
        }

        return libri;
    }

    public Libro? GetById(int id)
    {
        const string query = "SELECT * FROM Libri WHERE IdLibro = @id";
        var parameters = new[] { new SqlParameter("@id", id) };

        using var reader = _database.GetExecuteReader(query, parameters);
        if (reader.Read())
        {
            return new Libro
            {
                IdLibro = reader.GetInt32(0),
                Titolo = reader.GetString(1),
                IdAutore = reader.GetInt32(2),
                Anno = reader.GetInt32(3),
                IdPaese = reader.GetInt32(4),
                IdLingua = reader.GetInt32(5),
                IdGenere = reader.GetInt32(6),
                CostoLibro = reader.GetDecimal(7),
                Pagine = reader.GetInt32(8),
            };
        }

        return null;
    }

    public int Add(Libro libro)
    {
        const string query = @"
            INSERT INTO Libri 
                (Titolo, IdAutore, Anno, IdPaese, IdLingua, IdGenere, CostoLibro, Pagine)
            VALUES 
                (@Titolo, @IdAutore, @Anno, @IdPaese, @IdLingua, @IdGenere, @CostoLibro, @Pagine)";

        var parameters = new[]
        {
            new SqlParameter("@Titolo", libro.Titolo),
            new SqlParameter("@IdAutore", libro.IdAutore),
            new SqlParameter("@Anno", libro.Anno),
            new SqlParameter("@IdPaese", libro.IdPaese),
            new SqlParameter("@IdLingua", libro.IdLingua),
            new SqlParameter("@IdGenere", libro.IdGenere),
            new SqlParameter("@CostoLibro", libro.CostoLibro),
            new SqlParameter("@Pagine", libro.Pagine)
        };

        return _database.ExecuteNonQuery(query, parameters);
    }

    public int Update(Libro libro)
    {
        const string query = @"
            UPDATE Libri 
            SET 
                Titolo = @Titolo, 
                IdAutore = @IdAutore,
                Anno = @Anno,
                IdPaese = @IdPaese,
                IdLingua = @IdLingua,
                IdGenere = @IdGenere,
                CostoLibro = @CostoLibro,
                Pagine = @Pagine
            WHERE 
                IdLibro = @IdLibro";

        var parameters = new[]
        {
            new SqlParameter("@IdLibro", libro.IdLibro),
            new SqlParameter("@Titolo", libro.Titolo),
            new SqlParameter("@IdAutore", libro.IdAutore),
            new SqlParameter("@Anno", libro.Anno),
            new SqlParameter("@IdPaese", libro.IdPaese),
            new SqlParameter("@IdLingua", libro.IdLingua),
            new SqlParameter("@IdGenere", libro.IdGenere),
            new SqlParameter("@CostoLibro", libro.CostoLibro),
            new SqlParameter("@Pagine", libro.Pagine)
        };

        return _database.ExecuteNonQuery(query, parameters);
    }

    public int Delete(int id)
    {
        const string query = "DELETE FROM Libri WHERE IdLibro = @id";
        var parameters = new[] { new SqlParameter("@id", id) };
        return _database.ExecuteNonQuery(query, parameters);
    }
}

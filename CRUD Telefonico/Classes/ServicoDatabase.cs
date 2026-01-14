using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using CRUD_Telefonico.Classes;

namespace CRUD_Telefonico.Classes;

public class ServicoDatabase
{
    private string connectionString = $"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "contatosAgenda.db")}";

    public void StartDatabase()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS contatos (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    nome TEXT NOT NULL,
                    telefone TEXT NOT NULL,
                    data_criacao TEXT NOT NULL
                );";
            command.ExecuteNonQuery();
        }
    }

    public void SaveContact(Contato c)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO contatos (nome, telefone, data_criacao) VALUES ($nome, $tel, $data)";
            cmd.Parameters.AddWithValue("$nome", c.Nome);
            cmd.Parameters.AddWithValue("$tel", c.Telefone);
            cmd.Parameters.AddWithValue("$data", c.DataCriacao);
            cmd.ExecuteNonQuery();
            
            var lastIdCmd = connection.CreateCommand();
            lastIdCmd.CommandText = "SELECT last_insert_rowid();";
            c.ID = Convert.ToInt32(lastIdCmd.ExecuteScalar());  
        }
    }

    public void UpdateContact(Contato c)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE contatos SET nome = $nome, telefone = $tel WHERE id = $id";
            cmd.Parameters.AddWithValue("$id", c.ID);
            cmd.Parameters.AddWithValue("$nome", c.Nome);
            cmd.Parameters.AddWithValue("$tel", c.Telefone);
            cmd.ExecuteNonQuery();
        }
    }

    public void RemoveContactDb(int id)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM contatos WHERE id = $id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }
    }

    public List<Contato> ListContacts()
    {
        var contactListDb = new List<Contato>();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT id, nome, telefone, data_criacao FROM contatos";

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    contactListDb.Add(new Contato(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3)
                    ));
                }
            }
        }
        return contactListDb;
    }
}

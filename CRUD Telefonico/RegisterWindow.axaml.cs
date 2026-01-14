using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CRUD_Telefonico.Classes;

namespace CRUD_Telefonico;

public partial class RegisterWindow : ShadUI.Window
{
    public Contato? NewContact { get; private set; }
    private ServicoDatabase _db;
    
    public RegisterWindow(int proximoId, ServicoDatabase db)
    {
        InitializeComponent();
        
        _db = db;
        txtId.Text = "AUTO";
        txtDtCriacao.Text = DateTime.Now.ToString("dd/MM/yyyy");
    }

    private void RegisterContact(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtTelefone.Text)) return;

        NewContact = new Contato(
            0, //temp - sqlite fornecera o real
            txtNome.Text,
            txtTelefone.Text,
            txtDtCriacao.Text
        ); //argumentos
        
        try 
        {
            _db.SaveContact(NewContact);
            this.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"/!\\ Erro ao salvar no SQLite: {ex.Message}");
        }
    }
}
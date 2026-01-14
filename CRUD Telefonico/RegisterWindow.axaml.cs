using System;
using System.Linq;
using System.Text.RegularExpressions;
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

    private bool MatchesRegexValidation()
    {
        string phoneForValidation = txtTelefone.Text;
        if (Regex.IsMatch(phoneForValidation, @"^\d*$"))
        {
            return true;
        }
        
        return false;
    }

    private string FormatPhoneNumber(string phoneForValidation)
    {
        string plainNumbers = Regex.Replace(phoneForValidation, @"[^0-9]", "");

        return plainNumbers.Length switch
        {
            10 =>
                $"({plainNumbers.Substring(0, 2)}) {plainNumbers.Substring(2, 4)}-{plainNumbers.Substring(6)}",
            11 =>
                $"({plainNumbers.Substring(0, 2)}) {plainNumbers.Substring(2, 5)}-{plainNumbers.Substring(7)}",
            12 =>
                $"+{plainNumbers.Substring(0, 2)} ({plainNumbers.Substring(2, 2)}) {plainNumbers.Substring(4, 4)}-{plainNumbers.Substring(8)}",
            13 =>
                $"+{plainNumbers.Substring(0, 2)} ({plainNumbers.Substring(2, 2)}) {plainNumbers.Substring(4, 5)}-{plainNumbers.Substring(9)}",
            
            _ => plainNumbers = "FORMATTING ERROR"
        };
    }
    
    private void RegisterContact(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtTelefone.Text)) return;
        if (txtTelefone.Text.Length < 10 || txtTelefone.Text.Length > 13)
        {
            Console.WriteLine("/!\\ O usuário tentou inserir um número de telefone com menos de 10 caracteres ou mais de 13 caracteres.");
            return;
        }

        if (MatchesRegexValidation())
        {
            string txtTelefoneFormatado = FormatPhoneNumber(txtTelefone.Text);
            NewContact = new Contato(
                0, //temp - sqlite fornecera o real
                txtNome.Text,
                txtTelefoneFormatado,
                txtDtCriacao.Text
            ); //argumentos
        }
        else
        {
            Console.WriteLine("/!\\ O usuário tentou inserir um número de telefone que contém caracteres especiais.");
        }
        
        try 
        {
            _db.SaveContact(NewContact);
            this.Close();
        }
        catch (Exception ex)
        {
            bool errorCausedByNotMatchingRegex = ex.Message.Contains("Object reference not set to an instance of an object");
            if (!errorCausedByNotMatchingRegex)
            {
                Console.WriteLine($"/!\\ {ex.Message}");
            }
        }
    }
}
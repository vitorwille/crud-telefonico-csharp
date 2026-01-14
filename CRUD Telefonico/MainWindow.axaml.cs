using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using CRUD_Telefonico.Classes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;
using ShadUI;

namespace CRUD_Telefonico;

public partial class MainWindow : ShadUI.Window
{
    // adiciona dados ao datagrid
    public ObservableCollection<Contato> ListaContatos { get; set; } = new ObservableCollection<Contato>();
    
    private ServicoDatabase _db = new ServicoDatabase();

    
    public MainWindow()
    {
        InitializeComponent();
        
        _db.StartDatabase();
        StartReadingDb();

        Console.WriteLine("--------------------\nCriado orgulhosamente com C# + Avalonia e ShadUI em ambiente Linux");
        this.Loaded += (_, _) => StartDataGrid();
        
    }

    private void StartReadingDb()
    {
        var listaContatosDb = _db.ListContacts(); 
        foreach (var c in listaContatosDb)
        {
            ListaContatos.Add(c);
        }
    }
    
    private async void RegisterContactMenu(object? sender, RoutedEventArgs routedEventArgs)
    {
        int proximoId = ListaContatos.Count + 1;
        
        var registerWindow = new RegisterWindow(proximoId, _db);
        await registerWindow.ShowDialog(this);

        if (registerWindow.NewContact != null)
        {
            ListaContatos.Add(registerWindow.NewContact);
        }
    }

    private async void EditContactMenu(object? sender, RoutedEventArgs e)
    {
        var contatoSelecionado = dataGrid1.SelectedItem as Contato;
        
        if (contatoSelecionado == null)
        {
            Console.WriteLine("/!\\ O usuário clicou no botão de edição de contato sem selecionar um item do DataGrid.");
            return;
        }
        
        var editWindow = new EditWindow(contatoSelecionado, _db);
        editWindow.ShowDialog(this);
    }
    
    private async void RemoveContact(object? sender, RoutedEventArgs e)
    {
        var contatoSelecionado = dataGrid1.SelectedItem as Contato;

        if (contatoSelecionado == null)
        {
            Console.WriteLine(
                "/!\\ O usuário clicou no botão de exclusão de contato sem selecionar um item do DataGrid.");
            return;
        }
        
        var rmvDialog = new RemovalConfirmationWindow(contatoSelecionado.Nome);
        await rmvDialog.ShowDialog(this);

        if (rmvDialog.IsConfirmed)
        {
            try
            {
                _db.RemoveContactDb(contatoSelecionado.ID);
                ListaContatos.Remove(contatoSelecionado);

                Console.WriteLine(
                    $"Contato '{contatoSelecionado.Nome}' removido. Novo total de itens no Grid: {ListaContatos.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"/!\\ Erro ao excluir do SQLite: {ex.Message}");
            }
        }
        else
        {
            Console.WriteLine($"Exclusão do contato '{contatoSelecionado.Nome}' cancelada.");
        }
    }

    private void StartDataGrid()
    {
        // colunas - id
        DataGridTextColumn campoID = new DataGridTextColumn();
        campoID.Header = "ID";
        campoID.Binding = new Binding("ID");
        campoID.IsReadOnly = true;
        campoID.Width =  new DataGridLength(80, DataGridLengthUnitType.Pixel);
        dataGrid1.Columns.Add(campoID);
        
        // colunas - nome
        DataGridTextColumn campoNome = new DataGridTextColumn();
        campoNome.Header = "Nome";
        campoNome.Binding = new Binding("Nome");
        campoNome.IsReadOnly = true;
        campoNome.Width = new DataGridLength(1, DataGridLengthUnitType.Star); // auto resize
        dataGrid1.Columns.Add(campoNome);
        
        // colunas - telefone
        DataGridTextColumn campoTelefone = new DataGridTextColumn();
        campoTelefone.Header = "Telefone";
        campoTelefone.Binding = new Binding("Telefone");
        campoTelefone.IsReadOnly = true;
        campoTelefone.Width = new DataGridLength(185, DataGridLengthUnitType.Pixel);
        dataGrid1.Columns.Add(campoTelefone);
        
        // colunas - data criacao
        DataGridTextColumn campoDataCriacao = new DataGridTextColumn();
        campoDataCriacao.Header = "Data de Criação";
        campoDataCriacao.Binding = new Binding("DataCriacao");
        campoDataCriacao.IsReadOnly = true;
        campoDataCriacao.Width = new DataGridLength(123, DataGridLengthUnitType.Pixel);
        dataGrid1.Columns.Add(campoDataCriacao);
        
        dataGrid1.ItemsSource = ListaContatos;
        
        Console.WriteLine($"\nTotal de itens no Grid: {ListaContatos.Count}\n--------------------");

    }
}
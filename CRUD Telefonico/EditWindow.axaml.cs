using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CRUD_Telefonico.Classes;

namespace CRUD_Telefonico;

public partial class EditWindow : ShadUI.Window
{
    private Contato _contatoAntesEdicao;
    private ServicoDatabase _db;
    
    public EditWindow(Contato contatoSelecionado, ServicoDatabase db)
    {
        InitializeComponent();
        _contatoAntesEdicao = contatoSelecionado;
        _db = db;

        txtId.Text = _contatoAntesEdicao.ID.ToString();
        txtDtCriacao.Text = _contatoAntesEdicao.DataCriacao;
        txtNome.Text = _contatoAntesEdicao.Nome;
        txtTelefone.Text = _contatoAntesEdicao.Telefone;
    }

    private void EditContact(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtTelefone.Text)) return;

        _contatoAntesEdicao.Nome = txtNome.Text;
        _contatoAntesEdicao.Telefone = txtTelefone.Text;
        
        try 
        {
            // 2. Persiste a alteração no SQLite usando o ID original
            _db.UpdateContact(_contatoAntesEdicao);
            
            this.Close();
        }
        catch (System.Exception ex)
        {
            System.Console.WriteLine($"/!\\ Erro ao atualizar no banco: {ex.Message}");
        }
    }
}
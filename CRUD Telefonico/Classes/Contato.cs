using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CRUD_Telefonico.Classes;

public class Contato : INotifyPropertyChanged
{
    private string _nome = "";
    private string _telefone = "";

    public int ID { get; set; }
    public string DataCriacao { get; set; } = "";

    public string Nome 
    { 
        get => _nome; 
        set { _nome = value; OnPropertyChanged(); } 
    }

    public string Telefone 
    { 
        get => _telefone; 
        set { _telefone = value; OnPropertyChanged(); } 
    }
    
    public Contato(int id, string nome, string telefone, string data)
    {
        ID = id;
        Nome = nome;
        Telefone = telefone;
        DataCriacao = data;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
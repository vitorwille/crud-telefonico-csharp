using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace CRUD_Telefonico;

public partial class RemovalConfirmationWindow : ShadUI.Window
{
    public bool IsConfirmed { get; set; }
    
    public RemovalConfirmationWindow(string nomeContato)
    {
        InitializeComponent();
        txtContatoConfirmar.Text=($"Contato selecionado: {nomeContato}");
        ToolTip.SetTip(txtContatoConfirmar, nomeContato);
    }

    private void ConfirmExclusion(object? sender, RoutedEventArgs e)
    {
        IsConfirmed = true;
        this.Close();
    }
}
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace CapstonePacMan.app.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void OnScoreBoardClick(object? sender, RoutedEventArgs e)
    {
        var window = new ScoreBoardWindow();
        await window.ShowDialog(this);
    }

    private void OnExitClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
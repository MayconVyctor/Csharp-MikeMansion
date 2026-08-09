using Avalonia.Controls;

namespace CapstonePacMan.app.Views;

public partial class GameOverWindow : Window
{
    public GameOverWindow()
    {
        InitializeComponent();
    }

    private void OnRestartClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close("restart");
    }
}
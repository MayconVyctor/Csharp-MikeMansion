using Avalonia.Controls;

namespace CapstonePacMan.app.Views;

public partial class GameResultWindow : Window
{
    public GameResultWindow(string message)
    {
        InitializeComponent();
        ResultText.Text = message;
    }

    private void OnRestartClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close("restart");
    }
}
using Avalonia.Controls;

namespace CapstonePacMan.app.Views;

public partial class GameResultWindow : Window
{
    public string PlayerName => PlayerNameInput.Text ?? string.Empty;

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
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace CapstonePacMan.app.Views;

public partial class StartView : UserControl
{
    public StartView()
    {
        InitializeComponent();
    }

    private void OnStartClick(object? sender, RoutedEventArgs e)
    {
        if (this.Parent is ContentControl host)
        {
            host.Content = new GameView();
        }
    }
}
using Avalonia.Controls;
using Avalonia.Interactivity;
using CapstonePacMan.app.ViewModels;

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

    private void OnStartGameClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
        {
            GameContent.Content = new GameBoardView(viewModel.SelectedLevel);
            viewModel.IsInMenu = false;
        }
    }

    private void OnExitClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
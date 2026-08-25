using Avalonia.Controls;
using Avalonia.Interactivity;
using CapstonePacMan.app.ViewModels;
using NetCoreAudio;

namespace CapstonePacMan.app.Views;

public partial class MainWindow : Window
{
    private Player _bgmPlayer;
    private bool _isMuted = false;
    public MainWindow()
    {
        InitializeComponent();
        _bgmPlayer = new Player();    
        _bgmPlayer.Play("Assets/pacman-art/sounds/ringtone-intro/pacman_ringtone.wav");
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
            GameContent.Content = new GameBoardView(
                viewModel.SelectedLevel,
                viewModel.SelectedDifficulty);
            viewModel.IsInMenu = false;
        }
    }

    private void OnExitClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    public void OnToggleMuteClick(object? sender, RoutedEventArgs e)
    {
            _isMuted = !_isMuted;
            
            if (_isMuted)
                _bgmPlayer.Pause();
            else
                _bgmPlayer.Resume();
    }
}
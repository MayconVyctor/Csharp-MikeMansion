using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using CapstonePacMan.app.Services;

namespace CapstonePacMan.app.Views;

public partial class ScoreBoardWindow : Window
{
    public ScoreBoardWindow()
    {
        InitializeComponent();
        ScoresList.ItemsSource = new ScoreBoardService()
            .LoadScores()
            .Select((entry, index) => new
            {
                Rank = index + 1,
                entry.PlayerName,
                entry.Score
            });
    }

    private void OnCloseClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}

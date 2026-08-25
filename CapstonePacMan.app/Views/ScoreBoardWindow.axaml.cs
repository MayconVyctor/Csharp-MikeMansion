using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using CapstonePacMan.app.Models;
using CapstonePacMan.app.Services;

namespace CapstonePacMan.app.Views;

public partial class ScoreBoardWindow : Window
{
    public ScoreBoardWindow()
    {
        InitializeComponent();
        ScoresList.ItemsSource = new ScoreBoardService()
            .LoadScores()
            .Select((entry, index) => new ScoreBoardRow
            {
                Rank = index + 1,
                PlayerName = entry.PlayerName,
                Score = entry.Score
            });
    }

    private void OnCloseClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}

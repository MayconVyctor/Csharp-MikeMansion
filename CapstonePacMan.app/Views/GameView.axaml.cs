using Avalonia.Controls;
using CapstonePacMan.app.ViewModels;


namespace CapstonePacMan.app.Views;

public partial class GameView : UserControl
{
    public GameView()
    {
        InitializeComponent();
        DataContext = new GameViewModel();
    }
}


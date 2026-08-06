using CapstonePacMan.app.Models;

namespace CapstonePacMan.app.ViewModels;

public class GameViewModel : ViewModelBase
{
    public Game Game { get; }

    public GameViewModel()
    {
        Game = new Game();
    }
}
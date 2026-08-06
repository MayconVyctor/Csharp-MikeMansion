using CapstonePacMan.app.Models;

namespace CapstonePacMan.app.ViewModels;

public class GameViewModel : ViewModelBase
{
    public Game Game { get; }
    public const int TileSize = 24;

    public double PacManScreenX => Game.PacMan.X * TileSize;
    public double PacManScreenY => Game.PacMan.Y * TileSize;

    public GameViewModel()
    {
        Game = new Game();
    }
      public void MovePacman(string direction)
    {
        int speed = 1;

        switch(direction)
        {
            case "Up":
                Game.PacMan.Y -= speed;
                break;

            case "Down":
                Game.PacMan.Y += speed;
                break;

            case "Left":
                Game.PacMan.X -= speed;
                break;

            case "Right":
                Game.PacMan.X += speed;
                break;
        }
         OnPropertyChanged(nameof(PacManScreenX));
         OnPropertyChanged(nameof(PacManScreenY));
    }
}
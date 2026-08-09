using CommunityToolkit.Mvvm.ComponentModel;

namespace CapstonePacMan.app.Models;

public partial class PacMan : ObservableObject
{
    public int _lives { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public double Speed { get; set; }
    public PacManState State { get; set; }

    [ObservableProperty]
    private string imagePath;

    public PacMan()
    {
        Lives = 3;
        State = PacManState.Alive;
        imagePath = "/Assets/pacman-art/pacman-right/1.png";
    }

    public int Lives
    {
        get => _lives;
        set => _lives = value < 0 ? 0 : value;
    }
}
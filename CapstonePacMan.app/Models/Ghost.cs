using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CapstonePacMan.app.Models;

public class Ghost : INotifyPropertyChanged
{
    private int _x;
    private int _y;
    private GhostState _state;
    private int _dirX = 0;
    private int _dirY = 0;
    private string _normalImagePath = "avares://CapstonePacMan.app/Assets/AssetsMikeMansion/AssetsGhosts/ghostFront.png";

    public int X { get => _x; set { _x = value; OnPropertyChanged(); OnPropertyChanged(nameof(ScreenX)); } }
    public int Y { get => _y; set { _y = value; OnPropertyChanged(); OnPropertyChanged(nameof(ScreenY)); } }
    public int Speed { get; set; }
    public double ScreenX => X * 24;
    public double ScreenY => Y * 24;

    public GhostState State
    {
        get => _state;
        set { _state = value; LoadImage(); OnPropertyChanged(); }
    }

    public int DirX { get => _dirX; set { _dirX = value; LoadImage(); } }
    public int DirY { get => _dirY; set { _dirY = value; LoadImage(); } }

    public int CameFromX { get; set; } = -1;
    public int CameFromY { get; set; } = -1;

    public string ImagePath
    {
        get => _normalImagePath;
        set { _normalImagePath = value; LoadImage(); }
    }

    public Bitmap? Image { get; private set; }

    public Ghost()
    {
        State = GhostState.Chase;
        Speed = 1;
        LoadImage();
    }

    private void LoadImage()
    {
        string prefix = "ghost";
        if (_normalImagePath.Contains("Pink")) prefix = "ghostPink";
        else if (_normalImagePath.Contains("Blue")) prefix = "ghostBlue";

        string dirStr = "Front";
        if (_dirX > 0) dirStr = "Right";
        else if (_dirX < 0) dirStr = "Left";
        else if (_dirY < 0) dirStr = "Back";
        else if (_dirY > 0) dirStr = "Front";

        string imagePath = State == GhostState.Scared
            ? $"avares://CapstonePacMan.app/Assets/AssetsMikeMansion/AssetsScaredGhots/Ghost1{dirStr}.png"
            : $"avares://CapstonePacMan.app/Assets/AssetsMikeMansion/AssetsGhosts/{prefix}{dirStr}.png";

        try 
        {
            var uri = new Uri(imagePath);
            Image = new Bitmap(AssetLoader.Open(uri));
            OnPropertyChanged(nameof(Image));
        } 
        catch { }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
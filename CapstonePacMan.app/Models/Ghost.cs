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

    public int X
    {
        get => _x;
        set
        {
            _x = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ScreenX));
        }
    }

    public int Y
    {
        get => _y;
        set
        {
            _y = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(ScreenY));
        }
    }

    public int Speed { get; set; }

    public double ScreenX => X * 24;
    public double ScreenY => Y * 24;

    public GhostState State { get; set; }

    public int DirX { get; set; } = 0;
    public int DirY { get; set; } = 0;

    public int CameFromX { get; set; } = -1;
    public int CameFromY { get; set; } = -1;

    private string _imagePath = "avares://CapstonePacMan.app/Assets/pacman-art/ghosts/blinky.png";
    public string ImagePath
    {
        get => _imagePath;
        set
        {
            _imagePath = value;
            LoadImage();
        }
    }

    public Bitmap? Image { get; private set; }

    public Ghost()
    {
        State = GhostState.Spread;
        Speed = 1;
        LoadImage();
    }

    private void LoadImage()
    {
        var uri = new Uri(_imagePath);
        Image = new Bitmap(AssetLoader.Open(uri));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
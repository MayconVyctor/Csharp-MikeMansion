using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CapstonePacMan.app.Models;

public class Tile : INotifyPropertyChanged
{
    public int X { get; set; }
    public int Y { get; set; }

    private TileType _type;
    public TileType Type
    {
        get => _type;
        set
        {
            _type = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsWall));
            OnPropertyChanged(nameof(IsPellet));
            OnPropertyChanged(nameof(IsPowerPellet));
        }
    }

    public bool IsWall => Type == TileType.Wall;
    public bool IsPellet => Type == TileType.Pellet;
    public bool IsPowerPellet => Type == TileType.PowerPellet;

    public double ScreenX => X * 24;
    public double ScreenY => Y * 24;

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
using CapstonePacMan.app.Models;
using System;
using System.Collections.Generic;
using Avalonia.Threading;

namespace CapstonePacMan.app.ViewModels;

public class GameViewModel : ViewModelBase
{
    public Game Game { get; }
    public bool IsFacingRight { get; private set; } = true;
    public bool IsFacingLeft { get; private set; }
    public bool IsFacingUp { get; private set; }
    public bool IsFacingDown { get; private set; }
    public int Score => Game.Score;
    public int Lives => Game.PacMan.Lives;
    public bool IsGameOver => Game.IsGameOver; 
    public IEnumerable<Tile> MapTiles => Game.Map.AllTiles();
    private bool _gameOverTriggered = false; 
    private bool _isProcessingDeath = false;
    public event Action? GameOver;

    public const int TileSize = 24;

    public double PacManScreenX => Game.PacMan.X * TileSize;
    public double PacManScreenY => Game.PacMan.Y * TileSize;
    private string? _pacmanDirection;
    private DispatcherTimer? _pacmanTimer;
    private DispatcherTimer? _ghostTimer;

    public GameViewModel()
    {
        Game = new Game();
        _pacmanTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(180)
        };
        _pacmanTimer.Tick += (s, e) =>
        {
            if (_pacmanDirection != null)
            {
                MovePacman(_pacmanDirection);
            }
        };
        _pacmanTimer.Start();

        _ghostTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(400) 
        };
        _ghostTimer.Tick += (s, e) =>
        {
            MoveGhosts();
            CheckGhostCollision();
            OnPropertyChanged(nameof(Game));
        };
        _ghostTimer.Start();
    }

    public void MovePacman(string direction)
    {
        _pacmanDirection = direction;
        int speed = 1;

        int nextX = Game.PacMan.X;
        int nextY = Game.PacMan.Y;

        switch (direction)
        {
            case "Up":
                nextY -= speed;
                break;

            case "Down":
                nextY += speed;
                break;

            case "Left":
                nextX -= speed;
                break;

            case "Right":
                nextX += speed;
                break;
        }

        if (Game.Map.IsWall(nextX, nextY))
        {
            return;
        }

        Game.PacMan.X = nextX;
        Game.PacMan.Y = nextY;

        Tile? tile = Game.Map.GetTile(nextX, nextY);

        if (tile != null)
        {
            if (tile.Type == TileType.Pellet)
            {
                Game.Score += 10;
                tile.Type = TileType.Empty;
            }
            else if (tile.Type == TileType.PowerPellet)
            {
                Game.Score += 50;
                tile.Type = TileType.Empty;
            }
        }

        switch (direction)
        {
            case "Up":
                IsFacingUp = true;
                IsFacingRight = false;
                IsFacingLeft = false;
                IsFacingDown = false;
                break;

            case "Down":
                IsFacingDown = true;
                IsFacingRight = false;
                IsFacingLeft = false;
                IsFacingUp = false;
                break;

            case "Left":
                IsFacingLeft = true;
                IsFacingRight = false;
                IsFacingUp = false;
                IsFacingDown = false;
                break;

            case "Right":
                IsFacingRight = true;
                IsFacingLeft = false;
                IsFacingUp = false;
                IsFacingDown = false;
                break;
        }

        OnPropertyChanged(nameof(PacManScreenX));
        OnPropertyChanged(nameof(PacManScreenY));

        OnPropertyChanged(nameof(IsFacingRight));
        OnPropertyChanged(nameof(IsFacingLeft));
        OnPropertyChanged(nameof(IsFacingUp));
        OnPropertyChanged(nameof(IsFacingDown));

        OnPropertyChanged(nameof(Score));
        OnPropertyChanged(nameof(Game));

        CheckGhostCollision();
    }

    private static readonly Random _rng = new Random();

    private static readonly (int dx, int dy)[] Directions = new (int, int)[]
    {
        (0, -1), 
        (0, 1), 
        (-1, 0), 
        (1, 0)   
    };

    public void MoveGhosts()
    {
        foreach (var ghost in Game.Ghosts)
        {

            var options = new List<(int dx, int dy)>();

            foreach (var dir in Directions)
            {
                int checkX = ghost.X + dir.dx;
                int checkY = ghost.Y + dir.dy;

                if (!Game.Map.IsWall(checkX, checkY))
                {
                    bool isBackwards = checkX == ghost.CameFromX && checkY == ghost.CameFromY;
                    if (!isBackwards)
                    {
                        options.Add(dir);
                    }
                }
            }
            if (options.Count == 0)
            {
                foreach (var dir in Directions)
                {
                    int checkX = ghost.X + dir.dx;
                    int checkY = ghost.Y + dir.dy;

                    if (!Game.Map.IsWall(checkX, checkY))
                    {
                        options.Add(dir);
                    }
                }
            }

            if (options.Count == 0)
            {
                continue; 
            }
            var chosen = options[_rng.Next(options.Count)];
            ghost.CameFromX = ghost.X;
            ghost.CameFromY = ghost.Y;

            ghost.DirX = chosen.dx;
            ghost.DirY = chosen.dy;

            ghost.X += chosen.dx;
            ghost.Y += chosen.dy;
        }
    }
   private void CheckGhostCollision()
    {
        bool collided = false;

        foreach (var ghost in Game.Ghosts)
        {
            if (ghost.X == Game.PacMan.X && ghost.Y == Game.PacMan.Y)
            {
                collided = true;
                break;
            }
        }

        if (collided && !_isProcessingDeath)
        {
            _isProcessingDeath = true;

            Game.PacMan.Lives -= 1;
            OnPropertyChanged(nameof(Lives));

            if (Game.PacMan.Lives <= 0)
            {
                OnPropertyChanged(nameof(IsGameOver));
                
                 if (!_gameOverTriggered)
                    {
                        _gameOverTriggered = true;
                        _pacmanTimer?.Stop();
                        _ghostTimer?.Stop();
                        GameOver?.Invoke();
                    }
            }
            else
            {
                ResetPositions();
            }
        }
        else if (!collided)
        {
            _isProcessingDeath = false;
        }
    }

    private void ResetPositions()
    {
        Tile? spawn = Game.Map.GetPacManSpawn();
        if (spawn != null)
        {
            Game.PacMan.X = spawn.X;
            Game.PacMan.Y = spawn.Y;
            OnPropertyChanged(nameof(PacManScreenX));
            OnPropertyChanged(nameof(PacManScreenY));
        }

        if (Game.Ghosts.Count >= 3)
        {
            Game.Ghosts[0].X = 12;
            Game.Ghosts[0].Y = 10;
            Game.Ghosts[1].X = 13;
            Game.Ghosts[1].Y = 10;
            Game.Ghosts[2].X = 14;
            Game.Ghosts[2].Y = 10;
        }
    }

    public void ResetGame()
    {
        Game.Reset();
        _pacmanDirection = null;
        _isProcessingDeath = false;
        _gameOverTriggered = false;

        OnPropertyChanged(nameof(Score));
        OnPropertyChanged(nameof(Lives));
        OnPropertyChanged(nameof(IsGameOver));
        OnPropertyChanged(nameof(PacManScreenX));
        OnPropertyChanged(nameof(PacManScreenY));
        OnPropertyChanged(nameof(MapTiles));
        OnPropertyChanged(nameof(Game));

        _pacmanTimer?.Start();
        _ghostTimer?.Start();
    }
}
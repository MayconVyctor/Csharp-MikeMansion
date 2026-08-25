using CapstonePacMan.app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    private bool _gameWonTriggered = false;
    private bool _isProcessingDeath = false;
    private int _vulnerableSecondsRemaining;
    private Tile? _cherryTile;
    private int _nextCherryScore = 100;
    public event Action? GameOver;
    public event Action? GameWon;

    public const int TileSize = 24;
    private const int PacmanMoveIntervalMilliseconds = 260;
    private const int GhostMoveIntervalMilliseconds = 600;

    public double PacManScreenX => Game.PacMan.X * TileSize;
    public double PacManScreenY => Game.PacMan.Y * TileSize;
    private string? _pacmanDirection;
    private DispatcherTimer? _pacmanTimer;
    private DispatcherTimer? _ghostTimer;
    private DispatcherTimer? _powerTimer;
    private DispatcherTimer? _ghostReleaseTimer;

    public GameViewModel()
    {
        Game = new Game();
        _pacmanTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(PacmanMoveIntervalMilliseconds)
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
            Interval = TimeSpan.FromMilliseconds(GhostMoveIntervalMilliseconds)
        };
        _ghostTimer.Tick += (s, e) =>
        {
            MoveGhosts();
            CheckGhostCollision();
            OnPropertyChanged(nameof(Game));
        };
        _ghostTimer.Start();

        _powerTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1)
        };
        _powerTimer.Tick += (s, e) => UpdatePowerMode();

        _ghostReleaseTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(10)
        };
        _ghostReleaseTimer.Tick += (s, e) => ReleaseNextGhost();
        _ghostReleaseTimer.Start();
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
                ActivatePowerMode();
            }
            else if (tile.Type == TileType.Cherry)
            {
                Game.Score += 100;
                if (Game.PacMan.Lives < 3)
                {
                    Game.PacMan.Lives++;
                    OnPropertyChanged(nameof(Lives));
                }

                tile.Type = TileType.Empty;
                _cherryTile = null;
            }

            SpawnCherryIfEligible();
            CheckWinCondition();
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

    public void ChangePacmanDirection(string direction)
    {
        if (_pacmanDirection == direction)
        {
            return;
        }

        MovePacman(direction);
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
            if (ghost.State == GhostState.InBase)
            {
                continue;
            }

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
            var chosen = ChooseGhostDirection(ghost, options);
            ghost.CameFromX = ghost.X;
            ghost.CameFromY = ghost.Y;

            ghost.DirX = chosen.dx;
            ghost.DirY = chosen.dy;

            ghost.X += chosen.dx;
            ghost.Y += chosen.dy;
        }
    }

    private (int dx, int dy) ChooseGhostDirection(
        Ghost ghost,
        List<(int dx, int dy)> options)
    {
        if (ghost.State != GhostState.Chase)
        {
            return options[_rng.Next(options.Count)];
        }

        int targetX = Game.PacMan.X;
        int targetY = Game.PacMan.Y;

        return options
            .OrderBy(direction => Math.Abs(ghost.X + direction.dx - targetX)
                + Math.Abs(ghost.Y + direction.dy - targetY))
            .First();
    }

    private void CheckGhostCollision()
    {
        foreach (var ghost in Game.Ghosts)
        {
            if (ghost.X == Game.PacMan.X && ghost.Y == Game.PacMan.Y)
            {
                if (ghost.State == GhostState.Scared)
                {
                    Game.Score += 200;
                    ReturnGhostToBase(ghost);
                    OnPropertyChanged(nameof(Score));
                    OnPropertyChanged(nameof(Game));
                    return;
                }

                if (!_isProcessingDeath)
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
                            _powerTimer?.Stop();
                            _ghostReleaseTimer?.Stop();
                            GameOver?.Invoke();
                        }
                    }
                    else
                    {
                        ResetPositions();
                    }
                }

                return;
            }
        }

        _isProcessingDeath = false;
    }

    private void ActivatePowerMode()
    {
        _vulnerableSecondsRemaining = 8;

        foreach (var ghost in Game.Ghosts)
        {
            if (ghost.State != GhostState.Eaten && ghost.State != GhostState.InBase)
            {
                ghost.State = GhostState.Scared;
            }
        }

        _powerTimer?.Start();
    }

    private void SpawnCherryIfEligible()
    {
        if (_cherryTile != null || Game.Score < _nextCherryScore)
        {
            return;
        }

        List<Tile> emptyTiles = Game.Map.AllTiles()
            .Where(tile => tile.Type == TileType.Empty)
            .ToList();

        if (emptyTiles.Count == 0)
        {
            return;
        }

        _cherryTile = emptyTiles[_rng.Next(emptyTiles.Count)];
        _cherryTile.Type = TileType.Cherry;
        _nextCherryScore += 100;
    }

    private void UpdatePowerMode()
    {
        _vulnerableSecondsRemaining--;
        if (_vulnerableSecondsRemaining > 0)
        {
            return;
        }

        foreach (var ghost in Game.Ghosts)
        {
            if (ghost.State == GhostState.Scared)
            {
                ghost.State = GhostState.Chase;
            }
        }

        _powerTimer?.Stop();
    }

    private void ReturnGhostToBase(Ghost ghost)
    {
        ghost.State = GhostState.Eaten;
        ghost.X = 13;
        ghost.Y = 10;
        ghost.State = GhostState.Chase;
    }

    private void ReleaseNextGhost()
    {
        Ghost? ghost = Game.Ghosts.FirstOrDefault(item => item.State == GhostState.InBase);
        if (ghost == null)
        {
            _ghostReleaseTimer?.Stop();
            return;
        }

        ghost.State = GhostState.Chase;
    }

    private void CheckWinCondition()
    {
        bool hasCollectibles = Game.Map.AllTiles().Any(tile =>
            tile.Type == TileType.Pellet || tile.Type == TileType.PowerPellet);

        if (!hasCollectibles && !_gameWonTriggered)
        {
            _gameWonTriggered = true;
            _pacmanTimer?.Stop();
            _ghostTimer?.Stop();
            _ghostReleaseTimer?.Stop();
            GameWon?.Invoke();
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
        _gameWonTriggered = false;
        _vulnerableSecondsRemaining = 0;
        _cherryTile = null;
        _nextCherryScore = 100;

        OnPropertyChanged(nameof(Score));
        OnPropertyChanged(nameof(Lives));
        OnPropertyChanged(nameof(IsGameOver));
        OnPropertyChanged(nameof(PacManScreenX));
        OnPropertyChanged(nameof(PacManScreenY));
        OnPropertyChanged(nameof(MapTiles));
        OnPropertyChanged(nameof(Game));

        _pacmanTimer?.Start();
        _ghostTimer?.Start();
        _powerTimer?.Stop();
        _ghostReleaseTimer?.Stop();
        _ghostReleaseTimer?.Start();
    }
}
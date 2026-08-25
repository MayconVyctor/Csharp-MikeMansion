namespace CapstonePacMan.app.Models
{
    using System.Collections.Generic;
    using System;
    
    public class Game
    {
        public PacMan PacMan { get; set; }
        public List<Ghost> Ghosts { get; set; }
        public GameMap Map { get; set; }
        public int Score { get; set; }
        public int CurrentLevel { get; set; }
        public bool IsPaused { get; set; }

        public bool IsGameOver => PacMan.Lives <= 0;

        public Game(int level = 1)
        {
            PacMan = new PacMan();

            Score = 0;
            CurrentLevel = level;
            IsPaused = false;

            Ghosts = new List<Ghost>();
            
            Ghosts.Add(new Ghost
            {
                X = 12,
                Y = 10,
                ImagePath = "avares://CapstonePacMan.app/Assets/AssetsMikeMansion/AssetsGhosts/ghostFront.png"
            });

            Ghosts.Add(new Ghost
            {
                X = 13,
                Y = 10,
                State = GhostState.InBase,
                ImagePath = "avares://CapstonePacMan.app/Assets/AssetsMikeMansion/AssetsGhosts/ghostPinkFront.png"
            });

            Ghosts.Add(new Ghost
            {
                X = 14,
                Y = 10,
                State = GhostState.InBase,
                ImagePath = "avares://CapstonePacMan.app/Assets/AssetsMikeMansion/AssetsGhosts/ghostBlueFront.png"
            });

            Map = new GameMap();
            Map.Load($"Maps/Level{CurrentLevel}.txt");
            IsPaused = false;

            Tile? spawn = Map.GetPacManSpawn();
            if (spawn != null)
            {
                PacMan.X = spawn.X;
                PacMan.Y = spawn.Y;
            }
        }
        
        public void Reset()
        {
            PacMan = new PacMan();
            Score = 0;
            Ghosts = new List<Ghost>();
            
            Ghosts.Add(new Ghost { X = 12, Y = 10, ImagePath = "avares://CapstonePacMan.app/Assets/AssetsMikeMansion/AssetsGhosts/ghostFront.png" });
            Ghosts.Add(new Ghost { X = 13, Y = 10, State = GhostState.InBase, ImagePath = "avares://CapstonePacMan.app/Assets/AssetsMikeMansion/AssetsGhosts/ghostPinkFront.png" });
            Ghosts.Add(new Ghost { X = 14, Y = 10, State = GhostState.InBase, ImagePath = "avares://CapstonePacMan.app/Assets/AssetsMikeMansion/AssetsGhosts/ghostBlueFront.png" });

            Map.Load($"Maps/Level{CurrentLevel}.txt");

            Tile? spawn = Map.GetPacManSpawn();
            if (spawn != null)
            {
                PacMan.X = spawn.X;
                PacMan.Y = spawn.Y;
            }
        }
    }
}
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

        public Game()
        {
            PacMan = new PacMan();

            Score = 0;
            CurrentLevel = 1;

            Ghosts = new List<Ghost>();
            Ghosts.Add(new Ghost
            {
                X = 12,
                Y = 10,
                ImagePath = "avares://CapstonePacMan.app/Assets/pacman-art/ghosts/blinky.png"
            });

            Ghosts.Add(new Ghost
            {
                X = 13,
                Y = 10,
                ImagePath = "avares://CapstonePacMan.app/Assets/pacman-art/ghosts/pinky.png"
            });

            Ghosts.Add(new Ghost
            {
                X = 14,
                Y = 10,
                ImagePath = "avares://CapstonePacMan.app/Assets/pacman-art/ghosts/inky.png"
            });

            Map = new GameMap();
            Map.Load("Maps/Level1.txt");
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
            CurrentLevel = 1;

            Ghosts = new List<Ghost>();
            Ghosts.Add(new Ghost { X = 12, Y = 10, ImagePath = "avares://CapstonePacMan.app/Assets/pacman-art/ghosts/blinky.png" });
            Ghosts.Add(new Ghost { X = 13, Y = 10, ImagePath = "avares://CapstonePacMan.app/Assets/pacman-art/ghosts/pinky.png" });
            Ghosts.Add(new Ghost { X = 14, Y = 10, ImagePath = "avares://CapstonePacMan.app/Assets/pacman-art/ghosts/inky.png" });

            Map.Load("Maps/Level1.txt");

            Tile? spawn = Map.GetPacManSpawn();
            if (spawn != null)
            {
                PacMan.X = spawn.X;
                PacMan.Y = spawn.Y;
            }
        }

    }
    
}

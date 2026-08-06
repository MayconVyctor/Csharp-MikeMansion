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

            Map = new GameMap();
            Map.Load("CapstonePacMan.app/Maps/Level1.txt");
            IsPaused = false;

            Tile? spawn = Map.GetPacManSpawn();
            if (spawn != null)
            {
                PacMan.X = spawn.X;
                PacMan.Y = spawn.Y;
            }
        }
    }
}

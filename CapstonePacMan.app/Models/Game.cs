namespace CapstonePacMan.app.Models
{
    using System.Collections.Generic;
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
            IsPaused = false;
        }
    }
}

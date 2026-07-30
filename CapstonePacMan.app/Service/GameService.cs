using CapstonePacMan.app.Models;
namespace CapstonePacMan.app.Services
{


    public class GameService
    {
        public Game CurrentGame { get; private set; }

        public GameService()
        {
            CurrentGame = new Game();
        }

        public void Start()
        {
            CurrentGame.Score = 0;
            CurrentGame.CurrentLevel = 1;
            CurrentGame.IsPaused = false;
            // resetar a posição do PacMan e Fantasmas 
        }

        public void Pause()
        {
            CurrentGame.IsPaused = true;
        }

        public void Resume()
        {
            CurrentGame.IsPaused = false;
        }

        public void TogglePause()
        {
            CurrentGame.IsPaused = !CurrentGame.IsPaused;
        }

        public void AddScore(int points)
        {
            if (points > 0)
            {
                CurrentGame.Score += points;
            }
        }

        public void NextLevel()
        {
            CurrentGame.CurrentLevel++;
            // Lógica para recarregar o mapa do próximo nível
        }
    }
}
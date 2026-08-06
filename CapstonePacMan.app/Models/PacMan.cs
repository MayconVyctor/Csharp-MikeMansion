namespace CapstonePacMan.app.Models;

public class PacMan
{
    public int Lives { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public double Speed { get; set; }
    public PacManState State { get; set; }
    public string ImagePath { get; set; }
        public PacMan()
        {
            Lives = 3;
            State = PacManState.Alive;
            ImagePath = "/Assets/pacman-art/pacman-right/1.png";
        }
}

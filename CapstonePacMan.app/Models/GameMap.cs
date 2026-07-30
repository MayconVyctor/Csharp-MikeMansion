namespace CapstonePacMan.app.Models
{
    public class GameMap
    {
        public int Height { get; set; }
        public int Width { get; set; }

        public Tile[,] Tiles { get; set; }
    }
}
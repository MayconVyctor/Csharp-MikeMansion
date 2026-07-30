using CapstonePacMan.Models;

namespace CapstonePacMan.app.Models;

public class Ghost
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Speed { get; set; }

    public GhostState State;

    public Ghost()
    {
        State = GhostState.Spread;
        
    }

}
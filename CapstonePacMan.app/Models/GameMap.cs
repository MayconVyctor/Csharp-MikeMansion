using System.IO;
using System;

namespace CapstonePacMan.app.Models;

public class GameMap
{
    public int Height { get; private set; }
    public int Width { get; private set; }

    public Tile[,]? Tiles { get; private set; }

    public void Load(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);

        Height = lines.Length;
        Width = lines[0].Length;

        Tiles = new Tile[Height, Width];

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                TileType type = TileType.Empty;

                switch (lines[y][x])
                {
                    case '#':
                        type = TileType.Wall;
                        break;

                    case '.':
                        type = TileType.Pellet;
                        break;

                    case 'o':
                        type = TileType.PowerPellet;
                        break;
                    case '-':
                        type = TileType.GhostSpawn;
                        break;
                    case 'P':
                        type = TileType.PacManSpawn;
                        break;
                    default:
                        type = TileType.Empty;
                        break;
                }

                Tiles[y, x] = new Tile
                {
                    X = x,
                    Y = y,
                    Type = type
                };
            }
        }
    }
    public bool IsWall(int x, int y)
    {   
        if (x < 0 || x >= Width || y < 0 || y >= Height)
        {
            return true;
        }
        return Tiles[y, x].Type == TileType.Wall;
    }

    public Tile? GetPacManSpawn()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (Tiles[y, x].Type == TileType.PacManSpawn)
                {
                    return Tiles[y, x];
                }
            }
        }
        return null;
    }
}
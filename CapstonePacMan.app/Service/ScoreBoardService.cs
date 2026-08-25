using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CapstonePacMan.app.Models;

namespace CapstonePacMan.app.Services;

public class ScoreBoardService
{
    private const int MaximumScores = 5;
    private readonly string _filePath;

    public ScoreBoardService()
    {
        string directory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "MikeMansion");
        Directory.CreateDirectory(directory);
        _filePath = Path.Combine(directory, "scores.json");
    }

    public IReadOnlyList<ScoreEntry> LoadScores()
    {
        if (!File.Exists(_filePath))
        {
            return Array.Empty<ScoreEntry>();
        }

        try
        {
            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<ScoreEntry>>(json)?
                .OrderByDescending(entry => entry.Score)
                .Take(MaximumScores)
                .ToList() ?? new List<ScoreEntry>();
        }
        catch (JsonException)
        {
            return Array.Empty<ScoreEntry>();
        }
        catch (IOException)
        {
            return Array.Empty<ScoreEntry>();
        }
    }

    public void SaveScore(string playerName, int score)
    {
        List<ScoreEntry> scores = LoadScores().ToList();
        scores.Add(new ScoreEntry
        {
            PlayerName = string.IsNullOrWhiteSpace(playerName) ? "Mike" : playerName.Trim(),
            Score = score
        });

        string json = JsonSerializer.Serialize(
            scores.OrderByDescending(entry => entry.Score).Take(MaximumScores),
            new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}

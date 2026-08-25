using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CapstonePacMan.app.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public int[] AvailableLevels { get; } = { 1, 2 };
    public string[] AvailableDifficulties { get; } = { "Facil", "Normal", "Dificil" };

    [ObservableProperty]
    private int selectedLevel = 1;

    [ObservableProperty]
    private string selectedDifficulty = "Normal";

    [ObservableProperty]
    private bool isInMenu = true;

    [RelayCommand]
    private void StartGame()
    {
        IsInMenu = false;
    }
}

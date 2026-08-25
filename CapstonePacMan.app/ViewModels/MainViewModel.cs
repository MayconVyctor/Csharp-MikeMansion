using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CapstonePacMan.app.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public int[] AvailableLevels { get; } = { 1, 2 };

    [ObservableProperty]
    private int selectedLevel = 1;

    [ObservableProperty]
    private bool isInMenu = true;

    [RelayCommand]
    private void StartGame()
    {
        IsInMenu = false;
    }
}

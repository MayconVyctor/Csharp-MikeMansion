using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CapstonePacMan.app.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool isInMenu = true;

    [RelayCommand]
    private void StartGame()
    {
        IsInMenu = false;
    }
}

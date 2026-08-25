using Avalonia.Controls;
using CapstonePacMan.app.ViewModels;
using Avalonia.Input;
using CapstonePacMan.app.Services;

namespace CapstonePacMan.app.Views;

public partial class GameBoardView : UserControl
{
    public GameBoardView() : this(1, "Normal")
    {
    }

    public GameBoardView(int level, string difficulty)
    
    {
        InitializeComponent();
        var vm = new GameBoardViewModel(level, difficulty);
        DataContext = vm;
        KeyDown += OnKeyDown;
        AttachedToVisualTree += (_, _) => Focus();

        vm.GameOver += async () => await ShowEndPopup(vm, "FIM DE JOGO");
        vm.GameWon += async () => await ShowEndPopup(vm, "VOCE VENCEU!");
    }

    private async System.Threading.Tasks.Task ShowEndPopup(GameBoardViewModel vm, string message)
    {
        var popup = new GameResultWindow(message);

        if (TopLevel.GetTopLevel(this) is not Window owner)
        {
            return;
        }

        var result = await popup.ShowDialog<string?>(owner);

        if (result == "restart")
        {
            new ScoreBoardService().SaveScore(popup.PlayerName, vm.Score);
            vm.ResetGame();
        }
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (DataContext is GameBoardViewModel vm)
        {
            if (e.Key == Key.Space)
            {
                vm.TogglePause();
                return;
            }

            switch (e.Key)
            {
                case Key.Up:
                    vm.ChangePacmanDirection("Up");
                    break;

                case Key.Down:
                    vm.ChangePacmanDirection("Down");
                    break;

                case Key.Left:
                    vm.ChangePacmanDirection("Left");
                    break;

                case Key.Right:
                    vm.ChangePacmanDirection("Right");
                    break;
            }
        }
    }
}
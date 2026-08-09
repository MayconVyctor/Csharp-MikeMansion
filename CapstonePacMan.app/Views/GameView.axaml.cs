using Avalonia.Controls;
using CapstonePacMan.app.ViewModels;
using Avalonia.Input;

namespace CapstonePacMan.app.Views;

public partial class GameView : UserControl
{
    public GameView()
    {
        InitializeComponent();
        var vm = new GameViewModel();
        DataContext = vm;
        KeyDown += OnKeyDown;

        vm.GameOver += async () => await ShowGameOverPopup(vm);
    }

    private async System.Threading.Tasks.Task ShowGameOverPopup(GameViewModel vm)
    {
        var popup = new GameOverWindow();

        var topLevel = TopLevel.GetTopLevel(this) as Window;
        var result = await popup.ShowDialog<string?>(topLevel);

        if (result == "restart")
        {
            vm.ResetGame();
        }
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (DataContext is GameViewModel vm)
        {
            switch (e.Key)
            {
                case Key.Up:
                    vm.MovePacman("Up");
                    break;

                case Key.Down:
                    vm.MovePacman("Down");
                    break;

                case Key.Left:
                    vm.MovePacman("Left");
                    break;

                case Key.Right:
                    vm.MovePacman("Right");
                    break;
            }
        }
    }
}
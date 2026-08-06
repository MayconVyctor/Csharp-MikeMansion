using Avalonia.Controls;
using CapstonePacMan.app.ViewModels;
using Avalonia.Input;

namespace CapstonePacMan.app.Views;

public partial class GameView : UserControl
{
    public GameView()
    {
        InitializeComponent();
        DataContext = new GameViewModel();
        KeyDown += OnKeyDown;
    }

      private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if(DataContext is ViewModels.GameViewModel vm)
        {
            switch(e.Key)
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


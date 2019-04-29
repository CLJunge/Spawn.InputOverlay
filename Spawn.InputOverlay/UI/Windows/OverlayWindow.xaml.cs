using Spawn.InputOverlay.UI.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace Spawn.InputOverlay.UI.Windows
{
    public partial class OverlayWindow : Window
    {
        public OverlayWindow() => InitializeComponent();

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (e.ChangedButton)
            {
                case MouseButton.Left:
                    if (e.ClickCount == 2)
                        (DataContext as OverlayViewModel).ToggleResizeGripCommand.Execute(null);
                    else
                        DragMove();
                    break;

                case MouseButton.Right:
                    break;
            }
        }
    }
}
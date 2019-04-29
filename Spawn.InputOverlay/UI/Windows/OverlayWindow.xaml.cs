#region Using
using Spawn.InputOverlay.UI.ViewModels;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
#endregion

namespace Spawn.InputOverlay.UI.Windows
{
    public partial class OverlayWindow : Window
    {
        #region Ctor
        public OverlayWindow() => InitializeComponent();
        #endregion

        #region Events
        #region OnMouseDown
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
        #endregion

        #region OnWindowLoaded
        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            //Add bindings later so WindowStartupLocation works

            Binding widthBinding = new Binding("WindowWidth")
            {
                Mode = BindingMode.TwoWay
            };

            Binding heightBinding = new Binding("WindowHeight")
            {
                Mode = BindingMode.TwoWay
            };

            SetBinding(WidthProperty, widthBinding);
            SetBinding(HeightProperty, heightBinding);
        }
        #endregion
        #endregion
    }
}
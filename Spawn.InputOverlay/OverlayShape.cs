#region Using
using System.ComponentModel;
#endregion

namespace Spawn.InputOverlay
{
    public enum OverlayShape
    {
        [Description("Hidden")] None,
        Eye,
        [Description("Cat Eye")] CatEye,
        Trapez
    }
}
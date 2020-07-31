#region Using
using NLog;
using System.Reflection;
using System.Windows;
#endregion

namespace Spawn.InputOverlay
{
    public partial class App : Application
    {
        #region Logger
        private static readonly Logger s_logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Static Fields
        public static string AppName => Assembly.GetExecutingAssembly().GetName().Name;
        public static string AppVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
        #endregion

        #region Ctor
        public App()
        {
            Exit += (s, e) =>
            {
                s_logger.Info("=========================================");
            };

            s_logger.Info("=========== {0} v{1} ===========", AppName, AppVersion);
            s_logger.Info("=========================================");
        }
        #endregion
    }
}
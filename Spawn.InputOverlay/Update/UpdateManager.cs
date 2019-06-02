#region Using
using NLog;
using Spawn.InputOverlay.Properties;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
#endregion

namespace Spawn.InputOverlay.Update
{
    public static class UpdateManager
    {
        #region Static Fields
        private static readonly Logger s_logger = LogManager.GetCurrentClassLogger();

        private static readonly Regex s_versionRegex;
        private static readonly Regex s_updateTextRegex;

        private static WebClient s_webClient;

        private static bool s_blnIsChecking;
        #endregion

        #region Static Properties
        #region Info
        public static UpdateInfo Info { get; private set; }
        #endregion
        #endregion

        #region Custom Events
        public static event DownloadProgressChangedEventHandler DownloadProgressChanged;

        public static event DownloadDataCompletedEventHandler DownloadCompleted;
        #endregion

        #region Static Ctor
        static UpdateManager()
        {
            s_versionRegex = new Regex("[0-9]\\.[0-9]{1,2}\\.?[0-9]{0,2}");
            s_updateTextRegex = new Regex("<div class=\"markdown-body\">\\s*?<p>(?<Content>.*)</p>\\s*?</div>");
        }
        #endregion

        #region CheckForUpdatesAsync
        public static async Task<bool> CheckForUpdatesAsync()
        {
            bool blnRet = false;

            if (!s_blnIsChecking)
            {
                s_blnIsChecking = true;

                Info = null;

                try
                {
                    s_logger.Info("Checking GitHub for updates...");

                    HttpWebRequest request = WebRequest.CreateHttp($"{Settings.Default.GitHubBaseUrl}/releases/latest");

                    using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Match versionMatch = s_versionRegex.Match(response.ResponseUri.AbsoluteUri);

                            if (versionMatch.Success)
                            {
                                Version newVersion = new Version(versionMatch.Value);

#if DEBUG
                                blnRet = newVersion > new Version(0, 0);
#else
                                blnRet = newVersion > System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
#endif

                                if (blnRet)
                                {
                                    Info = new UpdateInfo(newVersion);

                                    string strResult;

                                    using (WebClient webClient = new WebClient())
                                        strResult = await webClient.DownloadStringTaskAsync(response.ResponseUri);

                                    //prepare for regex check
                                    strResult = strResult.Trim().Replace("\n", string.Empty).Replace("\r", string.Empty);

                                    Match updateTextMatch = s_updateTextRegex.Match(strResult);

                                    if (updateTextMatch.Success)
                                        Info.ReleaseNotes = updateTextMatch.Groups["Content"].Value.Replace("<br>", Environment.NewLine);

                                    s_logger.Info("Update available");
                                }
                                else
                                {
                                    s_logger.Info("No update available");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //No internet connection or github down
                    s_logger.Error(ex, "Couldn't perform update check!");
                }

                s_blnIsChecking = false;
            }

            return blnRet;
        }
        #endregion

        #region Download
        public static void Download(Version version)
        {
            string strVersionString = version.ToString(3);

            using (s_webClient = new WebClient())
            {
                s_webClient.DownloadProgressChanged += (s, e) => DownloadProgressChanged?.Invoke(s, e);

                s_webClient.DownloadDataCompleted += (s, e) =>
                {
                    if (!e.Cancelled)
                    {
                        DownloadCompleted?.Invoke(s, e);

                        s_logger.Trace("Download completed");
                    }
                    else
                    {
                        s_logger.Trace("User canceled download");
                    }
                };

                s_webClient.DownloadDataAsync(new Uri($"{Settings.Default.GitHubBaseUrl}/releases/download/{strVersionString}/Spawn.InputOverlay.zip"));
            }
        }
        #endregion

        #region CancelDownload
        public static void CancelDownload() => s_webClient?.CancelAsync();
        #endregion
    }
}
#region Using
using System;
#endregion

namespace Spawn.InputOverlay.Update
{
    public class UpdateInfo
    {
        #region Properties
        #region Version
        public Version Version { get; }
        #endregion

        #region ReleaseNotes
        public string ReleaseNotes { get; set; }
        #endregion

        #region HasReleaseNotes
        public bool HasReleaseNotes => !string.IsNullOrEmpty(ReleaseNotes);
        #endregion
        #endregion

        #region Ctor
        public UpdateInfo(Version version, string releaseNotes = "")
        {
            Version = version;
            ReleaseNotes = releaseNotes;
        }
        #endregion
    }
}
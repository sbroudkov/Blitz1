using System;
using System.Windows.Forms;

namespace Blitz1
{
    public class UCListView : ListView
    {
        protected override void OnHandleCreated(EventArgs iArgs)
        {
            base.OnHandleCreated(iArgs);
            if (!this.DesignMode &&
                (Environment.OSVersion.Platform == PlatformID.Win32NT) &&
                (Environment.OSVersion.Version.Major >= 6))
            {
                NativeMethods.SetWindowTheme(this.Handle, "explorer", null);
            }
        }
    }
}

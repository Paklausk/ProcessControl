using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProcessControl.FileClickActions
{
    public static class WindowsFileClickActions
    {
        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public static void RegisterForFileExtension(string extension, string applicationPath)
        {
            RegistryKey classesReg = Registry.CurrentUser.CreateSubKey("Software\\Classes\\" + extension);
            classesReg.CreateSubKey("shell\\open\\command").SetValue("", applicationPath + " %1");
            classesReg.Close();

            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }
        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
    }
}

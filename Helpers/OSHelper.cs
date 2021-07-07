using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProcessControl.Helpers
{
    public static class OSHelper
    {
        static IEnumerable<OSPlatform> _platforms = new[] { OSPlatform.Windows, OSPlatform.Linux, OSPlatform.OSX, OSPlatform.FreeBSD };

        public static OSPlatform GetOSType()
        {
            foreach (var platform in _platforms)
                if (RuntimeInformation.IsOSPlatform(platform))
                    return platform;
            throw new Exception("OS platform could't be determined");
        }
    }
}

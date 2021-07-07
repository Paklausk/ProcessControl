using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ProcessControl.Objects
{
    public class ExtendedProcess : Process
    {
        public string StoredProcessName { get; private set; }
        public new bool Start()
        {
            bool result = base.Start();
            StoredProcessName = ProcessName;
            return result;
        }
    }
}

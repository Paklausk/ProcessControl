using System;
using System.Collections.Generic;
using System.Text;

namespace ProcessControl.Objects
{
    public class ProcessData
    {
        public string Process { get; set; }
        public string Arguments { get; set; }

        public ProcessData(string process, string arguments)
        {
            Process = process;
            Arguments = arguments;
        }
    }
}

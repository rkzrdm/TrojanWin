using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TrojanWin.Core
{
    public class ConfigurationProcess : ProcessBase
    {
        public ConfigurationProcess() : this("config.json")
        {
        }
        public ConfigurationProcess(string path) : base(path)
        {
        }

        protected override void MakeProcessObject()
        {
            Process = new Process();
            Process.StartInfo.FileName = Path;
            Process.StartInfo.UseShellExecute = true;
        }
    }
}

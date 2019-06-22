using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TrojanWin.Core
{
    public abstract class ProcessBase
    {
        public string Path { get; protected set; }
        public Process Process { get; protected set; }

        public ProcessBase(string path)
        {
            Path = path;
        }

        public bool IsPathValid { get => System.IO.File.Exists(Path); }

        public void Start()
        {
            if (!IsPathValid) throw new System.IO.FileNotFoundException();

            MakeProcessObject();
            Process.Start();
            AfterStart();
        }

        public void Stop()
        {
            if (Process == null) return;
            if (!Process.HasExited) Process.Kill();
            Process = null;
        }

        protected abstract void MakeProcessObject();
        protected virtual void AfterStart() { }
    }
}

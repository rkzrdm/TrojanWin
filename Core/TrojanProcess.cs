using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Messaging;
using TrojanWin.Model;

namespace TrojanWin.Core
{
    public class TrojanProcess
    {
        public string Path { get; private set; }
        public Process Process { get; private set; }

        public TrojanProcess(string path)
        {
            Path = path;
        }

        public bool IsPathValid { get => System.IO.File.Exists(Path); }

        public void Start()
        {
            if (!IsPathValid) throw new System.IO.FileNotFoundException();

            MakeProcessObject();
            Process.Start();
            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();
        }

        public void Stop()
        {
            if (Process == null) return;
            Process.Kill();
            Process = null;
        }

        private void MakeProcessObject()
        {
            Process = new Process();
            Process.StartInfo.FileName = Path;
            Process.StartInfo.RedirectStandardOutput = true;
            Process.StartInfo.RedirectStandardError = true;
            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.CreateNoWindow = true;
            Process.OutputDataReceived += (sender, e) =>
            {
                if (e.Data == null) return;
                Messenger.Default.Send(new NewProcessOutputMessage(e.Data));
            };
            Process.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data == null) return;
                Messenger.Default.Send(new NewProcessOutputMessage(e.Data));
            };
        }
    }
}

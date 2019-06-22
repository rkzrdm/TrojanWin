using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Messaging;
using TrojanWin.Model;

namespace TrojanWin.Core
{
    public class TrojanProcess : ProcessBase
    {
        public TrojanProcess() : this("trojan.exe")
        {
        }
        public TrojanProcess(string path) : base(path)
        {
        }

        protected override void MakeProcessObject()
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

        protected override void AfterStart()
        {
            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();
        }
    }
}

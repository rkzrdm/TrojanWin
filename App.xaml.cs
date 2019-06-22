using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using TrojanWin.Core;
using TrojanWin.Model;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace TrojanWin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public NotifyIcon NotifyIcon { get; set; }
        public ProcessBase TrojanProcess { get; set; }

        public void StartTrojanProcess()
        {
            TrojanProcess.Start();
            Messenger.Default.Send(new NewAppLogMessage("Trojan started"));
        }

        public void RestartTrojanProcess()
        {
            if (TrojanProcess != null) TrojanProcess.Stop();
            CheckProcessPathAndCreate();
            TrojanProcess.Start();
            Messenger.Default.Send(new NewAppLogMessage("Trojan restarted"));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CheckOnlyOneInstanceRunning();
            if (!shouldContinue) return;
            CheckProcessPathAndCreate();
            if (!shouldContinue) return;
            InitializeTray();
            RegisterTrayContextMenu();
        }

        private Mutex singleInstanceMutex;
        private bool shouldContinue;
        private void CheckOnlyOneInstanceRunning()
        {
            singleInstanceMutex = new Mutex(true, "TrojanWinSingleInstance", out shouldContinue);
            if (!shouldContinue)
            {
                MessageBox.Show("Please do not start app twice (ノ｀Д)ノ");
                Current.Shutdown(-2);
            }
        }

        private void CheckProcessPathAndCreate()
        {
            TrojanProcess = new TrojanProcess();
            if (!TrojanProcess.IsPathValid)
            {
                MessageBox.Show("Could not find Trojan, please ensure that trojan.exe exists in the current directory");
                shouldContinue = false;
                Current.Shutdown(-1);
            }
        }

        private void InitializeTray()
        {
            NotifyIcon = new NotifyIcon()
            {
                Text = "TrojanWin\nDouble click to open window",
                Visible = true,
                Icon = TrojanWin.Properties.Resources.Icon
            };
            NotifyIcon.MouseDoubleClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (MainWindow.Visibility == Visibility.Visible)
                    {
                        MainWindow.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        MainWindow.Visibility = Visibility.Visible;
                    }
                }
            };
        }
        private void RegisterTrayContextMenu()
        {
            NotifyIcon.ContextMenu = new ContextMenu(new MenuItem[]
            {
                new MenuItem("Exit", (sender, e) =>
                {
                    Current.Shutdown(0);
                })
            });
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (TrojanProcess != null) TrojanProcess.Stop();
            base.OnExit(e);
        }
    }
}

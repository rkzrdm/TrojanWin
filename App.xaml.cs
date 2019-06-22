using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
        public TrojanProcess TrojanProcess { get; set; }

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

            CheckProcessPathAndCreate();
            InitializeTray();
            RegisterTrayContextMenu();
        }

        private void CheckProcessPathAndCreate()
        {
            TrojanProcess = new TrojanProcess("trojan.exe");
            if (!TrojanProcess.IsPathValid)
            {
                MessageBox.Show("Could not find Trojan, please ensure that trojan.exe exists in the current directory");
                Current.Shutdown(-1);
                return;
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
            TrojanProcess.Stop();
            base.OnExit(e);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace TrojanWin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public NotifyIcon NotifyIcon { get; set; }
        
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            InitializeTray();
            RegisterTrayContextMenu();
        }

        private void InitializeTray()
        {
            NotifyIcon = new NotifyIcon()
            {
                Text = "A Simple Trojan GUI Client for Windows",
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
                new MenuItem("退出", (sender, e) =>
                {
                    Current.Shutdown();
                })
            });
        }
    }
}

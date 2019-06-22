using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Windows;
using TrojanWin.Model;

namespace TrojanWin.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            RestartTrojanProcess = new RelayCommand(DoRestartTrojanProcess);
            Messenger.Default.Register<NewProcessOutputMessage>(this, (messenger) => 
            {
                TrojanLog += messenger.Output + "\n";
            });
            Messenger.Default.Register<NewAppLogMessage>(this, (messenger) =>
            {
                AppLog += $"[{messenger.Time}] {messenger.Content}\n";
            });
            (Application.Current as App).StartTrojanProcess();
        }

        private string trojanLog;
        public string TrojanLog
        {
            get { return trojanLog; }
            set { Set(() => TrojanLog, ref trojanLog, value); }
        }

        private string appLog;
        public string AppLog
        {
            get { return appLog; }
            set { Set(() => AppLog, ref appLog, value); }
        }

        public RelayCommand RestartTrojanProcess { get; set; }
        private void DoRestartTrojanProcess()
        {
            TrojanLog = "";
            (Application.Current as App).RestartTrojanProcess();
        }
    }
}
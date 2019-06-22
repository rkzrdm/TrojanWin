using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Messaging;

namespace TrojanWin.Model
{
    public class NewAppLogMessage : MessageBase
    {
        public string Content { get; set; }
        public DateTime Time { get; set; }

        public NewAppLogMessage(string content)
        {
            Content = content;
            Time = DateTime.Now;
        }
    }
}

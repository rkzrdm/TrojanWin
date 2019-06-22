using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight.Messaging;

namespace TrojanWin.Model
{
    public class NewProcessOutputMessage : MessageBase
    {
        public string Output { get; set; }

        public NewProcessOutputMessage(string output)
        {
            Output = output;
        }
    }
}

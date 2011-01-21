using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTerminal.Models
{
    public class ResultObject
    {
        public ResultObject()
        {
            this.ClearScreen = false;
            DisplayArray = new List<DisplayObject>();
            IsPassword = false;
            ScrollToBottom = true;
        }

        public bool ClearScreen { get; set; }
        public List<DisplayObject> DisplayArray { get; set; }
        public string EditText { get; set; }
        public Context CurrentContext { get; set; }
        public bool IsPassword { get; set; }
        public bool ScrollToBottom { get; set; }

        public void ClearScreenAndContext()
        {
            this.ClearScreen = true;
            this.CurrentContext.Deactivate();
        }
    }
}
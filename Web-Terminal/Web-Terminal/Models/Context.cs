using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTerminal.Models
{
    public class Context
    {
        public Context()
        {
            this.Forced = false;
            this.Enabled = false;
        }

        public string CurrentController { get; set; }
        public string CurrentCommand { get; set; }
        public List<string> CurrentArgs { get; set; }
        public string PreviousCommand { get; set; }
        public List<string> PreviousArgs { get; set; }
        public string CommandString { get; set; }
        public bool Forced { get; set; }
        public bool PreviouslyForced { get; set; }
        public bool Enabled { get; set; }
        public bool PreviouslyEnabled { get; set; }
        public string ContextDisplay { get; set; }
        public string PreviousContextDisplay { get; set; }

        public void Activate(string command, List<string> args, bool forced, string contextDisplay)
        {
            this.CurrentCommand = command;
            this.CurrentArgs = args;
            this.Forced = forced;
            this.Enabled = true;
            this.ContextDisplay = contextDisplay;
        }

        public void Deactivate()
        {
            this.CurrentCommand = null;
            this.CurrentArgs = null;
            this.Forced = false;
            this.Enabled = false;
            this.ContextDisplay = null;
        }

        public void Backup()
        {
            this.PreviousCommand = this.CurrentCommand;
            this.PreviousArgs = this.CurrentArgs;
            this.PreviouslyForced = this.Forced;
            this.PreviouslyEnabled = this.Enabled;
            this.PreviousContextDisplay = this.ContextDisplay;
        }

        public void Restore()
        {
            this.CurrentCommand = this.PreviousCommand;
            this.CurrentArgs = this.PreviousArgs;
            this.Forced = this.PreviouslyForced;
            this.Enabled = this.PreviouslyEnabled;
            this.ContextDisplay = this.PreviousContextDisplay;
        }
    }
}
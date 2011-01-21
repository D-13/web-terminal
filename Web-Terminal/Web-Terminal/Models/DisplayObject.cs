using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTerminal.Models
{
    public class DisplayObject
    {

        public DisplayObject()
        {
            this.Type = true;
            this.PlaySound = true;
            this.Text = " ";
            this.Speed = 10;
            this.DelayAfter = 0;
            this.InsertAfter = "";
            this.CaretChar = "\u2588";
            this.DelayBefore = 0;
            this.InsertBefore = "&gt; ";
            this.Style = "";
        }

        public bool PlaySound { get; set; }
        public bool Type { get; set; }
        public string Text { get; set; }
        public int Speed { get; set; }
        public int DelayAfter { get; set; }
        public string InsertAfter { get; set; }
        public string CaretChar { get; set; }
        public int DelayBefore { get; set; }
        public string InsertBefore { get; set; }
        public string Style { get; set; }

        public static DisplayObject UnknownCommand(string command)
        {
            return BuildTemplateDisplayObject(String.Format("'{0}' is not a recognized command or is not available in the current context.", command));
        }

        public static DisplayObject UnknownSubcommand(string subcommand)
        {
            return BuildTemplateDisplayObject(String.Format("'{0}' is not a recognized subcommand or is not available in the current context.", subcommand));
        }

        public static DisplayObject BlankLine
        {
            get 
            {
                DisplayObject blankLine = BuildTemplateDisplayObject("");
                blankLine.Type = false;
                return blankLine;
            }
        }

        public static DisplayObject Canceled { get { return BuildTemplateDisplayObject("Action canceled."); } }
        public static DisplayObject InvalidArguments { get { return BuildTemplateDisplayObject("Invalid arguments supplied."); } }
        public static DisplayObject CommandTakesNoArguments { get { return BuildTemplateDisplayObject("Command takes no arguments."); } }
        public static DisplayObject NotAuthorized { get { return BuildTemplateDisplayObject("Not authorized."); } }
        public static DisplayObject NotImplemented { get { return BuildTemplateDisplayObject("Method or operation not implemented yet."); } }
        public static DisplayObject BuildTemplateDisplayObject(string text)
        {
            return new DisplayObject()
            {
                Text = text,
                InsertAfter = "<br />",
            };
        }
    }
}
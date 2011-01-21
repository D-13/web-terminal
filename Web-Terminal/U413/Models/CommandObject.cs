using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTerminal.Models
{
    public class CommandObject
    {
        public string Command { get; set; }
        public List<string> Args { get; set; }
    }
}
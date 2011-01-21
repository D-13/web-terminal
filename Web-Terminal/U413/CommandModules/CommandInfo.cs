using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebTerminal.Models;

namespace WebTerminal.CommandModules
{
    // This attribute is used to include a description of the command it is attached to.
    // This description is displayed in the HELP menu.
    // A name is not required because the name is read from the name of the method.

    class CommandInfo : Attribute
    {
        public CommandInfo(string description)
        {
            this.Description = description;
        }

        public string Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebTerminal.Models;

namespace WebTerminal.CommandModules
{
    // This attribute is to be used at the top of every command module.
    // It is the category that displays to separate commands in the HELP menu.

    class CommandCategoryAttribute : Attribute
    {
        public CommandCategoryAttribute(string category)
        {
            this.Category = category;
        }

        public string Category { get; set; }
    }
}

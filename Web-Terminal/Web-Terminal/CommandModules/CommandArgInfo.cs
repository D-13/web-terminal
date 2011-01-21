using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebTerminal.Models;

namespace WebTerminal.CommandModules
{
    // This attribute takes in up to four arguments.
    //
    // Name - The name of the command parameter.
    // Description - The description that will show up in the detailed HELP menu for the command.
    // Required - A boolean denoting whether or not the parameter is required when using the command.
    // SortOrder - Optionally included for use with a command that has multiple parameters. The sort
    //             order determines in what order the parameters will show up in the HELP menu.

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    class CommandArgInfo : Attribute
    {
        public CommandArgInfo(string name, string description, bool required)
        {
            this.Name = name;
            this.Description = description;
            this.SortOrder = 0;
            this.Required = required;
        }

        public CommandArgInfo(string name, string description, bool required, int sortOrder)
        {
            this.Name = name;
            this.Description = description;
            this.SortOrder = sortOrder;
            this.Required = required;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
        public bool Required { get; set; }
    }
}

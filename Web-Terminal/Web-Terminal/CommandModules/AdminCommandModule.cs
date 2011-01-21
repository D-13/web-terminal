using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using System.Web.Security;
using System.Reflection;
using WebTerminal.Models;
using System.Text;
using WebTerminal.Utilities;

namespace WebTerminal.CommandModules
{
    [CommandCategory("-= Admin Commands =-")]
    public class AdminCommandModule : UserCommandModule
    {
        public AdminCommandModule(Controller c, ResultObject ro) : base(c, ro) { }

        #region -= ADMINTEST =-

        // This is an example command showing the ability to add commands to a user in a
        // specific role.
        [CommandInfo("Example administrator command.")]
        public ResultObject ADMINTEST(List<string> args)
        {
            result.DisplayArray.Add(new DisplayObject
            {
                Text = "This is an example command to show the use of command modules that inherit from existing command modules. You simply load the appropriate command module based on the user's status. Whatever modules it inherits from beneathe it will be included.",
                InsertAfter = "<br />"
            });
            return result;
        }

        #endregion
    }
}
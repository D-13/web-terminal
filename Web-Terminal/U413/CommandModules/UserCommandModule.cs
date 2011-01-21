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
    [CommandCategory("-= User Commands =-")]
    public class UserCommandModule : CommandModule
    {
        public UserCommandModule(Controller c, ResultObject ro) : base(c, ro) { }

        #region -= INITIALIZE =-

        // This is the same as the visitor command module INITIALIZE but instead of displaying
        // a welcome message, it informs them they are logged into the terminal. This way if
        // they navigate away from the site then come back and forgot they were logged in, they
        // will immediatly know instead of typing LOGIN only to find out they already are.
        public ResultObject INITIALIZE(List<string> args)
        {
            result.DisplayArray.Add(new DisplayObject
            {
                Text = "You are logged in as " + controller.User.Identity.Name + ".",
                InsertAfter = "<br />",
                Speed = 25
            });
            return result;
        }

        #endregion

        #region -= LOGOUT =-

        [CommandInfo("Logs out the current authenticated user.")]
        public ResultObject LOGOUT(List<string> args)
        {
            if (args.Count == 0)
                LOGOUT_Execute();
            else
                result.DisplayArray.Add(DisplayObject.InvalidArguments);
            return result;
        }

        // Clear any existing context and deauthenticate the user.
        private void LOGOUT_Execute()
        {
            result.ClearScreenAndContext();
            FormsAuthentication.SignOut();
            result.DisplayArray.Add(new DisplayObject()
            {
                Text = "You have been logged out.",
                InsertAfter = "<br />"
            });
        }

        #endregion

        #region -= LOGIN =-

        // This is included for the same reason that the LOGOUT method is included in visitor command module.
        public ResultObject LOGIN(List<string> args)
        {
            result.DisplayArray.Add(new DisplayObject
            {
                Text = "You are already logged in as " + controller.HttpContext.User.Identity.Name + ".",
                InsertAfter = "<br />"
            });

            return result;
        }

        #endregion

        #region -= MUTE =-

        // Unfinished command to mute the terminal sounds. Requires data storage to store the user's preference.
        [CommandInfo("Mutes/Unmutes the terminal.")]
        [CommandArgInfo("ON/OFF", "Specify to enable or disable the terminal sounds.", true)]
        public ResultObject MUTE(List<string> args)
        {
            if (args.Count == 1)
                MUTE_Execute(args);
            else
                result.DisplayArray.Add(DisplayObject.InvalidArguments);

            return result;
        }

        private void MUTE_Execute(List<string> args)
        {
            // Add code to modify your data source to enable or disable MUTE for the current user.
            result.DisplayArray.Add(DisplayObject.NotImplemented);
        }

        #endregion
    }
}
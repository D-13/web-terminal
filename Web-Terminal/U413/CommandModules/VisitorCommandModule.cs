using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using System.Web.Security;
using System.Reflection;
using WebTerminal.Models;

namespace WebTerminal.CommandModules
{
    [CommandCategory("-= Visitor Commands =-")]
    public class VisitorCommandModule : CommandModule
    {
        public VisitorCommandModule(Controller c, ResultObject ro) : base(c, ro) { }

        #region -= INITIALIZE =-

        // This command contains no HELP attributes because it is an internal command.
        // The TerminalFramework javascript file sends this command once it is loaded.
        // Anything in the INITIALIZE method will be run when the web-terminal is first
        // loaded.
        //
        // NOTE: Users could still type "initialize" and execute this command. It just
        // doesn't show up in the HELP menu.
        public ResultObject INITIALIZE(List<string> args)
        {
            List<string> welcomeMessages = new List<string>()
            {
                "Welcome to the open-source Web-Terminal project.",
                "As you may or may not be aware, this project has been derrived from the U413.com source code.",
                "Things related specifically to U413 have been removed and this project customized to help you with integrating a web terminal into your own projects.",
                " ",
                "Please use the \"TUTORIAL\" command to see a live run down of the features and functions of the Web-Terminal project.",
                "The source code is well commented as well, should you get lost while navigating it."
            };

            foreach (string msg in welcomeMessages)
            {
                result.DisplayArray.Add(new DisplayObject
                {
                    Text = msg,
                    InsertAfter = "<br />"
                });
            }

            return result;
        }

        #endregion

        #region -= LOGIN =-

        // This is a sample command that tages advantage of forced context if prompts are used.
        // It does not verify username and password against any data source so if you run this
        // sample project you will be able to login as any name you want it it will succeed.
        //
        // The [CommandInfo()] and [CommandArgInfo()] attributes are read by the HELP command
        // using reflection. This allows you to easily document new commands you add and have
        // them easily show up in the HELP menu.

        // The main command method usually contains an IF statement that determines how many parameters
        // were included and what types of parameters they are in order to determine what actions to take.
        [CommandInfo("Authenticates the user.")]
        [CommandArgInfo("Username", "The username chosen during sign up.", false, 0)]
        [CommandArgInfo("Password", "The password chosen during sign up.", false, 1)]
        public ResultObject LOGIN(List<string> args)
        {
            if (args.Count == 0)
                LOGIN_PromptUsername(args);
            else if (args.Count == 1)
                LOGIN_PromptPassword(args);
            else if (args.Count == 2)
                LOGIN_Execute(args);
            else
                result.DisplayArray.Add(DisplayObject.InvalidArguments);

            return result;
        }

        // This method adds a prompt to the result displayarray and activates a forced context.
        private void LOGIN_PromptUsername(List<string> args)
        {
            result.ClearScreen = false;
            result.DisplayArray.Add(new DisplayObject
            {
                Text = "Enter Username",
                InsertAfter = "<br />"
            });
            result.CurrentContext.Activate("LOGIN", args, true, "LOGIN USERNAME");
        }

        // This method adds a prompt to the result displayarray and activates a forced context.
        private void LOGIN_PromptPassword(List<string> args)
        {
            result.DisplayArray.Add(new DisplayObject
            {
                Text = "Enter Password",
                InsertAfter = "<br />"
            });
            result.CurrentContext.Activate("LOGIN", args, true, "LOGIN PASSWORD");
            result.IsPassword = true;
        }

        // This method performs the actual login operation.
        private void LOGIN_Execute(List<string> args)
        {
            string username = args[0];
            FormsAuthentication.SetAuthCookie(username, false);
            result.DisplayArray.Add(new DisplayObject
            {
                Text = "You are now logged in as " + username + ".",
                InsertAfter = "<br />"
            });
            result.CurrentContext.Deactivate();
        }

        #endregion

        #region -= LOGOUT =-

        // There is no HELP attributes because LOGOUT isn't actually available in the
        // visitor command module. It is included so that we can inform the user they
        // are not logged in if they type it, rather than have it confuse them by saying
        // command unknown.
        public ResultObject LOGOUT(List<string> args)
        {
            result.DisplayArray.Add(new DisplayObject
            {
                Text = "You are not logged in.",
                InsertAfter = "<br />"
            });

            return result;
        }

        #endregion
    }
}
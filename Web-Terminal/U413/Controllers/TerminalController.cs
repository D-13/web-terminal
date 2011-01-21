using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using WebTerminal.Models;
using System.Reflection;
using WebTerminal.CommandModules;
using WebTerminal.Utilities;

namespace WebTerminal.Controllers
{
    public class TerminalController : Controller
    {
        public ViewResult TerminalFramework()
        {
            return View();
        }

        #region -= Execute Command =-

        // ExecuteCommand is an action method that responds to AJAX calls.
        // It binds a JSON object containing properties passed from the client to a Context object.
        // This method determines if the Context object contains current context commands and arguments.
        // It then looks to see if the Context is forced or passive.
        //
        // If the Context is forced then all commands will be overwritten by the context command.
        // Any new command/args passed in will be added to the context as additional arguments.
        //
        // If the Context is passive then it will first check existing commands for a match.
        // If it finds a command corresponding to the passed in command then it will invoke it.
        // If it does not find a corresponding command then it will overwrite the commands with data
        // from the context and try again, making the new command and it's args become additional args
        // to the context command.
        //
        // The method will finally return a result object containing client-side properties to be
        // acted upon as well as an array of DisplayObjects to be iterated and cool-typed to the client.
        [ObjectFilter(Param = "currentContext", RootType = typeof(Context))]
        public JsonResult ExecuteCommand(Context currentContext)
        {
            string commandString = currentContext.CommandString;
            ResultObject result = null;
            CommandObject commandObject = new CommandObject { Command = commandString };

            if ((currentContext.Enabled) && (!currentContext.Forced))
            {
                currentContext.Backup();
                commandObject = LoadCommandObject(commandString, null);
                result = InvokeCommand(currentContext, commandObject);
                if (result == null)
                {
                    commandObject = LoadCommandObject(commandString, currentContext);
                    result = InvokeCommand(currentContext, commandObject);
                }
            }
            else if ((currentContext.Enabled) && (currentContext.Forced))
            {
                commandObject = LoadCommandObject(commandString, currentContext);
                result = InvokeCommand(currentContext, commandObject);
            }
            else if (!currentContext.Enabled)
            {
                currentContext.Backup();
                commandObject = LoadCommandObject(commandString, null);
                result = InvokeCommand(currentContext, commandObject);
            }

            if (result == null)
            {
                result = new ResultObject { CurrentContext = currentContext };
                result.DisplayArray.Add(DisplayObject.UnknownCommand(commandObject.Command));
            }

            // The following statement turns the sound off if the user has muted the sound.
            // However, this sample project does not have real data storage from which to pull
            // the user's preference, so this is commented out for now.
            //
            // if (User.Identity.IsAuthenticated)
            //     if (!currentUser.Sound)
            //         result.DisplayArray.ForEach(x => x.PlaySound = false);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // This method will generate a CommandObject containing a command and a list of arguments.
        // If a Context is passed in and is not null then it will overwrite the command and arguments
        // with the contextual command and arguments, adding the new command and arguments as additional
        // arguments on the context.
        private CommandObject LoadCommandObject(string commandString, Context currentContext)
        {
            string command = commandString;
            List<string> args = new List<string>();
            if ((currentContext != null) && (command.ToUpper() != "CANCEL"))
            {
                command = currentContext.CurrentCommand;
                if (currentContext.CurrentArgs != null)
                    args.AddRange(currentContext.CurrentArgs);
                if (commandString.Contains(' '))
                {
                    char[] c = new char[] { ' ' };
                    args.AddRange(commandString.Split(c, StringSplitOptions.RemoveEmptyEntries).ToList());
                }
                else
                    args.Add(commandString);
            }
            else
            {
                if (commandString.Contains(' '))
                {
                    command = commandString.Remove(commandString.IndexOf(' '));
                    commandString = commandString.Remove(0, commandString.IndexOf(' '));
                    char[] c = new char[] { ' ' };
                    args.AddRange(commandString.Split(c, StringSplitOptions.RemoveEmptyEntries).ToList());
                }
            }
            return new CommandObject
            {
                Command = command,
                Args = args
            };
        }

        // This method loads the various command modules based on the current user's status.
        private ResultObject InvokeCommand(Context currentContext, CommandObject commandObject)
        {
            MethodInfo commandMethods;
            Object commandModule;
            ResultObject result = new ResultObject { CurrentContext = currentContext };

            if (User.Identity.IsAuthenticated)
            {
                // Load custom role command modules.
                if (User.Identity.Name.ToLower() == "admin")
                {
                    commandModule = new AdminCommandModule(this, result);
                    commandMethods = typeof(AdminCommandModule).GetMethod(commandObject.Command.ToUpper());
                }
                else
                {
                    commandModule = new UserCommandModule(this, result);
                    commandMethods = typeof(UserCommandModule).GetMethod(commandObject.Command.ToUpper());
                }
            }
            else
            {
                // Load visitor command module.
                commandModule = new VisitorCommandModule(this, result);
                commandMethods = typeof(VisitorCommandModule).GetMethod(commandObject.Command.ToUpper());
            }

            // If a command was found, invoke its method. If not, return null.
            if (commandMethods != null)
                result = (ResultObject)commandMethods.Invoke(commandModule, new object[] { commandObject.Args });
            else
                result = null;

            return result;
        }

        #endregion
    }
}

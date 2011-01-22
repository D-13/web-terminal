using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTerminal.Models;
using System.Reflection;

namespace WebTerminal.CommandModules
{
    [CommandCategory("-= Global Commands =-")]
    public class CommandModule
    {
        protected Controller controller;
        protected ResultObject result;

        public CommandModule(Controller c, ResultObject ro)
        {
            this.controller = c;
            this.result = ro;
        }

        #region -= TUTORIAL =-

        // This command is here to help you along as you try to adapt the Web-Terminal source to suite your needs.
        [CommandInfo("Begins a demo/tutorial on the Web-Terminal project and its functionality.")]
        [CommandArgInfo("-i", "Instantly loads the tutorial instead of typing it.", false)]
        public ResultObject TUTORIAL(List<string> args)
        {
            List<string> messages = new List<string>()
            {
                "Welcome to the Web-Terminal tutorial.",
                "In this tutorial I will attempt to demonstrate the functionality of the Web-Terminal open-source project.",
                " ",
                "There are three main sections to this tutorial:",
                "1) The basic fundamentals for how Web-Terminal works and interacts with the client.",
                "2) The various objects passed back and forth from client to server and vice versa, as well as their properties.",
                "3) How Web-Terminal deals with utilities, markup, and on-the-fly client scripting.",
                " ",
                " ",
                " ",
                "-= BASIC FUNDAMENTALS =-",
                " ",
                "At its core Web-Terminal is a combination of the following technologies, starting with server-side then moving to client-side:",
                "<server-side>",
                "- ASP.NET MVC 2.0",
                "- C# with extensive use of reflection and inheritance.",
                "<client-side>",
                "- Javascript/JQuery",
                "- Ajax",
                " ",
                "U413's source adds onto the web terminal project and takes advantage of several more technologies that are out of the scope of this tutorial as they deal with database modeling, language integrated queries, and UI libraries.",
                " ",
                "When Web-Terminal first loads, the request is passed to the TerminalController and the TerminalFramework action method. This method loads the only required markup view: TerminalFramework.aspx",
                "TerminalFramework.aspx is parsed and rendered, then shunted back to the client via a standard web response. All communication after that point, between client and server, is done via Ajax.",
                "The TerminalFramework is responsible for loading all client-side scripting and styling. It pulls in a list of javascript libraries such as TerminalFramework, soundmanager2, json2, JQuery, and various JQuery plugins.",
                "TerminalFramework.js is where the client-side magic happens. It is responsible for displaying text using Cool-Type™ style (printing one letter at a time), interacting with sound manager to play the sounds, as well as sending and receiving commands.",
                " ",
                "When you type a command and press enter, it is sent via AJAX to the server where TerminalController is again loaded. However this time the request does not go to TerminalFramework action method, it goes to the ExecuteCommand action method.",
                "The ExecuteCommand method is where much of the magic begins. TerminalFramework.js passes in JSON that models a Web-Terminal custom Context object.",
                "Once the context object is instantiated, it is then parsed for data. ExecuteCommand takes the object and determines a couple things:",
                "- Does the context object contain a contexed command/arguments?",
                "- If so, is the context forced or passive?",
                " ",
                "If the context contains no command then the command sent by the user is the command that gets executed. It builds a CommandObject containing the user's command and all paramaters they sent with it.",
                "If the context does contain a command and is a \"forced context\", then it will override the command sent by the user. The command that gets executed will be the contexted command. It then treats the command and parameters sent by the user as additional parameters on the context command.",
                "For example: If the user types LOGIN and presses enter, they will see \"LOGIN USERNAME\" displayed next to the command line. LOGIN is now their current context command. That is because the login command sets a forced context when asking for the username.",
                "Now anything they type will be passed as a parameter of LOGIN, instead of being treated as it's own command. This allows them to simply type their username after the prompt. The command that actually gets executed on the server is LOGIN [Username] because it read LOGIN from the context, and added the last command sent by the user (their username) as a parameter of LOGIN.",
                "The only way to get out of a forced context is to either satisfy the context's requirements (i.e. giving it what it asks for) or using the CANCEL command. Cancel is a special-case command that busts any existing context.",
                " ",
                "If the context does contain a command and is a \"passive context\", then it will not override the command sent by the user. Instead it will look to see if what the user sent is an existing command. If it is, then that command is executed. If it's not, then before it sends \"Command Unknown\" it will override the command with the contexted command and try again to find a matching command.",
                "If it finds a command at that point then it does the same thing as the forced context, tacks the sent command onto the contexted command as a parameter. An example would be, viewing a topic on U413. If you viewed a topic, then TOPIC would be your current contexted command, but it would be a passive context. Which means you could still type other commands like LOGOUT and they would work.",
                "But if it did not find commands, then it would pass the command to the contexted command as a paramter (ex: using REPLY, which was actually a parameter of TOPIC). This is how you could just type REPLY, instead of REPLY [topicID].",
                " ",
                "Once a CommandObject is built, it then tries to execute the command. In order to execute the command it first has to know what commands are available. The way it does this is by looking at the current user, deciding what level of security they have, and building what is called a \"Command Module\" which contains all the available commands for that user.",
                "There are a few modules built into this project:",
                "- CommandModule.cs",
                "- VisitorCommandModule.cs",
                "- UserCommandModule.cs",
                "- AdminCommandModule.cs",
                " ",
                "CommandModule.cs is the base class for all the others. Every module you create MUST inherit from CommandModule. In this project CommandModule contains four commands:",
                "- TUTORIAL - which simply displays this tutorial.",
                "- CLS - which clears the screen.",
                "- CANCEL - which cancels the current forced context.",
                "- HELP - which uses reflection to look at itself (i.e. the CommandModule it is part of) to see what commands it has.",
                " ",
                "Because these commands are part of the base command module class, they are available to any user no matter their security status. To give commands to users with different security levels you will have to create custom command modules that inherit from CommandModule.",
                "VisitorCommandModule inherits from CommandModule. Within this class it adds a few more commands, such as LOGIN. The ExecuteCommand method will determine if the current user is authenticated, if not then it loads the visitor command module. ExecuteCommand will NEVER load only the base command module. It will always load something that inherits from it.",
                "Object commandModule = new VisitorCommandModule(this, result);",
                "The generic commandModule object could be loaded with any of the modules. Then, because VisitorCommandModule inherits from CommandModule, that commandModule object would contain all command methods on both CommandModule and VisitorCommandModule.",
                "If the user is authenticated it would load UserCommand module which is the same concept, just a different module. The really cool part comes in when, say, you get an administrator who logs in. In that instance ExecuteCommand would load AdminCommandModule which inherits from UserCommandModule which inherits from CommandModule. That would build a CommandModuleObject that contained all the parameters of the base command module, the user command moduel, AND the admin command moduel. Pretty slick huh?",
                "After that it just uses reflection to find commands on the object and either execute them, or display command unknown in the event they aren't found.",
                "You'll notice that the command modules take in two parameters, \"this\" and \"result\". Result is a custom type ResultObject and is passed in so that commands can modify it and add to it. In the next section I will describe the custom types in Web-Terminal and their properties so you will better understand their uses.",
                "\"this\" is a reference to the controller currently building the module. This allows you to access things like the related Request and Response objects, as well as the User object to verify authentication.",
                " ",
                " ",
                " ",
                "-= OBJECTS & PROPERTIES =-",
                " ",
                "Web-Terminal contains the following custom types:",
                "- Context.cs",
                "- CommandObject.cs",
                "- DisplayObject.cs",
                "- ResultObject.cs",
                " ",
                "The first one you ought to know about is the Context object, as it is the only object that gets passed in a circle, back and forth between the client and server.",
                "The Context object has too many properties to go into in this tutorial but here is the basic functions it performs. It stores several things; when the client sends the context to the server it tacks on the command string (a string containing everything sent by the user) as part of the context object. When ExecuteCommand gets ahold of this object it reads currentContext.CommandString to find what the user sent. It then does the appropriate parsing to separate the command from its parameters.",
                "CommandString is the only thing that client script ever modifies before it sends it to the server. All the other properties are set on the server and sent to the client for both storage, and to be read by certain client functions.",
                "The context object is used to store, everything related to the current context, as well as everything related to a previous context. It stores one previous context so that it can restore a passive context after a forced context is finished.",
                "An example would be in U413 when you were on a topic. Your current passive context was TOPIC, and you decide to type REPLY then press enter. Your new \"forced\" context is TOPIC [id] REPLY and it's asking you to type your reply. After you finish that reply it would be horrible to set your context to nothing in order to clear the forced prompt context, because that would mean, in order for the user to type REPLY again they would have to reactivate their passive TOPIC context, or use the full command TOPIC [id] REPLY. That's just not acceptable, so before it activates the forced context reply prompt, it backs up the current passive context to the context object. Then after the forced context is done it calls a restore method on the context object which overwrites the current context with the previous context. Viola!",
                "There are a few methods on the context object you should be aware of:",
                "- Activate(string command, List<string> args, bool forced, string contextDisplay)",
                "- Deactivate()",
                "- Backup()",
                "- Restore()",
                " ",
                "Use the activate method to set a current context when you need to. The string contextDisplay is what appears next to the user's command line. This allows you to customize the display to say things like \"Enter Password\" instead of showing the actual context of LOGIN someName PASSWORD which just looks silly to the user.",
                "Deactivate clears any existing context, including passive ones.",
                "Backup backs up the current context into its previous context storage.",
                "Restore overwrites the current context with values from its previous context storage, essentially restoring the old context.",
                " ",
                "The CommandObject is a basic object containing args and a command. It only exists for easy passing of this information between methods in the controller. It is nothing that this tutorial needs to go into in any detail.",
                " ",
                "The next object you should be aware of is the DisplayObject. A DisplayObject is ONE line of text to be typed to the screen. I will now show you a display object.",
                "I am one display object, from the start of this sentence to the end.",
                "A display object contains several properties that can be modified. One thing to note about a display object is, if the object is set to Cool-Type™ to the screen, then it cannot include HTML directly in the Text property of the object. If you need to include HTML you can set the Type property to false.",
                "The following are the properties of a display object that can be modified as you please:",
                "- Type - boolean that disables typing, usually for passing in a view containing HTML markup.",
                "- PlaySound - boolean that disables the typing sound for that object only. (If the user sets his preferences to mute, then there is a piece of commented out code in the TerminalController that will loop through all display objects going to the client and set their PlaySound properties to false.",
                "- Text - string property that contains the text to be displayed to the screen. This can also contain a view rendered to a string as long as Type is set to false. I will go into views and how to render them later in this tutorial.",
                "- Speed - This is the duration, in milliseconds, between the typing of the letters. The default is 1. NOTE: DO NOT set this to 0 as 0 stops the typing in Internet Explorer. 1 is the minimum you should set this to. If you want slower typing, increase the number. It only applies if Type is true.",
                "- DelayAfter - This is the duration, in milliseconds, to pause after typing the line of text.",
                "- DelayBefore - This is the duration, in milliseconds, to pause before typing the line of text.",
                "- InsertAfter - This is usually some HTML to be inserted (not typed) after the line. It is usually \"<br />\" so that the cursor drops to the next line before rendering the next display object.",
                "- InsertBefore - This one is not modified as often, though it can be if you need to. You could put opening HTML tags in InsertBefore and closing HTML tags in InsertAfter and they would wrap the typed line of text. It's default value is \"&gt; \" which displays the angle bracket you usually see before text in the terminal.",
                "- Style - There is a span tag wrapped around every display object that is rendered. This string property is CSS styles that should be applied to the line. Usually easier to just do Style=\"color: red;\" on the display object, then to put an opening span tag in the InsertBefore property and a closing tag in the InsertAfter. Especially because you have to remember, if you overwrite the InsertBefore property and you still want the angle bracket, you'll have to include it when you set it.",
                "- CaretChar - This is a string property that sets the character used for the caret (cursor) that is displayed while Cool-Typing™ and blinks while pausing before or after the display object. It's default is \"\\u2588\" which is the escape sequence for the alt+255 character. You can include HTML in the CharetChar property, so you could wrap the character in a span tag and do CSS styling on it.",
                " ",
                "That is all the properties you will need to know when setting up display objects. Most of the properties have decent defaults so that you don't have to set EVERY property every time you make a display object. In fact, the minimum you need to do U413 style typing is this:",
                "DisplayObject myCustomDisplayObject = new DisplayObject",
                "{",
                "     Text = \"My Cool-Typed™ line of text.\",",
                "     InsertAfter = \"<br />\"",
                "};",
                " ",
                "That is what most of my display objects look like. Now I will show you where to put those display objects in order to get them to the screen, and I'll do that by introducing you to the next custom type.",
                " ",
                "The next type you need to know about is ResultObject. Result objects only make a one way trip, from the server to the client. ExecuteCommand returns a Result Object.",
                "A ResultObject has a lot of little properties for the client to read, but the two I want to discuss first are the CurrentContext and the DisplayArray.",
                "The CurrentContext property of ResultObject houses the context object that the client passed in, and commands possibly modified. The client-script reads this property and stores the context object so it can be transmitted back to the server with the next request.",
                "The DisplayArray property contains an array of display objects. It reads this property and sends the array to the Cool-Type™ function which will iterate through the array and do it's magic to type the text out to the screen.",
                "Remember how we passed in a result object when building the command module? Well that is how the commands have access to the result object. For example: if you want to add a HELLOWORLD command that simply prints \"Hello World!\" to the screen, you would do the following:",
                "1. Create your HELLOWORLD command, including attributes if you want it to show up in the help menu. Make sure you put it in the module where you want it to show up. It would probably be easiest to modify it to look like one of the other command methods. I usually surround my command methods in #regions because it lets me collapse them and sometimes I have multiple private methods that branch off of the main command method so I can divide up work; it's nice to have them grouped together in a #region.",
                "2. Once you have your HELLOWORLD method set up then add the following:",
                "result.DisplayArray.Add(new DisplayObject",
                "{",
                "     Text = \"Hello World!\",",
                "     InsertAfter = \"<br />\"",
                "});",
                " ",
                "Then just make sure the method ends with \"RETURN result;\" and viola! You now have your own custom command :)",
                " ",
                "Here is a list of properties on the Result Object that you will need to know about:",
                "- ClearScreen - a boolean that tells the client script to clear the screen. All CLS does is flip this to true, but you can make any command clear the screen. For example, in U413 using the TOPIC command would clear the screen before it rendered the topic markup.",
                "- IsPassword - a boolean that tells the client script to change the CLI to a password field. Very useful for password prompts.",
                "- ScrollToBottom - a boolean that tells the client script to scroll to the bottom of the page after loading. By default this is true, to better emulate a terminal. However, you can set this to false if you do not want the scrolling to happen. To go back to the U413 TOPIC example, when you open a topic this is set to false so that you stay at the top of the page when the topic loads and you can read the topic without having to scroll back up to see it.",
                "- EditText - string to be inserted into the command line by the client script. This was useful for the edit command on U413. The command would populate this property with the text of the post so that you could edit it on the client.",
                "- DisplayArray - Already described above. Contains an array of DisplayObjects to pass to the Cool-Type™ function.",
                "- CurrentContext - Already described above. Contains the current context object to be passed back to the server again on the next request.",
                " ",
                "There is also one method on the result object that was purely for convenience. I often had a need to both clear the screen and clear the current context at the same time. So I put a method on the result object called ClearScreenAndContext();, it does just what it says; it sets ClearScreen to true and calls Deactivate() on the context object.",
                " ",
                " ",
                " ",
                "-= UTILITIES & MARKUP =-",
                " ",
                "Obviously, sometimes you can't do what you want with just text and the Cool-Type™ client script; you need to be able to code complex html to do things like displaying tables of data, etc...",
                "If you had to type HTML in strings within the code that would be a huge mess and would be absolutely horrible. Besides, this is MVC right? So why can't we take advantage of views and partial views?",
                "The answer is....you can! The difficulty when first beginning this project was figuring out how to send back views and/or partial views when the ExecuteCommand function was returning JsonResult. After much head scratching I finally came up with a beautiful utility that renders views and partial views to a string!",
                "It actually builds the view exactly the same way MVC would normally build it, except it does it before returning view. Instead it does renders it to a string.",
                " ",
                "I placed this utility in Utilities.Common. The method signature looks like this:",
                "public static string RenderPartialViewToString(Controller controller, string viewName, object model)",
                " ",
                "Now, if you are not familiar with ASP.NET MVC and you don't understand what views are then I'm afraid that is far beyond the scope of this tutorial. You will need to go do some learning before you use the Web-Terminal project. I highly recommend Pro ASP.NET MVC 2 Framework, Second Edition by Steve Sanderson. Excellent read.",
                " ",
                "So given that you are still reading and are obviously fluent in ASP.NET MVC 2, you can see the beauty in this little method. Now when you build your display object you can set Type to false and call this utility, passing in your view name and your model. Just be sure to place all your views in Views/Terminal or Views/Shared.",
                "Another cool thing about views is that they can contain in-line javascript. That was how I made it possible for boards and topics on U413 to refresh every so often. They included AJAX script that would contact custom action methods and retrieve the latest updates.",
                " ",
                "Here is an example of loading a partial view into a display object and sending it to the client. The model contains a sample string list of animals that the view will iterate over and render into a table. It also contains a button with client script to show you an example of adding custom javascript in a view.",
                " ",
                "$view$",
                " ",
                " ",
                " ",
                "You have reached the end of the tutorial. I wish you happy coding! Enjoy."
            };
            if ((args.Count > 0) && (args[0].ToLower() == "-i"))
            {
                result.ScrollToBottom = false;
                foreach (string msg in messages)
                {
                    if (msg == "$view$")
                    {
                        List<string> sampleList = new List<string>()
                        {
                            "Dog",
                            "Cat",
                            "Bear",
                            "Bird",
                            "Fox",
                            "Fish",
                            "Lion",
                            "Tiger",
                            "Wolf",
                            "Snake"
                        };

                        result.DisplayArray.Add(new DisplayObject
                        {
                            Type = false,
                            Text = Utilities.Common.RenderPartialViewToString(controller, "PartialViewExample", sampleList),
                            InsertAfter = "<br />"
                        });
                    }
                    else
                    {
                        result.DisplayArray.Add(new DisplayObject
                        {
                            Text = HttpUtility.HtmlEncode(msg),
                            InsertAfter = "<br />",
                            Type = false
                        });
                    }
                }
            }
            else
            {
                foreach (string msg in messages)
                {
                    if (msg == "$view$")
                    {
                        List<string> sampleList = new List<string>()
                        {
                            "Dog",
                            "Cat",
                            "Bear",
                            "Bird",
                            "Fox",
                            "Fish",
                            "Lion",
                            "Tiger",
                            "Wolf",
                            "Snake"
                        };

                        result.DisplayArray.Add(new DisplayObject
                        {
                            Type = false,
                            Text = Utilities.Common.RenderPartialViewToString(controller, "PartialViewExample", sampleList),
                            InsertAfter = "<br />"
                        });
                    }
                    else
                    {
                        result.DisplayArray.Add(new DisplayObject
                        {
                            Text = msg,
                            InsertAfter = "<br />",
                            Speed = 35,
                            DelayAfter = 2000
                        });
                    }
                }
            }
            result.ClearScreen = true;
            return result;
        }

        #endregion

        #region -= CLS =-

        // This command sets a simple property in the result object which is read by the
        // javascript and clears the screen.
        [CommandInfo("Clears the screen.")]
        public ResultObject CLS(List<string> args)
        {
            if (args.Count == 0)
                result.ClearScreenAndContext();
            else
                result.DisplayArray.Add(DisplayObject.InvalidArguments);
            return result;
        }

        #endregion

        #region -= CANCEL =-

        // CANCEL is a special command in that it is never overridden by a context.
        // If this command is received, by itself with no args, then it will cancel the current context.
        // It will try to restore the previous (usually passive) context.
        [CommandInfo("Cancels the current action.")]
        public ResultObject CANCEL(List<string> args)
        {
            if (args.Count == 0)
                if ((!result.CurrentContext.Enabled) || (!result.CurrentContext.Forced))
                    result.DisplayArray.Add(DisplayObject.InvalidArguments);
                else
                    CANCEL_Execute();
            else
                result.DisplayArray.Add(DisplayObject.InvalidArguments);
            return result;
        }

        private void CANCEL_Execute()
        {
            result.CurrentContext.Restore();
            result.DisplayArray.Add(DisplayObject.Canceled);
        }

        #endregion

        #region -= HELP =-

        // HELP is the most unique command of all and should not be modified unless you know what you are doing.
        // It takes advantage of reflection to read all the commands within the instantiated command module.
        // It then looks for their describing attributes and displays them accordingly.
        [CommandInfo("Displays the help menu.")]
        [CommandArgInfo("Command", "Specify to display detailed information about a specific command.", false)]
        public ResultObject HELP(List<string> args)
        {
            if (args.Count == 0)
                HELP_Execute();
            else if (args.Count == 1)
                HELP_Command(args[0]);
            else
                result.DisplayArray.Add(DisplayObject.InvalidArguments);
            return result;
        }

        private void HELP_Command(string command)
        {
            MethodInfo method = this.GetType().GetMethod(command.ToUpper());
            if ((method != null) && (method.GetCustomAttributes(typeof(CommandInfo), false).Length > 0))
            {
                CommandInfo cmdInfo = (CommandInfo)method.GetCustomAttributes(typeof(CommandInfo), false)[0];
                var cmdArgInfoList = from x in (CommandArgInfo[])method.GetCustomAttributes(typeof(CommandArgInfo), false)
                                     orderby x.SortOrder
                                     select x;
                List<DisplayObject> argDetails = new List<DisplayObject>();
                string argString = "";
                foreach (CommandArgInfo cmdArgInfo in cmdArgInfoList)
                {
                    string parameterInfo = cmdArgInfo.Name + " - ";
                    if (cmdArgInfo.Required)
                        parameterInfo += "Required parameter. ";
                    else
                        parameterInfo += "Optional parameter. ";
                    parameterInfo += cmdArgInfo.Description;
                    argDetails.Add(new DisplayObject
                    {
                        Style = "margin-left: 50px;",
                        Text = parameterInfo,
                        InsertAfter = "<br />"
                    });
                    string openBracket = " [";
                    string closeBracket = "]";
                    if (cmdArgInfo.Required)
                    {
                        openBracket = " <";
                        closeBracket = ">";
                    }
                    argString += openBracket + cmdArgInfo.Name + closeBracket;
                }
                result.DisplayArray.Add(DisplayObject.BlankLine);
                result.DisplayArray.Add(new DisplayObject
                {
                    Text = method.Name + argString + " - " + cmdInfo.Description,
                    InsertAfter = "<br />"
                });
                if (argDetails.Count > 0)
                    result.DisplayArray.AddRange(argDetails);
                else
                {
                    DisplayObject noArgs = DisplayObject.CommandTakesNoArguments;
                    noArgs.Style = "margin-left: 50px;";
                    result.DisplayArray.Add(noArgs);
                }
                result.DisplayArray.Add(DisplayObject.BlankLine);
            }
            else
            {
                result.DisplayArray.Add(DisplayObject.UnknownCommand(command));
            }
        }

        private void HELP_Execute()
        {
            result.DisplayArray.Add(DisplayObject.BlankLine);
            result.DisplayArray.Add(new DisplayObject
            {
                Text = "[] denotes optional parameters.",
                InsertAfter = "<br />"
            });
            result.DisplayArray.Add(new DisplayObject
            {
                Text = "<> denotes required parameters.",
                InsertAfter = "<br />"
            });

            var sortedMethods = from x in this.GetType().GetMethods()
                                where x.GetCustomAttributes(typeof(CommandInfo), false).Length > 0
                                orderby ((CommandCategoryAttribute)x.DeclaringType.GetCustomAttributes(typeof(CommandCategoryAttribute), false)[0]).Category ascending, x.Name ascending
                                select x;
            string commandSection = "";
            foreach (MethodInfo mi in sortedMethods)
            {
                if (mi.DeclaringType.Name != commandSection)
                {
                    commandSection = mi.DeclaringType.Name;
                    MemberInfo inf = mi.DeclaringType;
                    CommandCategoryAttribute cca = (CommandCategoryAttribute)inf.GetCustomAttributes(typeof(CommandCategoryAttribute), false)[0];
                    result.DisplayArray.Add(DisplayObject.BlankLine);
                    result.DisplayArray.Add(new DisplayObject
                    {
                        Text = cca.Category,
                        InsertAfter = "<br />",
                        Style = "font-weight: bold;"
                    });
                }
                CommandInfo cmdInfo = (CommandInfo)mi.GetCustomAttributes(typeof(CommandInfo), false)[0];
                var cmdArgInfoList = from x in (CommandArgInfo[])mi.GetCustomAttributes(typeof(CommandArgInfo), false)
                                     orderby x.SortOrder
                                     select x;
                string argString = "";
                foreach (CommandArgInfo cmdArgInfo in cmdArgInfoList)
                {
                    string openBracket = " [";
                    string closeBracket = "]";
                    if (cmdArgInfo.Required)
                    {
                        openBracket = " &lt;";
                        closeBracket = "&gt;";
                    }
                    argString += openBracket + cmdArgInfo.Name.Replace(">", "&gt;").Replace("<", "&lt;") + closeBracket;
                }
                result.DisplayArray.Add(new DisplayObject
                {
                    Type = false,
                    Text = mi.Name + argString + " - " + cmdInfo.Description,
                    InsertAfter = "<br />"
                });
            }
            result.DisplayArray.Add(DisplayObject.BlankLine);
        }

        #endregion
    }
}
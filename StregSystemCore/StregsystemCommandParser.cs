using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StregsystemCore
{
    delegate void AdminCommandHandler(string commandName, string[] args);
    internal class StregsystemCommandParser
    {
        Dictionary<string, AdminCommandHandler> _adminCommands;
        IStregsystem _stregsystem;
        IStregsystemUI _stregsystemUI;
        public StregsystemCommandParser(IStregsystem stregsystem, IStregsystemUI stregsystemUI)
        {
            _stregsystem = stregsystem;
            _stregsystemUI = stregsystemUI;
        }

        public void AddAdminComand(string commandName, AdminCommandHandler adminCommandHandler)
        {
            _adminCommands.Add(commandName, adminCommandHandler);
        }

        public void ParseCommand(string command)
        {
            string[] commandParts = SplitCommand(command)
                .Where(part => part.Length > 0)
                .ToArray();

            if (commandParts.Length == 0)
            {
                //No command
                return;
            }
            if (commandParts.Length == 1)
            {
                //Just username, display user info
                User user = _stregsystem.GetUserByUsername(commandParts[0]);
                if (user == null)
                {
                    _stregsystemUI.DisplayUserNotFound(commandParts[0]);
                    return;
                }
                _stregsystemUI.DisplayUserInfo(user);
            }
        }

        private string[] SplitCommand(string command)
        {
            return command.Split(" ");
        }
    }
}

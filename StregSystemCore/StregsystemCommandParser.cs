using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StregsystemCore
{
    class UserNotFoundException : Exception
    {
        public string GivenUsername { get; private set; }
        public UserNotFoundException(string givenUsername) 
        {
            GivenUsername = givenUsername;
        }
    }

    class ProductNotFoundException : Exception
    {
        public int GivenID { get; private set; }
        public ProductNotFoundException(int givenID)
        {
            GivenID = givenID;
        }
    }

    class BadArgumentException : Exception
    {
        public int ArgumentIndex { get; private set; }
        public BadArgumentException(int argumentIndex, string message) : base(message)
        {
            ArgumentIndex = argumentIndex;
        }
    }

    class AdminCommandNotFoundException : Exception
    {
        public string GivenCommand { get; private set; }
        public AdminCommandNotFoundException(string givenCommand)
        {
            GivenCommand = givenCommand;
        }
    }

    delegate void AdminCommandHandler(string commandName, string[] args);
    delegate void PurchaseProductHandler(User customer, BaseProduct product, int amount);
    internal class StregsystemCommandParser
    {
        public event PurchaseProductHandler PurchaseProduct;
        Dictionary<string, AdminCommandHandler> _adminCommands;
        IStregsystem _stregsystem;
        IStregsystemUI _stregsystemUI;
        public StregsystemCommandParser(IStregsystem stregsystem, IStregsystemUI stregsystemUI)
        {
            _stregsystem = stregsystem;
            _stregsystemUI = stregsystemUI;
            _adminCommands = new Dictionary<string, AdminCommandHandler>();
        }

        public void AddAdminCommand(string commandName, AdminCommandHandler adminCommandHandler)
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
                return;
            }

            if (commandParts[0].StartsWith(":"))
            {
                string commandName = commandParts[0].Substring(1);
                if (!_adminCommands.TryGetValue(commandName, out AdminCommandHandler handler))
                {
                    throw new AdminCommandNotFoundException(commandName);
                }
                handler(commandName, commandParts[1..]);
                return;
            }

            if (commandParts.Length == 1)
            {
                UserInfoCommand(commandParts[0]);
                
            }
            else if (commandParts.Length == 2)
            {
                PurchaseCommand(commandParts[0], commandParts[1]);
            } 
            else if (commandParts.Length == 3)
            {
                MultiPurchaseCommand(commandParts[0], commandParts[1], commandParts[2]);
            }
            else
            {
                _stregsystemUI.DisplayTooManyArgumentsError(command);
            }
        }

        public User GetUserOrThrow(string username)
        {
            User user = _stregsystem.GetUserByUsername(username);
            if (user == null)
            {
                throw new UserNotFoundException(username);
            }
            return user;
        }

        public BaseProduct GetProductOrThrow(string productId, int argIndex)
        {
            if (!int.TryParse(productId, out int idArg))
            {
                throw new BadArgumentException(argIndex, "Product Id needs to be a valid integer");
            }

            BaseProduct targetProduct = _stregsystem.GetProductByID(idArg);
            if (targetProduct == null)
            {
                //It might be debateable whether or not the system should tell the user that
                //The product does not exist or that it isnt activated
                throw new ProductNotFoundException(idArg);
            }
            return targetProduct;
        }

        public int ParseCountOrThrow(string givenCount, int argIndex)
        {
            if (!int.TryParse(givenCount, out int count) || count < 1)
            {
                throw new BadArgumentException(argIndex, "count needs to be a valid positive integer");
            }
            return count;
        }

        private void UserInfoCommand(string username)
        {
            User user = GetUserOrThrow(username);
            _stregsystemUI.DisplayUserInfo(user);
            return;
        }

        private void PurchaseCommand(string username, string productId)
        {
            User user = GetUserOrThrow(username);
            
            BaseProduct product = GetProductOrThrow(productId, 2);

            PurchaseProduct?.Invoke(user, product, 1);
        }

        private void MultiPurchaseCommand(string username, string count, string productId)
        {
            User user = GetUserOrThrow(username);

            //There is an issue here getting the argument index if it changes elsewhere
            BaseProduct product = GetProductOrThrow(productId, 3);

            int parsedCount = ParseCountOrThrow(count, 2);

            PurchaseProduct?.Invoke(user, product, parsedCount);
        }

        private string[] SplitCommand(string command)
        {
            return command.Split(" ");
        }
    }
}

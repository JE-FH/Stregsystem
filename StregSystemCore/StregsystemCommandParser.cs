using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StregsystemCore
{
    delegate void AdminCommandHandler(string commandName, string[] args);
    delegate void PurchaseProductHandler(User customer, BaseProduct product, int amount);
    internal class StregsystemCommandParser
    {
        //Should we use on?
        public event PurchaseProductHandler PurchaseProduct;
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
                return;
            } 
            else if (commandParts.Length == 1)
            {
                UserInfoCommand(commandParts[0]);
                
            }
            else if (commandParts.Length == 2)
            {
                PurchaseCommand(commandParts[0], commandParts[1]);
                return;
            } 
            else if (commandParts.Length == 3)
            {

            }
        }

        //TODO: der er overlappende funktionalitet i alle de her commands, så der skal laves noget generelt
        private void UserInfoCommand(string username)
        {
            User user = _stregsystem.GetUserByUsername(username);
            if (user == null)
            {
                _stregsystemUI.DisplayUserNotFound(username);
                return;
            }
            _stregsystemUI.DisplayUserInfo(user);
            return;
        }

        private void PurchaseCommand(string username, string productId)
        {
            User user = _stregsystem.GetUserByUsername(username);
            if (user == null)
            {
                _stregsystemUI.DisplayUserNotFound(username);
                return;
            }

            if (!int.TryParse(productId, out int idArg))
            {
                _stregsystemUI.DisplayGeneralError("Product id (argument 2) needs to be a number");
                return;
            }

            BaseProduct targetProduct = _stregsystem.GetProductByID(idArg);
            if (targetProduct == null)
            {
                //It might be debateable whether or not the system should tell the user that
                //The product does not exist or that it isnt activated
                _stregsystemUI.DisplayProductNotFound(productId);
            }

            PurchaseProduct?.Invoke(user, targetProduct, 1);
        }

        private void MultiPurchaseCommand(string username, string amount, string productId)
        {
            User user = _stregsystem.GetUserByUsername(username);
            if (user == null)
            {
                _stregsystemUI.DisplayUserNotFound(username);
                return;
            }

            if (!int.TryParse(productId, out int idArg))
            {
                _stregsystemUI.DisplayGeneralError("Product id (argument 3) needs to be an integer");
                return;
            }

            BaseProduct targetProduct = _stregsystem.GetProductByID(idArg);
            if (targetProduct == null)
            {
                _stregsystemUI.DisplayProductNotFound(productId);
            }

            if (!int.TryParse(amount, out int amountArg) || amountArg < 1)
            {
                _stregsystemUI.DisplayGeneralError("amount (argument 2) needs to be a valid positive integer");
                return;
            }

            PurchaseProduct?.Invoke(user, targetProduct, amountArg);
        }

        private string[] SplitCommand(string command)
        {
            return command.Split(" ");
        }
    }
}

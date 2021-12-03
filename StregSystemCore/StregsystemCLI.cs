using StregsystemCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StregsystemCore
{
    public class StregsystemCLI : IStregsystemUI
    {
        private bool _running;
        private IStregsystem _stregsystem;
        public event StregsystemEvent CommandEntered;

        public StregsystemCLI(IStregsystem stregsystem)
        {
            _stregsystem = stregsystem;
            _running = false;
            _stregsystem.UserBalanceWarning += DisplayUserBalanceWarning;
        }

        private void DisplayUserBalanceWarning(User user, decimal balance)
        {
            Console.WriteLine($"{user.Username} has a low balance {balance}");
        }

        public void Close()
        {
            _running = false;
        }

        public void DisplayAdminCommandNotFoundMessage(string adminCommand)
        {
            Console.WriteLine($"Given admin command \"{adminCommand}\" does not exist");
        }

        public void DisplayGeneralError(string errorString)
        {
            Console.WriteLine($"Error: {errorString}");
        }

        public void DisplayInsufficientCash(User user, BaseProduct product)
        {
            Console.WriteLine($"You only have {user.Balance}, but {product.Name} costs {product.Price}");
        }
        public void DisplayInsufficientCash(User user, BaseProduct product, int count)
        {
            Console.WriteLine($"You only have {user.Balance}, but {count} x {product.Name} costs {product.Price * count}");
        }

        public void DisplayProductNotFound(string product)
        {
            Console.WriteLine($"Requested product {product} was not found");
        }

        public void DisplayTooManyArgumentsError(string command)
        {
            Console.WriteLine($"Too many arguments given for {command}");
        }

        public void DisplayUserBuysProduct(BuyTransaction transaction)
        {
            Console.WriteLine($"{transaction.User.Username} bought {transaction.Product.Name} for {transaction.Amount}");
        }

        public void DisplayUserBuysProduct(int count, BuyTransaction transaction)
        {
            Console.WriteLine($"{transaction.User.Username} bought {count} x {transaction.Product.Name} for {transaction.Amount}");
        }

        public void DisplayUserInfo(User user)
        {
            Console.WriteLine($"{user} has {user.Balance} stregdollars");
            var shownTransactions = _stregsystem.GetTransactions(user, 10)
                .ToList();
            
            shownTransactions.Sort((a, b) => b.Date.CompareTo(a.Date));
            
            foreach (Transaction transaction in shownTransactions)
            {
                Console.WriteLine(transaction.ToString());
            }

            if (user.Balance < 50)
            {
                DisplayUserBalanceWarning(user, user.Balance);
            }
        }

        public void DisplayUserNotFound(string username)
        {
            Console.WriteLine($"User \"{username}\" does not exist");
        }

        public void Start()
        {
            _running = true;
            while (_running)
            {
                foreach (BaseProduct product in _stregsystem.ActiveProducts)
                {
                    Console.WriteLine(product);
                }
                Console.Write("> ");
                string rawCommand = Console.ReadLine();
                CommandEntered?.Invoke(rawCommand);
                Console.Write("\n\n");
            }
        }
    }
}
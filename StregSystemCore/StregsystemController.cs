using System;

namespace StregsystemCore
{
    public class StregsystemController
    {
        IStregsystem _stregsystem;
        IStregsystemUI _stregsystemUI;
        StregsystemCommandParser _stregsystemCommandParser;

        public StregsystemController(IStregsystem stregsystem, IStregsystemUI stregsystemUI)
        {
            _stregsystem = stregsystem;
            _stregsystemUI = stregsystemUI;
            _stregsystemUI.CommandEntered += CommandEntered;
            _stregsystemCommandParser = new StregsystemCommandParser(_stregsystem, _stregsystemUI);
            _stregsystemCommandParser.PurchaseProduct += PurchaseProduct;
            
            _stregsystemCommandParser.AddAdminCommand("quit", QuitCommand);
            _stregsystemCommandParser.AddAdminCommand("q", QuitCommand);
            
            _stregsystemCommandParser.AddAdminCommand("activate", (name, args) =>
                SetProductActiveCommand(args, true)
            );
            _stregsystemCommandParser.AddAdminCommand("deactivate", (name, args) =>
                SetProductActiveCommand(args, false)
            );
            
            _stregsystemCommandParser.AddAdminCommand("crediton", (name, args) => 
                SetProductCreditCommand(args, true)
            );
            _stregsystemCommandParser.AddAdminCommand("creditoff", (name, args) => 
                SetProductCreditCommand(args, false)
            );
            
            _stregsystemCommandParser.AddAdminCommand("addcredits", AddCreditsCommand);
        }

        private void AddCreditsCommand(string commandName, string[] args)
        {
            if (args.Length != 2)
            {
                throw new BadArgumentException(0, "expected exactly two argument");
            }

            User user = _stregsystemCommandParser.GetUserOrThrow(args[0]);
            if (!decimal.TryParse(args[1], out decimal creditsToAdd))
            {
                throw new BadArgumentException(1, "expected valid number");
            }

            _stregsystem.AddCreditsToAccount(user, creditsToAdd);
        }

        private void SetProductActiveCommand(string[] args, bool newActiveState)
        {
            if (args.Length != 1)
            {
                throw new BadArgumentException(0, "expected exactly one argument");
            }

            BaseProduct targetProduct = _stregsystemCommandParser.GetProductOrThrow(args[0], 1);
            if (targetProduct is SeasonalProduct)
            {
                throw new BadArgumentException(1, "target product cannot be seasonal");
            }
            targetProduct.Active = newActiveState;
        }

        private void SetProductCreditCommand(string[] args, bool canBeBoughtOnCredit)
        {
            if (args.Length != 1)
            {
                throw new BadArgumentException(0, "expected exactly one argument");
            }
            
            BaseProduct targetProduct = _stregsystemCommandParser.GetProductOrThrow(args[0], 1);
            if (targetProduct is SeasonalProduct)
            {
                throw new BadArgumentException(1, "target product cannot be seasonal");
            }

            targetProduct.CanBeBoughtOnCredit = canBeBoughtOnCredit;
        }

        private void QuitCommand(string commandname, string[] args)
        {
            _stregsystemUI.Close();
        }

        private void CommandEntered(string rawCommand)
        {
            try
            {
                _stregsystemCommandParser.ParseCommand(rawCommand);
            }
            catch (UserNotFoundException ex)
            {
                _stregsystemUI.DisplayUserNotFound(ex.GivenUsername);
            }
            catch (ProductNotFoundException ex)
            {
                _stregsystemUI.DisplayProductNotFound(ex.GivenID.ToString());
            }
            catch (BadArgumentException ex)
            {
                _stregsystemUI.DisplayGeneralError($"Invalid argument {ex.ArgumentIndex}, {ex.Message}");
            }
            catch (AdminCommandNotFoundException ex)
            {
                _stregsystemUI.DisplayAdminCommandNotFoundMessage(ex.GivenCommand);
            }
            catch (InsufficientCreditsException ex)
            {
                _stregsystemUI.DisplayInsufficientCash(ex.User, ex.Product, ex.Count);
            }
        }

        private void PurchaseProduct(User customer, BaseProduct product, int count)
        {
            if (!product.CanBeBoughtOnCredit && count * product.Price > customer.Balance)
            {
                throw new InsufficientCreditsException(customer, product, count);
            }
            BuyTransaction lastTransaction = null;
            try
            {
                for (int i = 0; i < count; i++)
                {
                    lastTransaction = _stregsystem.BuyProduct(customer, product);
                }
            } 
            catch (ProductInactiveException ex)
            {
                throw new ProductNotFoundException(ex.Product.ID);
            }
            _stregsystemUI.DisplayUserBuysProduct(count, lastTransaction);
        }
    }
}

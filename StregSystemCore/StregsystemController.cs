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
        }

        private void CommandEntered(string rawCommand)
        {
            _stregsystemCommandParser.ParseCommand(rawCommand);
        }

        private void PurchaseProduct(User customer, BaseProduct product, int amount)
        {
            if (!product.CanBeBoughtOnCredit && amount * product.Price > customer.Balance)
            {
                _stregsystemUI.DisplayInsufficientCash(customer, product, amount);
            }
            BuyTransaction lastTransaction = null;
            try
            {
                for (int i = 0; i < amount; i++)
                {
                    lastTransaction = _stregsystem.BuyProduct(customer, product);
                }
            } 
            catch (ProductInactiveException ex)
            {
                _stregsystemUI.DisplayProductNotFound(product.ID.ToString());
                return;
            }
            _stregsystemUI.DisplayUserBuysProduct(amount, lastTransaction);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StregsystemCore {
    public class BuyTransaction : Transaction {
        public BaseProduct Product {get; private set;}

        public BuyTransaction(int id, User user, BaseProduct product) : base(id, user, product.Price) {
            Product = product;
        }

        public override void Execute() {
            if (!Product.CanBeBoughtOnCredit && User.Balance < Amount) {
                throw new InsufficientCreditsException(User, Product);
            }
            
            if (!Product.Active) {
                throw new ProductInactiveException(User, Product);
            }

            User.Balance -= Amount;
            base.Execute();
        }
        public override string ToString() {
            return $"";
        }
    }
}

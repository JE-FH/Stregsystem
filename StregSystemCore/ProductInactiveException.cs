using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StregsystemCore
{
    //TODO: There could be some generalization between InsufficentCreditsException and this class
    public class ProductInactiveException : Exception {
        public User User { get; private set; }

        public BaseProduct Product { get; private set; }

        public ProductInactiveException(User user, BaseProduct product) : base("Production inavtive") {
            User = user;
            Product = product;
        }
    }
}

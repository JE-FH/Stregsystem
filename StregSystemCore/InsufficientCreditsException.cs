using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StregsystemCore
{
    public class InsufficientCreditsException : Exception {
        public User User { get; private set; }

        public BaseProduct Product { get; private set; }

        public InsufficientCreditsException(User user, BaseProduct product) : base("Insufficient streg dollars") {
            User = user;
            Product = product;
        }
    }
}

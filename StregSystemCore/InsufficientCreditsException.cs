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
        public int Count { get; private set; }

        public InsufficientCreditsException(User user, BaseProduct product) : this(user, product, 1) { }
        public InsufficientCreditsException(User user, BaseProduct product, int count) : base("Insufficient streg dollars") {
            User = user;
            Product = product;
            Count = count;
        }
    }
}

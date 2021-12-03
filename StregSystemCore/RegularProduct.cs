using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StregsystemCore
{
    public class RegularProduct : BaseProduct {
        public override bool Active { get; set; }

        public RegularProduct(int id, string name, decimal price, bool canBeBoughtOnCredit, bool active) : base(id, name, price, canBeBoughtOnCredit) {
            Active = active;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StregsystemCore {
    public abstract class BaseProduct {
        private int _id;
        public int ID { 
            get => _id; 
            private set {
                if (value < 1) {
                    throw new ArgumentOutOfRangeException("Product ID cannot be less than 1");
                }
                _id = value;
            }
        }

        private string _name;
        public string Name { 
            get => _name; 
            set {
                if (value == null) {
                    throw new ArgumentNullException("Product Name cannot be null");
                }
                _name = value;
            }
        }

        public decimal Price { get; set; }

        public abstract bool Active { get; set; }

        public bool CanBeBoughtOnCredit { get; set; }

        public override string ToString() {
            return $"{ID} {Name} {Price}";
        }

        public BaseProduct(int id, string name, decimal price, bool canBeBoughtOnCredit) {
            ID = id;
            Name = name;
            Price = price;
            CanBeBoughtOnCredit = canBeBoughtOnCredit;
        }
    }
}

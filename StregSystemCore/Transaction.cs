using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StregsystemCore {
    public class Transaction {
        public int ID { get; private set; }

        public User User { get; private set; }

        public DateTime Date { get; private set; }

        public decimal Amount { get; private set; }

        public Transaction(int id, User user, decimal amount) {
            ID = id;
            User = user;
            Amount = amount;
        }
        
        public override string ToString() {
            return $"TX{ID}\t{User.Username}\t{Amount}\t{Date}";
        }

        virtual public void Execute() {
            Date = DateTime.Now;
        }
    }
}

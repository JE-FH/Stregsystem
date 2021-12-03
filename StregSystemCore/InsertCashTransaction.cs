using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StregsystemCore
{
    public class InsertCashTransaction : Transaction {
        public InsertCashTransaction(int id, User user, decimal amount) : base(id, user, amount) {

        }

        public override void Execute() {
            User.Balance += Amount;
            base.Execute();
        }

        public override string ToString() {
            return $"Insert cash transaction {Amount} {User} {Date} {ID}";
        }
    }
}

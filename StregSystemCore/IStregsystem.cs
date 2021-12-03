using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StregsystemCore
{
    public interface IStregsystem {
        IEnumerable<BaseProduct> ActiveProducts { get; }
        InsertCashTransaction AddCreditsToAccount(User user, int amount);
        BuyTransaction BuyProduct(User user, BaseProduct product);
        BaseProduct GetProductByID(int id);
        IEnumerable<Transaction> GetTransactions(User user, int count);
        IEnumerable<User> GetUsers(Func<User, bool> predicate);
        User GetUserByUsername(string username);
        event UserBalanceNotification UserBalanceWarning;
    }
}

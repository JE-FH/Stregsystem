using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StregsystemCore {
    public class Stregsystem : IStregsystem {
        static readonly Regex HTMLTagPattern = new Regex("<[^>]+>");
        private TransactionLogger _transactionLogger;

        int _lastTransactionId;
        
        List<Transaction> _transactions;

        List<BaseProduct> _products;

        List<User> _users;
        public event UserBalanceNotification UserBalanceWarning;

        public Stregsystem() {
            _lastTransactionId = 1;
            _transactions = new List<Transaction>();
            _products = new List<BaseProduct>();
            _users = new List<User>();
            _transactionLogger = new TransactionLogger("log.txt");
        }

        public void ReadProductsFromFile(string filename) {
            CSVReader csvReader = new CSVReader(";");
            string[][] res = csvReader.ReadCSVFile(filename, 5, 1);

            foreach (string[] row in res) {
                try
                {
                    int id = int.Parse(row[0]);
                    string name = HTMLTagPattern.Replace(row[1], "");
                    decimal price = decimal.Parse(row[2]) / 100;
                    bool canBeBoughtOnCredit = false;
                    bool active = row[3] == "1";
                    BaseProduct productToAdd = null;
                    if (row[4] != "")
                    {
                        if (!DateTime.TryParse(row[4], out DateTime deactivateDate))
                        {
                            throw new Exception("deactivate date is invalid");
                        }

                        productToAdd = new SeasonalProduct(
                            id,
                            name,
                            price,
                            active,
                            canBeBoughtOnCredit,
                            DateTime.MinValue,
                            deactivateDate
                        );
                    }
                    else
                    {
                        productToAdd = new RegularProduct(id, name, price, false, active);
                    }
                    
                    if (GetProductByID(productToAdd.ID) != null)
                    {
                        throw new Exception("ID is not unique");
                    }

                    _products.Add(productToAdd);
                } catch (Exception e) {
                    Console.WriteLine($"Error while loading product '{string.Join(";", row)}'");
                    Console.WriteLine(e.Message);
                }
            }
        }
        
        public void ReadUsersFromFile(string filename) {
            CSVReader csvReader = new CSVReader(",");
            string[][] res = csvReader.ReadCSVFile(filename, 6, 1);

            foreach (string[] row in res) {
                try {
                    User newUser = new User(int.Parse(row[0]), row[1], row[2], row[3], row[5], decimal.Parse(row[4]) / 100, UserLowOnStregDollarsHandler);

                    if (GetUserByUsername(newUser.Username) != null) {
                        throw new Exception("Username is not unique");
                    }

                    if (GetUserByID(newUser.ID) != null) {
                        throw new Exception("ID is not unique");
                    }

                    _users.Add(newUser);
                } catch (Exception e) {
                    Console.WriteLine($"Error while loading user '{string.Join(";", row)}'");
                    Console.WriteLine(e.Message);
                }
            }
        }

        public IEnumerable<BaseProduct> ActiveProducts { 
            get {
                return _products.Where((product) => product.Active);
            } 
        }


        public InsertCashTransaction AddCreditsToAccount(User user, decimal amount) {
            InsertCashTransaction insertCashTransaction = new InsertCashTransaction(_lastTransactionId++, user, amount);
            insertCashTransaction.Execute();
            _transactionLogger.LogTransaction(insertCashTransaction);
            return insertCashTransaction;
        }

        public BuyTransaction BuyProduct(User user, BaseProduct product) {
            BuyTransaction buyTransaction = new BuyTransaction(_lastTransactionId++, user, product);
            buyTransaction.Execute();
            _transactionLogger.LogTransaction(buyTransaction);
            _transactions.Add(buyTransaction);
            return buyTransaction;
        }

        public BaseProduct GetProductByID(int id) {
            return _products.Find((BaseProduct baseProduct) => baseProduct.ID == id);
        }

        public IEnumerable<Transaction> GetTransactions(User user, int count) {
            return _transactions.Where((Transaction transaction) => transaction.User.Equals(user));
        }

        public User GetUserByUsername(string username) {
            return _users.Find((User user) => user.Username == username);
        }

        public IEnumerable<User> GetUsers(Func<User, bool> predicate) {
            return _users.Where(predicate);
        }

        private User GetUserByID(int id) {
            return _users.Find((User user) => user.ID == id);
        }

        private void UserLowOnStregDollarsHandler(User user, decimal balance) {
            UserBalanceWarning?.Invoke(user, balance);
        }
    }
}

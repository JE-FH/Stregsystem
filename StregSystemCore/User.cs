using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StregsystemCore {
    public delegate void UserBalanceNotification(User user, decimal balance);
    public class User : IComparable<User> {

        public int ID { get; private set; }

        private string _firstName;
        public string Firstname { 
            get => _firstName; 
            set {
                if (value == null) {
                    throw new ArgumentNullException("Firstname cannot be null");
                }
                _firstName = value;
            }
        }
        
        private string _lastName;
        public string Lastname { 
            get => _lastName; 
            set {
                if (value == null) {
                    throw new ArgumentNullException("Lastname cannot be null");
                }
                _firstName = value;
            }
        }

        //Jeg antager at username skal være en lang
        static readonly Regex _usernamePattern = new Regex(@"^[0-9a-z_]+$");

        private string _username;
        public string Username { 
            get => _username; 
            set {
                if (_usernamePattern.Match(value) == null) {
                    throw new ArgumentException("Usernames can only contain characters lowercase alphanumeric characters and underscore");
                }
                _username = value;
            }
        }

        //Jeg antager at local-part skal være mindst 1 character lang
        static readonly Regex _emailPattern = new Regex(@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9][a-zA-Z0-9.-]*\.[a-zA-Z0-9.-]*[a-zA-Z0-9]$");

        private string _email;
        public string Email { 
            get => _email; 
            set {
                if (_emailPattern.Match(value) == null) {
                    throw new ArgumentException("Email was not valid");
                }
                _email = value;
            } 
        }

        private decimal _balance;
        public decimal Balance { 
            get => _balance; 
            set {
                if (_balance >= 50 && value < 50) {
                    if (UserBalanceNotification != null) {
                        UserBalanceNotification(this, value);
                    }
                }
                _balance = value;
            }
        }

        public UserBalanceNotification UserBalanceNotification { get; set; }

        public User(int id, string firstname, string lastname, string username, string email, decimal balance, UserBalanceNotification userBalanceNotification) {
            ID = id;
            Firstname = firstname;
            Lastname = lastname;
            Username = username;
            Email = email;
            Balance = balance;
            UserBalanceNotification = userBalanceNotification;
        }

        public override string ToString() {
            return $"{Firstname} {Lastname} ({Email})";
        }

        public override bool Equals(object obj) {
            if (obj is not User) {
                return false;
            }
            User otherUser = (User)obj;
            //ID er unikt, så derfor checker vi bare det
            return ID == otherUser.ID;
        }

        public override int GetHashCode() {
            //ID er unikt så det burde være en god hashcode, dog så ved jeg ikke om hashcode skal have en 
            //bestemt distribution af tal så derfor tager jeg bare hashcode fra int og så burde det være fint
            return ID.GetHashCode();
        }

        public int CompareTo(User other) {
            //TODO: Skal lige tjekke om det her fungere rigtigt
            return ID - other.ID;
        }
    }
}

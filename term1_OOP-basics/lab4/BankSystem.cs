using System;
using System.Collections.Generic;

using Human;
using BankEvent;
using CustomUserCollection;

namespace Bank
{
    static class BankSystem
    {
        public const string SecretPassword = "Bank228";

        public static Action<BankEventArg> AddAccountEvent;
        public static Func<string> GetEmployeeRigths;
        public static Func<string> GetClientRigths;

        public static int UsersСount => users.Count;
        public static UserCollection Users => users;
        public static Dictionary<int, Account> Accounts => accounts;

        private static Dictionary<int, Account> accounts;
        private static UserCollection users;

        public static Account GetAccountById(long accId)
        {
            try
            {
                Account foundAcc = accounts[(int)accId];
                return foundAcc;
            }
            catch
            {
                return null;
            }

        }

        public static void AddUser(User user)
        {
            users.Insert(user);
        }
        
        // CLEAN CODE - camalCase function arguments naming
        public static bool RemoveUser(User user)
        {
            try
            {
                users.Remove((int)user.id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool RemoveUser(int userId)
        {
            try
            {
                // throwing error is better than returning error codes, but here its more clear to undestand
                users.Remove(userId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void AddAccounts(Account[] accArr)
        {
            // less lines in loops and conditional statements
            foreach (var acc in accArr)
            {
                AddAccount(acc);
            }
        }


        public static void AddAccount(Account acc)
        {
            accounts[(int)acc.id] = acc;
        }

        public static void AddAccount(BankEventArg bankArg)
        {
            AddAccount(bankArg.account);
            Console.WriteLine("Reacted on activating account.\nAdding it into system...\nSuccesfully added");
        }
        

        static BankSystem()
        {
            users = new UserCollection();
            accounts = new Dictionary<int, Account>();
            AddAccountEvent = AddAccount;
            GetEmployeeRigths = () =>
            {
                return "\nAddAccountId()\n" +
                "TakeCredit(moneyValue) - the money is assigned to the first account with the same or more money amount\n" +
                "ShowInfo() - show employee info\n" +
                "systemUsersCount - get users count in system. Permission reqired\n";
            };

            GetClientRigths = delegate ()
            {
                return "\nAddAccountId()\n" +
                "TakeCredit(moneyValue) - the money is assigned to the first account with the same or more money amount\n" +
                "ShowInfo() - show user info\n";
            };
        }
    }

    [Serializable]
    public class Account : IDisposable
    {
        public readonly long id;

        public string currency;


        public void IncreaseAmount(long value) => this._moneyAmount += value;

        private static long _nextId = 0;
        private long _moneyAmount;
        private bool _disposed = false;
        
        // no SCREAMING_CAPS for constants acording to C# namig conventions
        private const string DefaultCurrency = "uah";

        private event AccountHandler AccountActivatingEvent;

        public Account() : this(0, DefaultCurrency) { }

        public Account(long moneyAmount, string currency)
        {
            this.id = _nextId++;
            if (_moneyAmount < 0)
            {
                this._moneyAmount = 0;
            }
            else
            {
                this._moneyAmount = moneyAmount;
            }
            this.currency = currency;

            this.AccountActivatingEvent += new AccountHandler(BankSystem.AddAccountEvent);
            // or 
            this.AccountActivatingEvent += (BankEventArg arg) =>
            {
                BankSystem.AddAccount(arg.account);
                Console.WriteLine("Reacted on activating account.\nAdding it into system...\nSuccesfully added");
            };

        }

        public void Activate(string bankPassword)
        {
            // loging is only for demo
            Console.WriteLine($"Activating new acconut with id - {this.id}");
            if (bankPassword == BankSystem.SecretPassword)
            {
                if (AccountActivatingEvent != null)
                {
                    AccountActivatingEvent(new BankEventArg(this));
                    Console.WriteLine("Account succesfully activated");
                } 
                else 
                {
                    throw new Exception("Couldnt activate account. Inner system error");
                }
            }
            else
            {
                throw new ArgumentException("ERROR: bank password is wrong, aborting account activating");
            }
        }

        public bool DecreaseAmount(long value)
        {
            if (value > _moneyAmount)
            {
                // CLEAN CODE
                // before
                // Console.WriteLine("Unable to procces transaction");
                // Console.WriteLine($"Not enough credits -> {_moneyAmount} {currency}");

                // it's better to throw errors than log errors or return err codes but here its
                // probaly less computationally expensive and easy to undestand

                // @todo ask

                // after
                return false;
            }
            _moneyAmount -= value;
            return true;
        }

        public long MoneyAmount
        {
            get { return _moneyAmount; }
            set
            {
                if (value < 0)
                {
                    // before
                    // Console.WriteLine("Error: invalid money amount value");

                    // it's better to throw errors than log errors or return err codes

                    //after
                    throw new ArgumentException("Invalid money amount value. More than 1 required");
                }
                _moneyAmount = value;
            }
        }

        public void ShowAmount()
        {
            Console.WriteLine($"id - {id}  money - {_moneyAmount} {currency}");
        }

        void IDisposable.Dispose()
        {
            Console.WriteLine($"Dispose method without flag {this.id}");

            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            //Console.WriteLine($"Disposing method with flag {disposing} {this.id}");

            if (this._disposed)
                return;

            if (disposing)
            {
                // free managed recourses

                // CLEAN CODE
                // the best if statements and loops are 1-2 lines heigth
                removeAllEventSubscriptions();
            }
            // free unmapped recourses
            // @todo ask

            this._disposed = true;
        }

        // CLEAN CODE - each func MUST do only one thing
        // The best functions are parameterless
        // Each func should cover only one level of abstraction
        private void removeAllEventSubscriptions()
        {
            foreach (Delegate func in AccountActivatingEvent.GetInvocationList())
            {
                AccountActivatingEvent -= (AccountHandler)func;
            }
        }

        ~Account()
        {
            // CLEAN CODE
            // it's not apropriate to log here but its done to expliciltly show finalizer
            Console.WriteLine($"d-ctor of acc with id {this.id}");
            Dispose(false);
        }
    }
}
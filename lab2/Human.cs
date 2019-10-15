using System;
using System.Collections.Generic;
using Utils;
using Bank;

namespace Human
{
    // just a human and nothing more
    abstract class Person {

        public abstract string FirstName { get; }
        public abstract string LastName  { get; }

        public abstract void ShowInfo();
    }

    // basic user with no rights
    class User : Person
    {
        public readonly long id;

        protected string firstName;
        protected string lastName;

        protected static readonly List<string> DefaultQuestions = new List<string>(){
            "Enter first name",
            "Enter last name",
            "Enter login",
            "Enter password"
        };

        public override string FirstName {
            get 
            {
                return String_IO.strFirstCharToUpper(firstName);
            }
        }

        public override string LastName {
            get 
            {
                return String_IO.strFirstCharToUpper(lastName);
            }
        }

        public string login { get; }
        public string password { get; }

        protected static long nextId;

        // public static User CreateUser()
        // {
        //     string firstName = String_IO.GetInputOnText("Enter first name");
        //     string lastName = String_IO.GetInputOnText("Enter last name");
        //     string login = String_IO.GetInputOnText("Enter login");
        //     string password = "";
        //     do
        //     {
        //         password = String_IO.GetHiddenString_IO("Enter password");
        //         string passwordToCheck = String_IO.GetHiddenString_IO("Enter password again");
        //         if (passwordToCheck != password)
        //         {
        //             Console.WriteLine("Error: passwords differ");
        //             continue;
        //         }
        //         break;
        //     } while (true);
        //     User newUser = new User(firstName, lastName, login, password);
        //     return newUser;
        // }
        
        static User()
        {
            nextId = 0;
        }

        public User(string firstName, string lastName, string login, string password)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.login = login;
            this.password = password;
        }

        public string FullName
        {
            get { return $"{this.FirstName} {this.LastName}"; }
        }

        //method to override
        public override void ShowInfo()
        {
            Console.WriteLine($"User info {this.id} - '{this.FullName}'");
        }

        protected static List<string> AnswerQuestions(List<string> questions) {
            var answers = new List<string>();
            foreach(var q in questions) {
                string lowerCaseQuestion = q.ToLower();
                string ans;
                if (lowerCaseQuestion.Contains("password")) {
                    ans = String_IO.GetHiddenConsoleInput(q);
                } else {
                    ans = String_IO.GetInputOnText(q);
                }
                answers.Add(ans);
            }
            return answers;
        }
    }

    class BankClient : User
    {
        private List<long> accountsIds; //agregation
        private string secretQuestion;
        private string secretAnswer;

        public override void ShowInfo()
        {
            Console.WriteLine("Client info-------");
            base.ShowInfo();
            foreach (var accId in accountsIds)
            {
                Bank.BankSystem.GetAccountById(accId).ShowAmount();
            }
            Console.WriteLine("Client info-------");
        }

        public void ShowInfo(string beginStr, string finishStr)
        {
            Console.WriteLine(beginStr);
            base.ShowInfo();
            foreach (var accId in accountsIds)
            {
                Bank.BankSystem.GetAccountById(accId).ShowAmount();
            }
            Console.WriteLine(finishStr);
        }

        private BankClient(string firstName,
                           string lastName,
                           string login,
                           string password,
                           string secretQuestion,
                           string secretAnswer) : base(firstName, lastName, login, password)
        {
            this.secretAnswer = secretAnswer;
            this.secretQuestion = secretQuestion;
            this.accountsIds = new List<long>();
        }


        public static BankClient CreateBankClient()
        {
            var    basicAnswers = User.AnswerQuestions(User.DefaultQuestions);
            string secretQuestion = String_IO.GetInputOnText("Enter secret question");
            string secretAnswer = String_IO.GetHiddenConsoleInput("Enter secret answer");
            return new BankClient(basicAnswers[0], basicAnswers[1], basicAnswers[2], basicAnswers[3], secretQuestion, secretAnswer);
        }

        public void AddAccountId(long accId)
        {
            this.accountsIds.Add(accId);
        }
    }
    class BankEmployee : User
    {
        public string Position { get; }

        private Boolean hasRights;

        public int systemUsersCount
        {
            get
            {
                if (this.hasRights)
                {
                    return BankSystem.UsersСount;
                }
                else
                {
                    return -1;
                }
            }
        }


        public override void ShowInfo()
        {
            Console.WriteLine("Employee info-----");
            base.ShowInfo();
            Console.WriteLine($"Position {this.Position}");
            Console.WriteLine("Employee info-----");
        }

        public void ShowInfo(string beginStr, string finishStr)
        {
            Console.WriteLine(beginStr);
            base.ShowInfo();
            Console.WriteLine($"Position {this.Position}");
            Console.WriteLine(finishStr);
        }

        private BankEmployee(string firstName,
                             string lastName,
                             string login,
                             string password,
                             string Position,
                             Boolean hasRights) : base(firstName, lastName, login, password)
        {
            this.Position = Position;
            this.hasRights = hasRights;
        }

        public static BankEmployee CreateBankEmployee()
        {
            var    basicAnswers = User.AnswerQuestions(User.DefaultQuestions);
            string position = String_IO.GetInputOnText("Enter your Position in bank");
            string bankPassword = String_IO.GetHiddenConsoleInput("Enter SUPER SECRET BANK PASSWORD");

            Boolean hasRights = (bankPassword == BankSystem.SecretPassword);

            if (hasRights)
                Console.WriteLine("Corerct password, access given");
            else
                Console.WriteLine("Wrong password, access denied");

            return new BankEmployee(basicAnswers[0], basicAnswers[1], basicAnswers[2], basicAnswers[3], position, hasRights);
        }
    }
}
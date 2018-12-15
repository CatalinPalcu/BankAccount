using System;
using System.Collections.Generic;
using System.Text;

namespace BankAccount
{
    public class Bank
    {
        private List<Account> accounts;
        private List<int> accountNumbers;

        private static List<int> bankIds;

        private string name;
        private int bankId;

        public string Name { get => this.name; }
        public int BankId { get => this.bankId; }

        private static int GenerateNewBankId()
        {
            int newBankId;
            Random r = new Random();
            do
            {
                newBankId = r.Next(1000, 10000) * 10000 + r.Next(0, 1000);
            } while (bankIds.Contains(newBankId));

            return newBankId;
        }

        private int GenerateNewAccountNumber()
        {
            int newAccountNumber;
            Random r = new Random();
            do
            {
                newAccountNumber = r.Next(1000, 10000) * 10000 + r.Next(0, 1000);
            } while (accountNumbers.Contains(newAccountNumber));

            return newAccountNumber;
        }

        private void CreateAccount()
        {
            string owner, pass1, pass2;
            int accountNumber;
            decimal initialValue;

            Console.Write("Insert the name of the owner:  ");
            owner = Console.ReadLine();

            do
            {
                Console.Write("Insert the password:  ");
                pass1 = Console.ReadLine();
                Console.Write("Retype the password:  ");
                pass2 = Console.ReadLine();
            } while (pass1.Length < 4 || pass1 != pass2);

            Console.WriteLine("Insert the initial amount ");
            if (!decimal.TryParse(Console.ReadLine(), out initialValue))
                initialValue = 0;

            accountNumber = GenerateNewAccountNumber();
            Account account = new Account(accountNumber, owner, pass1.GetHashCode(), initialValue);

            accounts.Add(account);
            accountNumbers.Add(accountNumber);

            Console.WriteLine("The account was successfully open. The account number is {0}",accountNumber);
        }

        private int ReadAccountNumber(string s)
        {
            int accountNumber = 0,
                numbersOfTry = 0;

            do
            {
                Console.WriteLine(s);
                numbersOfTry++;
            } while ((!Int32.TryParse(Console.ReadLine(), out accountNumber) || !accountNumbers.Contains(accountNumber))
            && (numbersOfTry < 3));

            return accountNumber;
        }

        public Account ReturnAccount(int idAccount)
        {
            int index = accountNumbers.IndexOf(idAccount);
            if (index < 0)
                return null;
            return accounts[index];
        }

        public void ChangePassword(int accountNumber = 0)
        {
            int index;
            string oldPassword, newPassword1, newPassword2;

            if (accountNumber == 0)
                 accountNumber = ReadAccountNumber("\t\tInsert the account number: ");

            if (!accountNumbers.Contains(accountNumber))
                Console.WriteLine("Something went wrong.");
            else
            {
                index = accountNumbers.IndexOf(accountNumber);

                Console.Write("Insert the old password:\t");
                oldPassword = Console.ReadLine();

                do
                {
                    Console.Write("Insert the new password:\t");
                    newPassword1 = Console.ReadLine();
                    Console.Write("Retype the new password:\t");
                    newPassword2 = Console.ReadLine();
                } while (newPassword1.Length < 4 || !newPassword1.Equals(newPassword2));

                accounts[index].ChangePassword(oldPassword.GetHashCode(), newPassword1.GetHashCode());
            }
        }

        private void CreateAccount(string owner, string pass, decimal value)
        {
            int accountNumber = GenerateNewAccountNumber();

            Account account = new Account(accountNumber, owner, pass.GetHashCode(), value);

            accounts.Add(account);
            accountNumbers.Add(accountNumber);

            Console.WriteLine("The account was successfully open. The account number is {0}", accountNumber);
        }

        private void AddRandomElements (string password)
        {
            Random r = new Random();
            if (password == "greedisgood")
            {
                for (int i = 1; i <= 5; i++)
                {
                    CreateAccount("[no name]", "password", r.Next(0, 1000));
                }
            }
        }

        private void CloseAccount(int accountNumber = 0)
        {
            int index;
            string password;

            if (accountNumber == 0)
                accountNumber = ReadAccountNumber("\t\tInsert the account number: ");

            if (!accountNumbers.Contains(accountNumber))
               Console.WriteLine("Something went wrong.");
            else
            {
                index = accountNumbers.IndexOf(accountNumber);

                Console.Write("Insert the password:\t");
                password = Console.ReadLine();

                accounts[index].DeleteAccount(password.GetHashCode());
                accounts.RemoveAt(index);
                accountNumbers.RemoveAt(index);
            }             
        }

        private void ChargingAccount(int accountNumber = 0)
        {
            int index;
            decimal amount = 0;

            if (accountNumber == 0)
                accountNumber = ReadAccountNumber("\t\tInsert the account number: ");

            if (!accountNumbers.Contains(accountNumber))
                Console.WriteLine("Something went wrong.");
            else
            {
                index = accountNumbers.IndexOf(accountNumber);
                do
                {
                    Console.Write("Insert the value:");
                } while (!decimal.TryParse(Console.ReadLine(), out amount));

                accounts[index].ChargingAccount(amount);
            }
        }

        private void CashWithdrawal(int accountNumber = 0)
        {
            int index;
            decimal amount = 0;
            string password;

            if (accountNumber == 0)
                accountNumber = ReadAccountNumber("\t\tInsert the account number: ");

            if (!accountNumbers.Contains(accountNumber))
                Console.WriteLine("Something went wrong.");
            else
            {
                index = accountNumbers.IndexOf(accountNumber);

                Console.Write("Insert the password:\t");
                password = Console.ReadLine();

                do
                {
                    Console.Write("Insert the value:");
                } while (!decimal.TryParse(Console.ReadLine(), out amount));

                accounts[index].CashWithdrawal(password.GetHashCode(), amount);
            }
        }

        private void TransferMoney(int sourceAccount = 0)
        {
            int indexSource, indexDestiation;
            decimal amount = 0;
            string password;

            if (sourceAccount == 0)
                sourceAccount = ReadAccountNumber("\t\tInsert the source account number: ");

            if (!accountNumbers.Contains(sourceAccount))
                Console.WriteLine("Something went wrong.");
            else
            {
                indexSource = accountNumbers.IndexOf(sourceAccount);

                Console.Write("Insert the password:\t");
                password = Console.ReadLine();

                int destinationAccount = ReadAccountNumber("\t\tInsert the destination account number: ");
                if (!accountNumbers.Contains(destinationAccount))
                    Console.WriteLine("Something went wrong.");
                else
                {
                    do
                    {
                        Console.Write("Insert the value:");
                    } while (!decimal.TryParse(Console.ReadLine(), out amount));

                    if (accounts[indexSource].CashWithdrawal(password.GetHashCode(), amount))
                    {
                        indexDestiation = accountNumbers.IndexOf(destinationAccount);
                        accounts[indexDestiation].ChargingAccount(amount);
                    }
                }

            }
        }

        private void TransferMoneyAnotherBank(int sourceAccount = 0)
        {
            int indexSource, indexDestiation, indexBank;
            decimal amount = 0;
            string password;
            int numbersOfTry = 0;

            if (sourceAccount == 0)
                sourceAccount = ReadAccountNumber("\t\tInsert the source account number: ");

            if (!accountNumbers.Contains(sourceAccount))
                Console.WriteLine("Something went wrong.");
            else
            {
                indexSource = accountNumbers.IndexOf(sourceAccount);

                Console.Write("Insert the password:\t");
                password = Console.ReadLine();

                numbersOfTry = 0;
                int bankId = 0;
                Bank destinationBank;
                do
                {
                    Console.WriteLine("\t\tInsert the Id of destination bank");
                    numbersOfTry++;
                } while ((!Int32.TryParse(Console.ReadLine(), out bankId) || Program.ReturnBank(bankId)==null) && (numbersOfTry < 3));

                destinationBank = Program.ReturnBank(bankId);

                if (destinationBank==null)
                    Console.WriteLine("Something went wrong");
                else
                {
                    int destinationAccountNr = 0;
                    numbersOfTry = 0;
                    do
                    {
                        Console.WriteLine("\t\tInsert the destination account number:");
                        numbersOfTry++;
                    } while ((!Int32.TryParse(Console.ReadLine(), out destinationAccountNr) ||  destinationBank.ReturnAccount(destinationAccountNr)==null) && (numbersOfTry < 3));


                    Account destinationAccount = destinationBank.ReturnAccount(destinationAccountNr);
                    if (destinationAccount==null)
                        Console.WriteLine("Something went wrong");
                    else
                    {
                        do
                        {
                            Console.Write("Insert the value:");
                        } while (!decimal.TryParse(Console.ReadLine(), out amount));

                        if (accounts[indexSource].CashWithdrawal(password.GetHashCode(), amount))
                        {
                            destinationAccount.ChargingAccount(amount);
                        }
                    }
                }
            }
        }

        private void AccountQuery(int accountNumber = 0)
        {
            int index;
            string password;

            if (accountNumber == 0)
                accountNumber = ReadAccountNumber("\t\tInsert the account number: ");

            if (!accountNumbers.Contains(accountNumber))
                Console.WriteLine("Something went wrong.");
            else
            {
                index = accountNumbers.IndexOf(accountNumber);

                Console.Write("Insert the password:\t");
                password = Console.ReadLine();

                decimal amount = accounts[index].AccountQuery(password.GetHashCode());

                if (amount >=0)
                {
                    Console.WriteLine("On this account you have {0:C}.",amount);
                }
            }
        }

        static Bank()
        {
            bankIds = new List<int>();
        }

        public Bank(string _name)
        {
            this.name = _name;
            this.bankId = GenerateNewBankId();
            bankIds.Add(bankId);

            Console.WriteLine($"The bank {_name} was created and have the ID: {BankId}");

            accounts = new List<Account>();
            accountNumbers = new List<int>();
        }

        public void Menu()
        {
            int option;
            int accountNumber = 0;
            do

            {
                Console.WriteLine("\t\tWelcome to {0}", this.name);
                Console.WriteLine("Choose an option");
                Console.WriteLine("\t1 - Create account");
                Console.WriteLine("\t2 - Close account");
                Console.WriteLine("\t3 - Charge account");
                Console.WriteLine("\t4 - Cash withdrawal");
                Console.WriteLine("\t5 - Transfer money to another account");
                Console.WriteLine("\t6 - Transfer money to an account from another bank");
                Console.WriteLine("\t7 - Account query");
                Console.WriteLine("\t8 - Change password");
                Console.WriteLine("\t9 - Login");
                Console.WriteLine("\t10 - Logout");
                Console.WriteLine("\tanything else - exit");

                if (!Int32.TryParse(Console.ReadLine(), out option))
                    option = 10;

                switch (option)
                {
                    case 1:
                        CreateAccount();
                        break;
                    case 2:
                        CloseAccount(accountNumber);
                        accountNumber = 0;
                        break;
                    case 3:
                        ChargingAccount(accountNumber);
                        break;
                    case 4:
                        CashWithdrawal(accountNumber);
                        break;
                    case 5:
                        TransferMoney(accountNumber);
                        break;
                    case 6:
                        TransferMoneyAnotherBank(accountNumber);
                        break;
                    case 7:
                        AccountQuery(accountNumber);
                        break;
                    case 8:
                        ChangePassword(accountNumber);
                        break;
                    case 9:
                        accountNumber = ReadAccountNumber("\t\tInsert the account number: ");

                        if (!accountNumbers.Contains(accountNumber))
                        {
                            Console.WriteLine("Something went wrong.");
                            accountNumber = 0;
                        } else
                            Console.WriteLine("You successfully logged in");
                        break;
                    case 10:
                        Console.WriteLine("You successfully logged out");
                        accountNumber = 0;
                        break;                          
                    case 187:
                        Console.WriteLine("Insert the password: ");
                        AddRandomElements(Console.ReadLine());
                        break;
                    default:
                        option = 11;
                        break;
                }
                Console.WriteLine("Press enter to continue");
                Console.ReadLine();

            } while (option != 11);
        }
    }
}

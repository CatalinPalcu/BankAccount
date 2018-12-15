using System;
using System.Collections.Generic;
using System.Text;

namespace BankAccount
{
    public class Account
    {
        private readonly int accountNumber;
        private int password;
        private decimal amount;
        private readonly string owner;

        public Account(int _accountNumber, string _owner, int _password, decimal _initialAmount = 0 )
        {
            this.accountNumber = _accountNumber;
            this.password = _password;
            this.owner = _owner;
            this.amount = _initialAmount;
        }

        public decimal AccountQuery (int _password)
        {
            if (this.password == _password)
            {
                return this.amount;
            }

            Console.WriteLine("You do not have access to this account");
            return -1;
        }

        public void ChargingAccount(decimal value)
        {
            if (value <= 0)
                Console.WriteLine("Nice try!");
            else
            {
                this.amount += value;
                Console.WriteLine("The transaction was successfully completed");
            }
        }

        public bool CashWithdrawal (int _password, decimal value)
        {
            if (this.password == _password)
            {
                if (value <=0)
                {
                    Console.WriteLine("Nice try");
                    return false;
                }
                else
                {
                    if (this.amount < value)
                    {
                        Console.WriteLine("You don't have enough money");
                        return false;
                    }
                    else
                    {
                        this.amount -= value;
                        Console.WriteLine("The transaction was successfuly completed. You have withdrawn {0:C}",value);
                        return true;
                    }
                }
            }
            else
                 Console.WriteLine("You do not have access to this account");
            return false;
        }

        public void ChangePassword (int _oldPassword, int _newPassword)
        {
            if (this.password == _oldPassword)
            {
                this.password = _newPassword;
                Console.WriteLine("The password was successfuly changed");
            }  else
            {
                Console.WriteLine("You don't have access to this account");
            }
        }

        public void DeleteAccount(int _password)
        {
            if(this.password != _password)
                Console.WriteLine("You don't have access to this account");
            else
            {
                Console.WriteLine("{0}, your account was closed. You got back {1:C}, the amount that was in the account.\n\t Have a nice day!",this.owner,this.amount);
                this.amount = 0;
            }
        }
    }
}

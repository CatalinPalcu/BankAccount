using System;
using System.Collections.Generic;

namespace BankAccount
{
    public class Program
    {
        private static List<Bank> banks;
        private static List<int> bankIds;

        static Program()
        {
            banks = new List<Bank>();
            bankIds = new List<int>();
        }

        public static void Main(string[] args)
        {
            Menu();
        }

        public static  Bank ReturnBank(int id)
            {
            int index = bankIds.IndexOf(id);

                if (index >=0)
                    return banks[index];
                return null;
            }

        public static void Menu()
        {
            int option;
            int bankId = 0;
            do
            {
                Console.WriteLine("Choose an option");
                Console.WriteLine("\t1 - Create a new Bank");
                Console.WriteLine("\t2 - Select a Bank");
                Console.WriteLine("\tanything else - exit");

                if (!Int32.TryParse(Console.ReadLine(), out option))
                    option = 3;

                switch (option)
                {
                    case 1:
                        Console.Write("Insert the name of the Bank: ");
                        string name = Console.ReadLine();
                        Bank newBank = new Bank(name);
                        banks.Add(newBank);
                        bankIds.Add(newBank.BankId);
                        break;
                    case 2:

                        int numbersOfTry = 0;
                        bankId = 0;
                        do
                        {
                            Console.WriteLine("Insert the bank Id");
                            numbersOfTry++;
                        } while ((!Int32.TryParse(Console.ReadLine(), out bankId) || !bankIds.Contains(bankId))
                        && (numbersOfTry < 3));

                        if (!bankIds.Contains(bankId))
                            Console.WriteLine("Something went wrong");
                        else
                        {
                            int index = bankIds.IndexOf(bankId);
                            banks[index].Menu();

                        }

                        break;
                    default:
                        option = 3;
                        break;
                }
                Console.WriteLine("Press enter to continue");
                Console.ReadLine();

            } while (option != 3);
        }

    }

}

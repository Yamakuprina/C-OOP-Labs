using System;
using System.Collections.Generic;
using Banks.Accounts;

namespace Banks
{
    internal static class Program
    {
        private static void Main()
        {
            var interestRates = new List<InterestRate>()
            {
                new InterestRate(5000, 2),
                new InterestRate(10000, new decimal(3.5)),
            };
            var centralBank = new CentralBank();
            Bank bank = centralBank.RegisterBank("Tinkoff Bank", 4, 10000, 15000, 1000, interestRates);
            Client client1 = Client.Builder("Tagir", "Faizullin").SetAddress("Vyazemskiy 5/7").GetClient();
            var accounts = new List<Account>();
            while (true)
            {
                Console.WriteLine("\nPick an option:\n" +
                                  "1. Create new Account\n" +
                                  "2. Transfer money\n" +
                                  "3. Withdraw money\n" +
                                  "4. Refill Account\n" +
                                  "5. Check Balance\n" +
                                  "6. Exit\n");
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.WriteLine("What Account do you want to create?\n" +
                                          "1. Debit Account\n" +
                                          "2. Deposit Account\n" +
                                          "3. Credit Account\n");
                        accounts.Add(Console.ReadLine() switch
                        {
                            "1" => bank.AddNewAccount(client1, AccountType.Debit),
                            "2" => bank.AddNewAccount(client1, AccountType.Deposit, DateTime.Today.AddYears(1)),
                            "3" => bank.AddNewAccount(client1, AccountType.Credit),
                            _ => default,
                        });
                        Console.WriteLine("Account created");
                        break;
                    case "2":
                        Console.WriteLine("Choose account number to transfer from and account to transfer to:\n");
                        for (int i = 0; i < accounts.Count; i++)
                        {
                            Console.WriteLine($"Account {i + 1}\n");
                        }

                        int accountNumber1 = int.Parse(Console.ReadLine());
                        int accountNumber2 = int.Parse(Console.ReadLine());
                        Console.WriteLine(
                            $"How much u want to transfer? U have {accounts[accountNumber1 - 1].Balance}\n");
                        accounts[accountNumber1 - 1].Transfer(decimal.Parse(Console.ReadLine()), accounts[accountNumber2 - 1], DateTime.Now);
                        Console.WriteLine("Transfer done");
                        break;
                    case "3":
                        Console.WriteLine("Choose account number to withdraw from\n");
                        for (int i = 0; i < accounts.Count; i++)
                        {
                            Console.WriteLine($"Account {i + 1}\n");
                        }

                        int accountNumber = int.Parse(Console.ReadLine());
                        Console.WriteLine(
                            $"How much u want to withdraw? U have {accounts[accountNumber - 1].Balance}\n");
                        accounts[accountNumber - 1].Withdrawal(decimal.Parse(Console.ReadLine()), DateTime.Now);
                        Console.WriteLine("Withdraw done");
                        break;
                    case "4":
                        Console.WriteLine("Choose account number to refill\n");
                        for (int i = 0; i < accounts.Count; i++)
                        {
                            Console.WriteLine($"Account {i + 1}\n");
                        }

                        int accountNumbe = int.Parse(Console.ReadLine());
                        Console.WriteLine($"How much u want to refill? U have {accounts[accountNumbe - 1].Balance}\n");
                        accounts[accountNumbe - 1].Refill(decimal.Parse(Console.ReadLine()), DateTime.Now);
                        Console.WriteLine("Refill done");
                        break;
                    case "5":
                        Console.WriteLine("Choose account number to check balance\n");
                        for (int i = 0; i < accounts.Count; i++)
                        {
                            Console.WriteLine($"Account {i + 1}\n");
                        }

                        int accountnumber = int.Parse(Console.ReadLine());
                        Console.WriteLine($"You have {accounts[accountnumber - 1].Balance}\n");
                        break;
                    case "6":
                        return;
                }
            }
        }
    }
}
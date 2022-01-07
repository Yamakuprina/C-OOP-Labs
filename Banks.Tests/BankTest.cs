using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Banks.Accounts;
using NUnit.Framework;

namespace Banks.Tests
{
    public class BanksTest
    {
        private Bank bank;
        private Client client1;
        private Client client2;
        private Account debitAccount1;
        private Account debitAccount2;
        private CentralBank centralBank;

        [SetUp]
        public void Setup()
        {
            var interestRates = new List<InterestRate>()
            {
                new InterestRate(5000, 2),
                new InterestRate(10000, new decimal(3.5)),
            };
            centralBank = new CentralBank();
            bank = centralBank.RegisterBank("Tinkoff Bank", 4, 10000, 15000, 1000, interestRates);
            client1 = Client.Builder("Tagir", "Faizullin").SetAddress("Vyazemskiy 5/7").GetClient();
            client2 = Client.Builder("Ivan", "Ivanov").SetAddress("Vyazemskiy 5/7").GetClient();
            debitAccount1 = bank.AddNewAccount(client1, AccountType.Debit);
            debitAccount2 = bank.AddNewAccount(client2, AccountType.Debit);
            debitAccount1.Refill(5000, DateTime.Now);
            debitAccount2.Refill(1000, DateTime.Now);
        }
        
        [Test]
        public void Refill()
        {
            debitAccount1.Refill(10000, DateTime.Now);
            Assert.AreEqual(15000, debitAccount1.Balance);
        }
        
        [Test]
        public void CancelRefill()
        {
            debitAccount1.Refill(10000, DateTime.Now);
            debitAccount1.UndoTransaction(debitAccount1.History.First().Id);
            Assert.AreEqual(10000, debitAccount1.Balance);
        }

        [Test]
        public void PayPercents()
        {
            DateTime dateMonthAhead = DateTime.Now.AddMonths(1);
            centralBank.Notify(dateMonthAhead);
            Assert.AreEqual(5018, Math.Round(debitAccount1.Balance));
        }

        [Test]
        public void CreditCommission()
        {
            Account creditAccount = bank.AddNewAccount(client1, AccountType.Credit);
            creditAccount.Withdrawal(5000, DateTime.Now);
            DateTime dateMonthAhead = DateTime.Now.AddDays(15);
            centralBank.Notify(dateMonthAhead);
            Assert.AreEqual(-6000, Math.Round(creditAccount.Balance));
        }

        [Test]
        public void Transfer()
        {
            debitAccount1.Transfer(2000, debitAccount2, DateTime.Now);
            Assert.AreEqual(3000, debitAccount1.Balance);
            Assert.AreEqual(3000, debitAccount2.Balance);
        }

        [Test]
        public void UnverifiedClientCheck()
        {
            Assert.Catch<BankException>((() => debitAccount1.Withdrawal(1000000, DateTime.Now)));
        }

        [Test]
        public void CheckSubscription()
        {
            var output = new StringWriter();
            Console.SetOut(output);
            bank.SubscribeClient(debitAccount1);
            bank.ChangeDebitPercent(3);
            Assert.AreEqual("Debit percentage changed for 3",output.ToString());
        }

        [Test]
        public void CreditBelowZeroLimit()
        {
            client1 = Client.BuilderPassport("Tagir","Faizullin","1234567").GetClient();
            Assert.Catch<BankException>((() => debitAccount1.Withdrawal(15000, DateTime.Now)));
        }

    }
}
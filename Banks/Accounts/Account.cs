using System;
using System.Collections.Generic;
using System.Linq;

namespace Banks.Accounts
{
    public abstract class Account
    {
        protected Account(Client client, int unverifiedLimit)
        {
            Client = client;
            Id = IdGenerator.AccountIdGenerate();
            UnverifiedLimit = unverifiedLimit;
            History = new List<Transaction>();
        }

        protected Account(Account account)
        {
            History = account.History;
            Client = account.Client;
            Id = IdGenerator.AccountIdGenerate();
            UnverifiedLimit = account.UnverifiedLimit;
        }

        public bool ClientSubscribed { get; set; } = false;
        public Client Client { get; }
        public decimal Balance { get; set; }
        public int Id { get; }
        public DateTime LastDayInterestsCommissionPaid { get; set; } = default;
        public decimal TempInterestCommissionSum { get; set; }
        public int UnverifiedLimit { get; set; }
        public List<Transaction> History { get; }

        public static AccountType GetAccountType(Account account)
        {
            return account switch
            {
                DebitAccount debitAccount => AccountType.Debit,
                DepositAccount depositAccount => AccountType.Deposit,
                CreditAccount creditAccount => AccountType.Credit,
                _ => AccountType.Any
            };
        }

        public abstract void Transfer(decimal transactionSum, Account destinationAccount, DateTime dateTime);
        public abstract void Withdrawal(decimal transactionSum, DateTime dateTime);
        public abstract void Refill(decimal transactionSum, DateTime dateTime);

        public void UndoTransaction(int transactionId)
        {
            Transaction transaction =
                History.FirstOrDefault(transaction => transaction.Id.Equals(transactionId));
            if (transaction == null)
            {
                throw new BankException("No transaction found");
            }

            transaction.UndoOperation();
            History.Remove(transaction);
        }
    }
}
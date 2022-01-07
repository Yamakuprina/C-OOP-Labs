using System;
using System.Collections.Generic;

namespace Banks.Accounts
{
    public class DebitAccount : Account
    {
        public DebitAccount(Client client, int transferLimitForUnverified)
            : base(client, transferLimitForUnverified)
        {
        }

        public DebitAccount(Account account)
            : base(account)
        {
        }

        public override void Transfer(decimal transactionSum, Account destinationAccount, DateTime dateTime)
        {
            if (Balance < transactionSum)
            {
                throw new BankException("Not enough money");
            }

            var transaction = new Transaction(TransactionType.Transfer, dateTime, this);
            transaction.CompleteOperation(transactionSum, destinationAccount);
            History.Add(transaction);
        }

        public override void Withdrawal(decimal transactionSum, DateTime dateTime)
        {
            if (Balance < transactionSum)
            {
                throw new BankException("Not enough money");
            }

            var transaction = new Transaction(TransactionType.Withdrawal, dateTime, this);
            transaction.CompleteOperation(transactionSum);
            History.Add(transaction);
        }

        public override void Refill(decimal transactionSum, DateTime dateTime)
        {
            var transaction = new Transaction(TransactionType.Refill, dateTime, this);
            transaction.CompleteOperation(transactionSum);
            History.Add(transaction);
        }

        public void AddInterest(decimal interestSum, DateTime dateTime)
        {
            var transaction = new Transaction(TransactionType.AddInterest, dateTime, this);
            transaction.CompleteOperation(interestSum);
            History.Add(transaction);
        }
    }
}
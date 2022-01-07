using System;

namespace Banks.Accounts
{
    public class CreditAccount : Account
    {
        public CreditAccount(Client client, int unverifiedLimit, int creditBelowZeroLimit)
            : base(client, unverifiedLimit)
        {
            BelowZeroLimit = creditBelowZeroLimit;
        }

        public CreditAccount(Account account, int creditBelowZeroLimit)
            : base(account)
        {
            BelowZeroLimit = creditBelowZeroLimit;
        }

        private int BelowZeroLimit { get; }

        public override void Transfer(decimal transactionSum, Account destinationAccount, DateTime dateTime)
        {
            if (Balance - transactionSum + BelowZeroLimit < 0)
            {
                throw new BankException("Credit Limit reached");
            }

            var transaction = new Transaction(TransactionType.Transfer, dateTime, this);
            transaction.CompleteOperation(transactionSum, destinationAccount);
            History.Add(transaction);
        }

        public override void Withdrawal(decimal transactionSum, DateTime dateTime)
        {
            if (Balance - transactionSum + BelowZeroLimit < 0)
            {
                throw new BankException("Credit Limit reached");
            }

            var transaction = new Transaction(TransactionType.Withdrawal, dateTime, this);
            transaction.CompleteOperation(transactionSum);
            History.Add(transaction);
        }

        public void SubtractCommission(decimal commissionSum, DateTime dateTime)
        {
            var transaction = new Transaction(TransactionType.SubtractCommission, dateTime, this);
            transaction.CompleteOperation(commissionSum);
            History.Add(transaction);
        }

        public override void Refill(decimal transactionSum, DateTime dateTime)
        {
            var transaction = new Transaction(TransactionType.Refill, dateTime, this);
            transaction.CompleteOperation(transactionSum);
            History.Add(transaction);
        }
    }
}
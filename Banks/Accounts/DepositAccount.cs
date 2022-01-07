using System;

namespace Banks.Accounts
{
    public class DepositAccount : Account
    {
        public DepositAccount(Client client, int unverifiedLimit, DateTime depositExpirationDate)
            : base(client, unverifiedLimit)
        {
            DepositExpirationDate = depositExpirationDate;
        }

        public DepositAccount(Account account, DateTime depositExpirationDate)
            : base(account)
        {
            DepositExpirationDate = depositExpirationDate;
        }

        public decimal DepositSum { get; set; } = decimal.Zero;

        public DateTime DepositExpirationDate { get; }

        public override void Transfer(decimal transactionSum, Account destinationAccount, DateTime dateTime)
        {
            if (dateTime < DepositExpirationDate)
            {
                throw new BankException("Cant transfer from Deposit Account now");
            }

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
            if (dateTime < DepositExpirationDate)
            {
                throw new BankException("Cant withdraw from Deposit Account now");
            }

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
            if (DepositSum == decimal.Zero)
            {
                DepositSum = transactionSum;
            }

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
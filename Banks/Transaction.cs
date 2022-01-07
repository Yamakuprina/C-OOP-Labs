using System;
using Banks.Accounts;

namespace Banks
{
    public class Transaction
    {
        public Transaction(TransactionType transactionType, DateTime dateTime, Account account)
        {
            TransactionType = transactionType;
            DateTime = dateTime;
            Account = account;
            Id = IdGenerator.TransactionIdGenerate();
            BalanceBeforeTransaction = Account.Balance;
        }

        public decimal BalanceBeforeTransaction { get; set; }
        public decimal BalanceAfterTransaction { get; set; }
        public int Id { get; }
        public TransactionType TransactionType { get; }
        public decimal TransactionSum { get; set; }
        public DateTime DateTime { get; }
        public Account Account { get; }
        private Account TransferDestination { get; set; }
        private bool OperationCanceled { get; set; } = false;

        public void CompleteOperation(decimal transactionSum, Account transferDestination = null)
        {
            if (OperationCanceled == true)
            {
                throw new BankException("Operation is already canceled");
            }

            if (transactionSum < 0)
            {
                throw new BankException("Operation Sum less than zero");
            }

            TransactionSum = transactionSum;
            TransferDestination = transferDestination;

            switch (TransactionType)
            {
                case TransactionType.Transfer:
                    if (transferDestination == null)
                    {
                        throw new BankException("Wrong Transfer destination");
                    }

                    if (Account.Client.Verified && TransactionSum > Account.UnverifiedLimit)
                    {
                        throw new BankException("Unverified account cant transfer above limit");
                    }

                    Account.Balance -= TransactionSum;
                    TransferDestination.Balance += TransactionSum;
                    BalanceAfterTransaction = Account.Balance;
                    return;
                case TransactionType.Refill:
                    Account.Balance += TransactionSum;
                    BalanceAfterTransaction = Account.Balance;
                    return;
                case TransactionType.AddInterest:
                    Account.Balance += TransactionSum;
                    BalanceAfterTransaction = Account.Balance;
                    return;
                case TransactionType.Withdrawal:
                    if (Account.Client.Verified && TransactionSum > Account.UnverifiedLimit)
                    {
                        throw new BankException("Unverified account cant withdraw above limit");
                    }

                    Account.Balance -= TransactionSum;
                    BalanceAfterTransaction = Account.Balance;
                    return;
                case TransactionType.SubtractCommission:
                    Account.Balance -= TransactionSum;
                    BalanceAfterTransaction = Account.Balance;
                    return;
            }
        }

        public void UndoOperation()
        {
            if (OperationCanceled == true)
            {
                throw new BankException("Operation is already canceled");
            }

            switch (TransactionType)
            {
                case TransactionType.Transfer:
                    Account.Balance += TransactionSum;
                    TransferDestination.Balance -= TransactionSum;
                    OperationCanceled = true;
                    BalanceAfterTransaction = BalanceBeforeTransaction;
                    return;
                case TransactionType.Refill:
                    Account.Balance -= TransactionSum;
                    OperationCanceled = true;
                    BalanceAfterTransaction = BalanceBeforeTransaction;
                    return;
                case TransactionType.AddInterest:
                    Account.Balance -= TransactionSum;
                    OperationCanceled = true;
                    BalanceAfterTransaction = BalanceBeforeTransaction;
                    return;
                case TransactionType.Withdrawal:
                    Account.Balance += TransactionSum;
                    OperationCanceled = true;
                    BalanceAfterTransaction = BalanceBeforeTransaction;
                    return;
                case TransactionType.SubtractCommission:
                    Account.Balance += TransactionSum;
                    OperationCanceled = true;
                    BalanceAfterTransaction = BalanceBeforeTransaction;
                    return;
            }
        }
    }
}
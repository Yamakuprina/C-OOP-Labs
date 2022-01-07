using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Accounts;

namespace Banks
{
    public class Bank
    {
        public Bank(string name, decimal debitPercent, int unverifiedLimit, int creditBelowZeroLimit, decimal creditCommission, List<InterestRate> interestRates)
        {
            Accounts = new List<Account>();
            Id = IdGenerator.BankIdGenerate();
            Name = name;
            DebitPercent = debitPercent;
            UnverifiedLimit = unverifiedLimit;
            CreditBelowZeroLimit = creditBelowZeroLimit;
            CreditCommission = creditCommission;
            InterestRates = interestRates;
        }

        public int Id { get; }
        public string Name { get; }
        public decimal DebitPercent { get; set; }
        public int UnverifiedLimit { get; set; }
        public int CreditBelowZeroLimit { get; set; }
        public decimal CreditCommission { get; set; }
        private List<Account> Accounts { get; }
        private List<InterestRate> InterestRates { get; set; }

        public decimal CheckDepositAccountBalanceAfterTime(DepositAccount account, DateTime skipTime)
        {
            var depositAccountCopy = new DepositAccount(account, account.DepositExpirationDate);
            depositAccountCopy.TempInterestCommissionSum = 0;
            CalculatePercentsForAccount(depositAccountCopy, skipTime, CalculateDepositPercent(depositAccountCopy.DepositSum));
            return depositAccountCopy.Balance + depositAccountCopy.TempInterestCommissionSum;
        }

        public decimal CheckDebitAccountBalanceAfterTime(DebitAccount account, DateTime skipTime)
        {
            var debitAccountCopy = new DebitAccount(account);
            debitAccountCopy.TempInterestCommissionSum = 0;
            CalculatePercentsForAccount(debitAccountCopy, skipTime, DebitPercent);
            return debitAccountCopy.Balance + debitAccountCopy.TempInterestCommissionSum;
        }

        public decimal CheckCreditAccountBalanceAfterTime(CreditAccount account, DateTime skipTime)
        {
            var creditAccountCopy = new CreditAccount(account, CreditBelowZeroLimit);
            creditAccountCopy.TempInterestCommissionSum = 0;
            CalculateCreditCommissionsForAccount(creditAccountCopy, skipTime);
            return creditAccountCopy.Balance - creditAccountCopy.TempInterestCommissionSum;
        }

        public void PayInterestsAndCommissions(DateTime dateTime)
        {
            foreach (Account account in Accounts)
            {
                switch (account)
                {
                    case DebitAccount debitAccount:
                        CalculatePercentsForAccount(debitAccount, dateTime, DebitPercent);
                        debitAccount.AddInterest(debitAccount.TempInterestCommissionSum, dateTime);
                        debitAccount.TempInterestCommissionSum = 0;
                        break;
                    case DepositAccount depositAccount:
                        CalculatePercentsForAccount(depositAccount, dateTime, CalculateDepositPercent(depositAccount.DepositSum));
                        depositAccount.AddInterest(depositAccount.TempInterestCommissionSum, dateTime);
                        depositAccount.TempInterestCommissionSum = 0;
                        break;
                    case CreditAccount creditAccount:
                        CalculateCreditCommissionsForAccount(creditAccount, dateTime);
                        creditAccount.SubtractCommission(creditAccount.TempInterestCommissionSum, dateTime);
                        creditAccount.TempInterestCommissionSum = 0;
                        break;
                }
            }
        }

        public Account AddNewAccount(Client client, AccountType accountType, DateTime depositExpirationDay = default)
        {
            switch (accountType)
            {
                case AccountType.Debit:
                    var debitAccount = new DebitAccount(client, UnverifiedLimit);
                    Accounts.Add(debitAccount);
                    return debitAccount;
                case AccountType.Credit:
                    var creditAccount = new CreditAccount(client, UnverifiedLimit, CreditBelowZeroLimit);
                    Accounts.Add(creditAccount);
                    return creditAccount;
                case AccountType.Deposit:
                    var depositAccount = new DepositAccount(client, UnverifiedLimit, depositExpirationDay);
                    Accounts.Add(depositAccount);
                    return depositAccount;
            }

            return null;
        }

        public void NotifySubscribers(AccountType accountType, string message)
        {
            Accounts.Where(account =>
                    (account.ClientSubscribed && Account.GetAccountType(account).Equals(accountType)) ||
                    (account.ClientSubscribed && accountType.Equals(AccountType.Any))).Select(account => account.Client)
                .ToList().ForEach(client => client.GetUpdate(message));
        }

        public void SubscribeClient(Account account)
        {
            account.ClientSubscribed = true;
        }

        public void ChangeDebitPercent(decimal newPercent)
        {
            DebitPercent = newPercent;
            NotifySubscribers(AccountType.Debit, $"Debit percentage changed for {newPercent}");
        }

        public void ChangeCreditCommission(decimal newCommission)
        {
            CreditCommission = newCommission;
            NotifySubscribers(AccountType.Credit, $"Credit commission changed for {newCommission}");
        }

        public void ChangeUnverifiedLimit(int newLimit)
        {
            UnverifiedLimit = newLimit;
            NotifySubscribers(AccountType.Any, $"Unverified limit changed for {newLimit}");
        }

        public void ChangeCreditBelowZeroLimit(int newLimit)
        {
            CreditBelowZeroLimit = newLimit;
            NotifySubscribers(AccountType.Any, $"BelowZero limit changed for {newLimit}");
        }

        public void ChangeInterestRates(List<InterestRate> newInterestRates)
        {
            InterestRates = newInterestRates;
            NotifySubscribers(AccountType.Deposit, $"Interest rates changed");
        }

        public void CalculatePercentsForAccount(Account account, DateTime dateTime, decimal percents)
        {
            if (account is CreditAccount) return;
            if (account.LastDayInterestsCommissionPaid == default)
            {
                account.LastDayInterestsCommissionPaid = account.History[0].DateTime;
            }

            for (DateTime curDate = account.LastDayInterestsCommissionPaid.Date;
                curDate <= dateTime.Date;
                curDate = curDate.AddDays(1))
            {
                Transaction lastTransaction =
                    account.History.LastOrDefault(transaction => transaction.DateTime.Date <= curDate);
                if (lastTransaction == null) continue;
                decimal curBalance = lastTransaction.BalanceAfterTransaction;
                account.TempInterestCommissionSum += (curBalance * (percents / 36500)) +
                                                     (account.TempInterestCommissionSum * (percents / 36500));
            }

            account.LastDayInterestsCommissionPaid = dateTime;
        }

        public void CalculateCreditCommissionsForAccount(CreditAccount creditAccount, DateTime dateTime)
        {
            if (creditAccount.LastDayInterestsCommissionPaid == default)
            {
                creditAccount.LastDayInterestsCommissionPaid = creditAccount.History[0].DateTime;
            }

            for (DateTime curDate = creditAccount.LastDayInterestsCommissionPaid.Date;
                curDate <= dateTime.Date;
                curDate = curDate.AddMonths(1))
            {
                Transaction lastTransaction =
                    creditAccount.History.LastOrDefault(transaction => transaction.DateTime.Date <= curDate);
                if (lastTransaction == null) continue;
                if (lastTransaction.BalanceAfterTransaction < 0)
                {
                    creditAccount.TempInterestCommissionSum += CreditCommission;
                }
            }

            creditAccount.LastDayInterestsCommissionPaid = dateTime;
        }

        private decimal CalculateDepositPercent(decimal depositSum)
        {
            foreach (InterestRate interestRate in InterestRates)
            {
                if (depositSum < interestRate.Limit) return interestRate.Percent;
            }

            return InterestRates.Last().Percent;
        }
    }
}
using System;
using System.Collections.Generic;

namespace Banks
{
    public class CentralBank
    {
        private List<Bank> _banks;

        public CentralBank()
        {
            _banks = new List<Bank>();
        }

        public void Notify(DateTime dateTime)
        {
            foreach (Bank bank in _banks)
            {
                bank.PayInterestsAndCommissions(dateTime);
            }
        }

        public Bank RegisterBank(string name, decimal debitPercent, int unverifiedLimit, int creditBelowZeroLimit, decimal creditCommission, List<InterestRate> interestRates)
        {
            var bank = new Bank(name, debitPercent, unverifiedLimit, creditBelowZeroLimit, creditCommission, interestRates);
            _banks.Add(bank);
            return bank;
        }
    }
}
namespace Banks
{
    public enum TransactionType
    {
        /// <summary> Transfer </summary>
        Transfer,

        /// <summary> Withdrawal </summary>
        Withdrawal,

        /// <summary> Refill </summary>
        Refill,

        /// <summary> SubtractCommission </summary>
        SubtractCommission,

        /// <summary> AddInterest </summary>
        AddInterest,
    }
}
using System.ComponentModel;

namespace ValueObjects.Shared
{
    public enum AccountTypeEnum
    {
        Unknown,

        [MinimumBalance("50.50")]
        [Description("Savings Account")]
        Savings,

        [MinimumBalance(0)]
        [Description("Cheque Account")]
        Cheque,

        [MinimumBalance(10)]
        [Description("Transmition Account")]
        Transmition,

        [MinimumBalance(20)]
        [Description("Investment Account")]
        Investment
    }
}
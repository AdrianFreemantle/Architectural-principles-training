using System;
using ValueObjects.Shared;
using ValueObjects.ValueTypes;
using ValueObjects.ValueTypes.AccountType;

namespace ValueObjects.WithValueObjects
{
    public class Person
    {
        private DateOfBirth dateOfBirth;

        public Person(DateTime dateOfBirth)
        {
            this.dateOfBirth = dateOfBirth;
        }

        public Account OpenAccount(AccountTypeEnum accountType, Money initialDeposit)
        {
            if (dateOfBirth.GetCurrentAge() < 18)
            {
                throw new InvalidOperationException("Must be older than 18 to open an account");
            }

            return new Account(accountType, initialDeposit);
        }
    }
}
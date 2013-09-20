using System;
using ValueObjects.Shared;

namespace ValueObjects.WithoutValueObjects
{
    public class Person
    {
        private DateTime dateOfBirth;

        public Person(DateTime dateOfBirth)
        {
            this.dateOfBirth = dateOfBirth;
        }

        public Account OpenAccount(AccountTypeEnum accountType, decimal initialDeposit)
        {
            if (GetCurrentAge() < 18)
            {
                throw new InvalidOperationException("Must be older than 18 to open an account");
            }

            return new Account(accountType, initialDeposit);
        }

        public int GetAgeAtDate(DateTime date)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - dateOfBirth.Year;

            if (dateOfBirth > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        public int GetCurrentAge()
        {
            return GetAgeAtDate(DateTime.Now);
        }
    }
}
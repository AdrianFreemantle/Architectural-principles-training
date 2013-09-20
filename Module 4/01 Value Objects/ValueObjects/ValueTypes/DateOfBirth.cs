using System;

namespace ValueObjects.ValueTypes
{
    public struct DateOfBirth : IEquatable<DateOfBirth>
    {
        private readonly DateTime birthDate; 

        public DateOfBirth(DateTime dateOfBith)
            : this()
        {
            if (dateOfBith >= DateTime.Today.AddDays(1))
            {
                throw new ArgumentException("The date of birth cannot be in the future.", "dateOfBith");
            }

            birthDate = dateOfBith.Date;
        }

        public PersonAge GetAgeAtDate(DateTime date)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;

            if (birthDate > today.AddYears(-age))
            {
                age--;
            }

            return new PersonAge(age);
        }

        public PersonAge GetCurrentAge()
        {
            return GetAgeAtDate(DateTime.Now);
        }

        public static implicit operator DateTime(DateOfBirth dateOfBirth)
        {
            return dateOfBirth.birthDate;
        }

        public static implicit operator DateOfBirth(DateTime dateOfBirth)
        {
            return new DateOfBirth(dateOfBirth);
        }

        public override int GetHashCode()
        {
            return birthDate.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is DateOfBirth && Equals((DateOfBirth)obj);
        }

        public bool Equals(DateOfBirth other)
        {
            return other.birthDate == birthDate;
        }

        public static bool operator ==(DateOfBirth left, DateOfBirth right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DateOfBirth left, DateOfBirth right)
        {
            return !Equals(left, right);
        }
    }
}
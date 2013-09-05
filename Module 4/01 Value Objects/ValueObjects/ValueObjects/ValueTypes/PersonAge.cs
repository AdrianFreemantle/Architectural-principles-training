using System;

namespace ValueObjects.ValueTypes
{
    public struct PersonAge : IEquatable<PersonAge>
    {
        private readonly int age;

        public PersonAge(int age)
            : this()
        {
            if (age < 0)
            {
                throw new ArgumentException("A persons age must be a non negative number", "age");
            }

            this.age = age;
        }

        public static implicit operator int(PersonAge personAge)
        {
            return personAge.age;
        }

        public override int GetHashCode()
        {
            return age;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is PersonAge && Equals((PersonAge)obj);
        }

        public bool Equals(PersonAge other)
        {
            return other.age == age;
        }

        public static bool operator ==(PersonAge left, PersonAge right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PersonAge left, PersonAge right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return age.ToString();
        }
    }
}

using System;

namespace AsbaBank.Domain.Models
{
    public class Address
    {
        public string StreetNumber { get; protected set; }
        public string Street { get; protected set; }
        public string PostalCode { get; protected set; }
        public string City { get; protected set; }

        protected Address()
        {
            //here for entity framework
        }

        public Address(string streetNumber, string street, string city, string postalCode)
        {
            ValidateInput("street number", streetNumber);
            ValidateInput("street", street);
            ValidateInput("postal code", postalCode);
            ValidateInput("city", city);
        }

        public static Address NullAddress()
        {
            return new Address
            {
                City = "Not yet captured",
                PostalCode = "Not yet captured",
                Street = "Not yet captured",
                StreetNumber = "Not yet captured",
            };
        }

        private void ValidateInput(string parameterName, string value)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(String.Format("Please provide a valid {0}.", parameterName));
            }
        }
    }
}
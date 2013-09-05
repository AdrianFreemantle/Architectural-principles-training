using System;
using System.Runtime.Serialization;
using AsbaBank.Core.Commands;

namespace AsbaBank.ApplicationService.Commands
{
    [DataContract]
    [CommandAuthorize("Administrator", "Guest")]
    public class UpdateClientAddress : ICommand
    {
        [DataMember]
        public int ClientId { get; private set; }
        [DataMember]
        public string StreetNumber { get; private set; }
        [DataMember]
        public string Street { get; private set; }
        [DataMember]
        public string PostalCode { get; private set; }
        [DataMember]
        public string City { get; private set; }

        public UpdateClientAddress(int clientId, string streetNumber, string street, string city, string postalCode)
        {
            if (String.IsNullOrWhiteSpace(streetNumber))
            {
                throw new ArgumentException("Please provide a valid street number.");
            }

            if (String.IsNullOrWhiteSpace(street))
            {
                throw new ArgumentException("Please provide a valid street.");
            }

            if (String.IsNullOrWhiteSpace(postalCode))
            {
                throw new ArgumentException("Please provide a valid postal code.");
            }

            if (String.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException("Please provide a valid city.");
            }

            ClientId = clientId;
            StreetNumber = streetNumber;
            Street = street;
            PostalCode = postalCode;
            City = city;
        }
    }
}
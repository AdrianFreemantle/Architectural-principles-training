using System;
using System.Runtime.Serialization;
using AsbaBank.Core;
using AsbaBank.Core.Commands;

namespace AsbaBank.ApplicationService.Commands
{
    [DataContract]
    [CommandAuthorize("Administrator")]
    [CommandRetry(RetryCount = 3, RetryMilliseconds = 1000)]
    public class RegisterClient : ICommand
    {
        [DataMember]
        public string ClientName { get; private set; }
        [DataMember]
        public string ClientSurname { get; private set; }
        [DataMember]
        public string PhoneNumber { get; private set; }

        public RegisterClient(string clientName, string clientSurname, string phoneNumber)
        {
            if (String.IsNullOrEmpty(clientName) || clientName.Length < 3)
            {
                throw new ArgumentException("Please provide a valid client name of at least three characters.");
            }

            if (String.IsNullOrEmpty(clientSurname) || clientSurname.Length < 3)
            {
                throw new ArgumentException("Please provide a valid client name of at least three characters.");
            }

            if (String.IsNullOrEmpty(phoneNumber) || phoneNumber.Length != 10 || !phoneNumber.IsDigitsOnly())
            {
                throw new ArgumentException("Please provide a valid phone number that is 10 digits long.");
            }

            ClientName = clientName;
            ClientSurname = clientSurname;
            PhoneNumber = phoneNumber;
        }
    }
}

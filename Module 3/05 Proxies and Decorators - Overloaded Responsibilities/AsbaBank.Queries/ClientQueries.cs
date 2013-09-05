using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AsbaBank.Core.Persistence;
using AsbaBank.Domain.Models;

namespace AsbaBank.Queries
{ 
    public class ClientQueries
    {
        private readonly IEntityQuery entityQuery;
        private readonly ISqlQuery sqlQuery;

        public ClientQueries(IEntityQuery entityQuery, ISqlQuery sqlQuery)
        {
            this.entityQuery = entityQuery;
            this.sqlQuery = sqlQuery;
        }

        private Expression<Func<Client, ClientDto>> ClientToDtoMapping
        {
            get
            {
                return c => new ClientDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Surname = c.Surname,
                    PhoneNumber = c.PhoneNumber
                };
            }
        }

        public ICollection<ClientDto> All()
        {
            return entityQuery
                .Query<Client>()
                .Select(ClientToDtoMapping)
                .ToList();
        }

        public ICollection<ClientDto> WithPhoneNumberLike(string phoneNumber)
        {
            return entityQuery.Query<Client>()
                              .Where(c => c.PhoneNumber.Contains(phoneNumber))
                              .Select(ClientToDtoMapping)
                              .ToList();
        }

        public ICollection<ClientDto> WithSurname(string surname)
        {
            return entityQuery.Query<Client>()
                              .Where(c => c.Surname.Equals(surname))
                              .Select(ClientToDtoMapping)
                              .ToList();
        }

        public ICollection<ClientDto> WithName(string name)
        {
            var query = sqlQuery.SqlQuery<ClientDto>("SELECT Id, Name, Surname, PhoneNumber FROM Client WHERE Name = {0}", name);

            return query.ToList();
        }

        public ICollection<ClientDto> WithPhoneNumber(string phoneNumber)
        {
            var clients = entityQuery.Query<Client>();

            var query = from c in clients
                        where c.PhoneNumber.Equals(phoneNumber)
                        select new ClientDto
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Surname = c.Surname,
                            PhoneNumber = c.PhoneNumber
                        };

            return query.ToList();
        }
    }
}

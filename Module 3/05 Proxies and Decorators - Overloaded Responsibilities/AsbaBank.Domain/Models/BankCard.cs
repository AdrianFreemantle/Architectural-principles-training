using System;
using System.ComponentModel.DataAnnotations;

namespace AsbaBank.Domain.Models
{
    public class BankCard
    {
        [Key]
        public virtual int Id { get; protected set; }
        public virtual bool Disabled { get; protected set; }
        public virtual DateTime Issued { get; protected set; }

        public virtual int AccountId { get; protected set; }
        public virtual Account Account { get; protected set; }

        protected BankCard()
        {
            //here for entity framework
        }

        internal BankCard(int accountId)
        {
            AccountId = accountId;
            Issued = DateTime.Now;
        }

        public void Disable()
        {
            Disabled = true;
        }
    }
}
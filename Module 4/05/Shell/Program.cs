using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Entities;

namespace Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            var ledger = StronglyTypedLedger.Open(new LedgerId(Guid.NewGuid()));

            ledger.Credit(10);
            ledger.Debit(10);
            ledger.Credit(20);

            var snapshot = ((IAmRestorable)ledger).GetSnapshot();

            var ledger2 = ActivatorHelper.CreateInstanceUsingNonPublicConstructor<StronglyTypedLedger>(snapshot.Identity);
            ((IAmRestorable)ledger2).RestoreSnapshot(snapshot);

        }
    }

    public class LedgerState : IMemento
    {
        public IHaveIdentity Identity { get; set; }
        public List<decimal> Transactions { get; set; }

        public LedgerState()
        {
            Transactions = new List<decimal>();
        }

        public void When(LegerDebited e)
        {
            Transactions.Add(-e.Amount);
        }

        public void When(LegerCredited e)
        {
            Transactions.Add(e.Amount);
        } 
    }

    public class StronglyTypedLedger : RestorableTypedAggregate<LedgerState>
    {
        protected StronglyTypedLedger(LedgerId identity) 
            : base(identity)
        {
        }

        public static StronglyTypedLedger Open(LedgerId identity)
        {
            return new StronglyTypedLedger(identity);
        }

        public void Debit(decimal amount)
        {
            RaiseEvent(new LegerDebited(amount));
        }

        public void Credit(decimal amount)
        {
            RaiseEvent(new LegerCredited(amount));
        }
    }

    public partial class Account
    {
        private Ledger ledger;
        protected bool isActive;

        private void When(LedgerCreated e)
        {
            ledger = new Ledger(this, e.LedgerId);
        }

        private void When(AccountOpened e)
        {
            isActive = true;
        }

        private void When(DebitAttemptExceededLedgerLimits e)
        {

        }
    }

    public partial class Account : Aggregate
    {       
        protected Account(AccountId accountId)
            :base(accountId)
        {
        }

        public static Account OpenAccount(AccountId accountId)
        {
            var account = new Account(accountId);
            account.OpenAccount();
            return account;
        }

        void OpenAccount()
        {
            RaiseEvent(new AccountOpened());
        }

        public void CreateLedger(LedgerId ledgerId)
        {
            RaiseEvent(new LedgerCreated(ledgerId));
        }

        public void CreditAccount(decimal amount)
        {
            ledger.Credit(amount);
        }

        public void DebitAccount(decimal amount)
        {
            if ((ledger.Balance() - amount) < 0)
            {
                RaiseEvent(new DebitAttemptExceededLedgerLimits(amount, ledger.Balance()));
                return;
            }

            ledger.Debit(amount);
        }
    }

    public class Ledger : Entity
    {
        readonly List<decimal> transactions;
 
        public Ledger(IAggregate parent, LedgerId identity)
            : base(parent, identity)
        {
            transactions = new List<decimal>();
        }

        public void Debit(decimal amount)
        {           
            RaiseEvent(new LegerDebited(amount));
        }

        public void Credit(decimal amount)
        {
            RaiseEvent(new LegerCredited(amount));
        }

        public decimal Balance()
        {
            return transactions.Sum();
        }

        private void When(LegerDebited e)
        {
            transactions.Add(-e.Amount);
        }

        private void When(LegerCredited e)
        {
            transactions.Add(e.Amount);
        }     
    }








    public class DebitAttemptExceededLedgerLimits : DomainEvent
    {
        public decimal Amount { get; protected set; }
        public decimal CurrentBalance  { get; protected set; }

        public DebitAttemptExceededLedgerLimits(decimal amount, decimal currentBalance)
        {
            CurrentBalance = currentBalance;
            Amount = amount;
        }
    }




    public class LegerDebited : DomainEvent
    {
        public decimal Amount { get; protected set; }

        public LegerDebited(decimal amount)
        {
            Amount = amount;
        }
    }

    public class LegerCredited : DomainEvent
    {
        public decimal Amount { get; protected set; }

        public LegerCredited(decimal amount)
        {
            Amount = amount;
        }
    }

    public class LedgerCreated : DomainEvent
    {
        public LedgerId LedgerId { get; protected set; }

        public LedgerCreated(LedgerId ledgerId)
        {
            LedgerId = ledgerId;
        }
    }


    public class AccountOpened : DomainEvent
    {

    }

    public class AccountId : Identity<string>
    {
        public AccountId(string id)
            : base(id)
        {
        }
    }

    public class LedgerId : Identity<Guid>
    {
        public LedgerId(Guid id)
            : base(id)
        {
        }
    }


    public static class ActivatorHelper
    {
        const BindingFlags Flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;

        public static T CreateInstance<T>(params object[] parameters) where T : class
        {
            if (parameters.Length == 0)
                return Activator.CreateInstance(typeof(T)) as T;

            return Activator.CreateInstance(typeof(T), parameters) as T;
        }

        public static T CreateInstanceUsingNonPublicConstructor<T>(params object[] parameters) where T : class
        {
            Type[] types = parameters.ToList().ConvertAll(input => input.GetType()).ToArray();

            var constructor = typeof(T).GetConstructor(Flags, null, types, null);

            return constructor.Invoke(parameters) as T;
        }

        public static List<TBase> CreateInstancesImplimentingBase<TBase>(string assemblyName) where TBase : class
        {
            var assembly = Assembly.Load(assemblyName);
            var concreteSubTypes = GetConcreteSubTypes<TBase>(assembly);
            return CreateInstancesImplimentingBase<TBase>(concreteSubTypes);
        }

        public static List<TBase> CreateInstancesImplimentingBase<TBase>() where TBase : class
        {
            var assembly = Assembly.GetAssembly(typeof(TBase));
            var concreteSubTypes = GetConcreteSubTypes<TBase>(assembly);
            return CreateInstancesImplimentingBase<TBase>(concreteSubTypes);
        }

        public static List<TBase> CreateInstancesImplimentingBase<TBase>(IEnumerable<Type> concreteTypes) where TBase : class
        {
            return concreteTypes.Select(type => Activator.CreateInstance(type) as TBase).ToList();
        }

        public static IEnumerable<Type> GetConcreteSubTypes<TBase>(Assembly sourceAssembly) where TBase : class
        {
            var assignableFromTypes = sourceAssembly.GetTypes().Where(type => type.IsAssignableFrom(typeof(TBase)));
            var implimentingInterfaceTypes = sourceAssembly.GetTypes().Where(type => type.GetInterfaces().Contains(typeof(TBase)));

            return assignableFromTypes.Union(implimentingInterfaceTypes).Where(type => !type.IsAbstract && type.IsClass).ToList();
        }
    }
}

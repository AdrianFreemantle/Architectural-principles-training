using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using AsbaBank.Domain.Models;

namespace AsbaBank.DataModel
{
    public class AsbaContext : DbContext 
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<BankCard> BankCards { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public AsbaContext()
        {
            Database.SetInitializer(new AsbaContextInitializer());
        }

        public AsbaContext(string connectionStringName)
            : base(connectionStringName)
        {
            Database.SetInitializer(new AsbaContextInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}

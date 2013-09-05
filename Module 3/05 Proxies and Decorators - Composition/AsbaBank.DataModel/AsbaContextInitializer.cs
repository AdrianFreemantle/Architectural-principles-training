using System.Data.Entity;

namespace AsbaBank.DataModel
{
    public class AsbaContextInitializer : IDatabaseInitializer<AsbaContext>
    {
        public void InitializeDatabase(AsbaContext context)
        {
            if (!context.Database.Exists())
            {
                context.Database.Create();
            }
        }
    }
}
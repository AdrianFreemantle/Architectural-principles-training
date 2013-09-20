using System.Diagnostics;

namespace Aggregates
{
    public class Entity : EntityBase
    {
        protected Entity(IHaveIdentity identity)
            : base(identity)
        {
        }
    }
}
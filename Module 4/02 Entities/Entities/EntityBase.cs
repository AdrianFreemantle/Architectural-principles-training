using System;

namespace Entities
{
    public abstract class EntityBase : IEntity
    {
        public IHaveIdentity Identity { get; set; }

        protected EntityBase(IHaveIdentity identity)
        {
            Identity = identity;
        }

        public override int GetHashCode()
        {
            return Identity.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as EntityBase);
        }

        public virtual bool Equals(EntityBase other)
        {
            if (null != other && other.GetType() == GetType())
            {
                return other.Identity.Equals(Identity);
            }

            return false;
        }

        public static bool operator ==(EntityBase left, EntityBase right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EntityBase left, EntityBase right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Identity.ToString();
        }
    }
}
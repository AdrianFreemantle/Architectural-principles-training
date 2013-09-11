﻿using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Entities
{
    [DataContract]
    public abstract class Identity : IEquatable<Identity>, IHaveIdentity
    {
        private static readonly Type[] SupportTypes = {typeof(int), typeof(long), typeof(uint), typeof(ulong), typeof(Guid), typeof(string)};

        protected dynamic Id { get; private set; }

        public abstract string GetTag();
               
        protected Identity(dynamic id)
        {           
            VerifyIdentityType(id);
            Id = id;
        }

        public dynamic GetId()
        {
            return Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;

            var identity = obj as Identity;

            return identity != null && Equals(identity);
        }

        public bool Equals(Identity other)
        {
            if (other != null)
            {
                return other.Id.Equals(Id) && other.GetTag() == GetTag();
            }

            return false;
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", GetTag(), Id);
        }

        public override int GetHashCode()
        {
            return (Id.GetHashCode());
        }

        void VerifyIdentityType(dynamic id)
        {
            if (id == null)
            {
                throw new ArgumentException("You must provide a non null value as an identity");
            }

            var type = id.GetType();

            if (SupportTypes.Any(t => t == type))
            {
                return;
            }

            throw new InvalidOperationException("Abstract identity inheritors must provide stable hash. It is not supported for:  " + type);
        }

        public virtual bool IsEmpty()
        {
            var type = Id.GetType();

            if (type == typeof (string))
            {
                return Id == string.Empty;
            }

            return Activator.CreateInstance(type) == Id;
        }

        public static bool operator ==(Identity left, Identity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Identity left, Identity right)
        {
            return !Equals(left, right);
        }
    }
}
using System;
using System.Collections.Generic;

namespace Mint.Core.Domain
{
    public abstract class Entity<TKey> : IEquatable<Entity<TKey>>
    {
        public TKey Id { get; protected set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        
        protected Entity() { }
       
        protected Entity(TKey id)
        {
            Id = id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;

            if (ReferenceEquals(this, obj)) return true;

            return obj.GetType() == GetType() && Equals((Entity<TKey>)obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TKey>.Default.GetHashCode(Id);
        }

        public bool Equals(Entity<TKey> other)
        {
            return other != null && Id.Equals(other.Id);
        }
    }

    public abstract class EntityWithGuid : Entity<Guid> 
    {
        protected EntityWithGuid()
        {
            Id = Guid.NewGuid();
        }
    }
}

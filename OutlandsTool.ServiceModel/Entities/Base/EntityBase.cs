using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Web;

namespace OutlandsTool.ServiceModel.Entities.Base
{
    [DataContract]
    public abstract class EntityBase<TId> : IEntity, IEquatable<EntityBase<TId>>
      where TId : struct
    {

        private object _id;
        private string _key;

        [DataMember]
        public virtual TId Id
        {
            get
            {
                if (_id == null && typeof(TId) == typeof(Guid))
                {
                    _id = Guid.NewGuid();
                }

                return _id == null ? default(TId) : (TId)_id;
            }

            set
            {
                _id = value;
            }
        }

        //[Unique]
        //[StringLength(50)]
        public virtual string Key
        {
            //get { return _key = _key ?? GenerateKey(); }
            //protected set { _key = value; }
            get { return _key; }
        }

        int IEntity.Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Equals(EntityBase<TId> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (other.GetType() != GetType()) return false;

            if (default(TId).Equals(Id) || default(TId).Equals(other.Id))
                return Equals(other._key, _key);

            return other.Id.Equals(Id);
        }

        protected virtual string GenerateKey()
        {
            return KeyGenerator.Generate();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Entity<TId>)) return false;
            return Equals((EntityBase<TId>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                //if (default(TId).Equals(Id))
                //    return Key.GetHashCode() * 397;

                return Id.GetHashCode();
            }
        }

        //public override string ToString()
        //{
        //    return Key;
        //}

        public static bool operator ==(EntityBase<TId> left, EntityBase<TId> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EntityBase<TId> left, EntityBase<TId> right)
        {
            return !Equals(left, right);
        }

        public static class KeyGenerator
        {
            public static string Generate()
            {
                return Generate(Guid.NewGuid().ToString("D").Substring(24));
            }

            public static string Generate(string input)
            {
                Contract.Requires(!string.IsNullOrWhiteSpace(input));
                return HttpUtility.UrlEncode(input.Replace(" ", "_").Replace("-", "_").Replace("&", "and"));
            }
        }
    }
}

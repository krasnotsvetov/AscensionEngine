using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ascension.Engine.Core.Common
{
    [DataContract]
    public class BaseReference<T> : IReference<T> where T : class
    {

        public bool IsValid = true;
        public BaseReference()
        {

        }

        protected BaseReference(T name)
        {
            this.Name = name;
        }

        [DataMember]
        public virtual T Name
        {
            get; set;
        }

        public override string ToString()
        {
            return Name.ToString();
        }

        public override bool Equals(object obj)
        {
            BaseReference<T> other = obj as BaseReference<T>;
            if (other == null) return false;
            return Name.Equals(other.Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}

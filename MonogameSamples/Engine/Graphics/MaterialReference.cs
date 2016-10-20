using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MonogameSamples.Engine.Graphics
{
    [DataContract]
    public class MaterialReference
    {
        [DataMember]
        public string MaterialName;

        public MaterialReference(string name)
        {
            MaterialName = name;
        }

        public override bool Equals(object obj)
        {
            MaterialReference other = obj as MaterialReference;
            if (other == null) return false;
            return MaterialName.Equals(other.MaterialName);
        }

        public override int GetHashCode()
        {
            return MaterialName.GetHashCode();
        }
    }
}

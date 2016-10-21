using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MonogameSamples.Engine.Graphics.Shaders
{
    [DataContract]
    public class ShaderReference
    {
        [DataMember]
        public string ShaderName;

        public ShaderReference(string name)
        {
            ShaderName = name;
        }

        public override bool Equals(object obj)
        {
            ShaderReference other = obj as ShaderReference;
            if (other == null) return false;
            return ShaderName.Equals(other.ShaderName);
        }

        public override int GetHashCode()
        {
            return ShaderName.GetHashCode();
        }
    }
}

using MonogameSamples.Engine.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MonogameSamples.Engine.Graphics
{
    public class MaterialReference : BaseReference<string>
    {
        public MaterialReference()
        {

        }

        protected MaterialReference(string name) : base(name)
        {
            
        }

        public static MaterialReference FromIdentifier(string name)
        {
            return new MaterialReference(name);
        }
    }
}

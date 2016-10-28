using MonogameSamples.Engine.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameSamples.Engine.Graphics
{
    public class Texture2DReference : BaseReference<string>
    {
        public Texture2DReference()
        {

        }

        protected Texture2DReference(string name) : base(name)
        {

        }

        public static Texture2DReference FromIdentifier(string name)
        {
            return new Texture2DReference(name);
        }



    }
}

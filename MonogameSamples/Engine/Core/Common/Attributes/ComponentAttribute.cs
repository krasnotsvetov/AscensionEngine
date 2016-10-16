using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameSamples.Engine.Core.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentAttribute : Attribute
    {

        public string Name { get; }
        public ComponentAttribute(string name)
        {
            Name = name;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascension.Engine.Core.Common.Collections.Pooling
{
    public class DefaultObjectCreator<T>: IObjectCreator<T> where T : class, new()
    {
        public T Create()
        {
            return Activator.CreateInstance<T>();
        }
    }
}

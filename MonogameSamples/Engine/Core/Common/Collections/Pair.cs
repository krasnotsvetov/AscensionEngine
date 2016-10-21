using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameSamples.Engine.Core.Common.Collections
{
    public class Pair<T, R>
    {
        public T first;
        public R second;
        public Pair(T first, R second)
        {
            this.first = first;
            this.second = second;
        }
    }
}

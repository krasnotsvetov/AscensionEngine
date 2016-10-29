using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascension.Engine.Core.Common.Collections.Pooling
{
    public interface IObjectCreator<T>
    {
        T Create();
    }
}

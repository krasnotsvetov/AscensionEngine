using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascension.Engine.Core.Common
{
    public interface IReference<T>
    {

        T Name { get; set; }
    }
}

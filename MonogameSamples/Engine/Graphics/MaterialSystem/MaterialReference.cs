using Ascension.Engine.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ascension.Engine.Graphics
{
    public class MaterialReference : BaseReference<string>
    {
        /// <summary>
        /// Don't use this constructor to get a reference for Material. Use Material.Reference to get reference else 
        /// if material changes name you will have a reference to missing material.
        /// </summary>
        public MaterialReference()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        internal MaterialReference(string name) : base(name)
        {
            
        }
    }
}

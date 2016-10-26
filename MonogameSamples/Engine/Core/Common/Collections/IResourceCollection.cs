using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameSamples.Engine.Core.Common.Collections
{
    public interface IResourceCollection<ReferenceTypeValue, ReferenceType, Value> where ReferenceType : IReference<ReferenceTypeValue> where Value : class
    {
        /// <summary>
        ///  If ResourceCollections contains element False will be return
        /// </summary>
        bool Add(ReferenceTypeValue t, Value r);

        /// <summary>
        ///  If ResourceCollections contains element False will be return
        /// </summary>
        bool Add(ReferenceType t, Value r);

        Value Remove(ReferenceTypeValue t);

        Value Remove(ReferenceType t);


        Value FromIdentifier(ReferenceTypeValue t);
        Value FromReference(ReferenceType t);


        Value this[ReferenceType t]
        {
            get;
            set;
        }


        Value this[ReferenceTypeValue t]
        {
            get;
            set;
        }



    }
}

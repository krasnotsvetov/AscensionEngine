using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameSamples.Engine.Core.Common.Collections
{
    public class StringReferenceCollection<Reference, Value> : IResourceCollection<string, Reference, Value> where Reference : BaseReference<string>, new() where Value : class
    {


        private Dictionary<Reference, Value> items = new Dictionary<Reference, Value>();
        public StringReferenceCollection()
        {

        }

        public Value this[Reference r]
        {
            get
            {
                return FromReference(r);
            }
            set
            {
                if (items.ContainsKey(r))
                {
                    items[r] = value;
                } else
                {
                    throw new IndexOutOfRangeException("Collection doesn't contain this key");
                }               
            }
        }

        public Value this[string r]
        {
            get
            {
                return FromIdentifier(r);
            }

            set
            {
                Reference _r = new Reference();
                _r.Name = r;
                items[_r] = value;
            }
        }

        public bool Add(Reference r, Value v)
        {
            if (items.ContainsKey(r))
            {
                return false;
            }

            items.Add(r, v);
            return true;
        }

        public bool Add(string r, Value v)
        {
            Reference _r = new Reference();
            _r.Name = r;
            return Add(_r, v);
        }

        public Value FromIdentifier(string r)
        {
            Reference _r = new Reference();
            _r.Name = r;
            return FromReference(_r);
        }

        public Value FromReference(Reference t)
        {
            if (!items.ContainsKey(t))
            {
                return null;
            }
            return items[t];
        }

        public bool Remove(Reference t)
        {
            return items.Remove(t);
        }

        public bool Remove(string t)
        {
            var _r = new Reference();
            _r.Name = t;
            return Remove(_r);
        }

        public IEnumerator<Value> GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }

       

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.Values.GetEnumerator();
        }
    }
}

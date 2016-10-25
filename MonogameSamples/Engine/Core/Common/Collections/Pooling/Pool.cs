using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonogameSamples.Engine.Core.Common.Collections.Pooling
{
    //Primitive pool

    public class Pool<T> : IPool<T> where T : IPoolable
    {
        private T[] items;
        private uint cursor;
        private IObjectCreator<T> creator;
        private IObjectInitializer<T> initializer;

        public Pool(int size, IObjectCreator<T> objectCreator, IObjectInitializer<T> initializer = null)
        {
            if (objectCreator == null)
            {
                throw new ArgumentNullException("objectCreator", "ObjectCreate should have a value");
            }
            creator = objectCreator;
            this.initializer = initializer;
            items = new T[size];
        }

        public void Add(T t)
        {
            if (cursor != items.Length)
            {
                t.Clear();
                items[cursor++] = t;
            }
        }

        public T Get()
        {
            T t;
            if (cursor == 0)
            {
                t = creator.Create();
            }
            else
            {
                t = items[--cursor];
            }
            if (initializer != null)
            {
                initializer.Initialize(t);
            }
            return t;
        }
    }
}

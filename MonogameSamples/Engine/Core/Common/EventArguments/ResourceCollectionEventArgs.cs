using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameSamples.Engine.Core.Common.EventArguments
{
    public enum Operation{
        None,
        Replaced,
        Removed,
        Added
    }

    public class ResourceCollectionEventArgs<T> : EventArgs
    {
        public Operation Operation;
        public object LastItem;
        public object NewItem;
        public IReference<T> Reference;

        public ResourceCollectionEventArgs(Operation operation,IReference<T> reference, object lastItem, object newItem) : base()
        {
            this.Operation = operation;
            this.Reference = reference;
            this.LastItem = lastItem;
            this.NewItem = newItem;
        }
    }
}

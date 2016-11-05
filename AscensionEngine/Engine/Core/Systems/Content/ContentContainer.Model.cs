using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascension.Engine.Core.Systems.Content
{
    public partial class ContentContainer
    {
        private Dictionary<string, Model> Models = new Dictionary<string, Model>();
        private Dictionary<string, List<IModelOwner>> modelOwners = new Dictionary<string, List<IModelOwner>>();

        public Model GetModel(string name, bool immediately = false)
        {
            return Models[name];
        }

        
        public void AddModelListener(IModelOwner owner, string value)
        {
            lock (modelOwners)
            {
                if (!modelOwners.ContainsKey(value))
                {
                    modelOwners.Add(value, new List<IModelOwner>());
                }

                if (!modelOwners[value].Contains(owner))
                {
                    modelOwners[value].Add(owner);
                }
            }
        }


        
        public void RemoveModelListener(IModelOwner owner, string value)
        {
            lock (modelOwners)
            {
                if (!modelOwners.ContainsKey(value))
                {
                    return;
                }

                if (modelOwners[value].Contains(owner))
                {
                    modelOwners[value].Remove(owner);
                }
                if (modelOwners[value].Count == 0)
                {
                    modelOwners.Remove(value);
                }
            }
        }
    }

    public interface IModelOwner
    {
        EventHandler<ContentOwnerEventArgs<Model>> ModelChangedHandler { get; set; }
    }
}

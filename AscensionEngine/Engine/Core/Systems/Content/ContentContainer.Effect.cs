using Ascension.Engine.Graphics;
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
        private Dictionary<string, Effect> Effects = new Dictionary<string, Effect>();
        private Dictionary<string, List<IEffectOwner>> effectOwners = new Dictionary<string, List<IEffectOwner>>();



        public Effect GetEffect(string requiredShader, bool immediately = false)
        {
            return Effects[requiredShader];
        }

        public void AddEffectListener(IEffectOwner owner, string value)
        {
            {
                if (!effectOwners.ContainsKey(value))
                {
                    effectOwners.Add(value, new List<IEffectOwner>());
                }

                if (!effectOwners[value].Contains(owner))
                {
                    effectOwners[value].Add(owner);
                }
            }
        }



        public void RemoveEffectListener(IEffectOwner owner, string value)
        {
            if (!effectOwners.ContainsKey(value))
            {
                return;
            }

            if (effectOwners[value].Contains(owner))
            {
                effectOwners[value].Remove(owner);
            }
            if (effectOwners[value].Count == 0)
            {
                effectOwners.Remove(value);
            }
        }
    }

    public interface IEffectOwner
    {
        EventHandler<ContentOwnerEventArgs<Effect>> EffectChangedHandler { get; set; }
    }
}

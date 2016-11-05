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
        private Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        private Dictionary<string, List<ITextureOwner>> texturesOwners = new Dictionary<string, List<ITextureOwner>>();


        public Texture2D GetTexture(string value, bool immediately = false)
        {
            return Textures[value];
        }


 
        public void AddTextureListener(ITextureOwner owner, string value)
        {
            
            if (!texturesOwners.ContainsKey(value))
            {
                texturesOwners.Add(value, new List<ITextureOwner>());
            }

            if (!texturesOwners[value].Contains(owner))
            {
                texturesOwners[value].Add(owner);
            }
        }


 
        public void RemoveTextureListener(ITextureOwner owner, string value)
        {
            if (!texturesOwners.ContainsKey(value))
            {
                return;
            }

            if (texturesOwners[value].Contains(owner))
            {
                texturesOwners[value].Remove(owner);
            }
            if (texturesOwners[value].Count == 0)
            {
                texturesOwners.Remove(value);
            }
        }
    }


    public interface ITextureOwner
    {
        EventHandler<ContentOwnerEventArgs<Texture2D>> TextureChangedHandler { get; set; }
    }

    
}

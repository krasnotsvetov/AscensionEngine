using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Ascension.Engine.Core.Systems.Content;

namespace Ascension.Engine.Graphics
{
    [DataContract]
    public class Material : IEffectOwner, ITextureOwner
    {

        [DataMember]
        public string MaterialName
        {
            get
            {
                return name;
            }
            internal set
            {
                name = value;
            }
        }
        private string name;


        public EventHandler<ContentOwnerEventArgs<Effect>> EffectChangedHandler { get; set; }
        public EventHandler<ContentOwnerEventArgs<Texture2D>> TextureChangedHandler { get; set; }



        public ReadOnlyDictionary<string, Texture2D> Textures
        {
            get
            {
                return new ReadOnlyDictionary<string, Texture2D>(textures);
            }
        }

        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        internal MaterialInformation info;

        public Effect Effect { get { return effect; } }
        private Effect effect;

        public string EffectName { get { return effectName; } }


        private string effectName;

        private RenderSystem renderSystem;

        public Material(MaterialInformation info, bool immediately = false) : this(info, ContentContainer.Instance(), immediately)
        {

        }

        public Material(MaterialInformation info, ContentContainer contentContainer, bool immediately = false)
        {
            this.name = info.Name;
            this.info = info;
            foreach (var p in info.Textures)
            {
                contentContainer.AddTextureListener(this, p.Value);
                textures.Add(p.Key, contentContainer.GetTexture(p.Value, immediately));
            }
            effectName = info.RequiredShader;
            contentContainer.AddEffectListener(this, info.RequiredShader);
            effect = contentContainer.GetEffect(info.RequiredShader, immediately);

            TextureChangedHandler += (s, e) =>
            {
                string type = "";
                foreach (var p in info.Textures)
                {
                    if (p.Value.Equals(e.LastName))
                    {
                        type = p.Key;
                        break;
                    }
                }
                switch (e.Action)
                {
                    case ContentAction.Add:
                        textures[type] = e.New;
                        break;
                    case ContentAction.Remove:
                        textures[type] = null;
                        break;
                    case ContentAction.Rename:
                        ContentContainer cc = ContentContainer.Instance();
                        info.textures.Remove(type);
                        textures.Remove(type);
                        cc.RemoveTextureListener(this, e.LastName);
                        info.textures.Add(type, e.NewName);
                        cc.AddTextureListener(this, e.NewName);
                        textures.Add(type, cc.GetTexture(e.NewName, true));
                        break;
                    case ContentAction.Replace:
                        textures[type] = e.New;
                        break;
                }
            };

            EffectChangedHandler += (s, e) =>
            {
                ContentContainer cc = ContentContainer.Instance();
                switch (e.Action)
                {
                    case ContentAction.Add:
                        effect = e.New;
                        break;
                    case ContentAction.Remove:
                        effect = null;
                        break;
                    case ContentAction.Rename:
                        cc.RemoveEffectListener(this, e.LastName);
                        info.RequiredShader = e.NewName;
                        effectName = e.NewName;
                        cc.AddEffectListener(this, e.NewName);
                        effect = cc.GetEffect(e.NewName, true);
                        break;
                    case ContentAction.Replace:
                        effect = e.New;
                        break;
                }
            };
        }


        public void AddOrChangeTexture(string type, string textureName)
        {
            ContentContainer cc = ContentContainer.Instance();
            if (info.Textures.ContainsKey(type))
            {
                cc.RemoveTextureListener(this, info.Textures[type]);
                info.textures[type] = textureName;
                cc.AddTextureListener(this, textureName);
                textures[type] = cc.GetTexture(textureName, true);
            } else
            {
                info.textures.Add(type, textureName);
                cc.AddTextureListener(this, textureName);
                textures.Add(type, cc.GetTexture(textureName, true));
            }
        }

        public void RemoveTexture(string type)
        {
            ContentContainer.Instance().RemoveTextureListener(this, info.Textures[type]);
            info.textures.Remove(type);
            textures.Remove(type);
        }

        
        public void ChangeEffect(string newEffectName)
        {
            ContentContainer cc = ContentContainer.Instance();
            cc.RemoveEffectListener(this, effectName);
            info.RequiredShader = newEffectName;
            //We should register before we will use immediately mode.
            cc.AddEffectListener(this, newEffectName);
            effect = cc.GetEffect(newEffectName, true);
        }
    }

    public interface IMaterialOwner
    {
        EventHandler<ContentOwnerEventArgs<Material>> MaterialChangedHandler { get; set; }
    }
}

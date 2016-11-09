using Microsoft.Xna.Framework.Graphics;
using Ascension.Engine.Core.Components;
using Ascension.Engine.Graphics;
using Ascension.Engine.Graphics.SceneSystem;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Ascension.Engine.Core.Systems.Content;

namespace Ascension.Engine.Core.Common
{
    [DataContract]
    public class EntityDrawableComponent : DrawableComponent, IMaterialOwner
    {

        protected GraphicsDevice device;
        public virtual Entity ParentEntity { get; set; }
        protected EventHandler<EventArgs> OnMaterialChanged;


        [DataMember]
        public string Name { get; set; }

        public Material Material
        {
            get
            {
                return material;
            }
            set
            {
                var cc = ContentContainer.Instance();
                if (material != null)
                {
                    cc.RemoveMaterialListener(this, MaterialName);
                }
                material = value;
                if (material != null)
                {
                    materialName = material.MaterialName;
                    cc.AddMaterialListener(this, materialName);
                }
                else
                {
                    materialName = "";
                }
                OnMaterialChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public EventHandler<ContentOwnerEventArgs<Material>> MaterialChangedHandler { get; set; }

        private Material material;

        [DataMember]
        public string MaterialName
        {
            get
            {
                return materialName;
            }
            set
            {
                if (!value.Equals(""))
                {
                    var cc = ContentContainer.Instance();
                    if (!cc.MaterialInformation.ContainsKey(value))
                    {
                        Material = null;
                        cc.AddMaterialListener(this, value);
                        materialName = value;
                    }
                    else
                    {
                        Material = cc.MaterialInformation[value].GetMaterial(true);
                    }
                }
                else
                {
                    Material = null;
                }
            }
        }

        private string materialName = "";

        public EntityDrawableComponent(string name, string materialName) : base()
        {
            this.MaterialName = materialName;
            this.Name = name;
            MaterialChangedHandler += MaterialChanged;
        }


        public override void Initialize()
        {
            device = ParentEntity.Scene.RenderSystem.Device;
        }



        private void MaterialChanged(object sender, ContentOwnerEventArgs<Material> e)
        {
            switch (e.Action)
            {
                case ContentAction.Add:
                    Material = e.New;
                    break;
                case ContentAction.Rename:
                    materialName = Material.MaterialName;
                    break;
                case ContentAction.Replace:
                    Material = e.New;
                    break;
                case ContentAction.Remove:
                    Material = null;
                    break;
            }
        }
        internal virtual void RenderSystemChange()
        {
            device = ParentEntity.Scene.RenderSystem.Device;
        }

        internal void DeserealizationInitialize()
        {
            throw new NotImplementedException();
        }
    }
}

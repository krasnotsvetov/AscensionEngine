using Microsoft.Xna.Framework;
using MonogameSamples.Engine.Core.Components;
using MonogameSamples.Engine.Graphics;
using MonogameSamples.Engine.Graphics.SceneSystem;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace MonogameSamples.Engine.Core.Common
{
    [DataContract]
    public class EntityDrawableComponent : DrawableComponent
    {
        public virtual Entity ParentEntity { get; set; }

        [DataMember]
        public string Name { get; set; }

        public MaterialReference MaterialReference {
            get
            {
                return reference;
            }
            set
            {
                reference = value;
                if (material != null)
                {
                    material.MaterialChanged -= MaterialChanged;
                }
                material = ParentEntity.Scene.Materials[MaterialReference];
                if (material != null)
                {
                    material.MaterialChanged += MaterialChanged;
                    MaterialChanged(material, EventArgs.Empty);
                }
            }
        }

        [DataMember]
        private MaterialReference reference;

        public Material Material
        {
            get
            {

                ///Happend if material was removed.
                if (!MaterialReference?.IsValid == true)
                {
                    material = null;
                    reference = null;
                    MaterialChanged(null, EventArgs.Empty);
                }
                return material;
            }
        }

        private Material material;



        public EntityDrawableComponent(string name, MaterialReference materialReference) : base()
        {
            this.reference = materialReference;
            this.Name = name;
        }


        public override void Initialize()
        {
            if (MaterialReference != null)
            {
                material = ParentEntity.Scene.Materials[MaterialReference];
                if (material != null)
                {
                    material.MaterialChanged += MaterialChanged;
                    MaterialChanged(material, EventArgs.Empty);
                }
            }
            base.Initialize();
        }

        internal virtual void RenderSystemChange()
        {
            if (MaterialReference != null)
            {
                material = ParentEntity.Scene.Materials[MaterialReference];
            }
        }


        protected virtual void MaterialChanged(object sender, EventArgs empty)
        { 

        }
    }
}

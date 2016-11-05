using Ascension.Engine.Core.Common;
using Ascension.Engine.Core.Common.Attributes;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;
using Ascension.Engine.Graphics.SceneSystem;
using Microsoft.Xna.Framework.Graphics;
using Ascension.Engine.Core.Systems.Content;
using System;

namespace Ascension.Engine.Core.Components._3DComponents
{
    [DataContract]
    [Component("MeshComponent")]
    public class ModelComponent : EntityDrawableComponent, IModelOwner
    {


        public Model Model
        {
            get
            {
                return model;
            }
            set
            {
                model = value;
            }
        }



        private Model model;


        public EventHandler<ContentOwnerEventArgs<Model>> ModelChangedHandler { get; set; }

        public ModelComponent(string name, string materialName) : base(name, materialName)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        internal override void SceneChanged(Scene lastScene)
        {

        }

        public override void Draw(Matrix view, Matrix projection, GameTime gameTime)
        {
            
            if (model == null)
            {
                return;
            }


            foreach (var m in model.Meshes)
            {
                foreach (var mp in m.MeshParts)
                {
                    device.Indices = mp.IndexBuffer;
                    device.SetVertexBuffer(mp.VertexBuffer);
                    device.DrawIndexedPrimitives(PrimitiveType.TriangleList, mp.VertexOffset, mp.StartIndex, mp.PrimitiveCount);   
                }
            }

            base.Draw(view, projection, gameTime);
        }

    }
}

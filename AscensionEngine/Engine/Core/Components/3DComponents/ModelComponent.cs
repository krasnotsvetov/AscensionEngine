using Ascension.Engine.Core.Common;
using Ascension.Engine.Core.Common.Attributes;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;
using Ascension.Engine.Graphics.SceneSystem;
using Microsoft.Xna.Framework.Graphics;
using Ascension.Engine.Graphics;
using Ascension.Engine.Core.Systems.Content;
using System;

namespace Ascension.Engine.Core.Components._3DComponents
{
    [DataContract]
    [Component("MeshComponent")]
    public class ModelComponent : EntityDrawableComponent, IModelOwner
    {


        public ModelInstance Model
        {
            get
            {
                return instance;
            }
            set
            {
                var cc = ContentContainer.Instance();
                if (instance != null)
                {
                    cc.RemoveModelListener(this, ModelName);
                }
                instance = value;
                if (instance != null)
                {
                    modelName = instance.Name;
                    cc.RemoveModelListener(this, modelName);
                }
                else
                {
                    modelName = "";
                }
            }
        }


        private ModelInstance instance;

        [DataMember]
        public string ModelName
        {
            get
            {
                return modelName;
            }
            set
            {
                if (!value.Equals(""))
                {
                    var cc = ContentContainer.Instance();
                    if (cc.GetModel(value, true) == null)
                    {
                        Model = null;
                        cc.AddModelListener(this, value);
                        modelName = value;
                    }
                    else
                    {
                        Model = cc.GetModel(value, true);
                    }
                }
                else
                {
                    Model = null;
                }
            }
        }

        private string modelName;

        public EventHandler<ContentOwnerEventArgs<ModelInstance>> ModelChangedHandler { get; set; }

        public ModelComponent(string name, string materialName) : base(name, materialName)
        {
            ModelChangedHandler += ModelChanged;
            ModelName = "";
            
        }

        public ModelComponent(string name, string materialName, string modelName) : this(name, materialName)
        {
            ModelName = modelName;
        }

        protected virtual void ModelChanged(object sender, ContentOwnerEventArgs<ModelInstance> e)
        {
            ContentContainer cc = ContentContainer.Instance();
            switch (e.Action)
            {
                case ContentAction.Add:
                    instance = cc.GetModel(modelName);
                    break;
                case ContentAction.Rename:
                    ModelName = e.NewName;
                    break;
                case ContentAction.Remove:
                    instance = null;
                    break;
                case ContentAction.Replace:
                    break;
            }
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
            
            if (instance == null)
            {
                return;
            }


            foreach (var m in instance.Model.Meshes)
            {
                try
                {
                    foreach (var mp in m.MeshParts)
                    {
                        device.Indices = mp.IndexBuffer;
                        device.SetVertexBuffer(mp.VertexBuffer);
                        device.DrawIndexedPrimitives(PrimitiveType.TriangleList, mp.VertexOffset, mp.StartIndex, mp.PrimitiveCount);
                    }
                } catch (InvalidOperationException e)
                {
                    Console.WriteLine("---" + ToString() + "---");
                    Console.WriteLine("Name: ", Name);
                    Console.WriteLine("Render error occured");
                    Console.WriteLine(e.Message);
                }
            }

            base.Draw(view, projection, gameTime);
        }

        public override string ToString()
        {
            return "ModelComponent";
        }

    }
}

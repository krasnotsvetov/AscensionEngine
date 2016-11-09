using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Ascension.Engine.Core.Common;
using Ascension.Engine.Graphics;
using Ascension.Engine.Graphics.SceneSystem;
using Ascension.Engine.Core.Common.Attributes;
using System;
using System.Runtime.Serialization;
using Ascension.Engine.Core.Systems.Content;

namespace Ascension.Engine.Core.Components
{
    [DataContract]
    [Component("Sprite")]
    public class Sprite : EntityDrawableComponent
    {
        public int PixelPerOne
        {
            get
            {
                return pixelPerOne;
            }
            set
            {
                pixelPerOne = value;
            }
        }


        [DataMember]
        private int pixelPerOne = 128;

        [DataMember]
        private Rectangle? sourceRectangle;

        private Texture2D texture;
        private VertexBuffer vertexBuffer;
        private VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[4];
        public Sprite(string name, string materialName) : base(name, materialName)
        {
            this.sourceRectangle = null;
            if (Material != null)
            {
                texture = Material.Textures["Albedo"];
            }

            OnMaterialChanged += MaterialChanged;
        }

        public Sprite(string name, string materialName, Rectangle sourceRectangle) : base(name, materialName)
        {
            this.sourceRectangle = sourceRectangle;
            if (Material != null)
            {
                texture = Material.Textures["Albedo"];
            }
            OnMaterialChanged += MaterialChanged;
        }

       
        private void setupVertices(Rectangle sourceRectangle)
        {
            sourceRectangle = new Rectangle(sourceRectangle.X / texture.Width, sourceRectangle.Y / texture.Height, sourceRectangle.Width / texture.Width, sourceRectangle.Height / texture.Height);

            vertices[0].Color = vertices[1].Color = vertices[2].Color = vertices[3].Color = Color.White;

            vertices[0].TextureCoordinate = new Vector2(sourceRectangle.X, sourceRectangle.Y + sourceRectangle.Height);
            vertices[1].TextureCoordinate = new Vector2(sourceRectangle.X * texture.Width, sourceRectangle.Y);
            vertices[2].TextureCoordinate = new Vector2(sourceRectangle.X + sourceRectangle.Width, sourceRectangle.Y + sourceRectangle.Height);
            vertices[3].TextureCoordinate = new Vector2(sourceRectangle.X + sourceRectangle.Width, sourceRectangle.Y);



            int halfWidth = texture.Width / 2;
            int halfHeight = texture.Height / 2;

            vertices[0].Position = new Vector3(halfWidth / (float)pixelPerOne, -halfHeight / (float)pixelPerOne, 0);
            vertices[1].Position = new Vector3(halfWidth / (float)pixelPerOne, halfHeight / (float)pixelPerOne, 0);
            vertices[2].Position = new Vector3(-halfWidth / (float)pixelPerOne, -halfHeight / (float)pixelPerOne, 0);
            vertices[3].Position = new Vector3(-halfWidth / (float)pixelPerOne, halfHeight / (float)pixelPerOne, 0);



            if (vertexBuffer != null)
            {
                vertexBuffer.Dispose();
            }
            vertexBuffer = new VertexBuffer(device, VertexPositionColorTexture.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);
        }

        public override void Initialize()
        {
            base.Initialize();
            if (texture != null)
            {
                if (sourceRectangle == null)
                {
                    setupVertices(new Rectangle(0, 0, texture.Width, texture.Height));
                }
                else
                {
                    setupVertices((Rectangle)sourceRectangle);
                }
            }
        }

        public override void Draw(Matrix view, Matrix projection, GameTime gameTime)
        {
            try
            {
                var rs = device.RasterizerState;
                device.RasterizerState = new RasterizerState() { CullMode = CullMode.None };
                device.SetVertexBuffer(vertexBuffer);
                device.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
                device.RasterizerState = rs;
            } catch (InvalidOperationException e)
            {
                Console.WriteLine("---" + ToString() + "---");
                Console.WriteLine("Name: " + Name);
                Console.WriteLine("Render error occured");
                Console.WriteLine(e.Message);
            }

            base.Draw(view, projection, gameTime);
        }

        public override string ToString()
        {
            return "Sprite";
        }

        private void MaterialChanged(object sender, EventArgs e)
        { 
            if (Material != null)
            {
                texture = Material.Textures["Albedo"];
                if (texture == null)
                {
                    return;
                }
                if (sourceRectangle == null)
                {
                    setupVertices(new Rectangle(0, 0, texture.Width, texture.Height));
                } else
                {
                    setupVertices((Rectangle)sourceRectangle);
                }
            }
        }

        internal override void RenderSystemChange()
        {
            base.RenderSystemChange();
        }


        public override void Dispose()
        {
            if (vertexBuffer != null && !vertexBuffer.IsDisposed)
            {
                vertexBuffer.Dispose();
            }
            base.Dispose();
        }
    }
}

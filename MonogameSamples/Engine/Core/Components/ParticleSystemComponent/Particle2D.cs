using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameSamples.Engine.Core.Common.Collections.Pooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonogameSamples.Engine.Core.Components.ParticleSystemComponent
{
    public class Particle2D : IPoolable
    {

        Texture2D texture;
        public Particle2D(ParticleSystem2D particleSystem2D)
        {
            texture = particleSystem2D.Material.textures[0];
        }


        public void Initialization(int textureNum, int width, int height, Vector2 position, Vector2 velocity, Vector2 acceleration, 
            float angle, float angularVelocity, float size, float sizeVelocity, int timeLife, Vector4 color, float alphaVelocity)
        {
            SourceRectangle = new Rectangle(width * textureNum, 0, width, height);
            Position = position;
            Velocity = velocity;
            Acceleration = acceleration;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Size = size;
            SizeVelocity = sizeVelocity;
            TimeLife = timeLife;
            Color = color;
            AlphaVelocity = alphaVelocity;
            
        }


        // Texture Rectangle
        public Rectangle SourceRectangle { get; set; }

        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }

        public float Angle { get; set; }
        public float AngularVelocity { get; set; }

        public float Size { get; set; }
        public float SizeVelocity { get; set; }

        public float TimeLife { get; set; }

        public Vector4 Color { get; set; }
        public float AlphaVelocity { get; set; }


        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(SourceRectangle.Width / 2, SourceRectangle.Height / 2);
            spriteBatch.Draw(texture, Position, SourceRectangle, new Color(Color), Angle, origin, Size, SpriteEffects.None, 0); 

        }


        public virtual void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f;
            TimeLife -= delta;

            Position += Velocity * delta;
            Velocity += Acceleration * delta;

            Angle += AngularVelocity * delta;

            Size += SizeVelocity * delta;

            Color = new Vector4(Color.X, Color.Y, Color.Z, Math.Max(0, Color.W - AlphaVelocity * delta));

        }

        public void Clear()
        {
            //Nothing to do.
        }
    }
}

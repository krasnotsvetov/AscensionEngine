using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameSamples.Engine.Core.Common;
using MonogameSamples.Engine.Core.Components;
using MonogameSamples.Engine.Graphics.SceneSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonogameSamples.Engine.Core.Common.Extension;
using System.Runtime.Serialization;

namespace MonogameSamples.Engine.Core.Components.ParticleSystemComponent
{

    [DataContract]
    public class ParticleSystem2D : EntityDrawableComponent
    {



        public bool EnableGenerate { get; set; }

        private ParticlePool2D particlePool2D;


        private List<Particle2D> particles = new List<Particle2D>();

        private SpriteBatch spriteBatch;



        private int width = 0;
        private int height = 0;

        [DataMember]
        private int textureCount = 1;

        Random rnd = new Random();

        Transform globalTransform;
        public ParticleSystem2D(IGameComponent parentComponent, float frequency, int textureCount) : base(parentComponent)
        {
            EnableGenerate = true;
            particlePool2D = new ParticlePool2D(1000, this);
            if (!(parentComponent is Entity))
            {
                //TODO
                throw new Exception();
            }

            globalTransform = (parentComponent as Entity).GlobalTransform;
            this.textureCount = textureCount;

        }


        public override void Initialize()
        {
            this.width = Material.textures[0].Width / textureCount;
            this.height = Material.textures[0].Height;
            spriteBatch = ((ParentComponent as Entity).Scene.scene2DDrawer as Scene2DDrawer).SpriteBatch;
            
            base.Initialize();
        }


        public virtual void Generate(GameTime gameTime)
        {
            Spawn(0, rnd.RandomVector2(10, 10), rnd.RandomVector2(100, 100), Vector2.Zero, 0f, 0f, (float)rnd.NextDouble() * 0.4f, -0.01f, 1, Color.White.ToVector4(), 0.01f);
        }


        public virtual void Simulate(GameTime gameTime)
        {
            if (EnableGenerate)
            {
                Generate(gameTime);
            }

            for (int i = 0; i < particles.Count; i++)
            {
                if (particles[i].Color.W <= 0f)
                {
                    particles.RemoveAt(i);
                    i--;
                    continue;
                }
                if (particles[i].Position.Y + globalTransform.Position.Y > 400)
                {
                    particles[i].Acceleration = new Vector2(rnd.RandomVector2(20, 0).X, -20);
                }
                particles[i].Update(gameTime);
            }
        }

        public void Spawn(int textureNum, Vector2 position, Vector2 Velocity, Vector2 Acceleration,
            float angle, float angularVelocity, float size, float sizeVelocity, int timeLife, Vector4 Color, float alphaVelocity)
        {
            Particle2D p = particlePool2D.GetParticle();
            p.Initialization(textureNum, width, height, position, Velocity, Acceleration, angle, angularVelocity, size, sizeVelocity, timeLife, Color, alphaVelocity);
            particles.Add(p);
        }


        public override void Draw(GameTime gameTime)
        {
            Simulate(gameTime);
            foreach (Particle2D p in particles)
            {
                p.Draw(spriteBatch);
            }
            base.Draw(gameTime);
        }

        public override string ToString()
        {
            return "ParticleSystem";
        }
    }
}

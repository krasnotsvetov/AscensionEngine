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
using MonogameSamples.Engine.Graphics;
using MonogameSamples.Engine.Core.Common.Collections.Pooling;

namespace MonogameSamples.Engine.Core.Components.ParticleSystemComponent
{

    [DataContract]
    public class ParticleSystem2D : EntityDrawableComponent
    {



        public bool EnableGenerate { get; set; }

        private IPool<Particle2D> particlePool2D;


        private List<Particle2D> particles;

        private SpriteBatch spriteBatch;



        private int width = 0;
        private int height = 0;

        [DataMember]
        private int textureCount = 1;

        Random rnd;

        Transform globalTransform;
        public ParticleSystem2D(string name, float frequency, int textureCount, MaterialReference reference) : base(name, reference)
        {
            this.textureCount = textureCount;
        }


        public override void Initialize()
        {
            base.Initialize();
            rnd = new Random();

            EnableGenerate = true;
            particlePool2D = new Pool<Particle2D>(1000, new ParticleCreator(this));
            particles = new List<Particle2D>();
            globalTransform = (ParentComponent as Entity).GlobalTransform;
            this.width = Material.textures[0].Width / textureCount;
            this.height = Material.textures[0].Height;
            spriteBatch = (ParentComponent as Entity).Scene.sceneRenderer.SpriteBatch;
            
        }


        public virtual void Generate(GameTime gameTime)
        {
            Spawn(0, rnd.RandomVector2(10, 10), rnd.RandomVector2(100, 100), Vector2.Zero, 0f, 0f, (float)rnd.NextDouble() * 0.4f, -0.01f, 1, Color.CornflowerBlue.ToVector4(), 1f);
        }


        public virtual void Simulate(GameTime gameTime)
        {
            if (EnableGenerate)
            {
                Generate(gameTime);
            }

            for (int i = 0; i < particles.Count; i++)
            {
                if (particles[i].Size <= 0)
                {
                    particlePool2D.Add(particles[i]);
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
            Particle2D p = particlePool2D.Get();
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


        internal override void RenderSystemChange()
        {
            base.RenderSystemChange();

            spriteBatch = (ParentComponent as Entity).Scene.sceneRenderer.SpriteBatch;
        }
    }
}

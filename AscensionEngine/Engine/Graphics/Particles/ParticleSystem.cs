using Ascension.Engine.Core.Common;
using Ascension.Engine.Core.Common.Attributes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ascension.Engine.Graphics.Particles
{
    [DataContract]
    [Component("ParticleSystem")]
    public partial class ParticleSystem : EntityDrawableComponent
    {







        Random random;
        private ParticleDeclaration[] particles;
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;

        private float currentTime = 0;
        private bool stopUpdate = false;
        private Color randomValues;
        private Texture2D texture;
        private Material lastMaterial;
        private int firstActiveParticle = 2;
        private int firstFreeParticle = 3;
        private int firstKilledParticle = 1;
        private int firstNewParticle = 3;
        private int drawCounter = 0;

        public ParticleSystem(string name, string materialName) : base(name, materialName)
        {
            OnMaterialChanged += MaterialChanged;
            //TODO, fix design.
            ignoreMaterialChagned = true;
            if (Material != null)
            {
                Material = Material.Clone() as Material;
                SetEffect();
            }
            ignoreMaterialChagned = false;
            random = new Random();
        }

        public override void Initialize()
        {
            base.Initialize();
            SetBuffer();
        }


        private bool ignoreMaterialChagned;


        private void MaterialChanged(object sender, EventArgs e)
        {
            if (ignoreMaterialChagned)
            {
                return;
            }
            ignoreMaterialChagned = true;
            if (lastMaterial != null)
            {
                lastMaterial.Dispose();
            }
            lastMaterial = Material = Material.Clone() as Material;
            SetEffect();
            ignoreMaterialChagned = false;
        }

        EffectParameterCollection parameters;
        private void SetEffect()
        {
            if (Material == null)
            {
                return;
            }
            parameters = Material.Effect.Parameters;


            parameters["FadeFactor"].SetValue(FadeFactor);
            parameters["Duration"].SetValue((float)Duration.TotalSeconds);
            parameters["DurationFactor"].SetValue(DurationFactor);
            parameters["Gravity"].SetValue(GravityFactor);
            parameters["EndVelocity"].SetValue(EndVelocity);
            parameters["MinColor"].SetValue(MinColor.ToVector4());
            parameters["MaxColor"].SetValue(MaxColor.ToVector4());

            parameters["RotationSpeed"].SetValue(new Vector2(MinRotationSpeed, MaxRotationSpeed));

            parameters["StartSize"].SetValue(new Vector2(MinStartSize, MaxStartSize));

            parameters["EndSize"].SetValue(new Vector2(MinEndSize, MaxEndSize));

        }

        private void SetBuffer()
        {
            if (device == null)
            {
                return;
            }

            if (vertexBuffer != null && !vertexBuffer.IsDisposed)
            {
                vertexBuffer.Dispose();
            }

            if (indexBuffer != null && !indexBuffer.IsDisposed)
            {
                indexBuffer.Dispose();
            }

            int[] indices = new int[MaxParticleCount * 6];

            for (int i = 0; i < MaxParticleCount; i++)
            {
                indices[i * 6 + 0] = (i * 4 + 0);
                indices[i * 6 + 1] = (i * 4 + 1);
                indices[i * 6 + 2] = (i * 4 + 2);

                indices[i * 6 + 3] = (i * 4 + 0);
                indices[i * 6 + 4] = (i * 4 + 2);
                indices[i * 6 + 5] = (i * 4 + 3);
            }

            indexBuffer = new IndexBuffer(device, IndexElementSize.ThirtyTwoBits, indices.Length, BufferUsage.WriteOnly);

            indexBuffer.SetData(indices);

            particles = new ParticleDeclaration[MaxParticleCount * 4];

            for (int i = 0; i < MaxParticleCount; i++)
            {
                particles[i * 4 + 0].Corner = new Vector2(-1, -1);
                particles[i * 4 + 1].Corner = new Vector2(1, -1);
                particles[i * 4 + 2].Corner = new Vector2(1, 1);
                particles[i * 4 + 3].Corner = new Vector2(-1, 1);
            }

            vertexBuffer = new VertexBuffer(device, typeof(ParticleDeclaration), particles.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(particles);
        }
        public void Update(GameTime gameTime)
        {
            if (stopUpdate)
            {
                return;
            }
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            killParticles();
            freeKilledParticles();

            if (firstActiveParticle == firstFreeParticle)
            {
                currentTime = 0;
                stopUpdate = true;
            }

            if (firstKilledParticle == firstActiveParticle)
            {
                drawCounter = 0;
            }





         
        }

        
        public override void Draw(Matrix view, Matrix projection, GameTime gameTime)
        {

           
            var prev = device.RasterizerState;

            device.RasterizerState = new RasterizerState() { CullMode = CullMode.None };

            if (firstNewParticle != firstFreeParticle)
            {
                AddNewParticlesToVertexBuffer();
            }
             
            if (firstActiveParticle != firstFreeParticle)
            {
                device.BlendState = BlendState;
                device.DepthStencilState = DepthStencilState.DepthRead;
                parameters["CurrentTime"].SetValue(currentTime);


                device.SetVertexBuffer(vertexBuffer);
                device.Indices = indexBuffer;

                foreach (EffectPass pass in Material.Effect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    if (firstActiveParticle < firstFreeParticle)
                    {

                        device.DrawIndexedPrimitives(PrimitiveType.TriangleList, firstActiveParticle * 4, 0, (firstFreeParticle - firstActiveParticle) * 2);
                    }
                    else
                    {
                        device.DrawIndexedPrimitives(PrimitiveType.TriangleList, firstActiveParticle * 4, 0, (MaxParticleCount - firstActiveParticle) * 2);
                        if (firstFreeParticle > 0)
                        {
                            device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, firstFreeParticle * 2);
                        }
                    }
                }
            }

            drawCounter++;
            device.RasterizerState = prev;
            base.Draw(view, projection, gameTime);
        }

        private void AddNewParticlesToVertexBuffer()
        {
            int stride = ParticleDeclaration.VertexDeclaration.VertexStride;

            if (firstNewParticle < firstFreeParticle)
            {
                vertexBuffer.SetData(firstNewParticle * stride * 4, particles, firstNewParticle * 4, (firstFreeParticle - firstNewParticle) * 4, stride);
            }
            else
            {
                vertexBuffer.SetData(firstNewParticle * stride * 4, particles, firstNewParticle * 4, (MaxParticleCount - firstNewParticle) * 4, stride);

                if (firstFreeParticle > 0)
                {
                    vertexBuffer.SetData(0, particles, 0, firstFreeParticle * 4, stride);
                }
            }

            firstNewParticle = firstFreeParticle;
        }

        private void freeKilledParticles()
        {
            while ((firstKilledParticle + 1) % MaxParticleCount != firstActiveParticle)
            {
                int age = drawCounter - (int)particles[firstKilledParticle * 4].Time.X;

                if (age < 3)
                    break;

                firstKilledParticle = (firstKilledParticle + 1) % MaxParticleCount;
            }
        }

        private void killParticles()
        {
            float particleDuration = (float)Duration.TotalSeconds;

            while ((firstActiveParticle + 1) % MaxParticleCount != firstNewParticle)
            {
                float particleAge = currentTime - particles[firstActiveParticle * 4].Time.X;

                if (particleAge < particleDuration)
                    break;

                particles[firstActiveParticle * 4].Time.X = drawCounter;

                firstActiveParticle = (firstActiveParticle + 1) % MaxParticleCount;
            }
        }



        public virtual void AddParticleAged(Vector3 position, Vector3 velocity, float ageSeconds)
        {
            ///TODO
            /// Move position into localTransform.
            stopUpdate = false;
            if (firstFreeParticle == firstKilledParticle)
            {
                return;
            }
            int nextFreeParticle = (firstFreeParticle + 1) % MaxParticleCount;

           
            // Adjust the input velocity based on how much
            // this particle system wants to be affected by it.
            velocity *= EmitterVelocitySensitivity;

            float velocityFactor = MathHelper.Lerp(MinStartVelocty, MaxStartVelocty, (float)random.NextDouble());

            double horizontalAngle = MinHorizontalAngle + random.NextDouble() * (MaxHorizontalAngle - MinHorizontalAngle);
            double verticalAngle= MinVerticalAngle + random.NextDouble() * (MaxVerticalAngle - MinVerticalAngle);


            Vector3 temp = Vector3.Transform(Vector3.Up * velocityFactor, Matrix.CreateRotationY((float)verticalAngle));
            temp = Vector3.Transform(temp, Matrix.CreateRotationZ((float)horizontalAngle));
            velocity += temp;

            randomValues.R = (byte)random.Next(255);
            randomValues.G = (byte)random.Next(255);
            randomValues.B = (byte)random.Next(255);
            randomValues.A = (byte)random.Next(255);


            for (int i = 0; i < 4; i++)
            {
                particles[firstFreeParticle * 4 + i].Position = position;
                particles[firstFreeParticle * 4 + i].Velocity = velocity;
                particles[firstFreeParticle * 4 + i].RandomValues = randomValues;
                particles[firstFreeParticle * 4 + i].Time.X = currentTime - ageSeconds;
            }

            firstFreeParticle = nextFreeParticle;
        }

        /// <summary>
        /// Adds a new particle to the system.
        /// </summary>
        public void AddParticle(Vector3 position, Vector3 velocity)
        {
            AddParticleAged(position, velocity, 0.0f);
        }


        internal override void RenderSystemChange()
        {
            base.RenderSystemChange();

            SetBuffer();
        }

        public override void Dispose()
        {
            base.Dispose();
            if (vertexBuffer != null && !vertexBuffer.IsDisposed)
            {
                vertexBuffer.Dispose();
            }

            if (indexBuffer != null && !indexBuffer.IsDisposed)
            {
                indexBuffer.Dispose();
            }

            if (Material != null)
            {
                Material.Dispose();
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

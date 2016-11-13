using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ascension.Engine.Graphics.Particles
{
    public partial class ParticleSystem
    {
        #region Settings

        [DataMember]
        public int MaxParticleCount
        {
            get
            {
                return maxParticleCount;
            }
            set
            {
                maxParticleCount = value;
                SetBuffer();
                firstActiveParticle = 2;
                firstFreeParticle = 3;
                firstKilledParticle = 1;
                firstNewParticle = 3;
                drawCounter = 0;
                currentTime = 0;

            }
        }

        [DataMember]
        public TimeSpan Duration { get { return duration; } set { duration = value; SetEffect(); } } 
        private TimeSpan duration = TimeSpan.FromSeconds(100);

        [DataMember]
        public float DurationFactor { get { return durationFactor; } set { durationFactor = value; SetEffect(); } } 
        private float durationFactor = 1;

        [DataMember]
        public float EndVelocity { get { return endVelocity; } set { endVelocity = value; SetEffect(); } }
        private float endVelocity = 1;

        [DataMember]
        public float MinStartVelocty { get; set; } = 2f;

        [DataMember]
        public float MaxStartVelocty { get; set; } = 2f;


        [DataMember]
        public float MinHorizontalAngle { get; set; } =  MathHelper.PiOver2;

        [DataMember]
        public float MaxHorizontalAngle { get; set; } = MathHelper.PiOver2;

        [DataMember]
        public float MinVerticalAngle { get; set; } = 0f;

        [DataMember]
        public float MaxVerticalAngle { get; set; } = 0f;

        [DataMember]
        public float EmitterVelocitySensitivity { get; set; } = 1;

        [DataMember]
        public Vector3 GravityFactor { get { return gravity; } set { gravity = value; SetEffect(); } }
        private Vector3 gravity = Vector3.Zero;

        [DataMember]
        public Color MinColor { get { return minColor; } set { minColor = value; SetEffect(); } }
        private Color minColor = Color.White;

        [DataMember]
        public Color MaxColor { get { return maxColor; } set { maxColor = value; SetEffect(); } }
        private Color maxColor = Color.White;

        [DataMember]
        public float FadeFactor { get { return fadeFactor; } set { fadeFactor = value; SetEffect(); } }
        private float fadeFactor = 6.7f;

        [DataMember]
        public float MinStartSize { get { return minStartSize; } set { minStartSize = value; SetEffect(); }}
        private float minStartSize = 1;

        [DataMember]
        public float MaxStartSize { get { return maxStartSize; } set { maxStartSize = value; SetEffect(); } }
        private float maxStartSize = 1;

        [DataMember]
        public float MinEndSize { get { return minEndSize; } set { minEndSize = value; SetEffect(); } }
        private float minEndSize = 0;

        [DataMember]
        public float MaxEndSize { get { return maxEndSize; } set { maxEndSize = value; SetEffect(); } }
        private float maxEndSize = 0;



        [DataMember]
        public float MinRotationSpeed { get { return minRotationSpeed; }  set{ minRotationSpeed = value; } }
        private float minRotationSpeed = 0;

        [DataMember]
        public float MaxRotationSpeed { get { return maxRotationSpeed; } set { maxRotationSpeed = value; } }
        private float maxRotationSpeed = 0;


        public BlendState BlendState { get { return blendState; } set { blendState = value; SetEffect(); } }
        private BlendState blendState = BlendState.NonPremultiplied;

        [DataMember]
        private string blendStateData
        {
            get
            {
                if (BlendState == null)
                {
                    return "BlendState.NonPremultiplied";
                }
                return BlendState.Name;
            }
            set
            {
                switch (value)
                {
                    case "BlendState.AlphaBlend":
                        BlendState = BlendState.AlphaBlend;
                        break;
                    case "BlendState.Additive":
                        BlendState = BlendState.Additive;
                        break;
                    default:
                        BlendState = BlendState.NonPremultiplied;
                        break;
                }
            }
        } 
        private int maxParticleCount = 100000;


        #endregion
    }
}

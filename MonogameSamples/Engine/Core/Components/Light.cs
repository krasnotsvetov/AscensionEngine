using Microsoft.Xna.Framework;
using MonogameSamples.Engine.Core.Common;
using MonogameSamples.Engine.Graphics;
using System;
using System.Collections.Generic;

namespace MonogameSamples.Engine.Core.Components
{
    public class Light : EntityDrawableComponent
    {

        public Vector3 LightColor { get; set; }
        public float Radius { get { return radius; } set { radius = value ; invRadius = 1 / value; } }
        public float InvRadius { get { return invRadius; } }
        public float Intensity { get; set; }

        private float radius;
        private float invRadius;
        public Light(IGameComponent parentComponent) : base(parentComponent)
        {

            Intensity = 1;
            Radius = 300;
            LightColor = Color.Orange.ToVector3();

            if (!(parentComponent is Entity))
            {
                //TODO
                throw new Exception("Light's parentComponent must be Entity");
            }
            (parentComponent as Entity).Scene.Lights.Add(this);
        }

        public override void Initialize()
        {

            base.Initialize();
        }
    }
}

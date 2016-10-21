using Microsoft.Xna.Framework;
using MonogameSamples.Engine.Core.Common;
using MonogameSamples.Engine.Core.Common.Attributes;
using MonogameSamples.Engine.Graphics;
using MonogameSamples.Engine.Graphics.SceneSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace MonogameSamples.Engine.Core.Components
{
    [Component("Light")]
    [DataContract]
    public class Light : EntityDrawableComponent
    {

        public override IGameComponent ParentComponent
        {
            get
            {
                return base.ParentComponent;
            }

            set
            {
                /*if (base.ParentComponent != null)
                {
                    (ParentComponent as Entity).Scene?.RemoveLight(this);
                }
                if (value == null) return;

                (value as Entity).Scene?.AddLight(this);*/
                base.ParentComponent = value;
            }
        }

        [DataMember]
        public Vector3 LightColor { get; set; }

        [DataMember]
        public float Radius { get { return radius; } set { radius = value ; invRadius = 1 / value; } }

        [DataMember]
        public float Intensity { get; set; }

        [DataMember]
        private float radius;

        public float InvRadius { get { return invRadius; } }
        private float invRadius;
        public Light(string name, MaterialReference reference) : base(name, reference)
        {
            Intensity = 1;
            Radius = 300;
            LightColor = Color.Orange.ToVector3();
        }

        public override void Initialize()
        {
            base.Initialize();
            (ParentComponent as Entity).Scene.AddLight(this);
        }

        public override void SceneChanged(Scene lastScene)
        {
            base.SceneChanged(lastScene);
            if (lastScene != null)
            {
                lastScene.RemoveLight(this);
            }
        }

        public override string ToString()
        {
            return "Light";
        }
    }
}

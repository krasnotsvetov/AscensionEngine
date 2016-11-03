using Microsoft.Xna.Framework;
using Ascension.Engine.Core.Common;
using Ascension.Engine.Core.Common.Attributes;
using Ascension.Engine.Graphics;
using Ascension.Engine.Graphics.SceneSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Ascension.Engine.Core.Components
{
    [Component("Light")]
    [DataContract]
    public class Light : EntityDrawableComponent
    {

        public override Entity ParentEntity
        {
            get
            {
                return base.ParentEntity;
            }

            set
            {
                /*if (base.ParentComponent != null)
                {
                    (ParentComponent as Entity).Scene?.RemoveLight(this);
                }
                if (value == null) return;

                (value as Entity).Scene?.AddLight(this);*/
                base.ParentEntity = value;
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
            ParentEntity.Scene.lights.Add(this);

        }

        internal override void SceneChanged(Scene lastScene)
        {
            base.SceneChanged(lastScene);
            if (lastScene != null)
            {
                lastScene.lights.Remove(this);
            }
        }

        public override string ToString()
        {
            return "Light";
        }
    }
}

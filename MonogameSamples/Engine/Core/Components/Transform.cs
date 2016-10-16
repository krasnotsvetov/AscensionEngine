using Microsoft.Xna.Framework;
using MonogameSamples.Engine.Core.Common;
using MonogameSamples.Engine.Core.Common.Attributes;
using MonogameSamples.Engine.Graphics;
using MonogameSamples.Engine.Graphics.SceneSystem;
using System;
using System.Runtime.Serialization;

namespace MonogameSamples.Engine.Core.Components
{
    [DataContract]
    [Component("Transform")]
    public class Transform : EntityUpdateableComponent
    {
        

        public Matrix World { get { return world; } }
        private Matrix world;


        public event EventHandler<EventArgs> PositionChanged;
        public event EventHandler<EventArgs> RotationChanged;
        public event EventHandler<EventArgs> ScaleChanged;

        [DataMember]
        public Vector3 Position
        {
            get { return position; }
            set
            {
                Vector3 lastPosition = position;
                position = value;
                world = Matrix.Identity * Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z) * Matrix.CreateTranslation(position);
                if (PositionChanged != null)
                {
                    PositionChanged(lastPosition, EventArgs.Empty);
                }
            }
        }

        [DataMember]
        public Vector3 Rotation
        {
            get { return rotation; }
            set
            {
                Vector3 lastRotation = rotation;
                rotation = value;
                world = Matrix.Identity * Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z) * Matrix.CreateTranslation(position);

                if (RotationChanged != null)
                {
                    RotationChanged(lastRotation, EventArgs.Empty);
                }
            }
        }

        [DataMember]
        public Vector3 Scale
        {
            get { return scale; }
            set
            {
                Vector3 lastScale = scale;
                scale = value;

                world = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z) * Matrix.CreateTranslation(position);

                if (ScaleChanged != null)
                {
                    ScaleChanged(lastScale, EventArgs.Empty);
                }
            }
        }

        public Transform ToGlobal()
        {
            Matrix world = Matrix.Identity * Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z) * Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
            Vector3 rot = this.rotation;
            Vector3 sc = this.scale;
            Entity entity = ParentComponent as Entity;
            while (entity.Parent != null)
            {
                Transform t = entity.Parent.Transform;
                world *= t.World;
                rot += t.Rotation;
                sc *= t.scale;
                entity = entity.Parent;
            }
            return new Transform(ParentComponent, world.Translation, rot, sc);
        }


        public Vector3 ToGlobalPosition(Vector3 localPosition)
        {
            Entity entity = ParentComponent as Entity;
            localPosition = Vector3.Transform(localPosition, World);

            while (entity.Parent != null)
            {
                Transform t = entity.Parent.Transform;
                localPosition = Vector3.Transform(localPosition, t.World);
                entity = entity.Parent;
            }
            return localPosition;
        }

        

  
        private Vector3 position;
        private Vector3 rotation;
        private Vector3 scale;

        public Transform(IGameComponent parentComponent) : base(parentComponent)
        {
            scale = new Vector3(1, 1, 1);
            world = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z) * Matrix.CreateTranslation(position);

        }


        public Transform(IGameComponent parentComponent, Vector3 position, Vector3 rotation, Vector3 scale) : base(parentComponent)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            world = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z) * Matrix.CreateTranslation(position);
        }

        public void UpdateTransformRec(Matrix matrix)
        {
            position = Vector3.Transform(position, matrix);
            Entity ent = ParentComponent as Entity;
            foreach (var e in ent.Entities)
            {
                e.Transform.UpdateTransformRec(matrix);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }


        public static Transform operator +(Transform t1, Transform t2)
        {
            Vector3 scale1 = t1.scale;
            Vector3 scale2 = t2.scale;
            return new Transform(t1.ParentComponent, t1.position + t2.position, t1.rotation + t2.rotation, new Vector3(t1.scale.X * t2.scale.X, t1.scale.Y * t2.scale.Y, t1.scale.Z * t2.scale.Z));
        }



       public void SetTransform(Vector3 position, Vector3 rotation, Vector3 scale)
       {
           this.Position = position;
           this.Rotation = rotation;
           this.Scale = scale;
       }

        /*public void UpdateTransform(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            world = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z) * Matrix.CreateTranslation(position);

          
        }*/


        /*  public void UpdateTransformRecursively(Transform transform, Transform parentTransform, Vector3 lastParentPositon, Vector3 lastParentRotation, Vector3 lastParentScale, Vector3 deltaPosition, Vector3 deltaRotation, Vector3 deltaScale)
        {

            Vector3 lP = transform.Position;
            Vector3 lR = transform.Rotation;
            Vector3 lS = transform.Scale;

            Entity entity = transform.ParentComponent as Entity;


            Vector3 localPosition = lP - lastParentPositon;
            Vector3 newPosition = Vector3.Transform(localPosition, Matrix.CreateScale(deltaScale));
            newPosition = Vector3.Transform(newPosition, Matrix.CreateRotationZ(deltaRotation.Z));
            newPosition += parentTransform.position;

            transform.rotation += deltaRotation;
            transform.scale *= scale;
            transform.position = newPosition;
            

            foreach (var e in entity.Entities)
            {
                UpdateTransformRecursively(e.Transform, transform, lP, lR, lS, deltaPosition, deltaRotation, deltaScale);
            }
        }*/


        public override string ToString()
        {
            return "Transform";
        }
    }
}

using Microsoft.Xna.Framework;
using Ascension.Engine.Core.Common;
using Ascension.Engine.Core.Common.Attributes;
using Ascension.Engine.Graphics;
using Ascension.Engine.Graphics.SceneSystem;
using System;
using System.Runtime.Serialization;

namespace Ascension.Engine.Core.Components
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
                world = Matrix.Identity * Matrix.CreateScale(scale) * Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z) * Matrix.CreateTranslation(position);
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
                world = Matrix.Identity * Matrix.CreateScale(scale) * Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z) * Matrix.CreateTranslation(position);

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

                world = Matrix.CreateScale(scale) * Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z) * Matrix.CreateTranslation(position);

                if (ScaleChanged != null)
                {
                    ScaleChanged(lastScale, EventArgs.Empty);
                }
            }
        }

        internal Transform ToGlobal()
        {
            Matrix world = Matrix.Identity * Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z) * Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
            Vector3 rot = this.rotation;
            Vector3 sc = this.scale;
            Entity entity = Parent as Entity;
            while (entity.Parent != null)
            {
                Transform t = entity.Parent.Transform;
                world *= t.World;
                rot += t.Rotation;
                sc *= t.scale;
                entity = entity.Parent;
            }
            return new Transform("temporaryTransform", world.Translation, rot, sc);
        }


        public Vector3 ToGlobalPosition(Vector3 localPosition)
        {
            Entity entity = Parent as Entity;
            localPosition = Vector3.Transform(localPosition, World);

            while (entity.Parent != null)
            {
                Transform t = entity.Parent.Transform;
                localPosition = Vector3.Transform(localPosition, t.World);
                entity = entity.Parent;
            }
            return localPosition;
        }

        
        //TODO, implement lazy calculation.
        public Vector3 Up { get { return Vector3.Transform(Vector3.Up, Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z)); } }
        public Vector3 Forward { get { return Vector3.Transform(Vector3.Forward, Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z)); } }
        public Vector3 Left { get { return Vector3.Transform(Vector3.Left, Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z)); } }
        public Vector3 Right { get { return Vector3.Transform(Vector3.Right, Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z)); } }


        private Vector3 position;
        private Vector3 rotation;
        private Vector3 scale;

        public Transform(string name) : base(name)
        {
            scale = new Vector3(1, 1, 1);
            world = Matrix.CreateScale(scale) * Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z) * Matrix.CreateTranslation(position);

        }


        public Transform(string name, Vector3 position, Vector3 rotation, Vector3 scale) : base(name)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
            world = Matrix.CreateScale(scale) * Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z) * Matrix.CreateTranslation(position);
        }

        public void UpdateTransformRec(Matrix matrix)
        {
            position = Vector3.Transform(position, matrix);
            Entity ent = Parent as Entity;
            foreach (var e in ent.Entities)
            {
                e.Transform.UpdateTransformRec(matrix);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }



       public void SetTransform(Vector3 position, Vector3 rotation, Vector3 scale)
       {
           this.Position = position;
           this.Rotation = rotation;
           this.Scale = scale;
       }

        public override string ToString()
        {
            return "Transform";
        }
    }
}

using Ascension.Engine.Core.Common;
using Ascension.Engine.Core.Common.Attributes;
using Ascension.Engine.Graphics.SceneSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ascension.Engine.Graphics.CameraSystem
{
    [DataContract]
    [Component("Camera")]
    public class Camera : EntityUpdateableComponent, ICamera
    {



        public Matrix World { get { return parent == null ? Matrix.Identity : parent.GlobalTransform.World; } }
        public Matrix View { get { return view; } }
        public Matrix Projection { get { return projection; } }
        public RenderTarget2D RenderTarget { get; set; }


        public override Entity Parent
        {
            get
            {
                return base.Parent;
            }

            set
            {
                base.Parent = value;
            }
        }

        private Entity parent;


        public float FieldOfView
        {
            get { return fieldOfView; }
            set
            {
                fieldOfView = value;
                if (ProjectionType == CameraProjectionType.Perspective)
                {
                    projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance);
                }
            }
        }

        public float AspectRatio
        {
            get { return aspectRatio; }
            set
            {
                aspectRatio = value;
                if (ProjectionType == CameraProjectionType.Perspective)
                {
                    projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance);
                }
            }
        }

        public float NearPlaneDistance
        {
            get { return nearPlaneDistance; }
            set
            {
                nearPlaneDistance = value;
                if (ProjectionType == CameraProjectionType.Perspective)
                {
                    projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance);
                }
                else
                {
                    projection = Matrix.CreateOrthographic(width, height, nearPlaneDistance, farPlaneDistance);
                }
            }
        }

        public float FarPlaneDistance
        {
            get { return farPlaneDistance; }
            set
            {
                farPlaneDistance = value;
                if (ProjectionType == CameraProjectionType.Perspective)
                {
                    projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance);
                } else
                {
                    projection = Matrix.CreateOrthographic(width, height, nearPlaneDistance, farPlaneDistance);
                }
            }
        }

        public float Width
        {
            get { return width; }
            set
            {
                width = value;
                if (ProjectionType == CameraProjectionType.Orthographic)
                {
                    projection = Matrix.CreateOrthographic(width, height, nearPlaneDistance, farPlaneDistance);
                }
            }
        }

        public float Height
        {
            get { return height; }
            set
            {
                height = value;
                if (ProjectionType == CameraProjectionType.Orthographic)
                {
                    projection = Matrix.CreateOrthographic(width, height, nearPlaneDistance, farPlaneDistance);
                }
            }
        }

        [DataMember]
        public CameraProjectionType ProjectionType { get; set; }

        [DataMember]
        private float fieldOfView;
        [DataMember]
        private float aspectRatio;
        [DataMember]
        private float nearPlaneDistance;
        [DataMember]
        private float farPlaneDistance;
        [DataMember]
        private float width;
        [DataMember]
        private float height;

        [DataMember]
        private Matrix view;
        [DataMember]
        private Matrix projection;



        public Camera(string name) : base(name)
        {
            view = Matrix.CreateLookAt(new Vector3(0, 0, -5), new Vector3(0, 0, 0), Vector3.Up);   
        }

        public override void Initialize()
        {
            Parent.Scene.cameras.Add(this);
            base.Initialize();
        }

        public void SetupPerspectiveCamera(Vector3 forward, Vector3 up, float fieldOfView, float aspectRatio, float nearPlaneDistance, float farPlaneDistance)
        {
            ProjectionType = CameraProjectionType.Perspective;
            projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearPlaneDistance, farPlaneDistance);

        }


        public void SetupOrthographicCamera(Vector3 forward, Vector3 up, float width, float height, float nearPlaneDistance, float farPlaneDistance)
        {
            ProjectionType = CameraProjectionType.Orthographic;
            projection = Matrix.CreateOrthographic(width, height, nearPlaneDistance, farPlaneDistance);
        }


        internal override void SceneChanged(Scene lastScene)
        {
            if (lastScene != null)
            {
                lastScene.cameras.Remove(this);
            }
            base.SceneChanged(lastScene);
        }

        protected void TransformChanged(object sender, EventArgs e)
        {

        }

        public override string ToString()
        {
            return Name;
        }
    }
}

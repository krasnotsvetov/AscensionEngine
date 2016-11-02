using Ascension.Engine.Core.Common;
using Ascension.Engine.Graphics.SceneSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscensionEngine.Engine.Graphics.CameraSystem
{
    public class Camera : EntityUpdateableComponent
    {



        public Matrix World { get { return world; } }
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

        public CameraProjectionType ProjectionType { get; set; }

        private float fieldOfView;
        private float aspectRatio;
        private float nearPlaneDistance;
        private float farPlaneDistance;
        private float width;
        private float height;

        private Matrix world = Matrix.Identity;
        private Matrix view;
        private Matrix projection;



        public Camera(string name) : base(name)
        {
            
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
            base.SceneChanged(lastScene);
        }

        protected void TransformChanged(object sender, EventArgs e)
        {

        }
    }
}

using Ascension.Engine.Core.Common;
using Ascension.Engine.Graphics.SceneSystem;
using Microsoft.Xna.Framework;
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
            
        public float FieldOfView { set {; } }


        private Matrix world = Matrix.Identity;
        private Matrix view;
        private Matrix projection;



        public Camera(string name, Vector3 target, float fieldOfView, float aspectRatio, float nearDistance, float farDistance) : base(name)
        {
            projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, nearDistance, farDistance);
        }

        /*public Camera(string name, Vector3 target, float width, float height, float nearPlane, float farPlane) : base(name)
        {
             projection = Matrix.CreateOrthographic(width, height, nearPlane, farPlane);
        }*/


        internal override void SceneChanged(Scene lastScene)
        {
            base.SceneChanged(lastScene);
        }
    }
}

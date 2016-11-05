using Ascension.Engine.Graphics.CameraSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AscensionEditor;

namespace AscensionEngineEditor.Editor
{
    public class EditorCamera : ICamera
    {


        public float TranslationSpeed = 50f;
        public float RotationSpeed = 0.1f;
        public Vector3 Position = new Vector3(0, 1, 0);
        public float Yaw = 0;
        public float Pitch = 0;


        private EditorMouseState lastMouseState;

        public Matrix View { get; set; }

        public Matrix Projection { get; set; }

        public Vector3 Forward { get; set; }

        private GraphicsDevice device;


        public CameraProjectionType ProjectionType
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                if (CameraProjectionType.Orthographic == value)
                {
                    Projection = Matrix.CreateOrthographic(device.Viewport.Width, device.Viewport.Height, 0.1f, 1000f);
                }
                else
                {
                    Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(75f), device.Viewport.AspectRatio, 1F, 1000f);
                }
            }
        }
        private CameraProjectionType type;

        public EditorCamera(CameraProjectionType type, GraphicsDevice device)
        {
            this.device = device;
            ProjectionType = type;

        }



        public void Update(EditorMouseState state, GameTime gameTime)
        {



            switch (ProjectionType)
            {
                case CameraProjectionType.Perspective:
                    updatePerspectiveCamera(state, gameTime);
                    break;
                case CameraProjectionType.Orthographic:
                    updateOrtographicCamera(state, gameTime);
                    break;
            }
        }

        float ortZoom = 1f;
        float zoomSpeed = 0.001f;

        private void updatePerspectiveCamera(EditorMouseState state, GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
            if (lastMouseState == null || !state.onControl || state.RightButton == ButtonState.Released)
            {
                lastMouseState = new EditorMouseState(state);
                return;
            }
            float deltaX = state.Position.X - lastMouseState.Position.X;
            float deltaY = state.Position.Y - lastMouseState.Position.Y;
            Yaw -= RotationSpeed * deltaX * elapsedTime;
            Pitch += RotationSpeed * deltaY * elapsedTime;


            Vector3 translation = new Vector3(0, 0, 0);
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.W))
            {
                translation += new Vector3(0, 0, TranslationSpeed) * elapsedTime;
            }

            if (keyState.IsKeyDown(Keys.S))
            {
                translation += new Vector3(0, 0, -TranslationSpeed) * elapsedTime;
            }

            if (keyState.IsKeyDown(Keys.D))
            {
                translation += new Vector3(-TranslationSpeed, 0, 0) * elapsedTime;
            }

            if (keyState.IsKeyDown(Keys.A))
            {
                translation += new Vector3(TranslationSpeed, 0, 0) * elapsedTime;
            }

            Matrix rotationMatrix = Matrix.CreateFromYawPitchRoll(Yaw, Pitch, 0);


            Vector3 Up = Vector3.Transform(Vector3.Up, rotationMatrix);

            Position += Vector3.Transform(translation, rotationMatrix);
            Forward = Vector3.Transform(new Vector3(0, 0, 1), rotationMatrix);
            Vector3 cameraFinalTarget = Position + Forward;

            View = Matrix.CreateLookAt(Position, cameraFinalTarget, Up);
            lastMouseState = new EditorMouseState(state);
        }

        private void updateOrtographicCamera(EditorMouseState state, GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
            float zoomDelta = (state.ScrollValue - lastMouseState.ScrollValue) * zoomSpeed;
            ortZoom += zoomDelta * ortZoom;
            Yaw = 0;
            Pitch = 0;
            Vector3 ortPosition = new Vector3(Position.X, Position.Y, -50f);
            View = Matrix.CreateLookAt(ortPosition, ortPosition + new Vector3(0, 0, 1), Vector3.Up);
            Projection = Matrix.CreateOrthographic(device.Viewport.Width * ortZoom, device.Viewport.Height * ortZoom, 0.1f, 1000f);
            lastMouseState = new EditorMouseState(state);

            Vector3 translation = Vector3.Zero;
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.W))
            {
                translation += new Vector3(0, TranslationSpeed, 0) * elapsedTime;
            }

            if (keyState.IsKeyDown(Keys.S))
            {
                translation += new Vector3(0, -TranslationSpeed, 0) * elapsedTime;
            }

            if (keyState.IsKeyDown(Keys.D))
            {
                translation += new Vector3(-TranslationSpeed, 0, 0) * elapsedTime;
            }

            if (keyState.IsKeyDown(Keys.A))
            {
                translation += new Vector3(TranslationSpeed, 0, 0) * elapsedTime;
            }
            Position += translation;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Ascension;
using Ascension.Engine.Graphics.SceneSystem;

namespace AscensionEditor
{
    using Ascension.Engine.Graphics.CameraSystem;
    using AscensionEngineEditor.Editor;
    using MouseButtonState = Microsoft.Xna.Framework.Input.ButtonState;
    public class GameEditor : GameEx
    {
        public Entity SelectedEntity = null;

        private Control drawingSurface;
        private EditorForm form;

        private EditorMouseState mouseState;

        internal EditorCamera camera;
        public GameEditor(EditorForm form, Control drawingSurface)
        {
            Form f = Control.FromHandle(Window.Handle) as Form;

            this.form = form;
            this.drawingSurface = drawingSurface;
            mouseState = new EditorMouseState(drawingSurface);
            form.FormClosing += (s, e) => f.Close();
            graphics.PreparingDeviceSettings +=
                new EventHandler<PreparingDeviceSettingsEventArgs>((s, e) => e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = drawingSurface.Handle);
            f.VisibleChanged += (s, e) => f.Visible = false;

        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = drawingSurface.Width;
            graphics.PreferredBackBufferHeight = drawingSurface.Height;

            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            camera = new EditorCamera(CameraProjectionType.Perspective, GraphicsDevice);
            base.Initialize();
        }


        protected override void LoadContent()
        {

            base.LoadContent();
            form.InitializateGUI(this);
            form.SetContent();

        }


        protected override void UnloadContent()
        {
            base.UnloadContent();
        }



        protected override void Update(GameTime gameTime)
        {
            renderSystem.ActiveCamera = camera;
            form.Text = mouseState.Position.ToString();
            camera.Update(mouseState, gameTime);
            if (mouseState.LeftButton == MouseButtonState.Pressed && mouseState.onControl)
            {

                if (SelectedEntity != null)
                {
                    float? length;
                    Vector3 camPosition = GraphicsDevice.Viewport.Unproject(new Vector3(mouseState.Position, 0), camera.Projection, camera.View, Matrix.Identity);
                    switch (camera.ProjectionType)
                    {
                        case CameraProjectionType.Perspective:
                            if ((length = new Ray(camera.Position, camPosition - camera.Position).Intersects(
                                new Plane(0, 1, 0, 0))) > 0)
                            {
                                SelectedEntity.GlobalTransform.Position = camPosition + (camPosition - camera.Position) * (float)length;

                            }
                            break;
                        case CameraProjectionType.Orthographic:
                            SelectedEntity.GlobalTransform.Position = new Vector3(camPosition.X, camPosition.Y, 0);
                            break;
                    }
                    //TODO


                }
            }
            base.Update(gameTime);
            mouseState.Update();
            //form.Text = camera.Forward.ToString() + camera.Position;

        }



        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

    }
}

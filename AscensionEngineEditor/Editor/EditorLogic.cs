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
    public class EditorLogic
    {
        internal Entity SelectedEntity = null;

        private Control drawingSurface;
        private EditorForm form;
        private GameEx game;
        private EditorMouseState mouseState;

        internal EditorCamera camera;
        public EditorLogic(EditorForm form, Control drawingSurface)
        {

            this.form = form;
            this.drawingSurface = drawingSurface;
           

        }

        internal void Construct(GameEx game)
        {
            form.GameApp = game;
            this.game = game;
            Form f = Control.FromHandle(game.Window.Handle) as Form;
            mouseState = new EditorMouseState(drawingSurface);
            form.FormClosing += (s, e) => f.Close();
            game.graphics.PreparingDeviceSettings +=
                new EventHandler<PreparingDeviceSettingsEventArgs>((s, e) => e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle = drawingSurface.Handle);
            f.VisibleChanged += (s, e) => f.Visible = false;
        }

        internal void Start()
        {
            game.graphics.PreferredBackBufferWidth = drawingSurface.Width;
            game.graphics.PreferredBackBufferHeight = drawingSurface.Height;

            game.graphics.IsFullScreen = false;
            game.graphics.ApplyChanges();

            camera = new EditorCamera(CameraProjectionType.Perspective, game.GraphicsDevice);
    
        }


        internal void LoadContent()
        {
             
            form.InitializateGUI();
            form.SetContent();

        }




        internal void Update(GameTime gameTime)
        {
            game.renderSystem.ActiveCamera = camera;
            form.Text = mouseState.Position.ToString();
            camera.Update(mouseState, gameTime);
            if (mouseState.LeftButton == MouseButtonState.Pressed && mouseState.onControl)
            {

                if (SelectedEntity != null)
                {
                    float? length;
                    Vector3 camPosition = game.GraphicsDevice.Viewport.Unproject(new Vector3(mouseState.Position, 0), camera.Projection, camera.View, Matrix.Identity);
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
                            SelectedEntity.GlobalTransform.Position = new Vector3(camPosition.X, camPosition.Y, SelectedEntity.GlobalTransform.Position.Z);
                            break;
                    }
                    //TODO


                }
            } 
            mouseState.Update();
            //form.Text = camera.Forward.ToString() + camera.Position;

        }



        internal void Draw(GameTime gameTime)
        { 
        }

    }
}

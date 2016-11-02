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
    using MouseButtonState = Microsoft.Xna.Framework.Input.ButtonState;
    public class GameEditor : GameEx
    {
        public Entity SelectedEntity = null;

        private Control drawingSurface;
        private EditorForm form;

        private EditorMouseState mouseState;

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

            if (mouseState.LeftButton == MouseButtonState.Pressed)
            {
                if (SelectedEntity != null)
                {
                    SelectedEntity.GlobalTransform.Position = new Vector3(mouseState.Position, 0);
                }
            }
            base.Update(gameTime);
            mouseState.Update();
        }



        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

    }
}


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameSamples.Engine.Core;
using MonogameSamples.Engine.Core.Common;
using MonogameSamples.Engine.Core.Components;
using MonogameSamples.Engine.Graphics;
using MonogameSamples.Engine.Graphics.Scene;
using System.Collections.Generic;

namespace MonogameSamples
{ 

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 


    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;



        private Dictionary<string, DrawableComponent> drawableComponents = new Dictionary<string, DrawableComponent>();
        private Dictionary<string, UpdateableComponent> updateableComponents = new Dictionary<string, UpdateableComponent>();
        RenderSystem renderSystem;
        UpdateSystem updateSystem;
        Scene2D scene2D;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            GameInfo.GraphicsDevice = GraphicsDevice;

            renderSystem = new RenderSystem();
            renderSystem.Initialize();
            updateSystem = new UpdateSystem();
            updateSystem.Initialize();
            scene2D = new Scene2D();
            
            drawableComponents.Add("RenderSystem", renderSystem);
            updateableComponents.Add("UpdateSystem", updateSystem);
            renderSystem.AddComponent(new KeyValuePair<string, DrawableComponent>("GlobalSceneDrawer", scene2D.scene2DDrawer));
            updateSystem.AddComponent(new KeyValuePair<string, UpdateableComponent>("GlobalSceneUpdater", scene2D.scene2DUpdater));
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Color[] color = new Color[100 * 100];
            for (int i = 0; i < color.Length; i++)
            {
                color[i] = Color.White;
            }

            Effect effect = Content.Load<Effect>("mainShader");

            Texture2D texture = new Texture2D(GraphicsDevice, 100, 100);
            Texture2D texture2 = new Texture2D(GraphicsDevice, 100, 100);

            texture.SetData<Color>(color);

            for (int i = 0; i < color.Length; i++)
            {
                color[i] = Color.Red;
            }
            texture2.SetData<Color>(color);

            Entity entity = new Entity(scene2D);
            Sprite sprite = new Sprite(entity);

            sprite.Material = new Material(texture, effect, m => m.effect.Parameters["test"].SetValue(m.textures[1]));
            sprite.Material.textures.Add(texture2);



            entity.AddDrawableComponent("Sprite0", sprite);

            scene2D.Entities.Add(entity);

        }

        protected override void UnloadContent()
        {
            foreach (var c in drawableComponents.Values)
            {
                c.Dispose();
            }

            foreach (var c in updateableComponents.Values)
            {
                c.Dispose();
            }
        }

        protected override void Update(GameTime gameTime)
        {

            foreach (var uc in updateableComponents.Values)
            {
                uc.Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (var dc in drawableComponents.Values)
            {
                dc.Draw(gameTime);
            }
            base.Draw(gameTime);
        }
    }
   
}


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogameSamples.Engine.Core;
using MonogameSamples.Engine.Core.Common;
using MonogameSamples.Engine.Core.Components;
using MonogameSamples.Engine.Graphics;
using MonogameSamples.Engine.Graphics.SceneSystem;
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
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

            GameInfo.GraphicsDevice = GraphicsDevice;
            GameInfo.Content = Content;

            renderSystem = new RenderSystem(GraphicsDevice);
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

        Entity entity;
        Entity entity2;
        Entity entity3;
        Entity entity4;

        protected override void LoadContent()
        {

            Matrix test = Matrix.CreateTranslation(new Vector3(2, 0, 0)) * Matrix.CreateRotationZ(MathHelper.PiOver4);
            Matrix test2 = Matrix.CreateTranslation(new Vector3(2, 0, 0)) * Matrix.CreateScale(2, 0, 0) * test;

            Vector3 pos = test2.Translation;

            Vector3 ttt = Vector3.Transform(Vector3.Zero, Matrix.CreateTranslation(1, 0, 0));
            spriteBatch = new SpriteBatch(GraphicsDevice);


            Effect effect = Content.Load<Effect>("mainShader");
            Effect effect2 = Content.Load<Effect>("mainShader2");

            Texture2D baseTexture = Content.Load<Texture2D>("house1Live");
            Texture2D baseNormal = Content.Load<Texture2D>("house1Normal");

            entity = new Entity(scene2D);
            entity2 = new Entity(scene2D);
            entity3 = new Entity(scene2D);
            entity4 = new Entity(scene2D);

            Light light = new Light(entity2);
           Light light2 = new Light(entity3);
            Light light3 = new Light(entity4);

           light2.LightColor = Color.CornflowerBlue.ToVector3();
            entity2.AddDrawableComponent("Light0", light);
            entity3.AddDrawableComponent("Light0", light2);
            entity4.AddDrawableComponent("Light0", light3);

            entity.Transform.Position = new Vector3(250, 0, 0);

            entity2.Transform.UpdateTransform(new Vector3(250, 0, 0), Vector3.Zero, new Vector3(1, 1, 1));
            entity3.Transform.UpdateTransform(new Vector3(250, 0, 0), Vector3.Zero, new Vector3(1, 1, 1));
            entity4.Transform.UpdateTransform(new Vector3(0, 250, 0), Vector3.Zero, new Vector3(1, 1, 1));

            Sprite sprite = new Sprite(entity);

            entity2.AddEntity(entity3);
            entity3.AddEntity(entity4);


            // If you need to skip texture in Material, for example, Normal Map, use this construction:
            // sprite.Material = new Material(baseTexture, effect, m => m.effect.Parameters["NormalMap"].SetValue((Texture2D)null));
            // You MUST set all values in your shader, but some values can be NullPointer (defualt value)

            sprite.Material = new Material(baseTexture, effect, m => m.effect.Parameters["NormalMap"].SetValue(m.textures[1]));


            //sprite.material = new material(basetexture, effect2,
            //    m =>
            //    {
            //        m.effect.parameters["screenwidth"].setvalue((float)graphicsdevice.viewport.width);
            //        m.effect.parameters["screenheight"].setvalue((float)graphicsdevice.viewport.height);
            //        m.effect.parameters["normalmap"].setvalue(m.textures[1]);

            //    });
            sprite.Material.textures.Add(baseNormal);



            entity.AddDrawableComponent("Sprite0", sprite);
            scene2D.Entities.Add(entity);
            scene2D.Entities.Add(entity2);

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
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                entity2.Transform.Position = new Vector3(Mouse.GetState().Position.ToVector2(), 0);
            }


            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                entity2.Transform.Scale += new Vector3(0.02f, 0, 0f);
            }


            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                entity2.Transform.Rotation += new Vector3(0f, 0f, 0.02f);
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

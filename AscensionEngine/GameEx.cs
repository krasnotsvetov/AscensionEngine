///
/// Disclaimer: 
/// 
/// This class contains a lot of rubbish and i use it only for features testing.
/// 
/// 







using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ascension.Engine.Core.Common;
using Ascension.Engine.Graphics;
using Ascension.Engine.Graphics.SceneSystem;
using System.Collections.Generic;
using Ascension.Engine.Core.Systems;
using Ascension.Engine.Graphics.CameraSystem;
using Ascension.Engine.Core.Systems.Content;
using Ascension.Engine.Core.Components._3DComponents;
using Ascension.Engine.Core.Components;

namespace Ascension
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 


    public class GameEx : Game
    {
        protected GraphicsDeviceManager graphics;



        public Dictionary<string, IDrawableSystem> drawableSystems = new Dictionary<string, IDrawableSystem>();
        public Dictionary<string, IUpdateableSystem> updateableSystems = new Dictionary<string, IUpdateableSystem>();
        public RenderSystem RenderSystem { get { return renderSystem; } }
        public UpdateSystem UpdateSystem { get { return updateSystem; } }


        public RenderSystem renderSystem;
        public UpdateSystem updateSystem;


        Scene scene;




        public GameEx()
        {
            graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }



        protected override void Initialize()
        {
            renderSystem = new RenderSystem(GraphicsDevice);
            renderSystem.Initialize();
            updateSystem = new UpdateSystem();
            updateSystem.Initialize();
            drawableSystems.Add("RenderSystem", renderSystem);
            updateableSystems.Add("UpdateSystem", updateSystem);

            base.Initialize();
        }

        protected override void LoadContent()
        {

            Cube cube = new Cube("Cube", "HouseMaterial");

            Effect effect = Content.Load<Effect>("Engine\\mainShader");
            Effect effect2 = Content.Load<Effect>("Engine\\mainShader2");
            Effect gridEffect = Content.Load<Effect>("Engine\\shaders\\GridEffect");
            Effect particleEffect = Content.Load<Effect>("Engine\\shaders\\particleShader");

            Texture2D baseTexture = Content.Load<Texture2D>("Engine\\house1Live");
            Texture2D baseNormal = Content.Load<Texture2D>("Engine\\house1Normal");
            Texture2D particleTexture = Content.Load<Texture2D>("Engine\\ParticleSystem\\playerParticle");

            Model model = Content.Load<Model>("Engine\\Primitives\\Cube");

            ContentContainer cc = ContentContainer.Instance();
            cc.AddUserContent<ModelInstance>(new ModelInstance("Cube",  model));
            cc.AddContent<Effect>(gridEffect);
            cc.AddContent<Texture2D>(baseTexture);
            cc.AddContent<Texture2D>(baseNormal);
            cc.AddContent<Effect>(effect);
            cc.AddContent<Effect>(Content.Load<Effect>("Engine\\Shaders\\LightEffect"));
            cc.AddContent<Effect>(Content.Load<Effect>("Engine\\Shaders\\ClearEffect"));
            cc.AddContent<Effect>(Content.Load<Effect>("Engine\\Shaders\\SpriteEffect"));



            foreach (var dc in drawableSystems.Values)
            {
                dc.LoadContent();
            }

            scene = new Scene("Scene0", renderSystem);
            scene.LoadContent();
            updateSystem.AddComponent(new KeyValuePair<string, UpdateableComponent>("GlobalSceneUpdater", scene.sceneUpdater));

            ContentContainer.Instance().LoadMaterials("Content\\Engine\\Materials.data");


            Sprite sprite = new Sprite("Sprite", "HouseMaterialSprite");

            string test = cc.MaterialInformation["HouseMaterial"].Save();



            Entity cameraEntity = new Entity("Camera", scene);
            Camera camera = new Camera("Camera");
            camera.SetupPerspectiveCamera(Vector3.Forward, Vector3.Up, MathHelper.ToRadians(75f), renderSystem.Device.Viewport.AspectRatio, 0.1f, 1000f);
            cameraEntity.AddUpdateableComponent(camera);

            Entity house = new Entity("House", scene);
            house.AddDrawableComponent(cube);
            Entity Sprite = new Entity("Sprite", scene);
            Sprite.AddDrawableComponent(sprite);

            renderSystem.ActiveScene = scene;

        }



        protected override void UnloadContent()
        {
            foreach (var c in drawableSystems.Values)

            {
                c.Dispose();
            }

            foreach (var c in updateableSystems.Values)
            {
                c.Dispose();
            }
        }



        protected override void Update(GameTime gameTime)
        {


            foreach (var uc in updateableSystems.Values)
            {
                uc.Update(gameTime);
            }
            
            /*
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && Keyboard.GetState().IsKeyDown(Keys.LeftControl))
            {
                Matrix world = entity2.Parent == null ? Matrix.Identity : entity2.Parent.GlobalTransform.World;
                entity2.Transform.Position = Vector3.Transform(new Vector3(Mouse.GetState().Position.ToVector2(), 0), Matrix.Invert(world));
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
            {
                
                entity2.GlobalTransform.Position = new Vector3(Mouse.GetState().Position.ToVector2(), 0);
            }
            */

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (var dc in drawableSystems.Values)
            {
                dc.Draw(gameTime);
            }


            base.Draw(gameTime);
        }
    }

}

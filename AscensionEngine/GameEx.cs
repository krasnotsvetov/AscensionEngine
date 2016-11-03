///
/// Disclaimer: 
/// 
/// This class contains a lot of rubbish and i use it only for features testing.
/// 
/// 







using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ascension.Engine.Content;
using Ascension.Engine.Core;
using Ascension.Engine.Core.Common;
using Ascension.Engine.Core.Common.Collections;
using Ascension.Engine.Core.Components;
using Ascension.Engine.Core.Components.ParticleSystemComponent;
using Ascension.Engine.Graphics;
using Ascension.Engine.Graphics.SceneSystem;
using Ascension.Engine.Graphics.Shaders;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using AscensionEngine.Engine.Core;
using AscensionEngine.Engine.Core.Systems;

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

        Entity entity;
        Entity entity2;
        Entity entity3;
        Entity entity4;
        Entity particleEntity;
        SpriteFont spriteFont;


        protected override void LoadContent()
        {

            foreach (var dc in drawableSystems.Values)
            {
                dc.LoadContent(Content);
            }

            scene = new Scene("Scene0", renderSystem);
            scene.LoadContent(Content);
            updateSystem.AddComponent(new KeyValuePair<string, UpdateableComponent>("GlobalSceneUpdater", scene.sceneUpdater));

            spriteFont = Content.Load<SpriteFont>("Engine\\font");
            Effect effect = Content.Load<Effect>("Engine\\mainShader");
            Effect effect2 = Content.Load<Effect>("Engine\\mainShader2");
            Effect particleEffect = Content.Load<Effect>("Engine\\shaders\\particleShader");

            Texture2D baseTexture = Content.Load<Texture2D>("Engine\\house1Live");
            Texture2D baseNormal = Content.Load<Texture2D>("Engine\\house1Normal");
            Texture2D particleTexture = Content.Load<Texture2D>("Engine\\ParticleSystem\\playerParticle");


            ContentSystem contentSystem = ContentSystem.GetInstance();
            contentSystem.Textures.Add("baseTexture", baseTexture);
            contentSystem.Textures.Add("baseNormal", baseNormal);
            contentSystem.Textures.Add("particleTexture", particleTexture);
            contentSystem.Effect.Add("effect", effect);
            contentSystem.Effect.Add("ParticleShader", effect2);

            entity = new Entity("House");
            entity2 = new Entity("LightA", scene);
            entity3 = new Entity("LightB");
            entity4 = new Entity("LightC");

    
           

            
            Light light = new Light("Light0", null);
            Light light2 = new Light("Light1", null);
            Light light3 = new Light("Light2", null);

          

            light2.LightColor = Color.CornflowerBlue.ToVector3();
            light3.LightColor = Color.Green.ToVector3();

            //We can't add drawable component to Entity which doesn't have  Scene.
            entity2.AddDrawableComponent(light);
            entity3.AddDrawableComponent(light2);
            entity4.AddDrawableComponent(light3);

            entity.Transform.Position = new Vector3(250, 0, 0);

            entity2.Transform.SetTransform(new Vector3(250, 0, 0), Vector3.Zero, new Vector3(1, 1, 1));
            entity3.Transform.SetTransform(new Vector3(250, 0, 0), Vector3.Zero, new Vector3(1, 1, 1));
            entity4.Transform.SetTransform(new Vector3(250, 0, 0), Vector3.Zero, new Vector3(1, 1, 1));


           



            scene.AddShader(effect, new NormalShaderPipelineStateSetter(), "effect");
            scene.AddShader(effect2, new ParticleShaderPipelineStateSetter(), "ParticleShader");




            scene.AddMaterial(new Material("HouseMaterial",
                new[] { Texture2DReference.FromIdentifier("baseTexture"), Texture2DReference.FromIdentifier("baseNormal") },
                ShaderReference.FromIdentifier("effect")));

            scene.AddMaterial(new Material("PSMaterial",
                new [] { Texture2DReference.FromIdentifier("particleTexture"), null },
                ShaderReference.FromIdentifier("ParticleShader")));



            Sprite sprite = new Sprite("Sprite0", scene.Materials["HouseMaterial"].Reference);


           // entity3.AddEntity(entity4);

           // scene2D.AddEntity(entity3);

            //entity2.AddEntity(entity3);

            // If you need to skip texture in Material, for example, Normal Map, use this construction:
            // sprite.Material = new Material(baseTexture, effect, m => m.effect.Parameters["NormalMap"].SetValue((Texture2D)null));
            // You MUST set all values in your shader, but some values can be NullPointer (defualt value)

           
           
            particleEntity = new Entity("Particles");
            ParticleSystem2D ps = new ParticleSystem2D("ParticleSystem0", 10f, 1, scene.Materials["PSMaterial"].Reference);

            particleEntity.AddDrawableComponent( ps);

            entity.AddDrawableComponent(sprite);

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

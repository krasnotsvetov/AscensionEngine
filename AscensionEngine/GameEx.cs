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
using System.IO;
using System.Text;
using System;
using Ascension.Engine.Graphics.Particles;

namespace Ascension
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 


    public class GameEx : Game
    {
        public GraphicsDeviceManager graphics;




        public Dictionary<string, IDrawableSystem> drawableSystems = new Dictionary<string, IDrawableSystem>();
        public Dictionary<string, IUpdateableSystem> updateableSystems = new Dictionary<string, IUpdateableSystem>();
        public RenderSystem RenderSystem { get { return renderSystem; } }
        public UpdateSystem UpdateSystem { get { return updateSystem; } }


        public RenderSystem renderSystem;
        public UpdateSystem updateSystem;



        public delegate void UpdateEditorDelegate(GameTime gameTime);
        public delegate void DrawEditorDelegate(GameTime gameTime);
        public delegate void StartEditorDelegate();
        public delegate void LoadEditorDelegate();
        public delegate void ConstructEditorDelegate(GameEx game);

        StartEditorDelegate startEditor;
        LoadEditorDelegate loadEditor;
        UpdateEditorDelegate updateEditor;
        DrawEditorDelegate drawEditor;

        private bool editorEnabled = false;



        public GameEx()
        {
            editorEnabled = false;
            graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
 
        }


        public GameEx(ConstructEditorDelegate ced, StartEditorDelegate sed, LoadEditorDelegate led, UpdateEditorDelegate ued, DrawEditorDelegate ded) : this()
        {
            this.startEditor = sed;
            this.updateEditor = ued;
            this.drawEditor = ded;
            this.loadEditor = led;
            editorEnabled = true;
            ced?.Invoke(this);
        }



        protected override void Initialize()
        {
            if (editorEnabled)
            {
                startEditor?.Invoke();
            }
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

            ParseConfigurationFile();
            foreach (var dc in drawableSystems.Values)
            {
                dc.LoadContent();
            }

            if (editorEnabled)
            {
                loadEditor?.Invoke();
            }

        }


        private Scene lastActiveScene;
        protected virtual void SetActiveScene(Scene scene)
        {
            
            
            if (lastActiveScene != null)
            {
                renderSystem.RemoveComponent(lastActiveScene.Name + "Renderer");
                updateSystem.RemoveComponent(lastActiveScene.Name + "Updater");
            }

            renderSystem.ActiveScene = scene;
            if (scene != null)
            {
                updateSystem.AddComponent(new KeyValuePair<string, UpdateableComponent>(scene.Name + "Updater", scene.sceneUpdater));
            }
            lastActiveScene = scene;
        }

        protected virtual void ParseConfigurationFile()
        {

            string currentStatus = "";
            string nameBlock = "";
            using (var sr = new StreamReader(new FileStream("Content\\Data.ascension", FileMode.Open)))
            {
                var cc = ContentContainer.Instance();
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.StartsWith("[")) {
                        currentStatus = "ParseNameBlock";
                    }
                    
                    switch (currentStatus)
                    {
                        case "ParseNameBlock":
                            StringBuilder sb = new StringBuilder();
                            for (int i = 1; i < line.Length - 1; i++)
                            {
                                if (!char.IsLetter(line[i]))
                                {
                                    throw new Exception("Can't parse config file");
                                }
                                sb.Append(line[i]);
                            }
                            nameBlock = sb.ToString();
                            currentStatus = "ParseBlock";
                            break;
                        case "ParseBlock":
                            switch (nameBlock)
                            {
                                case "Scenes" : 
                                   
                                    if (line.StartsWith("ActiveScene"))
                                    {
                                        SetActiveScene(cc.GetScene(line.Split('|')[1]));
                                    } else
                                    {
                                        Scene scene = Scene.Load(line, renderSystem);
                                        scene.Initialize();
                                        scene.LoadContent();
                                        cc.AddUserContent(scene);
                                    }
                                    break;
                                case "Effects":
                                    cc.AddContent<Effect>(Content.Load<Effect>(line));
                                    break;
                                case "Textures":
                                    cc.AddContent<Texture2D>(Content.Load<Texture2D>(line));
                                    break;
                                case "MaterialsData":
                                    cc.LoadMaterials(line);
                                    break;
                                case "Models":
                                    var tempArr = line.Split('|');
                                    cc.AddUserContent<ModelInstance>(new ModelInstance(tempArr[0], Content.Load<Model>(tempArr[1])));
                                    break;
                            }
                            break;
                    }

                     
                }
            }
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

            if (editorEnabled)
            {
                updateEditor?.Invoke(gameTime);

            }
            //else
            {

                foreach (var uc in updateableSystems.Values)
                {
                    uc.Update(gameTime);
                }
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
            drawEditor?.Invoke(gameTime);

            base.Draw(gameTime);
        }
    }

}

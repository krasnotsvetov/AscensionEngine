using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonogameSamples.Engine.Core;
using MonogameSamples.Engine.Core.Components;
using MonogameSamples.Engine.Graphics.SceneSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace MonogameSamples.Engine.Graphics.Filters
{
    public class DefferedLightFilter : Filter
    {

        Effect effect;
        
        public DefferedLightFilter(RenderSystem renderSystem) : base(renderSystem)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            effect = contentManager.Load<Effect>("shaders\\lightEffect");
        }


        public void Render(Texture2D diffuse, Texture2D normalMap, Texture2D lightMap, ReadOnlyCollection<Light> lights) 
        {

            spriteBatch.Begin(effect: effect); 
            effect.Parameters["ScreenWidth"].SetValue(width);
            effect.Parameters["ScreenHeight"].SetValue(height);
            effect.Parameters["LightCount"].SetValue(lights.Count);
            effect.Parameters["AmbientColor"].SetValue(Color.Black.ToVector3());
            effect.Parameters["NormalMap"].SetValue(normalMap);
            effect.Parameters["LightMap"].SetValue(lightMap);
           

            effect.Parameters["positionLight"].SetValue(lights.Select(t => (t.ParentEntity as Entity).GlobalTransform.Position).ToArray());
            effect.Parameters["colorLight"].SetValue(lights.Select(t => t.LightColor).ToArray());
            effect.Parameters["invRadiusLight"].SetValue(lights.Select(t => t.InvRadius).ToArray());
            spriteBatch.Draw(diffuse, Vector2.Zero, Color.White);
            spriteBatch.End();    
        }
    }
}

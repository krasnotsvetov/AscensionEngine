using System.Collections.Generic;
using MonogameSamples.Engine.Core.Common;
using MonogameSamples.Engine.Core.Components;

namespace MonogameSamples.Engine.Graphics.SceneSystem
{

    public class Scene2D
    {
        public UpdateableComponent scene2DUpdater;
        public DrawableComponent scene2DDrawer;
        //public List<Scene2D> Scenes { get { return scenes; } }
        public List<Entity> Entities { get { return entities; } }
        public List<Light> Lights { get { return lights; } }


        private Background background;

        // private List<Scene2D> scenes = new List<Scene2D>();
        private List<Entity> entities = new List<Entity>();
        private List<Light> lights = new List<Light>();

        public Scene2D()
        {
            scene2DDrawer = new Scene2DDrawer(this);
            scene2DUpdater = new Scene2DUpdater(this);
        }

        
    }

    
}

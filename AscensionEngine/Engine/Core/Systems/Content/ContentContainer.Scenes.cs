using Ascension.Engine.Graphics;
using Ascension.Engine.Graphics.SceneSystem;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascension.Engine.Core.Systems.Content
{
    public partial class ContentContainer
    {
        private Dictionary<string, Scene> Scenes = new Dictionary<string, Scene>();


        public Scene GetScene(string value, bool immediately = false)
        {
            return Scenes[value];
        }
        public List<string> GetSceneNames()
        {
            return Scenes.Keys.ToList();
        }
    }

    
}

using Ascension.Engine.Core.Systems.Content;
using Ascension.Engine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascension.Engine.Graphics.SceneSystem
{
    public partial class Scene 
    {
        public List<string> requiredMaterials;

        public Material GetMaterial(string name)
        {
            return contentContainer.MaterialInformation[name].GetMaterial(true);
        }
    }
}

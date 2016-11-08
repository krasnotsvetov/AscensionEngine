using Ascension.Engine.Core.Systems.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascension.Engine.Graphics
{
    public class ModelInstance : IContentObject
    {

        public ModelInstance(string name, Model model) 
        {
            this.Name = name;
            this.Model = model;
        }

        public Model Model { get; }
        public string Name { get; internal set; }
    }
}

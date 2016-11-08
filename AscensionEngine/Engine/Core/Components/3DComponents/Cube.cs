using Ascension.Engine.Core.Common.Attributes;
using System.Runtime.Serialization;
using Ascension.Engine.Graphics.SceneSystem;
using Ascension.Engine.Core.Systems.Content;

namespace Ascension.Engine.Core.Components._3DComponents
{
    [DataContract]
    [Component("Cube")]
    public class Cube : ModelComponent
    {

        public Cube(string name, string materialName) : base(name, materialName, "Cube")
        {
            this.MaterialName = "HouseMaterial";
        }

        public override void Initialize()
        {
            base.Initialize();
        }
        

        internal override void SceneChanged(Scene lastScene)
        {
            base.SceneChanged(lastScene);
        }

        public override string ToString()
        {
            return "Cube";

        }
    }
}

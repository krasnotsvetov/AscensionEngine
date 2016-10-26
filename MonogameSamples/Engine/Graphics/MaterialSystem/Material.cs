using Microsoft.Xna.Framework.Graphics;
using MonogameSamples.Engine.Graphics.MaterialSystem;
using MonogameSamples.Engine.Graphics.SceneSystem;
using MonogameSamples.Engine.Graphics.Shaders;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MonogameSamples.Engine.Graphics
{
    [DataContract]
    public class Material
    {
        public Scene Scene { get; internal set; }

        [DataMember]
        public ShaderReference ShaderReference;

        
        public ReadOnlyCollection<Texture2D> Textures { get { return textures.AsReadOnly(); } }
        public List<Texture2DReference> References = new List<Texture2DReference>();

        private List<Texture2D> textures = new List<Texture2D>();

        [DataMember]
        public IMaterialParameters Parameters;

        [DataMember]
        public IMaterialFlags Flags;



        public Material(Scene scene, IEnumerable<Texture2DReference> textureCollection, ShaderReference shaderReference)
        {
            this.Scene = scene;
            this.ShaderReference = shaderReference;
            References.AddRange(textureCollection);
            foreach (var r in References)
            {
                if (r == null)
                {
                    textures.Add(null);
                } else
                {
                    textures.Add(scene.Textures[r]);
                }
            }
        }
    }
}

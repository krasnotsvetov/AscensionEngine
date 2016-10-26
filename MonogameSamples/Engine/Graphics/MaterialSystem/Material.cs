using Microsoft.Xna.Framework.Graphics;
using MonogameSamples.Engine.Content;
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
 

        [DataMember]
        public ShaderReference ShaderReference;

        [DataMember]
        public string MaterialName;

        public ReadOnlyCollection<Texture2D> Textures { get { return textures.AsReadOnly(); } }

        [DataMember]
        public List<Texture2DReference> References = new List<Texture2DReference>();

        internal List<Texture2D> textures = new List<Texture2D>();

        [DataMember]
        public IMaterialParameters Parameters;

        [DataMember]
        public IMaterialFlags Flags;



        public Material(string materialName, IEnumerable<Texture2DReference> textureCollection, ShaderReference shaderReference)
        {
            ContentSystem contentSystem = ContentSystem.GetInstance();
            this.MaterialName = materialName;
            this.ShaderReference = shaderReference;
            References.AddRange(textureCollection);
            foreach (var r in References)
            {
                if (r == null)
                {
                    textures.Add(null);
                } else
                {
                    textures.Add(contentSystem.Textures[r.Name]);
                }
            }
        }
    }
}

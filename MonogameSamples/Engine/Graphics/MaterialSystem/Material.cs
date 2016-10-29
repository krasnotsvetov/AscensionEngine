using Microsoft.Xna.Framework.Graphics;
using MonogameSamples.Engine.Content;
using MonogameSamples.Engine.Graphics.MaterialSystem;
using MonogameSamples.Engine.Graphics.SceneSystem;
using MonogameSamples.Engine.Graphics.Shaders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace MonogameSamples.Engine.Graphics
{
    [DataContract]
    public class Material
    {


        internal static int MAXTEXTURESCOUNT = 16; 

        public EventHandler<EventArgs> MaterialChanged;

        [DataMember]
        public ShaderReference ShaderReference;

        public string MaterialName
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                reference.Name = value;
            }
        }

        public ReadOnlyCollection<Texture2D> Textures { get { return textures.AsReadOnly(); } }

        [DataMember]
        public ObservableCollection<Texture2DReference> TextureReferences = new ObservableCollection<Texture2DReference>();

        internal List<Texture2D> textures = new List<Texture2D>();

        [DataMember]
        public IMaterialParameters Parameters;

        [DataMember]
        public IMaterialFlags Flags;


        public MaterialReference Reference {get { return reference;}}


        private MaterialReference reference;

        [DataMember]
        private string name;

        public Material(string materialName, IEnumerable<Texture2DReference> textureCollection, ShaderReference shaderReference)
        {
            reference = new MaterialReference(materialName);
            this.MaterialName = materialName;
            this.ShaderReference = shaderReference;
            textures = new List<Texture2D>();
            for (int i = 0; i < MAXTEXTURESCOUNT; i++)
            {
                textures.Add(null);
                TextureReferences.Add(null);
            }
            TextureReferences.CollectionChanged += TexturesChanged;

            var index = 0;
            if (textureCollection != null)
            {
                foreach (var t in textureCollection)
                {
                    TextureReferences[index] = t;
                    index++;
                }
            }
        }


        /// <summary>
        /// Only for deserializer. 
        /// </summary>
        internal void Initialize()
        {
            ContentSystem contentSystem = ContentSystem.GetInstance();
            reference = new MaterialReference(MaterialName);
            textures = new List<Texture2D>();
            for (int i = 0; i < MAXTEXTURESCOUNT; i++)
            {
                textures.Add(null);
            }
            TextureReferences.CollectionChanged += TexturesChanged;

            var index = 0;
            foreach (var t in TextureReferences)
            {
                if (t != null)
                {
                    textures[index] = contentSystem.Textures[t.Name];
                }
                index++;
            }
        }

        private void TexturesChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            ContentSystem contentSystem = ContentSystem.GetInstance();
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add | NotifyCollectionChangedAction.Remove | NotifyCollectionChangedAction.Reset:
                    //TODO
                    throw new Exception(String.Format("You can only replace textures in material from 0 to {0}", MAXTEXTURESCOUNT));
                    //break;
                case NotifyCollectionChangedAction.Replace:
                    string name = TextureReferences[args.OldStartingIndex]?.Name;
                    if (name == null)
                    {
                        textures[args.OldStartingIndex] = null;
                    }
                    else {
                        textures[args.OldStartingIndex] = contentSystem.Textures[name];
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
            }
            MaterialChanged?.Invoke(this, EventArgs.Empty);

        }

        
    }
}

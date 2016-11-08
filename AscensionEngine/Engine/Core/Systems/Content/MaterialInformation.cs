using Ascension.Engine.Core.Common;
using Ascension.Engine.Graphics;
using Ascension.Engine.Graphics.SceneSystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Ascension.Engine.Core.Systems.Content
{
    public class MaterialInformation : IContentObject
    {

        private Material material = null;

        public Material GetMaterial(bool immediately = false)
        {
            ContentContainer cc = ContentContainer.Instance();
            if (material == null)
            {
                material = new Material(this, cc, immediately);
                IsAvailable = true;
            }
            return material;
        } 

        public bool IsAvailable { get; private set; }

        public string Name
        {
            get
            {
                return materialName;
            }
            internal set
            {
                if (material != null)
                {
                    material.MaterialName = value;
                }
                materialName = value;
            }
        }

        private string materialName;

        public ReadOnlyDictionary<string, string> Textures
        {
            get { return new ReadOnlyDictionary<string, string>(textures); }
        }

        internal Dictionary<string, string> textures = new Dictionary<string, string>();
        public string RequiredShader { get { return requiredShader; } internal set { requiredShader = value; } }

        private string requiredShader;

        public int ReferenceCount
        {
            get
            {
                return referenceCount;
            }

            internal set
            {
                referenceCount = value;
            }
        }

        private int referenceCount;

        public MaterialInformation(StreamReader streamReader)
        {

            if (!streamReader.ReadLine().Equals("{"))
            {
                throw new Exception("Bad signature");
            }
            ReferenceCount = 0;
            Name = streamReader.ReadLine().Split('|')[1];
            int requiredTextureCount = int.Parse(streamReader.ReadLine().Split('|')[1]);
            for (int i = 0; i < requiredTextureCount; i++)
            {
                parseTextureBlock(streamReader);
            }

            parseShaderBlock(streamReader);
            if (!streamReader.ReadLine().Equals("}"))
            {
                throw new Exception("Bad signature");
            }

        }


        public MaterialInformation(string materialName, Dictionary<string, string> textureMap, string requiredShader)
        {
            this.Name = materialName;
            this.textures = textureMap;
            this.requiredShader = requiredShader;
        }

        private void parseTextureBlock(StreamReader streamReader)
        {
            string textureName = "";
            string textureType = "";
            if (!streamReader.ReadLine().StartsWith("{"))
            {
                throw new Exception("Bad signature");
            }
            string line;
            while (!(line = streamReader.ReadLine()).StartsWith("}"))
            {
                string[] tokens = line.Split('|');
                switch (tokens[0])
                {
                    case "TextureName":
                        textureName = tokens[1];
                        break;
                    case "TextureType":
                        textureType = tokens[1];
                        break;
                }
            }
            textures.Add(textureType, textureName);
        }

        private void parseShaderBlock(StreamReader streamReader)
        {
            if (!streamReader.ReadLine().Equals("{"))
            {
                throw new Exception("Bad signature");
            }

            string[] tokens = streamReader.ReadLine().Split('|');

            if (!tokens[0].Equals("ShaderName"))
            {
                throw new Exception("Bad signature");
            }

            RequiredShader = tokens[1];

            if (!streamReader.ReadLine().Equals("}"))
            {
                throw new Exception("Bad signature");
            }
        }

        public void Save(StreamWriter sw)
        {
            foreach (var s in Save().Split('\n'))
            {
                sw.WriteLine(s);
            }
        }

        public string Save()
        {
            string data = "";
            data += "{\n";
            data += "Name|" + materialName + "\n";
            data += "RequiredTextureCount|" + textures.Count + "\n";
            foreach (var p in textures)
            {
                data += "{\n";
                data += "TextureName|" + p.Value +"\n";
                data += "TextureType|" + p.Key + "\n";
                data += "}\n";
            }
            data += "ShaderName|" + requiredShader + "\n";
            data += "}\n";
            return data;
        }
       
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Ascension.Engine.Graphics;
using System.Collections.ObjectModel;
using Ascension.Engine.Graphics.SceneSystem;

namespace Ascension.Engine.Core.Systems.Content
{
    public partial class ContentContainer
    {


        private static ContentContainer instance = null;
        
        public static ContentContainer Instance()
        {
            if (instance == null)
            {
                instance = new ContentContainer();
            }
            return instance;
        }

        Dictionary<Type, object> content = new Dictionary<Type, object>();

       

        public ReadOnlyDictionary<string, MaterialInformation> MaterialInformation
        {
            get
            {
                return new ReadOnlyDictionary<string, Content.MaterialInformation>(materialInformation);
            }
        }
        Dictionary<string, MaterialInformation> materialInformation = new Dictionary<string, MaterialInformation>();
        Dictionary<string, List<IMaterialOwner>> materialOwners = new Dictionary<string, List<IMaterialOwner>>();

        private ContentContainer()
        {
            content.Add(typeof(Texture2D), Textures);
            content.Add(typeof(Effect), Effects);
            content.Add(typeof(Model), Models);
        }

        public void AddMaterial(MaterialInformation info)
        {
            materialInformation.Add(info.MaterialName, info);
            if (materialOwners.ContainsKey(info.MaterialName))
            {
                var tempList = new List<IMaterialOwner>();

                //User code can change materialOwners[] collection
                foreach (var owner in materialOwners[info.MaterialName])
                {
                    tempList.Add(owner);
                }
                foreach (var owner in tempList)
                {
                    owner.MaterialChangedHandler?.Invoke(this, new ContentOwnerEventArgs<Material>
                        (ContentAction.Add, info.MaterialName, info.MaterialName, info.GetMaterial(), info.GetMaterial()));
                }
            }
        }

        public void LoadMaterials(string path)
        {
            using (var sr = new StreamReader(new FileStream(path, FileMode.Open)))
            {
                if (!sr.ReadLine().Equals("#MaterialData"))
                {
                    throw new Exception("Bad signature");
                }
                int materialCount = int.Parse(sr.ReadLine());
                for (int i = 0; i < materialCount; i++)
                {
                     AddMaterial(new MaterialInformation(sr));
                }
            }
        }

        

        public void AddContent<T>(T obj) where T : GraphicsResource
        {

            var d = content[obj.GetType()] as Dictionary<string, T>;
            d.Add(obj.Name, obj);
        }

        public void AddContent<T>(string name, T obj)
        {

            var d = content[obj.GetType()] as Dictionary<string, T>;
            d.Add(name, obj);
        }


        public void AddMaterialListener(IMaterialOwner owner, string name)
        {
            if (!materialOwners.ContainsKey(name))
            {
                materialOwners.Add(name, new List<IMaterialOwner>());
            }

            if (!materialOwners[name].Contains(owner))
            {
                materialOwners[name].Add(owner);
            }
        }

        public void RemoveMaterialListener(IMaterialOwner owner, string name)
        {
            if (!materialOwners.ContainsKey(name))
            {
                return;
            }

            if (materialOwners[name].Contains(owner))
            {
                materialOwners[name].Remove(owner);
            }
            if (materialOwners[name].Count == 0)
            {
                materialOwners.Remove(name);
            }
        }

    }

    public class ContentOwnerEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Last content name
        /// </summary>
        public string LastName { get; }
        /// <summary>
        /// New content name
        /// </summary>
        public string NewName { get; }
        /// <summary>
        /// Last texture
        /// </summary>
        public T Previous { get; }
        /// <summary>
        /// New texture
        /// </summary>
        public T New { get; }
        /// <summary>
        /// Action
        /// </summary>
        public ContentAction Action { get; }

        public ContentOwnerEventArgs(ContentAction action, string lastName, string newName, T previousContent, T newContent)
        {
            this.Action = action;
            this.LastName = lastName;
            this.NewName = newName;
            this.Previous = previousContent;
            this.New = newContent;
        }
    }

    public enum ContentAction
    {
        Rename,
        Replace,
        Remove,
        Add
    }
}

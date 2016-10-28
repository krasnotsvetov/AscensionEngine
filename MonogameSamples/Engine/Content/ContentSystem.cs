using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonogameSamples.Engine.Content
{
    /// <summary>
    /// TEMPORARY implementation of ContentSystem.
    /// </summary>
    public class ContentSystem
    {
        public Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        public Dictionary<string, Effect> Effect = new Dictionary<string, Effect>();
        public Dictionary<string, Model> Models = new Dictionary<string, Model>();
        public Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
        public Dictionary<string, SoundEffect> SoundEffects = new Dictionary<string, SoundEffect>();
        public Dictionary<string, Song>  Songs = new Dictionary<string, Song>();



        protected ContentSystem()
        {

        }


        private static ContentSystem instance = null;

        /// <summary>
        /// Return instace of content system.
        /// </summary>
        /// <returns></returns>
        public static ContentSystem GetInstance()
        {
            if (instance == null)
            {
                instance = new ContentSystem();
            }
            return instance;
        }
    }
}

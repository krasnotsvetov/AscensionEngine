using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Ascension.Engine.Graphics.SceneSystem;
using System;
using System.Runtime.Serialization;

namespace Ascension.Engine.Core.Common
{
    [DataContract]
    public class DrawableComponent : IComparable<DrawableComponent>, IDrawable, IDisposable, IGameComponent
    {

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        [DataMember]
        private int drawOrder = 0;

        [DataMember]
        private bool isVisible = true;


        public virtual int DrawOrder
        {
            get
            {
                return drawOrder;
            }
            set
            {
                drawOrder = value;
                DrawOrderChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public virtual bool Visible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;
                VisibleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public virtual void SceneChanged(Scene lastScene)
        {
        }

        public virtual void Initialize()
        {

        }



      


        public virtual void LoadContent(ContentManager contentManager)
        {

        }

        public virtual void Draw(GameTime gameTime)
        {
           
        }

        public virtual int CompareTo(DrawableComponent other)
        {
            return drawOrder.CompareTo(other.drawOrder);
        }

        public virtual void Dispose()
        {

        }
    }
}

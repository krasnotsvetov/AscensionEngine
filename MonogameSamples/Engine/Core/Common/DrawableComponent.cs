using Microsoft.Xna.Framework;
using System;
using System.Runtime.Serialization;

namespace MonogameSamples.Engine.Core.Common
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
                if (DrawOrderChanged != null)
                {
                    DrawOrderChanged(this, EventArgs.Empty);
                }
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
                if (VisibleChanged != null)
                {
                    VisibleChanged(this, EventArgs.Empty);
                }
            }
        }

        public virtual void Initialize()
        {

        }



        public virtual int CompareTo(DrawableComponent other)
        {
            return drawOrder.CompareTo(other.drawOrder);
        }

      

        public virtual void Draw(GameTime gameTime)
        {
           
        }

    

        public virtual void Dispose()
        {

        }
    }
}

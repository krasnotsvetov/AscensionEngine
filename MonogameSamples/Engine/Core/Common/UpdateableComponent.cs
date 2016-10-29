using Microsoft.Xna.Framework;
using MonogameSamples.Engine.Graphics.SceneSystem;
using System;
using System.Runtime.Serialization;

namespace MonogameSamples.Engine.Core.Common
{
    [DataContract]
    public class UpdateableComponent : IComparable<UpdateableComponent>, IUpdateable, IDisposable, IGameComponent
    {


        

        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> EnabledChanged;

        [DataMember]
        private int updateOrder = 0;

        [DataMember]
        private bool isEnabled = true;

        public virtual int UpdateOrder
        {
            get
            {
                return updateOrder;
            }
            set
            {
                updateOrder = value;
                UpdateOrderChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public virtual bool Enabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                isEnabled = value;
                EnabledChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public virtual void SceneChanged(Scene lastScene)
        {

        }

        public virtual void Initialize()
        {

        }


        public virtual int CompareTo(UpdateableComponent other)
        {
            return updateOrder.CompareTo(other.updateOrder);
        }


        public virtual void Dispose()
        {

        }

        public virtual void Update(GameTime gameTime)
        {
        }
    }
}

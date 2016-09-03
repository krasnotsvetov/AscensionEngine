using Microsoft.Xna.Framework;
using System; 

namespace MonogameSamples.Engine.Core.Common
{
    public class UpdateableComponent : IComparable<UpdateableComponent>, IUpdateable, IDisposable, IGameComponent
    {


        

        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> EnabledChanged;

        private int updateOrder = 0;
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
                if (UpdateOrderChanged != null)
                {
                    UpdateOrderChanged(this, EventArgs.Empty);
                }
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
                if (EnabledChanged != null)
                {
                    EnabledChanged(this, EventArgs.Empty);
                }
            }
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

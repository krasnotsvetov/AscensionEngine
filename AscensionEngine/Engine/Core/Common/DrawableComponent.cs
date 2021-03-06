﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Ascension.Engine.Graphics.SceneSystem;
using System;
using System.Runtime.Serialization;

namespace Ascension.Engine.Core.Common
{
    [DataContract]
    public class DrawableComponent : IComparable<DrawableComponent>, IDisposable, IGameComponent
    {

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;
        public bool IsInitialized { get; protected set; }

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

        internal virtual void SceneChanged(Scene lastScene)
        {
        }

        public virtual void Initialize()
        {
            IsInitialized = true;
        }



      


        public virtual void LoadContent()
        {

        }

        public virtual void Draw(Matrix view, Matrix projection, GameTime gameTime)
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

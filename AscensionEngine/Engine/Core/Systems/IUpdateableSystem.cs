using Ascension.Engine.Core.Common;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscensionEngine.Engine.Core.Systems
{
    public interface IUpdateableSystem : IDisposable
    {
        void Initialize();
        void Update(GameTime gameTime);
        List<UpdateableComponent> GameComponents { get; }
        bool Enabled { get; set; }

    }
}

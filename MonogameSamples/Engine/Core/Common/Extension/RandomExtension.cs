using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonogameSamples.Engine.Core.Common.Extension
{
    public static class RandomExtension
    {
        public static Vector2 RandomVector2(this Random rnd, float X, float Y)
        {
            Vector2 v;
            v.X = (float)(rnd.NextDouble() * X * 2) - X;
            v.Y = (float)(rnd.NextDouble() * Y * 2) - Y;
            return v;
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascension.Engine.Core.Common.Mathematics
{
    public static class AscensionMath
    {
        public static int TurnPredicate(Vector2 a, Vector2 b, Vector2 c)
        {
            return Math.Sign((b.X - a.X) * (c.Y - a.Y) - (c.X - a.X) * (b.Y - a.Y));
        }
    }
}

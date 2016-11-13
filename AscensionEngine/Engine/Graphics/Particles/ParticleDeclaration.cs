using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System.Runtime.InteropServices;

namespace Ascension.Engine.Graphics.Particles
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ParticleDeclaration : IVertexType
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public Vector2 Corner;
        public Color RandomValues;
        public Vector2 Time;

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
           new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
           new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
           new VertexElement(sizeof(float) * 6, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
           new VertexElement(sizeof(float) * 8, VertexElementFormat.Color, VertexElementUsage.TextureCoordinate, 1),
           new VertexElement(sizeof(float) * 8 + sizeof(byte) * 4, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 2)
        );

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get
            {
                return VertexDeclaration;
            }
        }

        public ParticleDeclaration(Vector3 position, Vector2 corner, Vector3 velocity, Color randomValues, Vector2 time)
        {
            Position = position;
            Velocity = velocity;
            Corner = corner;
            RandomValues = randomValues;
            Time = time;
        }
    }
}
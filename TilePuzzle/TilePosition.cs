using System;

namespace TilePuzzle
{
    public struct TilePosition : IEquatable<TilePosition>
    {
        private readonly int hComponent;
        private readonly int vComponent;
        public int HComponent { get { return hComponent; } }
        public int VComponent { get { return vComponent; } }

        public TilePosition(int hComponent, int vComponent)
        {
            this.hComponent = hComponent;
            this.vComponent = vComponent;
        }

        public static bool operator ==(TilePosition position1, TilePosition position2)
        {
            return position1.Equals(position2);
        }

        public static bool operator !=(TilePosition position1, TilePosition position2)
        {
            return !(position1 == position2);
        }

        public bool Equals(TilePosition other)
        {
            return other.HComponent == this.HComponent && other.VComponent == this.VComponent;
        }
    }
}

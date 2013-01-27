namespace TilePuzzle
{
    public class Tile
    {
        private TilePosition currentPosition;
        /// <summary>
        /// Represents the current position of the tile in the puzzle. 
        /// </summary>
        public TilePosition CurrentPosition
        {
            get { return currentPosition; }

            set
            {
                currentPosition = value;
                OnPositionChanged(currentPosition,value);
            }
 
        }

        private readonly TilePosition targetPosition;
        /// <summary>
        /// Gives the correct position of the tile when the puzzle is solved. 
        /// </summary>
        public TilePosition TargetPosition {
            get { return targetPosition; }
        }


        public delegate void PositionChangedDelegate(Tile sender, TilePosition oldPosition ,TilePosition newPosition);
        public event PositionChangedDelegate PositionChanged;

        protected virtual void OnPositionChanged(TilePosition oldposition, TilePosition newposition)
        {
            PositionChangedDelegate handler = PositionChanged;
            if (handler != null) handler(this, oldposition, newposition);

        }

        public Tile(TilePosition targetPosition)
        {
            this.targetPosition = targetPosition;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Tile)) return false;
            Tile tile = (Tile) obj;
            return (tile.TargetPosition == this.TargetPosition);
        }


        public override int GetHashCode()
        {
            int hashcode = 13;
            hashcode = (7 * hashcode) + TargetPosition.GetHashCode();
            return hashcode;
        }
    }
}

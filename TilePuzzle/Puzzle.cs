using System;
using System.Collections.Generic;
using System.Linq;

namespace TilePuzzle
{
    public class Puzzle
    {
        private readonly int size;
        public int Size { get { return size; } }

        /// <summary>
        /// Contains the current position of the game.the first coordinate is 
        /// </summary>
        private readonly Tile[,] puzzleState;

        private readonly Tile hole;

        public event Tile.PositionChangedDelegate TilePositionChanged;

        public enum MoveDirection
        {
            Up,
            Down,
            Left,
            Right,
            None
        }

        public struct MoveProperties
        {
            /// <summary>
            /// A list of the tile which will be moved ordered starting from the target in the direction of the hole
            /// </summary>
            public IList<Tile> InvolvedTiles;
            public TilePosition HolePosition;
            public MoveDirection Direction;
        } 

        public Puzzle(int size)
        {
            this.size = size;
            puzzleState = new Tile[size,size];
            
            for (int horizontalPosition = 0; horizontalPosition < size; horizontalPosition++)
            {
                for (int verticalPosition = 0; verticalPosition < size; verticalPosition++)
                {

                    TilePosition position = new TilePosition(horizontalPosition, verticalPosition);
                    Tile tile = new Tile(position);
                    tile.PositionChanged += TilePositionChanged; 
                    this[position] = tile;
                    if (horizontalPosition == size - 1 && verticalPosition == size - 1)
                    {
                        hole = tile;
                    }
                }
            }
             
        }

        public MoveProperties MakeMove(TilePosition target)
        {
            MoveProperties moveProperties = CheckMove(target);
            if (moveProperties.Direction != MoveDirection.None)
            {
                IList<Tile> tiles = moveProperties.InvolvedTiles;
                TilePosition holePosition = hole.CurrentPosition;

                this[tiles[0].CurrentPosition] = hole;
                for (int i = 0; i < tiles.Count-1; i++)
                {
                    this[tiles[i + 1].CurrentPosition] = tiles[i];
                }
                this[holePosition] = tiles[tiles.Count - 1];
                
            }

            return moveProperties;
        }

        /// <summary>
        /// When using a real tile puzzle, the player makes a move by placing a finger on a tile and 
        /// slideing it towrads the hole. This method return a MoveProperties struct containgn the direction of the
        /// slide movement and a list of all tile that would be moved along with the one being held. 
        /// If the target position is not in the same horizontal or vertical position or is the exact position 
        /// of the hole, the method return a MoveProperties with an empty list and MoveDirection.None. 
        /// 
        /// 
        /// </summary>
        /// <param name="target"></param>
        public MoveProperties CheckMove(TilePosition target)
        {
            MoveDirection direction = GetDirection(hole.CurrentPosition, target);
            List<Tile> involvedTiles = new List<Tile>();

            if (direction != MoveDirection.None)
            {
                for (Tile tile = this[target]; tile != hole; tile = GetNextTile(tile, direction))
                {
                    involvedTiles.Add(tile);
                }
            }

            return new MoveProperties
                {
                    Direction = direction,
                    InvolvedTiles = involvedTiles,
                    HolePosition = hole.CurrentPosition
                };
        }

        public Tile this[TilePosition position]
        {
            get { return puzzleState[position.HComponent, position.VComponent];}
            private set
            {
                value.CurrentPosition = position;
                puzzleState[position.HComponent, position.VComponent] = value;
            }

        }

        private Tile GetNextTile(Tile currentTile, MoveDirection direction)
        {

            TilePosition currentPosition = currentTile.CurrentPosition;
            switch (direction)
            {
                case MoveDirection.Up:
                    return puzzleState[currentPosition.HComponent, currentPosition.VComponent - 1];
                case MoveDirection.Down:
                    return puzzleState[currentPosition.HComponent, currentPosition.VComponent + 1];
                case MoveDirection.Left:
                    return puzzleState[currentPosition.HComponent-1, currentPosition.VComponent];
                case MoveDirection.Right:
                    return puzzleState[currentPosition.HComponent +1, currentPosition.VComponent];
                default:
                    return null;
            }
        }

        private MoveDirection GetDirection(TilePosition holePosition, TilePosition target)
        {
            if (!holePosition.Equals(target))
            {
                if (holePosition.HComponent == target.HComponent)
                {
                    if (holePosition.VComponent > target.VComponent)
                    {
                        return MoveDirection.Down;
                    }
                    else
                    {
                        return MoveDirection.Up;
                    }
                }
                else if (holePosition.VComponent == target.VComponent)
                {
                    if (holePosition.HComponent > target.HComponent)
                    {
                        return MoveDirection.Right;
                    }
                    else
                    {
                        return MoveDirection.Left;
                    }
                }
            }
            return MoveDirection.None;
        }

        public IList<Tile> GetAllTiles()
        {
            IList<Tile> tiles = new List<Tile>(puzzleState.OfType<Tile>().ToList());
            tiles.Remove(hole);
            return tiles;
        }

        public IList<TilePosition> GetValidMoves()
        {
            IList<TilePosition> validMoves = new List<TilePosition>();
            for (int i = 0; i < Size; i++)
            {
                int holeVComponent = hole.CurrentPosition.VComponent;
                int holeHComponent = hole.CurrentPosition.HComponent;
                if (i != holeHComponent)
                {
                    validMoves.Add(new TilePosition(i,holeVComponent));      
                }

                if (i!= holeVComponent)
                {
                    validMoves.Add(new TilePosition(holeHComponent,i)); 
                }
            }
            return validMoves;
        }

        public void MakeRandomMove()
        {
            Random random = new Random();
            IList<TilePosition> validMoves = GetValidMoves();
            int randomIndex = random.Next(validMoves.Count - 1);
            MakeMove(validMoves[randomIndex]);

        }
    }
}

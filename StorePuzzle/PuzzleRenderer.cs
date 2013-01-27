using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using StorePuzzle.Annotations;
using TilePuzzle;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace StorePuzzle
{
    public class PuzzleRenderer : INotifyPropertyChanged
    {
        private readonly Canvas canvas;
        private readonly Puzzle puzzle;
        private readonly Dictionary<Tile, TileRenderer> tileToRenderer = new Dictionary<Tile, TileRenderer>();
        private Puzzle.MoveProperties moveProperties;
        private Point startingPosition;
        private readonly TimeSpan defaultAnimationTime = TimeSpan.FromSeconds(0.25);
        private int userMoves;
        public int UserMoves
        {
            get { return userMoves; }
            set { 
                userMoves = value;
                OnPropertyChanged();
            }
        }


        public PuzzleRenderer(Canvas canvas,Puzzle puzzle)
        {
            this.canvas = canvas;
            this.puzzle = puzzle;
            canvas.Width = TileRenderer.DefaultWidth * puzzle.Size;
            canvas.Height = TileRenderer.DefaultHeight * puzzle.Size;

            foreach (Tile tile in puzzle.GetAllTiles())
            {
                int tileNumber = tile.TargetPosition.VComponent*puzzle.Size + tile.TargetPosition.HComponent;
                string displayText = tileNumber.ToString(CultureInfo.InvariantCulture);
                TileRenderer tileRenderer = new TileRenderer(canvas, displayText, tile)
                    {
                        AnimationTime = defaultAnimationTime
                    };

                tileRenderer.PointerPressed += HandleTilePressed;
                tileRenderer.PointerReleased += HandleTileReleased;

                tileToRenderer.Add(tile, tileRenderer);
            }
        }



        private void HandleTilePressed(object sender, PointerRoutedEventArgs e)
        {
            TileRenderer renderer = (TileRenderer) sender;
            moveProperties = puzzle.CheckMove(renderer.Tile.CurrentPosition);

            if (moveProperties.Direction == Puzzle.MoveDirection.None) return;

            startingPosition = e.GetCurrentPoint(canvas).Position;
            canvas.PointerMoved += MoveTile;
        }

        private void MoveTile(object sender, PointerRoutedEventArgs e)
        {
            if (!e.Pointer.IsInContact) return;

            Point position = e.GetCurrentPoint(canvas).Position;
            double pointerDelta;
            double delta;
            switch (moveProperties.Direction)
            {
                case Puzzle.MoveDirection.Up:
                    pointerDelta = position.Y - startingPosition.Y;
                    delta = Math.Min(Math.Max(-TileRenderer.DefaultHeight, pointerDelta), 0);
                    break;
                case Puzzle.MoveDirection.Down:
                    pointerDelta = position.Y - startingPosition.Y;
                    delta = Math.Min(Math.Max(0, pointerDelta), TileRenderer.DefaultHeight);
                    break;
                case Puzzle.MoveDirection.Left:
                    pointerDelta = position.X - startingPosition.X;
                    delta = Math.Min(Math.Max(-TileRenderer.DefaultWidth, pointerDelta), 0);
                    break;
                case Puzzle.MoveDirection.Right:
                    pointerDelta = position.X - startingPosition.X;
                    delta = Math.Min(Math.Max(0, pointerDelta), TileRenderer.DefaultWidth);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            foreach (Tile tile in moveProperties.InvolvedTiles)
            {
                switch (moveProperties.Direction)
                {
                    case Puzzle.MoveDirection.Up:
                    case Puzzle.MoveDirection.Down:
                        double initialTop = tile.CurrentPosition.VComponent*TileRenderer.DefaultHeight;
                        Canvas.SetTop(tileToRenderer[tile], initialTop + delta);
                        break;
                    case Puzzle.MoveDirection.Left:
                    case Puzzle.MoveDirection.Right:
                        double initialLeft = tile.CurrentPosition.HComponent*TileRenderer.DefaultWidth;
                        Canvas.SetLeft(tileToRenderer[tile], initialLeft + delta);
                        break;
                    case Puzzle.MoveDirection.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                    
            }
        }

        private void HandleTileReleased(object sender, PointerRoutedEventArgs e)
        {
            if (moveProperties.Direction == Puzzle.MoveDirection.None) return;

            Point position = e.GetCurrentPoint(canvas).Position;
            double vDelta = position.Y - startingPosition.Y;
            double hDelta = position.X - startingPosition.X;
            bool makeMove = (moveProperties.Direction == Puzzle.MoveDirection.Down && vDelta > 0)
                            || (moveProperties.Direction == Puzzle.MoveDirection.Up && vDelta < 0)
                            || (moveProperties.Direction == Puzzle.MoveDirection.Right && hDelta > 0)
                            || (moveProperties.Direction == Puzzle.MoveDirection.Left && hDelta < 0);
            if (!makeMove) return;
            TileRenderer tileRenderer = (TileRenderer)sender;
            puzzle.MakeMove(tileRenderer.Tile.CurrentPosition);
            UserMoves++;
        }

        private void HandleTapped(object sender, TappedRoutedEventArgs e)
        {
           
            TileRenderer tileRenderer = (TileRenderer)sender;
            puzzle.MakeMove(tileRenderer.Tile.CurrentPosition);
        }

        public void SetAsComputerControlled(TimeSpan computerMoveTime)
        {
            foreach (TileRenderer tileRenderer in tileToRenderer.Values)
            {
                tileRenderer.PointerPressed -= HandleTilePressed;
                tileRenderer.PointerReleased -= HandleTileReleased;
                tileRenderer.AnimationTime = computerMoveTime;
            }
        }

        public void SetAsUserControlled()
        {
            foreach (TileRenderer tileRenderer in tileToRenderer.Values)
            {
                tileRenderer.PointerPressed += HandleTilePressed;
                tileRenderer.PointerReleased += HandleTileReleased;
                tileRenderer.AnimationTime = defaultAnimationTime;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

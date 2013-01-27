using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Input;
using TilePuzzle;

namespace MinimalWpfUI
{
    public class PuzzleRenderer
    {
        private readonly Canvas canvas;
        private readonly Puzzle puzzle;
        private readonly Dictionary<Tile, TileRenderer> tileToRenderer = new Dictionary<Tile, TileRenderer>(); 

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
                TileRenderer tileRenderer = new TileRenderer(canvas, displayText, tile);
                tileRenderer.MouseUp += HandleClick;
                tileToRenderer.Add(tile, tileRenderer);
            }
        }

        private void HandleClick(object sender, MouseButtonEventArgs e)
        {
            TileRenderer tileRenderer = (TileRenderer) sender;
            puzzle.MakeMove(tileRenderer.Tile.CurrentPosition);
        }
    }
}

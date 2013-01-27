using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TilePuzzle;

namespace MinimalWpfUI
{
    public class TileRenderer : Grid
    {
        public Tile Tile { get; private set; }
        private readonly Canvas canvas;
        public const int DefaultWidth = 100;
        public const int DefaultHeight = 100;
        private readonly Rectangle rectangle;
        private readonly TextBlock text;



        public TileRenderer(Canvas canvas, string displayText, Tile tile)
        {
            Tile = tile;
            this.canvas = canvas;
            Width = DefaultWidth;
            Height = DefaultHeight;
            rectangle = new Rectangle
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Stroke = new SolidColorBrush(Colors.Black),
                Fill = new SolidColorBrush(Colors.WhiteSmoke)
            };
            Children.Add(rectangle);
            
            text = new TextBlock
            {
                Text = displayText,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                TextAlignment = TextAlignment.Center,
                FontSize = 36,
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 26, 0, 26)

            };

            Children.Add(text);
            canvas.Children.Add(this);
            SetPosition(tile.CurrentPosition);
            tile.PositionChanged += UpdatePosition;
        }

        private void UpdatePosition(Tile sender, TilePosition oldposition, TilePosition newposition)
        {
            SetPosition(newposition);
        }

        private void SetPosition(TilePosition position)
        {
            int left = DefaultWidth * position.HComponent;
            int top = DefaultHeight * position.VComponent;
            Canvas.SetLeft(this, left);
            Canvas.SetTop(this, top);
        }
    }
}

using System;
using TilePuzzle;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace StorePuzzle
{
    public class TileRenderer : Grid
    {
        public Tile Tile { get; private set; }
        private readonly Canvas canvas;
        public const int DefaultWidth = 100;
        public const int DefaultHeight = 100;
        private readonly Rectangle rectangle;
        private readonly TextBlock text;
        private Storyboard storyboard;
        public TimeSpan AnimationTime { get; set; }
    


        public TileRenderer(Canvas canvas, string displayText, Tile tile)
        {
            storyboard = new Storyboard();
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
                Foreground = new SolidColorBrush(Colors.Black),
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 26, 0, 26)

            };

            Children.Add(text);
            canvas.Children.Add(this);
            SetPosition();
            tile.PositionChanged += UpdatePosition;
        }

        private void UpdatePosition(Tile sender, TilePosition oldposition, TilePosition newposition)
        {
            storyboard = new Storyboard();
            int left = DefaultWidth * newposition.HComponent;
            int top = DefaultHeight * newposition.VComponent;

            AnimationTime = TimeSpan.FromSeconds(0.25);
            DoubleAnimation horizontalAnimation = new DoubleAnimation
                {
                    From = Canvas.GetLeft(this),
                    To = left,
                    Duration = AnimationTime

                };

            horizontalAnimation.Completed += StopAnimation;
            Storyboard.SetTarget(horizontalAnimation, this);
            Storyboard.SetTargetProperty(horizontalAnimation, "(Canvas.Left)");

            DoubleAnimation verticalAnimation = new DoubleAnimation
            {
                From = Canvas.GetTop(this),
                To = top,
                Duration = AnimationTime

            };

            horizontalAnimation.Completed += StopAnimation;
            Storyboard.SetTarget(verticalAnimation, this);
            Storyboard.SetTargetProperty(verticalAnimation, "(Canvas.Top)");

            storyboard.Children.Add(horizontalAnimation);
            storyboard.Children.Add(verticalAnimation);
            storyboard.Begin();

        }

        private void SetPosition()
        {
            int left = DefaultWidth * Tile.CurrentPosition.HComponent;
            int top = DefaultHeight * Tile.CurrentPosition.VComponent;
            Canvas.SetLeft(this, left);
            Canvas.SetTop(this, top);
        }

        private void StopAnimation(object sender, object e)
        {
            SetPosition();
            storyboard.Stop();
        }
    }
}

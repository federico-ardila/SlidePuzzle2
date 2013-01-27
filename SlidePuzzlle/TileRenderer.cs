using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TilePuzzle;

namespace WpfUI
{
    public class TileRenderer : Grid
    {
        public Tile Tile { get; private set; }
        private readonly Canvas canvas;
        public const int DefaultWidth = 100;
        public const int DefaultHeight = 100;
        private readonly Rectangle rectangle;
        private readonly TextBlock text;
        private Storyboard storyboard = new Storyboard();
        public TimeSpan AnimationTime { get; set; }


        public TileRenderer(Canvas canvas, string displayText, Tile tile)
        {
            Tile = tile;
            AnimationTime = TimeSpan.FromSeconds(0.25);
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
            SetPosition();
            tile.PositionChanged += UpdatePosition;
        }

        private void UpdatePosition(Tile sender, TilePosition oldposition, TilePosition newposition)
        {
            
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
            Storyboard.SetTargetProperty(horizontalAnimation, new PropertyPath("(Canvas.Left)"));

            DoubleAnimation verticalAnimation = new DoubleAnimation
            {
                From = Canvas.GetTop(this),
                To = top,
                Duration = AnimationTime

            };

            horizontalAnimation.Completed += StopAnimation;
            Storyboard.SetTarget(verticalAnimation, this);
            Storyboard.SetTargetProperty(verticalAnimation, new PropertyPath("(Canvas.Top)"));

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

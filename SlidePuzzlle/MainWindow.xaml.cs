

using System.Windows;
using TilePuzzle;
using WpfUI;


namespace MinimalWpfUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Puzzle puzzle = new Puzzle(5);
            PuzzleRenderer puzzleRenderer = new PuzzleRenderer(Canvas,puzzle);
            SizeToContent = SizeToContent.WidthAndHeight;
            ResizeMode = ResizeMode.NoResize;
        }

    }
}

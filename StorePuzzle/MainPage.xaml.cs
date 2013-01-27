using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TilePuzzle;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StorePuzzle
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Puzzle puzzle;
        public PuzzleRenderer PuzzleRenderer { get; set; }
    

        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            puzzle = new Puzzle(6);
            PuzzleRenderer = new PuzzleRenderer(Canvas, puzzle);
            Shuffle();
        }

        private async void Shuffle()
        {
            PuzzleRenderer.SetAsComputerControlled(TimeSpan.FromSeconds(0.05));
            await Task.Delay(TimeSpan.FromSeconds(2));
            for (int i = 0; i < 100; i++)
            {
                puzzle.MakeRandomMove();
                await Task.Delay(TimeSpan.FromSeconds(0.07));
            }
            PuzzleRenderer.SetAsUserControlled();
        }
    }
}

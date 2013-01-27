using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TilePuzzle;


namespace ConsolePuzzleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Size: ");
            String input = Console.ReadLine();
            int size = Int32.Parse(input);
            Puzzle puzzle = new Puzzle(size);
            PuzzlePrinter printer = new PuzzlePrinter(puzzle);
            printer.PrintPuzzle();
            while (true)
            {     
                Console.WriteLine("Move Target: ");
                input = Console.ReadLine();
                if (input != null)
                {
                    if (input.Equals("quit"))
                    {
                        break;
                    }
                    Tile targetTile = printer[input];
                    if (targetTile != null)
                    {
                        Puzzle.MoveProperties moveProperties = puzzle.MakeMove(targetTile.CurrentPosition);
                        if (moveProperties.Direction != Puzzle.MoveDirection.None)
                        {
                            printer.PrintPuzzle();
                        }
                    }
                }
            }
 
        }

        class PuzzlePrinter
        {
            private readonly Puzzle puzzle;
            private readonly Dictionary<Tile,String> tileTosting = new Dictionary<Tile, string>();
            private readonly Dictionary<String, Tile> stringToTile = new Dictionary<string, Tile>(); 

            public PuzzlePrinter(Puzzle puzzle)
            {
                this.puzzle = puzzle;
                for (int i = 0; i < puzzle.Size; i++)
                {
                    for (int j = 0; j < puzzle.Size; j++)
                    {
                        TilePosition position = new TilePosition(i, j);
                        if (i == puzzle.Size - 1 && j == puzzle.Size - 1)
                        {
                            tileTosting[puzzle[position]] = "#";
                            stringToTile["#"] = puzzle[position];
                            break;
                        }
                        int tileNumber = (j + (i*puzzle.Size));
                        string numberString = tileNumber.ToString();
                        tileTosting[puzzle[position]] = numberString;
                        stringToTile[numberString] = puzzle[position];
                    }
                }
                
            }

            public void PrintPuzzle()
            {
                for (int i = 0; i <puzzle.Size; i++)
                {
                    for (int j = 0; j < puzzle.Size; j++)
                    {
                        TilePosition position = new TilePosition(i, j);
                        Console.Write(tileTosting[puzzle[position]]+"\t");
                    }
                    Console.Write(System.Environment.NewLine);
                }
            }

            public Tile this[String stringRepresentaion]
            {
                get {return stringToTile[stringRepresentaion]; }
            }
        }

    }
}

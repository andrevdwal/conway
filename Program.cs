using System;
using System.IO;
using System.Threading;

namespace Conway
{
    class Program
    {
        static void Main(string[] args)
        {
            bool[,] grid = new bool[150, 50];
            LoadPattern(grid, "patterns/glidercrashhuge.cells");

            while (true)
            {
                Console.Clear();
                DisplayGrid(grid);
                Thread.Sleep(100);
                grid = Cycle(grid);
            }
        }

        static bool[,] NewRandomGrid(int x, int y)
        {
            Random r = new Random();

            bool[,] grid = new bool[x, y];
            for (int ix = 0; ix < x; ix++)
                for (int iy = 0; iy < y; iy++)
                {
                    if (r.Next(0, 10) == 3)
                        grid[ix, iy] = true;
                }
            return grid;
        }

        static bool[,] Cycle(bool[,] grid)
        {
            bool[,] nextGrid = new bool[grid.GetLength(0), grid.GetLength(1)];

            for (int y = 0; y < nextGrid.GetLength(1); y++)
            {
                for (int x = 0; x < nextGrid.GetLength(0); x++)
                {

                    int minX = x - 1;
                    int maxX = x + 1;
                    int minY = y - 1;
                    int maxY = y + 1;
                    int onCount = 0;

                    for (int nx = minX; nx <= maxX; nx++)
                        for (int ny = minY; ny <= maxY; ny++)
                        {
                            if (nx < 0 || ny < 0)
                                continue;
                            if (nx >= nextGrid.GetLength(0) || ny >= nextGrid.GetLength(1))
                                continue;
                            if (nx == x && ny == y)
                                continue;

                            onCount += grid[nx, ny] ? 1 : 0;
                        }

                    if (grid[x, y])
                    {
                        if (onCount == 2 || onCount == 3)
                            nextGrid[x, y] = true;
                    }
                    else if (onCount == 3)
                    {
                        nextGrid[x, y] = true;
                    }
                }
            }

            return nextGrid;
        }

        static void DisplayGrid(bool[,] grid)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
                Console.Write("-");
            Console.WriteLine();

            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Console.Write("|");
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    if (grid[x, y])
                        Console.Write(".");
                    else
                        Console.Write(" ");
                }
                Console.Write("|");
                Console.WriteLine();
            }

            for (int x = 0; x < grid.GetLength(0); x++)
                Console.Write("-");
            Console.WriteLine();
        }

        static void LoadPattern(bool[,] grid, String patternFilePath)
        {
            string[] lines = File.ReadAllLines(patternFilePath);
            int row = -1;
            foreach (string line in lines)
            {
                if (line.StartsWith("!"))
                    continue;
                row++;
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == 'O')
                        grid[i, row] = true;
                }
            }
        }
    }
}

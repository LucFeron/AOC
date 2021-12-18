using System.Text;
using System.Text.RegularExpressions;

namespace Aoc2021
{
    static class Day17
    {

        //static
        static string input = @"target area: x=20..30, y=-10..-5";
  static string input2 = @"target area: x=117..164, y=-140..-89";


        public static void Execute()
        {
            Regex rx = new Regex(@"target area: x=(-?\d*).{2}(-?\d*).*=(-?\d*).{2}(-?\d*)");
            string inp = input2;
            int maxX = 0;
            int maxY = 0;
            int minX = 0;
            int minY = 0;
            MatchCollection matches = rx.Matches(inp);
            if (matches.Any())
            {
                foreach (Match match in matches)
                {
                    minX = Math.Min(Convert.ToInt32(match.Groups[1].Value), Convert.ToInt32(match.Groups[2].Value));
                    maxX = Math.Max(Convert.ToInt32(match.Groups[1].Value), Convert.ToInt32(match.Groups[2].Value));
                    minY = Math.Min(Convert.ToInt32(match.Groups[3].Value), Convert.ToInt32(match.Groups[4].Value));
                    maxY = Math.Max(Convert.ToInt32(match.Groups[3].Value), Convert.ToInt32(match.Groups[4].Value));
                }
            }
          /*  Console.WriteLine($"{IsInArea(minX, maxX, minY, maxY, 0, 0)}");
            Console.WriteLine($"{IsInArea(minX, maxX, minY, maxY, 100, 100)}");
            Console.WriteLine($"{IsInArea(minX, maxX, minY, maxY, 25, -7)}");
            Console.WriteLine($"{IsInArea(minX, maxX, minY, maxY, 20, 0)}");
            Console.WriteLine($"{IsInArea(minX, maxX, minY, maxY, 0, -6)}");
*/
            int bestx = 0;
            int besty = 0;
            int bestheight = 0;
            List<(int,int)> pos = new List<(int, int)>();
            for (int i = -maxX ; i < maxX +3; i++)
            {
                for (int j = minY*2; j < Math.Abs( minY)*2; j++)
                {
                    int s = 0;
                    Probe p = new Probe(i,j);
                    while(s < 1000)
                    {
                        s++;
                        p.Step();
                        if(IsInArea(minX, maxX, minY, maxY,p.X,p.Y))
                        {
                            Console.WriteLine($"{i},{j}");
                            pos.Add((i,j));
                            break;
                        }
                    }
                    if (IsInArea(minX, maxX, minY, maxY,p.X,p.Y) && p.MaxHeightReached > bestheight)
                    {
                        bestx = i;
                        besty = j;
                        bestheight = p.MaxHeightReached;
                    }
                }
                
            }

        }

        private static bool IsInArea(int minx, int maxx, int miny, int maxy, int x, int y)
        {
            return minx <= x && x <= maxx && miny <= y && y <= maxy;
        }
        public class Probe
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int SpeedX { get; set; }
            public int SpeedY { get; set; }
            public int MaxHeightReached { get; set; }     
            public void Step()
            {
                X += SpeedX;
                Y += SpeedY;
                if (MaxHeightReached < Y) MaxHeightReached = Y;
                if(SpeedX !=0)
                {
                    if(SpeedX > 0) SpeedX--; else SpeedX++;
                }
                SpeedY--;
            }
            public Probe(int initialSpeedX, int initialSpeedY)
            {
                SpeedX = initialSpeedX;
                SpeedY = initialSpeedY;
            }
        }

    }
}
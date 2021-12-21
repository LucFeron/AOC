using System.Collections;
using System.Text;

namespace Aoc2021
{
    static class Day21
    {

        //static
        static string input = @"Player 1 starting position: 4
Player 2 starting position: 8";
        static string input2 = @"Player 1 starting position: 7
Player 2 starting position: 5";
        static Player P1, P2;
        static List<(int steps, int count)> DiceRoles = new List<(int steps, int count)>();
        public static void Execute()
        {
            string inp = input2;
            var lines = inp.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            P1 = new Player(lines[0]);
            P2 = new Player(lines[1]);
            //Step1();
            DiceRoles = GetDiceRoles();
            var result = Play(P1.CurrentPosition, 0, P2.CurrentPosition, 0, true);
        }
        private static (long p1wins, long p2wins) Play(int p1pos, int p1score, int p2pos, int p2score, bool p1)
        {
            (long p1wins, long p2wins) res = (0,0);
            foreach (var dr in DiceRoles)
            {
                int p1posc = p1pos,  p1scorec = p1score,  p2posc = p2pos,  p2scorec = p2score;
                bool end = false;
                if (p1)
                {
                    p1posc += dr.steps;
                    if (p1posc > 10) p1posc -= 10;
                    p1scorec += p1posc;
                    if (p1scorec >= 21)
                    {
                        res.p1wins += dr.count;
                        end = true;
                    }
                }
                else
                {
                    p2posc += dr.steps;
                    if (p2posc > 10) p2posc -= 10;
                    p2scorec += p2posc;
                    if (p2scorec >= 21)
                    {
                        res.p2wins += dr.count;
                        end =true;
                    }
                }
                if(!end)
                {
                    var x = Play(p1posc,p1scorec, p2posc,p2scorec,!p1);
                    res.p1wins +=x.p1wins * dr.count;
                    res.p2wins += x.p2wins * dr.count;
                }
            }
            return res;
        }
        private static List<(int steps, int count)> GetDiceRoles()
        {
            List<int> c = new List<int>();
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    for (int k = 1; k <= 3; k++)
                    {
                        c.Add(i + j + k);
                    }
                }
            }
            return c.GroupBy(c => c).Select(c => (c.Key, c.Count())).ToList();
           // return c.Select(c => (c,1)).ToList();
        }

        private static void Step1()
        {
            int diceRolls = 0;
            int totalRolls = 0;
            Player p = P1;
            string log = "Play 1: ";
            while (P1.Score < 1000 && P2.Score < 1000)
            {

                for (int i = 0; i < 3; i++)
                {
                    diceRolls++;
                    totalRolls++;

                    if (diceRolls == 101)
                    {
                        diceRolls = 1;
                    }
                    log += $" {diceRolls} ";
                    p.CurrentPosition += diceRolls;
                }
                while (p.CurrentPosition > 10)
                {
                    p.CurrentPosition -= 10;
                }
                log += $" moves to {p.CurrentPosition} ";
                if (p.CurrentPosition == 0)
                {

                }
                p.Score += p.CurrentPosition;
                log += $" score {p.Score}";
                Console.WriteLine(log);
                if (p == P1)
                {
                    p = P2;
                    log = "Play 2: ";
                }
                else
                {
                    p = P1;
                    log = "Play 1: ";
                }
            }
            if (P1.Score > P2.Score)
            {
                p = P2;
            }
            else
            {
                p = P1;
            }
            Console.WriteLine($"{p.Score * totalRolls}");
        }
    }
    public class Player
    {
        public int CurrentPosition { get; set; }
        public int Score { get; set; }

        public Player(string input)
        {
            CurrentPosition = int.Parse(input.Substring(28));
            Score = 0;
        }
    }
}
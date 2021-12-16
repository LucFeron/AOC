using System.Text;

namespace Aoc2021
{

    static class Day14
    {
        static string input = @"NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C";
        static string input2 = @"FSKBVOSKPCPPHVOPVFPC

BV -> O
OS -> P
KP -> P
VK -> S
FS -> C
OK -> P
KC -> S
HV -> F
HC -> K
PF -> N
NK -> F
SC -> V
CO -> K
PO -> F
FB -> P
CN -> K
KF -> N
NH -> S
SF -> P
HP -> P
NP -> F
OV -> O
OP -> P
HH -> C
FP -> P
CS -> O
SK -> O
NS -> F
SN -> S
SP -> H
BH -> B
NO -> O
CB -> N
FO -> N
NC -> C
VF -> N
CK -> C
PC -> H
BP -> B
NF -> O
BB -> C
VN -> K
OH -> K
CH -> F
VB -> N
HO -> P
FH -> K
PK -> H
CC -> B
VH -> B
BF -> N
KS -> V
PV -> B
CP -> N
PB -> S
VP -> V
BO -> B
HS -> H
BS -> F
ON -> B
HB -> K
KH -> B
PP -> H
BN -> C
BC -> F
KV -> K
VO -> P
SO -> V
OF -> O
BK -> S
PH -> V
SV -> F
CV -> H
OB -> N
SS -> H
VV -> B
OO -> V
CF -> H
KB -> F
NV -> B
FV -> V
HK -> P
VS -> P
FF -> P
HN -> N
FN -> F
OC -> K
SH -> V
KO -> C
HF -> B
PN -> N
SB -> F
VC -> B
FK -> S
KK -> N
FC -> F
NN -> P
NB -> V
PS -> S
KN -> S";

        public static void Execute()
        {
            string inp = input2;
          //  StepM1(inp);
            StepM2(inp);
        }
        private static void StepM2(string inp)
        {
            Dictionary<string, (string p1, string p2)> polymers = new Dictionary<string, (string p1, string p2)>();
            Dictionary<string, long> start = GetPolymers2(inp, polymers);
        string LastStart = inp.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0].Last().ToString();
            for (int i = 0; i < 40; i++)
            {
                start = blend2(start, polymers);
            }
            var x = start.GroupBy(p => p.Key.Substring(0,1)).Select(e => new { key = e.Key, aantal = e.Sum(e => (long)e.Value) + (long)(e.Key == LastStart ?1:0) }).OrderBy(e => e.aantal);
            var result = x.Last().aantal - x.First().aantal;
            Console.WriteLine(result);
        }

        private static Dictionary<string, long> blend2(Dictionary<string, long> start, Dictionary<string, (string p1, string p2)> polymers)
        {
            Dictionary<string, long> res = new Dictionary<string, long>();
            foreach (var i in start)
            {
                AddIfNotExistsIncrease(res, polymers[i.Key].p1 , i.Value);
                AddIfNotExistsIncrease(res, polymers[i.Key].p2 , i.Value);
            }
            return res;
        }

        private static Dictionary<string, long> GetPolymers2(string inp, Dictionary<string, (string p1, string p2)> polymers)
        {
            Dictionary<string, long> start = new Dictionary<string, long>();
            var lines = inp.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < (lines[0].Length - 1); i++)
            {

                var pp = lines[0].Substring(i, 2);
                AddIfNotExistsIncrease(start, pp,1);
            }

            foreach (var l in lines.Where(li => li.Contains("->")))
            {
                polymers.Add(l.Substring(0, 2), (l.Substring(0, 1) + l.Substring(6, 1), l.Substring(6, 1) + l.Substring(1, 1)));
            }

            return start;
        }

        private static void AddIfNotExistsIncrease(Dictionary<string, long> start, string pp, long value =1)
        {
            if (start.ContainsKey(pp))
            {
                start[pp]+=value;
            }
            else
            {
                start.Add(pp, value);
            }
        }

        private static void StepM1(string inp)
        {
            Dictionary<string, string> polymers = new Dictionary<string, string>();
            string start = "";
            start = GetPolymers(inp, polymers);
            string polymer = start;
            for (int i = 0; i < 10; i++)
            {
                polymer = blend(polymer, polymers);
                Console.WriteLine($"Stap: {i} - {polymer.Length}");
            }
            var x = polymer.GroupBy(p => p).Select(e => new { key = e.Key.ToString(), aantal = e.Count() }).OrderBy(e => e.aantal);
            var result = x.Last().aantal - x.First().aantal;
            Console.WriteLine(result);
        }

        private static string blend(string polymer, Dictionary<string, string> polymers)
        {
            StringBuilder res = new StringBuilder();
            string pp;
            for (int i = 0; i < (polymer.Length - 1); i++)
            {

                pp = polymer.Substring(i, 2);
                if (polymers.ContainsKey(pp))
                {
                    res.Append(polymers[pp].Substring(i == 0 ? 0 : 1));
                }
                else
                {
                    res.Append(pp.Substring(i == 0 ? 0 : 1));
                }
            }
            return res.ToString();
        }

        private static string GetPolymers(string inp, Dictionary<string, string> polymers)
        {
            string start;
            var lines = inp.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            start = lines[0];
            foreach (var l in lines.Where(li => li.Contains("->")))
            {
                polymers.Add(l.Substring(0, 2), l.Substring(0, 1) + l.Substring(6, 1) + l.Substring(1, 1));
            }

            return start;
        }
    }
}
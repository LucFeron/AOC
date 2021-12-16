using System.Text;

namespace Aoc2021
{
    public static class Day16
    {
        static string inputt0 = "D2FE28";

        static string inputt1 = "38006F45291200";
        static string inputt2 = "EE00D40C823060";
        static string inputt3 = "8A004A801A8002F478";
        static string inputt4 = "620080001611562C8802118E34";
        static string inputt5 = "C0015000016115A2E0802F182340";
        static string inputt6 = "A0016C880162017C3686B18A3D4780";
        static string input2 = "005532447836402684AC7AB3801A800021F0961146B1007A1147C89440294D005C12D2A7BC992D3F4E50C72CDF29EECFD0ACD5CC016962099194002CE31C5D3005F401296CAF4B656A46B2DE5588015C913D8653A3A001B9C3C93D7AC672F4FF78C136532E6E0007FCDFA975A3004B002E69EC4FD2D32CDF3FFDDAF01C91FCA7B41700263818025A00B48DEF3DFB89D26C3281A200F4C5AF57582527BC1890042DE00B4B324DBA4FAFCE473EF7CC0802B59DA28580212B3BD99A78C8004EC300761DC128EE40086C4F8E50F0C01882D0FE29900A01C01C2C96F38FCBB3E18C96F38FCBB3E1BCC57E2AA0154EDEC45096712A64A2520C6401A9E80213D98562653D98562612A06C0143CB03C529B5D9FD87CBA64F88CA439EC5BB299718023800D3CE7A935F9EA884F5EFAE9E10079125AF39E80212330F93EC7DAD7A9D5C4002A24A806A0062019B6600730173640575A0147C60070011FCA005000F7080385800CBEE006800A30C023520077A401840004BAC00D7A001FB31AAD10CC016923DA00686769E019DA780D0022394854167C2A56FB75200D33801F696D5B922F98B68B64E02460054CAE900949401BB80021D0562344E00042A16C6B8253000600B78020200E44386B068401E8391661C4E14B804D3B6B27CFE98E73BCF55B65762C402768803F09620419100661EC2A8CE0008741A83917CC024970D9E718DD341640259D80200008444D8F713C401D88310E2EC9F20F3330E059009118019A8803F12A0FC6E1006E3744183D27312200D4AC01693F5A131C93F5A131C970D6008867379CD3221289B13D402492EE377917CACEDB3695AD61C939C7C10082597E3740E857396499EA31980293F4FD206B40123CEE27CFB64D5E57B9ACC7F993D9495444001C998E66B50896B0B90050D34DF3295289128E73070E00A4E7A389224323005E801049351952694C000";
        static string inputt21 = "C200B40A82";
        public static void Execute()
        {
            string inp = packet.StringToBinary(input2);
            packet p = DecodePackets(inp, out _);
            //int sum = GetVersionSum(p);
            //Console.WriteLine(sum.ToString());
            Console.WriteLine(p.GetValue()); 
        }

        private static int GetVersionSum(packet p)
        {
            int r = p.Version;
            foreach (var sp in p.subpackets)
            {
                r += GetVersionSum(sp);
            }
            return r;
        }

        private static packet DecodePackets(string inp, out string rest)
        {
            rest = "";
            string removed;
            packet r = new packet();
            //Eerste 3 altijd versie
            //volgende 3 type
            //als type = 4 dan lees set van 5. rechtse 4 zijn waarde, linkse is 0=stop.
            //anders bit 7 = 0 dan: volgende 15 bits zijn lengte van sub packetes
            //anders bit 7 = 1 dan: volgende 11 bits aantal packetes
            inp = RemoveFrom(inp, 3, out removed);
            r.Version = packet.ToInt(removed);
            inp = RemoveFrom(inp, 3, out removed);
            r.Type = packet.ToInt(removed);
            switch (r.Type)
            {
                case 4:
                    ProcesType4(ref inp, r);
                    break;
                default:
                    GetOperatorTypes(ref inp, r);

                    break;
            }
            rest = inp;
            return r;
        }

        private static void GetOperatorTypes(ref string inp, packet r)
        {
            string removed;
            inp = RemoveFrom(inp, 1, out removed);
            if (removed == "0")
            {
                r.LengteType = packet.lengthType.lenght;
            }
            else if (removed == "1")
            {
                r.LengteType = packet.lengthType.count;
            }
            switch (r.LengteType)
            {
                case packet.lengthType.lenght:
                    inp = RemoveFrom(inp, 15, out removed);
                    int packetlength = packet.ToInt(removed);
                    inp = RemoveFrom(inp, packetlength, out removed);
                    while (removed.Length > 6)
                    {
                        string tmp = "";
                        r.subpackets.Add(DecodePackets(removed, out tmp));
                        removed = tmp;
                    }

                    break;
                case packet.lengthType.count:
                    inp = RemoveFrom(inp, 11, out removed);
                    int packetcount = packet.ToInt(removed);
                    for (int i = 0; i < packetcount; i++)
                    {
                        string tmp = "";
                        r.subpackets.Add(DecodePackets(inp, out tmp));
                        inp = tmp;
                    }
                    break;
            }
        }

        private static void ProcesType4(ref string inp, packet r)
        {
            string removed = "";
            bool stop = false;
            string tmp = "";
            while (!stop)
            {
                inp = RemoveFrom(inp, 5, out removed);
                if (removed.Substring(0, 1) == "0")
                {
                    stop = true;
                }
                tmp += removed.Substring(1);
            }
            r.LiteralyValue = packet.ToLong(tmp);
        }

        private static string RemoveFrom(string inp, int length, out string removed)
        {
            string res;
            res = inp.Substring(length);
            removed = inp.Substring(0, length);
            return res;

        }
    }
    class packet
    {
        public enum lengthType
        {
            none,
            lenght,
            count
        }
        public static Dictionary<string, string> decode = new Dictionary<string, string>()
        {
{"0","0000"},
{"1","0001"},
{"2","0010"},
{"3","0011"},
{"4","0100"},
{"5","0101"},
{"6","0110"},
{"7","0111"},
{"8","1000"},
{"9","1001"},
{"A","1010"},
{"B","1011"},
{"C","1100"},
{"D","1101"},
{"E","1110"},
{"F","1111"}
        };
        public static string StringToBinary(string data)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in data)
            {
                sb.Append(decode[c.ToString()]);

            }
            return sb.ToString();
        }
        public static string BinaryToString(string data)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(decode.GetValueOrDefault(data.ToString().PadLeft(4, '0')));
            return sb.ToString();
        }
        public static UInt64 ToLong(string inp)
        {
            return Convert.ToUInt64(inp, 2);
        }
        public static int ToInt(string inp)
        {
            return Convert.ToInt32(inp, 2);
        }
        public int Version { get; set; }
        public int Type { get; set; }

        public lengthType LengteType { get; set; }
        public List<packet> subpackets = new List<packet>();
        public UInt64 LiteralyValue { get; set; }
        public UInt64 GetValue()
        {
            UInt64 res = 0;
            switch (Type)
            {
                case 0:
                    foreach(var sp in subpackets)
                    {
                        res += sp.GetValue();
                    }
                    break;
                case 1:
                    res = 1;
                    foreach (var sp in subpackets)
                    {
                        res *= sp.GetValue();
                    }
                    break;
                case 2:
                    res = (ulong)subpackets.Min(s => Convert.ToInt32(s.GetValue()));
                    break;
                case 3:
                    res = (ulong)subpackets.Max(s => Convert.ToInt32(s.GetValue()));
                    break;
                case 4:
                    res = LiteralyValue;
                    break;
                case 5:
                    res = (ulong)(subpackets[0].GetValue() >subpackets[1].GetValue() ? 1:0);
                    break;
                case 6:
                    res = (ulong)(subpackets[0].GetValue()<subpackets[1].GetValue() ? 1:0);

                    break;
                case 7:
                    res = (ulong)(subpackets[0].GetValue() ==subpackets[1].GetValue() ? 1:0);

                    break;
            }

            return res;
        }
    }
}
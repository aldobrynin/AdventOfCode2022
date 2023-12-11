using System.Text;

namespace AoC2021.Day16;

public partial class Day16
{
    public static void Solve(IEnumerable<string> input)
    {
        var rawPacket = input.Single()
            .Select(c => Convert.ToInt32(c.ToString(), 16))
            .Select(c => Convert.ToString(c, 2))
            .Select(x => x.PadLeft(4, '0'))
            .StringJoin(string.Empty);

        var packet = ParsePacket(rawPacket);
        packet.VersionsSum().Part1();
        packet.GetValue().Part2();
    }

    private static Packet ParsePacket(string s)
    {
        var offset = 0;
        return ParseInternal(s, ref offset);
    }

    private static Packet ParseInternal(string rawPacket, ref int offset)
    {
        var packet = new Packet(ReadNumber(rawPacket, ref offset, 3), ReadNumber(rawPacket, ref offset, 3));
        if (packet.TypeId == 4)
            packet.Value = ReadLiteralValue(rawPacket, ref offset);
        else
            packet.SubPackets = ReadSubPackets(rawPacket, ref offset);
        return packet;
    }

    private static IReadOnlyList<Packet> ReadSubPackets(string rawPacket, ref int offset)
    {
        var result = new List<Packet>();
        var lengthTypeId = ReadNumber(rawPacket, ref offset, 1);
        switch (lengthTypeId)
        {
            case 0:
            {
                var subPacketsTotalSize = ReadNumber(rawPacket, ref offset, 15);
                var endOfSubPackets = offset + subPacketsTotalSize;
                while (offset < endOfSubPackets) result.Add(ParseInternal(rawPacket, ref offset));
                break;
            }
            case 1:
                var subPacketsCount = ReadNumber(rawPacket, ref offset, 11);
                while (subPacketsCount-- > 0) result.Add(ParseInternal(rawPacket, ref offset));
                break;
            default:
                throw new ArgumentOutOfRangeException($"Unexpected length type id: {lengthTypeId}");
        }

        return result;
    }

    private static int ReadNumber(string source, ref int offset, int length)
    {
        var result = Convert.ToInt32(source.Substring(offset, length), 2);
        offset += length;
        return result;
    }

    private static long ReadLiteralValue(string s, ref int offset)
    {
        var result = 0L;
        while (offset < s.Length)
        {
            var isLast = s[offset++] == '0';
            result = result * 16 + ReadNumber(s, ref offset, 4);
            if (isLast)
                break;
        }

        return result;
    }

    private record Packet(int Version, int TypeId)
    {
        public long Value { get; set; }
        public IReadOnlyList<Packet> SubPackets { get; set; } = Array.Empty<Packet>();

        public long VersionsSum() => Version + SubPackets.Sum(x => x.VersionsSum());

        public long GetValue()
        {
            return TypeId switch
            {
                4 => Value,
                0 => SubPackets.Sum(x => x.GetValue()),
                1 => SubPackets.Product(x => x.GetValue()),
                2 => SubPackets.Min(x => x.GetValue()),
                3 => SubPackets.Max(x => x.GetValue()),
                5 => SubPackets[0].GetValue() > SubPackets[1].GetValue() ? 1 : 0,
                6 => SubPackets[0].GetValue() < SubPackets[1].GetValue() ? 1 : 0,
                7 => SubPackets[0].GetValue() == SubPackets[1].GetValue() ? 1 : 0,
                _ => throw new ArgumentOutOfRangeException(nameof(TypeId), TypeId, "Unexpected type id")
            };
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"[Version: {Version}, ");
            if (TypeId == 4)
                stringBuilder.Append($"Value: {Value}]");
            else
                stringBuilder.Append($"SubPackets: <{SubPackets.StringJoin()}>]");

            return stringBuilder.ToString();
        }
    }
}
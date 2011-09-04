using System.IO;

namespace SilkroadSecurityApi
{
    internal class PacketReader : System.IO.BinaryReader
    {
        byte[] m_input;

        //don't care about solving problem CA2000
        public PacketReader(byte[] input)
            : base(new MemoryStream(input, false))
        {
            m_input = input;
        }

        //don't need about solving problem CA2000
        public PacketReader(byte[] input, int index, int count)
            : base(new MemoryStream(input, index, count, false))
        {
            m_input = input;
        }
    }
}

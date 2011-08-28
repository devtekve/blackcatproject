using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proxy
{
    class Packet
    {
        private byte[] _buffer;
        private int _size;

        public Packet(byte[] buffer, int size)
        {
            _buffer = buffer;
            _size = size;
        }

        public int Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
            }
        }

        public byte[] Buffer
        {
            get
            {
                return _buffer;
            }
            set
            {
                _buffer = value;
            }
        }
    }
}

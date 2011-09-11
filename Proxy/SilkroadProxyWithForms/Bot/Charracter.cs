using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SilkroadProxyWithForms;
using SilkroadSecurityApi;

namespace Proxy
{
    class Charracter
    {
        private MainForm _mainForm;
        private string _charname;
        private uint _maxHP;
        private uint _maxMP;
        private uint _id;
        private uint _currentHP;
        private uint _currentMP;
        private Packet _0x3013;
        private int _xCoord;
        private int _yCoord;

        public int XCoord
        {
            get
            {
                return _xCoord;
            }
            set
            {
                _xCoord = value;
            }
        }

        public int YCoord
        {
            get
            {
                return _yCoord;
            }
            set
            {
                _yCoord = value;
            }
        }


        public Packet Opcode3013
        {
            get
            {
                return _0x3013;
            }
            set
            {
                _0x3013 = value;
            }
        }

        public uint Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public uint MaxHP
        {
            get
            {
                return _maxHP;
            }
            set
            {
                _maxHP = value;
            } 
        }

        public uint MaxMP
        {
            get
            {
                return _maxMP;
            }
            set
            {
                _maxMP = value;
            }
        }

        public uint CurrentHP
        {
            get
            {
                return _currentHP;
            }
            set
            {
                _currentHP = value;
            }
        }

        public uint CurrentMP
        {
            get
            {
                return _currentMP;
            }
            set
            {
                _currentMP = value;
            }
        }

        public string CharName
        {
            get
            {
                return _charname;
            }
            set
            {
                _charname = value;
            }
        }

        public Charracter(MainForm mainForm)
        {
            // TODO: Complete member initialization
            this._mainForm = mainForm;
        }
    }
}

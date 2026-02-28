using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Data
{
    public class PlayerSkillData0
    {
        public PlayerSkillData0(byte[] data)
        {
            Unknown0 = data[0x0];
            Unknown1 = data[0x1];
            Unknown2 = data[0x2];
            Unknown3 = data[0x3];
            Unknown4 = data[0x4];
            Unknown5 = data[0x5];
            Unknown6 = data[0x6];
            Unknown7 = data[0x7];
            Unknown8 = data[0x8];
            Unknown9 = data[0x9];
            UnknownA = data[0xA];
            UnknownB = data[0xB];
        }

        public byte Unknown0;
        public byte Unknown1;
        public byte Unknown2;
        public byte Unknown3;
        public byte Unknown4;
        public byte Unknown5;
        public byte Unknown6;
        public byte Unknown7;
        public byte Unknown8;
        public byte Unknown9;
        public byte UnknownA;
        public byte UnknownB;
    }
}

namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Data
{
    public class Skill2Effect
    {
        public Skill2Effect(byte[] data)
        {
            Unknown00 = BitConverter.ToUInt16(data, 0);
            Unknown2 = data[0x2];
            Unknown3 = data[0x3];
            Unknown4 = data[0x4];
            Unknown5 = data[0x5];
            Unknown6 = data[0x6];
            Unknown7 = data[0x7];
            Unknown8 = data[0x8];
            Unknown9 = data[0x9];
        }


        public ushort Unknown00;
        public byte Unknown2;
        public byte Unknown3;
        public byte Unknown4;
        public byte Unknown5;
        public byte Unknown6;
        public byte Unknown7;
        public byte Unknown8;
        public byte Unknown9;
    }
}

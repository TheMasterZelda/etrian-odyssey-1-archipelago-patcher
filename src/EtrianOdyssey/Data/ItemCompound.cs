namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Data
{
    public class ItemCompound
    {
        public ItemCompound(byte[] data, EtrianString[] nameTable)
        {
            item_id = BitConverter.ToUInt16(data, 0);
            name = nameTable[item_id - 1];
            unknown_02 = BitConverter.ToUInt16(data, 2);
            if (unknown_02 != 0)
                name2 = nameTable[unknown_02 - 1];
            unknown_04 = BitConverter.ToUInt16(data, 4);
            if (unknown_04 != 0)
                name4 = nameTable[unknown_04 - 1];
            unknown_06 = BitConverter.ToUInt16(data, 6);
            if (unknown_06 != 0)
                name6 = nameTable[unknown_06 - 1];
            unknown_08 = BitConverter.ToUInt16(data, 8);
            if (unknown_08 != 0)
                name8 = nameTable[unknown_08 - 1];
            unknown_0A = BitConverter.ToUInt16(data, 0xA);
            if (unknown_0A != 0)
                nameA = nameTable[unknown_0A - 1];
            unknown_0C = data[0xC];
            unknown_0D = data[0xD];
            unknown_0E = data[0xE];
        }


        public override string ToString()
        {
            return name.StringValue;
        }

        public ushort item_id; // 00-01
        public EtrianString name;
        public ushort unknown_02; // 02-03
        public EtrianString name2;
        public ushort unknown_04; // 04-05
        public EtrianString name4;
        public ushort unknown_06; // 06-07
        public EtrianString name6;
        public ushort unknown_08; // 08-09
        public EtrianString name8;
        public ushort unknown_0A; // 0A-0B
        public EtrianString nameA;
        public byte unknown_0C; // 0C
        public byte unknown_0D; // 0D
        public byte unknown_0E; // 0E
    }
}

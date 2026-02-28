namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Data
{
    public class ItemOther
    {
        public ItemOther(byte[] data, EtrianString[] nameTable)
        {
            item_id = BitConverter.ToUInt16(data, 0);
            name = nameTable[item_id - 1];
            skill_effect_id = BitConverter.ToUInt16(data, 2);
            skill_effect_level = BitConverter.ToUInt16(data, 4);
            hp_restore = BitConverter.ToUInt16(data, 6);
            sp_restore = BitConverter.ToUInt16(data, 8);
            bp_restore = BitConverter.ToUInt16(data, 0xA);
            unknown_0E = data[0xE];
            unknown_0F = data[0xF];
            buy_price = BitConverter.ToUInt32(data, 0x10);
            sell_price = BitConverter.ToUInt32(data, 0x14);
        }

        public override string ToString()
        {
            return name.StringValue;
        }

        public ushort item_id; // 00-01
        public EtrianString name;
        public ushort skill_effect_id; // 02-03
        public ushort skill_effect_level; // 04-05
        public ushort hp_restore; // 06-07
        public ushort sp_restore; // 08-09
        public ushort bp_restore; // 0a-0b
        public ushort effect_type; // 0c-0d
        public byte unknown_0E; // 0E
        public byte unknown_0F; // 0F
        public uint buy_price; // 10-13
        public uint sell_price; // 14-17
    }
}

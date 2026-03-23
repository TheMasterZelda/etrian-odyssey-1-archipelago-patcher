namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Data
{
    public class ItemCompound
    {
        public ItemCompound(byte[] data, EtrianString[] nameTable)
        {
            item_id = BitConverter.ToUInt16(data, 0);
            name = nameTable[item_id - 1];
            material_1_item_id = BitConverter.ToUInt16(data, 2);
            if (material_1_item_id != 0)
                material_1_item_name = nameTable[material_1_item_id - 1];
            material_2_item_id = BitConverter.ToUInt16(data, 4);
            if (material_2_item_id != 0)
                material_2_item_name = nameTable[material_2_item_id - 1];
            material_3_item_id = BitConverter.ToUInt16(data, 6);
            if (material_3_item_id != 0)
                material_3_item_name = nameTable[material_3_item_id - 1];
            material_4_item_id = BitConverter.ToUInt16(data, 8);
            if (material_4_item_id != 0)
                material_4_item_name = nameTable[material_4_item_id - 1];
            material_5_item_id = BitConverter.ToUInt16(data, 0xA);
            if (material_5_item_id != 0)
                material_5_item_name = nameTable[material_5_item_id - 1];
            material_1_count = data[0xC];
            material_2_count = data[0xD];
            material_3_count = data[0xE];
        }


        public override string ToString()
        {
            return name.StringValue;
        }

        public ushort item_id; // 00-01
        public EtrianString name;
        public ushort material_1_item_id; // 02-03
        public EtrianString material_1_item_name;
        public ushort material_2_item_id; // 04-05
        public EtrianString material_2_item_name;
        public ushort material_3_item_id; // 06-07
        public EtrianString material_3_item_name;

        // NEVER USED
        public ushort material_4_item_id; // 08-09
        public EtrianString material_4_item_name;
        public ushort material_5_item_id; // 0A-0B
        public EtrianString material_5_item_name;
        // ===========

        public byte material_1_count; // 0C
        public byte material_2_count; // 0D
        public byte material_3_count; // 0E
    }
}

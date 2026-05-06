namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Data
{
    public class BarQuestData
    {
        private byte[] data;

        public BarQuestData(byte[] data, EtrianString name)
        {
            this.data = data;

            quest_id = BitConverter.ToUInt16(data, 0);

            floor_requirement = BitConverter.ToInt16(data, 2);
            level_requirement = BitConverter.ToInt16(data, 4);
            flag_requirement = BitConverter.ToInt16(data, 6);

            accepted_flag = BitConverter.ToUInt16(data, 0xE); // 0xE-0xF
            ready_for_report_flag = BitConverter.ToUInt16(data, 0x10); // 0x10-0x11
            completed_flag = BitConverter.ToUInt16(data, 0x12); // 0x12-0x13



            Name = name;
        }

        public EtrianString Name;
        public ushort quest_id;

        public short floor_requirement; // 0x2-0x3

        public short level_requirement; // 0x4-0x5
        public short flag_requirement; // 0x6-0x7

        // 0x8-0x9= Always 0xFFFF.
        // 0xA-0xD= Always 0x00.

        public ushort accepted_flag; // 0xE-0xF
        public ushort ready_for_report_flag; // 0x10-0x11
        public ushort completed_flag; // 0x12-0x13
    }
}

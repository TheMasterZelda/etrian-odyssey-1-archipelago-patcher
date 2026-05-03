namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Data
{
    public class BarQuestData
    {
        private byte[] data;

        public BarQuestData(byte[] data, EtrianString name)
        {
            this.data = data;

            quest_id = BitConverter.ToUInt16(data, 0);


            Name = name;
        }

        public EtrianString Name;
        public ushort quest_id;
    }
}

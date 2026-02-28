namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Data
{
    public class EncounterGroup
    {
        public EncounterGroup(byte[] data, ushort group_id, Dictionary<ushort, EncounterData> encounters = null)
        {
            GroupID = group_id;
            Background = BitConverter.ToUInt32(data, 0);
            Music = BitConverter.ToUInt32(data, 4);

            EncounterID1 = BitConverter.ToUInt16(data, 8);
            EncounterChance1 = BitConverter.ToUInt16(data, 0xA);
            EncounterID2 = BitConverter.ToUInt16(data, 0xC);
            EncounterChance2 = BitConverter.ToUInt16(data, 0xE);
            EncounterID3 = BitConverter.ToUInt16(data, 0x10);
            EncounterChance3 = BitConverter.ToUInt16(data, 0x12);

            Encounters = new EncounterData[3];

            if (encounters == null)
                return;

            if (EncounterID1 != 0)
                Encounters[0] = encounters[EncounterID1];
            if (EncounterID2 != 0)
                Encounters[1] = encounters[EncounterID2];
            if (EncounterID3 != 0)
                Encounters[2] = encounters[EncounterID3];
        }

        public override string ToString()
        {
            return string.Join(',', Encounters.Where(w => w != null).Select(s => s?.ToString()));
        }

        public ushort GroupID;
        public uint Background;
        public uint Music;
        public ushort EncounterID1;
        public ushort EncounterChance1;
        public ushort EncounterID2;
        public ushort EncounterChance2;
        public ushort EncounterID3;
        public ushort EncounterChance3;

        public EncounterData[] Encounters;
    }
}

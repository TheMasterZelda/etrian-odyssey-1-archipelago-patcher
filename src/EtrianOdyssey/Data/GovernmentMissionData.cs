namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Data
{
    public class GovernmentMissionData
    {
        public GovernmentMissionData(byte[] data, EtrianString name)
        {
            mission_id = BitConverter.ToUInt16(data, 0);
            Name = name;
            unknown_06 = BitConverter.ToUInt16(data, 6);
            unknown_0E = BitConverter.ToUInt16(data, 0x0E);
            unknown_10 = BitConverter.ToUInt16(data, 0x10);
            MissionResultsReportedFlagID = BitConverter.ToUInt16(data, 0x12);
        }

        public override string ToString()
        {
            return Name.StringValue;
        }

        public ushort mission_id;
        public ushort unknown_06;
        public ushort unknown_0E;
        public ushort unknown_10;
        public ushort MissionResultsReportedFlagID;
        public EtrianString Name;
    }
}

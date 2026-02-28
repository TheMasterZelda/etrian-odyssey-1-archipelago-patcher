namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Data
{
    public class Class2Skill
    {
        public Class2Skill(byte[] data)
        {
            SkillID = BitConverter.ToUInt16(data, 0);
            Unknown02 = BitConverter.ToUInt16(data, 2);
            RequiredSkillID1 = BitConverter.ToUInt16(data, 4);
            RequiredSkillLevel1 = BitConverter.ToUInt16(data, 6);
            RequiredSkillID2 = BitConverter.ToUInt16(data, 8);
            RequiredSkillLevel2 = BitConverter.ToUInt16(data, 0xA);
        }

        public ushort SkillID;
        public ushort Unknown02;
        public ushort RequiredSkillID1;
        public ushort RequiredSkillLevel1;
        public ushort RequiredSkillID2;
        public ushort RequiredSkillLevel2;
    }
}

using System.Xml.Linq;

namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Data
{
    public enum SkillValueType : ushort
    {
        TP_Cost = 0x01,
        Damage_Modifier = 0x02, // Power.
        Skill_Speed = 0x03,
        Accuracy = 0x04,
        ActivationRate = 0x05,
        AllSlashNumberOfHit = 0x06,
        NumberOfHit = 0x0C, // Multihit, Midareba and Suicide.
        GatheringSkill = 0x0D,
        StepCount = 0x0E,
        Mastery = 0x0F, // validate.
        Atk_Up = 0x10,

        AilmentRate = 0x12, // ?

        Adjacent_Attack_Power = 0x16, // Bait and Tornado.
        Unknown17 = 0x17, // Used by Ambush and

        Max_HP_Up = 0x32,
        Max_TP_Up = 0x33,
        Def_Up = 0x34,
        Agi_Up = 0x35,


        Buff_Damage_Done_Modifier = 0x64,
        Buff_Damage_Taken_Modifier = 0x65,
        Buff_Defense_Modifier = 0x66,

        Buff_Max_HP_Modifier = 0x69,
        Buff_Accuracy_Modifier = 0x6A,
        Buff_Aggro_Modifier = 0x6B,
        Buff_Agility_Modifier = 0x6C,
        Buff_Escape_Modifier = 0x71,


    }

    public class PlayerSkillData1
    {
        public PlayerSkillData1(byte[] data, EtrianString[] skillNames)
        {
            SkillID = BitConverter.ToUInt16(data, 0);

            if (SkillID != 0)
                Name = skillNames[SkillID - 1];

            Unknown02 = BitConverter.ToUInt16(data, 2);

            ValuePerLevel = new uint[15];
            for (int i = 0; i < 15; i++)
            {
                ValuePerLevel[i] = BitConverter.ToUInt32(data, 4 + i * 4);
            }
        }

        public override string ToString()
        {
            return Name?.StringValue ?? base.ToString();
        }

        public SkillValueType GetSkillValueType()
        {
            return (SkillValueType)Unknown02;
        }

        public EtrianString Name;

        public ushort SkillID;
        public ushort Unknown02;
        public uint[] ValuePerLevel;

        public List<PlayerSkillData1> SubEntry = new List<PlayerSkillData1>();
        public PlayerSkillData0 Data0Entry;
    }
}

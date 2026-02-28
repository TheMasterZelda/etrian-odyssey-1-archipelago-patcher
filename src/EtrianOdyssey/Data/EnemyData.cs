namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Data
{
    public enum DropCondition : byte
    {
        STAB = 0x02,
        FIRE = 0x04,
        ICE = 0x05,
        NOT_BASH = 0x08,
        NOT_STAB = 0x09,
        NOT_PHYSICAL = 0x0A,
        NOT_FIRE = 0x0B,
        INSTANT_DEATH = 0x0F,
        FULL_BIND = 0x13,
        KILL_1_TURNS = 0x41,
        KILL_2_TURNS = 0x42,
        KILL_3_TURNS = 0x43,
        KILL_7_TURNS = 0x47,
        NONE = 0xFF
    }

    public enum EnemyType : byte
    {
        REGULAR = 1,
        FOE = 2,
        BOSS = 3
    }

    public class EnemyData
    {
        public EnemyData(byte[] data, ushort index, EtrianString name)
        {
            Data = data;
            enemy_id = index;
            Name = name;
            unknown_04 = BitConverter.ToUInt32(data, 4);
            Drop1ItemID = BitConverter.ToUInt16(data, 0x28);
            Drop2ItemID = BitConverter.ToUInt16(data, 0x2A);
            Drop3ItemID = BitConverter.ToUInt16(data, 0x2C);
            Item1Chances = data[0x2E];
            Item2Chances = data[0x2F];
            Item3Chances = data[0x30];
            DropCondition = data[0x31];
            level = BitConverter.ToUInt16(data, 0x38);
            unknown_58 = BitConverter.ToUInt16(data, 0x58);
            codex_id = BitConverter.ToUInt16(data, 0x5A);
        }

        public byte[] Data;

        public DropCondition GetDropCondition()
        {
            return (DropCondition)DropCondition;
        }

        public EnemyType GetEnemyType()
        {
            return (EnemyType)unknown_04;
        }

        public override string ToString()
        {
            return Name.StringValue;
        }

        public ushort enemy_id;
        public EtrianString Name;
        public uint unknown_04;
        public ushort Drop1ItemID;
        public ushort Drop2ItemID;
        public ushort Drop3ItemID;
        public byte Item1Chances;
        public byte Item2Chances;
        public byte Item3Chances;
        public byte DropCondition;
        public ushort level;

        public ushort unknown_58;
        public ushort codex_id;
    }
}

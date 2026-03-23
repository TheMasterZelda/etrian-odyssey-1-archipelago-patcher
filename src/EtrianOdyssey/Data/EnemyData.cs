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
            HP = BitConverter.ToUInt32(data, 0);
            enemy_type = BitConverter.ToUInt32(data, 4);

            STR = BitConverter.ToUInt16(data, 0x08); // 0x08-0x09
            VIT = BitConverter.ToUInt16(data, 0x0A); // 0x0A-0x0B
            AGI = BitConverter.ToUInt16(data, 0x0C); // 0x0C-0x0D
            LUC = BitConverter.ToUInt16(data, 0x0E); // 0x0E-0x0F
            TEC = BitConverter.ToUInt16(data, 0x10); // 0x10-0x11
            CutResistance = BitConverter.ToUInt16(data, 0x12); // 0x12-0x13
            BashResistance = BitConverter.ToUInt16(data, 0x14); // 0x14-0x15
            StabResistance = BitConverter.ToUInt16(data, 0x16); // 0x16-0x17
            FireResistance = BitConverter.ToUInt16(data, 0x18); // 0x18-0x19
            IceResistance = BitConverter.ToUInt16(data, 0x1A); // 0x1A-0x1B
            VoltResistance = BitConverter.ToUInt16(data, 0x1C); // 0x1C-0x1D

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
            return (EnemyType)enemy_type;
        }

        public override string ToString()
        {
            return Name.StringValue;
        }

        public ushort enemy_id; // determined by the enemy position in the file.
        public EtrianString Name;

        public uint HP; // 0x00-0x03
        public uint enemy_type; // 0x04-0x07

        public ushort STR; // 0x08-0x09
        public ushort VIT; // 0x0A-0x0B
        public ushort AGI; // 0x0C-0x0D
        public ushort LUC; // 0x0E-0x0F
        public ushort TEC; // 0x10-0x11
        public ushort CutResistance; // 0x12-0x13
        public ushort BashResistance; // 0x14-0x15
        public ushort StabResistance; // 0x16-0x17
        public ushort FireResistance; // 0x18-0x19
        public ushort IceResistance; // 0x1A-0x1B
        public ushort VoltResistance; // 0x1C-0x1D

        public ushort Drop1ItemID; // 0x28-0x29
        public ushort Drop2ItemID; // 0x2A-0x2B
        public ushort Drop3ItemID; // 0x2C-0x2D
        public byte Item1Chances; // 0x2E
        public byte Item2Chances; // 0x2F
        public byte Item3Chances; // 0x30
        public byte DropCondition; // 0x31
        public ushort level; // 0x38-0x39

        public ushort unknown_58; // 0x58-0x59. Unused enemy type.
        public ushort codex_id; // 0x5A-0x5B
    }
}

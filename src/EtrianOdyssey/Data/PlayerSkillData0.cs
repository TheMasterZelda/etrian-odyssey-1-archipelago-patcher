using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Data
{
    public enum SkillElement
    {
        NONE,
        SLASH,
        BASH,
        STAB,
        FIRE,
        ICE,
        THUNDER
    }

    public class PlayerSkillData0
    {
        public PlayerSkillData0(byte[] data)
        {
            skill_type = data[0x0];
            skill_need = data[0x1];
            skill_mastery = data[0x2];
            skill_target = data[0x3];
            skill_side = data[0x4];
            skill_usage = data[0x5];
            Unknown6 = data[0x6];
            skill_effect = data[0x7];

            skill_flags = BitConverter.ToUInt32(data, 0x8);
            Unknown8 = data[0x8];
            Unknown9 = data[0x9];
            UnknownA = data[0xA];
            UnknownB = data[0xB];
        }

        public override string ToString()
        {
            return SkillType.ToString();
        }

        public Skill_eSK SkillType => (Skill_eSK)skill_type;

        public SkillElement PrimaryDamageType => GetPrimaryDamageType();
        public SkillElement SecondaryDamageType => GetSecondaryDamageType();

        public SkillFlag SkillFlags => (SkillFlag)skill_flags;

        private SkillElement GetPrimaryDamageType()
        {
            // Disgusting code.

            if (SkillType == Skill_eSK.ABIL_REINFORCED ||
                SkillType == Skill_eSK.COUNTER_DEFENSE)
                return SkillElement.NONE;

            SkillFlag skill_flags = SkillFlags;

            if (skill_flags.HasFlag(SkillFlag.Slash))
                return SkillElement.SLASH;
            if (skill_flags.HasFlag(SkillFlag.Bash))
                return SkillElement.BASH;
            if (skill_flags.HasFlag(SkillFlag.Stab))
                return SkillElement.STAB;
            if (skill_flags.HasFlag(SkillFlag.Fire))
                return SkillElement.FIRE;
            if (skill_flags.HasFlag(SkillFlag.Ice))
                return SkillElement.ICE;
            if (skill_flags.HasFlag(SkillFlag.Thunder))
                return SkillElement.THUNDER;

            return SkillElement.NONE;
        }

        private SkillElement GetSecondaryDamageType()
        {
            // Also disgusting code.

            SkillElement primaryDamageType = GetPrimaryDamageType();
            if (primaryDamageType == SkillElement.NONE)
                return SkillElement.NONE;
            if (primaryDamageType == SkillElement.FIRE ||
                primaryDamageType == SkillElement.ICE ||
                primaryDamageType == SkillElement.THUNDER)
                return SkillElement.NONE;

            SkillFlag skill_flags = SkillFlags;

            if (skill_flags.HasFlag(SkillFlag.Fire))
                return SkillElement.FIRE;
            if (skill_flags.HasFlag(SkillFlag.Ice))
                return SkillElement.ICE;
            if (skill_flags.HasFlag(SkillFlag.Thunder))
                return SkillElement.THUNDER;

            return SkillElement.NONE;
        }

        internal string GetAilmentStr()
        {
            switch (GetAilment())
            {
                case SkillFlag.None:
                    return "NONE";
                case SkillFlag.Head:
                    return "HEAD_BIND";
                case SkillFlag.Arm:
                    return "ARM_BIND";
                case SkillFlag.Leg:
                    return "LEG_BIND";
                case SkillFlag.Dead:
                    return "INSTANT_DEATH";
                case SkillFlag.Stone:
                    return "PETRIFY";
                case SkillFlag.Sleep:
                    return "SLEEP";
                case SkillFlag.Paralysis:
                    return "PARALYSIS";
                case SkillFlag.Confusion:
                    return "CONFUSION";
                case SkillFlag.Terror:
                    return "FEAR";
                case SkillFlag.Poison:
                    return "POISON";
                case SkillFlag.Blind:
                    return "BLIND";
                case SkillFlag.Curse:
                    return "CURSE";
                case SkillFlag.Stun:
                    return "STUN";
                case SkillFlag.Slash:
                case SkillFlag.Bash:
                case SkillFlag.Stab:
                case SkillFlag.Fire:
                case SkillFlag.Ice:
                case SkillFlag.Thunder:
                case SkillFlag.StayOneAct:
                case SkillFlag.StayTrgNormalAttack:
                case SkillFlag.StayTrgTargetSelf:
                case SkillFlag.StayTrgTargetSelflr:
                case SkillFlag.StayTrgTargetFront:
                case SkillFlag.StayTrgTargetBack:
                case SkillFlag.BoostLvUp:
                default:
                    throw new NotImplementedException();
            }
        }

        internal SkillFlag GetAilment()
        {
            SkillFlag skill_flag = SkillFlag.None;

            if (Do(SkillFlag.Head))
                return skill_flag;
            if (Do(SkillFlag.Arm))
                return skill_flag;
            if (Do(SkillFlag.Leg))
                return skill_flag;
            if (Do(SkillFlag.Dead))
                return skill_flag;
            if (Do(SkillFlag.Stone))
                return skill_flag;
            if (Do(SkillFlag.Sleep))
                return skill_flag;
            if (Do(SkillFlag.Paralysis))
                return skill_flag;
            if (Do(SkillFlag.Confusion))
                return skill_flag;
            if (Do(SkillFlag.Terror))
                return skill_flag;
            if (Do(SkillFlag.Poison))
                return skill_flag;
            if (Do(SkillFlag.Blind))
                return skill_flag;
            if (Do(SkillFlag.Curse))
                return skill_flag;
            if (Do(SkillFlag.Stun))
                return skill_flag;

            return SkillFlag.None;

            bool Do(SkillFlag boo)
            {
                if (SkillFlags.HasFlag(boo))
                {
                    skill_flag = boo;
                    return true;
                }

                return false;
            }
        }

        internal bool HasMoreThanOneAilment()
        {
            bool ailmentFound = false;
            bool duplicate = false;
            duplicate |= Do(HasHeadFlag);
            duplicate |= Do(HasArmFlag);
            duplicate |= Do(HasLegFlag);
            duplicate |= Do(HasDeadFlag);
            duplicate |= Do(HasStoneFlag);
            duplicate |= Do(HasSleepFlag);
            duplicate |= Do(HasParalysisFlag);
            duplicate |= Do(HasConfusionFlag);
            duplicate |= Do(HasTerrorFlag);
            duplicate |= Do(HasPoisonFlag);
            duplicate |= Do(HasBlindFlag);
            duplicate |= Do(HasCurseFlag);
            duplicate |= Do(HasStunFlag);

            return duplicate;

            bool Do(bool boo)
            {
                if (boo && ailmentFound)
                    return true;

                ailmentFound |= boo;
                return false;
            }
        }

        public Skill_eSK_NEED SkillNeed => (Skill_eSK_NEED)skill_need;
        public Skill_eSK_MASTER SkillMastery => (Skill_eSK_MASTER)skill_mastery;
        public Skill_eSK_TGT SkillTarget => (Skill_eSK_TGT)skill_target;
        public Skill_eSK_SIDE SkillSide => (Skill_eSK_SIDE)skill_side;
        public SkillUsageType SkillUsage => (SkillUsageType)skill_usage;
        public SkillEff SkillEffect => (SkillEff)skill_effect;


        public byte skill_type; // Skill_eSK
        public byte skill_need; // Skill_eSK_NEED
        public byte skill_mastery; // Skill_eSK_MASTER
        public byte skill_target; // Skill_eSK_TGT
        public byte skill_side; // Skill_eSK_SIDE
        public byte skill_usage; // SkillUsageType
        public byte Unknown6; // Always 0x00
        public byte skill_effect; // SkillEff
        public uint skill_flags; // SkillFlag
        public byte Unknown8;
        public byte Unknown9;
        public byte UnknownA;
        public byte UnknownB;

        public bool HasSlashFlag => SkillFlags.HasFlag(SkillFlag.Slash);
        public bool HasBashFlag => SkillFlags.HasFlag(SkillFlag.Bash);
        public bool HasStabFlag => SkillFlags.HasFlag(SkillFlag.Stab);
        public bool HasFireFlag => SkillFlags.HasFlag(SkillFlag.Fire);
        public bool HasIceFlag => SkillFlags.HasFlag(SkillFlag.Ice);
        public bool HasThunderFlag => SkillFlags.HasFlag(SkillFlag.Thunder);
        public bool HasHeadFlag => SkillFlags.HasFlag(SkillFlag.Head);
        public bool HasArmFlag => SkillFlags.HasFlag(SkillFlag.Arm);
        public bool HasLegFlag => SkillFlags.HasFlag(SkillFlag.Leg);
        public bool HasDeadFlag => SkillFlags.HasFlag(SkillFlag.Dead);
        public bool HasStoneFlag => SkillFlags.HasFlag(SkillFlag.Stone);
        public bool HasSleepFlag => SkillFlags.HasFlag(SkillFlag.Sleep);
        public bool HasParalysisFlag => SkillFlags.HasFlag(SkillFlag.Paralysis);
        public bool HasConfusionFlag => SkillFlags.HasFlag(SkillFlag.Confusion);
        public bool HasTerrorFlag => SkillFlags.HasFlag(SkillFlag.Terror);
        public bool HasPoisonFlag => SkillFlags.HasFlag(SkillFlag.Poison);
        public bool HasBlindFlag => SkillFlags.HasFlag(SkillFlag.Blind);
        public bool HasCurseFlag => SkillFlags.HasFlag(SkillFlag.Curse);
        public bool HasStunFlag => SkillFlags.HasFlag(SkillFlag.Stun);
        public bool HasStayOneActFlag => SkillFlags.HasFlag(SkillFlag.StayOneAct);
        public bool HasStayTrgNormalAttackFlag => SkillFlags.HasFlag(SkillFlag.StayTrgNormalAttack);
        public bool HasStayTrgTargetSelfFlag => SkillFlags.HasFlag(SkillFlag.StayTrgTargetSelf);
        public bool HasStayTrgTargetSelflrFlag => SkillFlags.HasFlag(SkillFlag.StayTrgTargetSelflr);
        public bool HasStayTrgTargetFrontFlag => SkillFlags.HasFlag(SkillFlag.StayTrgTargetFront);
        public bool HasStayTrgTargetBackFlag => SkillFlags.HasFlag(SkillFlag.StayTrgTargetBack);
        public bool HasBoostLvUpFlag => SkillFlags.HasFlag(SkillFlag.BoostLvUp);

    }

    [Flags]
    public enum SkillFlag : uint
    {
        None = 0,
        Slash = 1,
        Bash = 2, // Originally "Shock" but renamed to avoid confusion.
        Stab = 4, // Originally "Thrust" but renamed for standard name.
        Fire = 8,
        Ice = 16,
        Thunder = 32,
        Head = 64,
        Arm = 128,
        Leg = 256,
        Dead = 512,
        Stone = 1024,
        Sleep = 2048,
        Paralysis = 4096,
        Confusion = 8192,
        Terror = 16384,
        Poison = 32768,
        Blind = 65536,
        Curse = 131072,
        Stun = 262144,
        StayOneAct = 524288,
        StayTrgNormalAttack = 1048576,
        StayTrgTargetSelf = 2097152,
        StayTrgTargetSelflr = 4194304,
        StayTrgTargetFront = 8388608,
        StayTrgTargetBack = 16777216,
        BoostLvUp = 33554432,
    }

    public enum SkillUsageType : byte
    {
        NONE = 0,
        TOWN = 1,
        DUNGEON = 2,
        BATTLE = 3,
        TOWN_DUNGEON = 4,
        DUNGEON_BATTLE = 5,
        ALL = 6,
    }

    public enum SkillEff : byte
    {
        ATTACK = 0,
        SPEED = 1,
        AVOID = 2,
        STYLE = 3,
        DEFENCE = 4,
        RECOVER = 5,
        AFFINITY = 6,
        HPMAX = 7,
        HIT = 8,
        SQUEEZE = 9,
    }

    public enum Skill_eSK_TGT : byte
    {
        ONE = 0,
        ALL = 1,
        ALL_RAND = 2,
        ALL_RAND_ONE_HIT = 3,
        ONE_CONVOLUTE = 4,
        SELF = 5,
        FRONT = 6,
        BACK = 7,
    }

    public enum Skill_eSK_SIDE : byte
    {
        SELF = 0,
        OTHER = 1,
        ALL = 2,
    }

    public enum Skill_eSK_NEED : byte
    {
        NONE = 0,
        HEAD = 1,
        ARM = 2,
        LEG = 3,
        ALL = 4,
    }

    public enum Skill_eSK_MASTER : byte
    {
        NONE = 0,
        AX = 1,
        SWORD = 2,
        SHOT = 3,
        SHIELD = 4,
        WHIP = 5,
        SAMURAI_SWORD = 6,
        FIRE = 7,
        ICE = 8,
        THUNDER = 9,
        BENOM = 10,
        RECOVERY = 11,
        SONG = 12,
        CURSE = 13,
    }

    public enum Skill_eSK : byte
    {
        PHYSICAL_ATTACK = 0,
        MAGIC_ATTACK = 1,
        NO_DAMAGE_ATTACK = 2,
        ABIL_WEAKNESS = 3,
        ABIL_REINFORCED = 4,
        COUNTER_ATTACK = 5,
        PURSUIT = 6,
        COUNTER_DEFENSE = 7,
        HP_RECOVE = 8,
        BST_SEAL_RECOVE = 9,
        PARAMETER = 10,
        MASTARESKIL = 11,
        DEFORESTATION = 12,
        MINING = 13,
        COLLECTION = 14,
        SPECIAL = 15,
        REINFORCED_DISAPPEARANCE = 16,
    }
}

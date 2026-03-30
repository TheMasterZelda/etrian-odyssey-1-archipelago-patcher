using System.Xml.Linq;

namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Data
{
    public class PlayerSkillData1
    {
        public PlayerSkillData1(byte[] data, EtrianString[] skillNames)
        {
            SkillID = BitConverter.ToUInt16(data, 0);

            if (SkillID != 0)
                Name = skillNames[SkillID - 1];

            value_type = BitConverter.ToUInt16(data, 2);

            ValuePerLevel = new uint[15];
            for (int i = 0; i < 15; i++)
            {
                ValuePerLevel[i] = BitConverter.ToUInt32(data, 4 + i * 4);
            }
        }

        public string GetSkillType()
        {
            switch (SkillAttributes.SkillType)
            {
                case Skill_eSK.PHYSICAL_ATTACK:
                    return "PHYSICAL_ATTACK"; // Active
                case Skill_eSK.MAGIC_ATTACK:
                    return "MAGICAL_ATTACK"; // Active
                case Skill_eSK.NO_DAMAGE_ATTACK:
                    return "AILMENT_ATTACK"; // Active
                case Skill_eSK.ABIL_WEAKNESS:
                    return "DEBUFF"; // Active
                case Skill_eSK.ABIL_REINFORCED:
                    return "BUFF"; // Active
                case Skill_eSK.COUNTER_ATTACK:
                    return "COUNTER"; // Active
                case Skill_eSK.PURSUIT:
                    return "CHASE"; // Active
                case Skill_eSK.COUNTER_DEFENSE:
                    return "DEFENSE"; // Active
                case Skill_eSK.HP_RECOVE:
                    return "HEAL"; // Active
                case Skill_eSK.BST_SEAL_RECOVE:
                    return "AILMENT_HEAL"; // Active
                case Skill_eSK.PARAMETER:
                    return "PASSIVE";
                case Skill_eSK.MASTARESKIL:
                    return "MASTERY";
                case Skill_eSK.DEFORESTATION:
                    if (this.SkillID == 0x47) // Dark Hunter Take skill is mistakenly defined as Chop.
                        return "TAKE";
                    return "CHOP";
                case Skill_eSK.MINING:
                    return "MINE";
                case Skill_eSK.COLLECTION:
                    return "TAKE";
                case Skill_eSK.SPECIAL:
                    {
                        // Result depend on specific skill.
                        switch (this.SkillID)
                        {
                            case 0x15: // LANDSKNECHT_FLEE
                            case 0x3E: // PROTECTOR_FLEE
                                return "ESCAPE"; // Active

                            case 0x25: // SURVIVALIST_APOLLON
                            case 0x67: // RONIN_KUBIUCHI
                            case 0x4D: // DARK_HUNTER_ECSTASY
                            case 0x4E: // DARK_HUNTER_CLIMAX
                            case 0xBC: // HEXER_REVENGE
                                return "SPECIAL_PHYSICAL_ATTACK"; // Active

                            case 0x1F: // SURVIVALIST_1ST_TURN
                                return "TURN_MANIPULATION"; // Active

                            case 0x6E: // MEDIC_PATCH_UP
                            case 0x7E: // MEDIC_H_TOUCH
                                return "FIELD_HEAL";

                            case 0xAC: // HEXER_STAGGER
                                return "FIELD_PASSIVE";

                            case 0x2A: // SURVIVALIST_OWL_EYE
                            case 0x93: // ALCHEMIST_WARP
                            case 0xA7: // TROUBADOUR_RETURN
                                return "FIELD_UTILITY";

                            case 0xA0: // TROUBADOUR_IFRIT
                            case 0xA1: // TROUBADOUR_YMIR
                            case 0xA2: // TROUBADOUR_TARANIS
                                return "BUFF"; // Active

                            case 0xB9:// HEXER_SUICIDE
                            case 0xBA:// HEXER_BETRAYAL
                            case 0xBB:// HEXER_PARALYZE
                                return "CURSE"; // Active
                            default:
                                throw new NotImplementedException();
                        }
                    }
                case Skill_eSK.REINFORCED_DISAPPEARANCE:
                    return "BUFF_REMOVAL"; // Active
                default:
                    throw new NotImplementedException();
            }
        }

        public string GetAilment()
        {
            if (SkillAttributes.SkillType == Skill_eSK.BST_SEAL_RECOVE ||
                SkillAttributes.SkillType == Skill_eSK.HP_RECOVE)
                return "NONE";

            if (SkillAttributes.HasMoreThanOneAilment())
                throw new NotImplementedException();

            return SkillAttributes.GetAilmentStr();
        }

        public override string ToString()
        {
            return Name?.StringValue ?? base.ToString();
        }

        public SkillValueType GetSkillValueType()
        {
            return (SkillValueType)value_type;
        }

        public SkillValueType ValueType => GetSkillValueType();

        public EtrianString Name;

        public ushort SkillID;
        public ushort value_type;
        public uint[] ValuePerLevel;

        public List<PlayerSkillData1> SubEntry = new List<PlayerSkillData1>();
        public PlayerSkillData0 Data0Entry;
        public PlayerSkillData0 SkillAttributes => Data0Entry;
    }

    public enum SkillValueType : ushort
    {
        NONE = 0,
        CONSUMPTION_TP = 1,
        SKILL_COEFFICIENT = 2,
        SPEED = 3,
        HIT_RATE = 4,
        RATE_X1 = 5,
        RATE_X3 = 6,
        RATE_X4 = 7,
        COUNTER_RATE = 8,
        COUNTER_DEC_RATE = 9,
        RECOVERY_VALUE = 10,
        BST_RECOVERY_LEVEL = 11,
        ATTACK_NUMBER = 12,
        GET_SUCCESS_COEFFICIENT = 13,
        TURN_NUMBER = 14,
        SKILL_MASTER_COEFFICIENT = 15,
        ATTACK_BOOST_COEFFICIENT = 16,
        ROLLED_DAMAGE_COEFFICIENT = 17,
        EFFICACY_SUCCESS_RATE = 18,
        EFFICACY_SKILL_COEFFICIENT = 19,
        ATTR_ADD_ATTACK_SC = 20,
        ATTR_ATTACK_VALUE = 21,
        SC2 = 22,
        VALX1 = 23,
        PARAM_BOOST_HP = 50,
        PARAM_BOOST_TP = 51,
        PARAM_BOOST_AFFINITY_ALL = 52,
        PARAM_BOOST_AGI = 53,
        PARAM_BOOST_CRITICAL = 54,
        PARAM_BOOST_EXP = 55,
        PARAM_BOOST_BOOSTADD = 56,
        MUL_SKILL_COEFFICIENT = 100,
        MUL_AFFINITY = 101,
        MUL_DEFENSE = 102,
        MUL_PHYSICAL_ATTACK = 103,
        MUL_MAGIC_ATTACK = 104,
        MUL_HP_MAX = 105,
        MUL_HIT = 106,
        MUL_TARGET = 107,
        MUL_ACTION_SPEED = 108,
        HP_RECOVERY_RATE = 109,
        TP_RECOVERY_RATE = 110,
        BST_RECOVERY_TURN_CORRECTION = 111,
        ABSORPTION_RATE = 112,
        ESCAPE_RATE = 113,
        CHANGE_ATC_ATTR = 114,
        MUL_AFFINITY2 = 115,
        REFLECTION_ATTR = 116,
    }

}

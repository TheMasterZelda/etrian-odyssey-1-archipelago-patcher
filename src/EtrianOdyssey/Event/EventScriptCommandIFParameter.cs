namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Event
{
    public class EventScriptCommandIFParameter
    {
        public override string ToString()
        {
            return $"{if_target}|{value_math}|{parameter3}|{chain_type}";
            //return $"{parameter1}|{parameter2}|{parameter3}|{parameter4}";
        }

        public IfTarget if_target => (IfTarget)parameter1;
        public ushort parameter1;

        public ValueMath value_math => (ValueMath)parameter2;
        public ushort parameter2;

        public ushort parameter3;

        public ChainType chain_type => (ChainType)parameter4;
        public ushort parameter4;
    }

    public enum IfTarget : ushort
    {
        E_IF_FLAG_ON = 0,
        E_IF_FLAG_OFF = 1,
        E_IF_GOLD = 2,
        E_IF_PARTY_LEVEL = 3,
        E_IF_SWORDMAN_LEVEL = 4,
        E_IF_RANGER_LEVEL = 5,
        E_IF_PALADIN_LEVEL = 6,
        E_IF_DARKHUNTER_LEVEL = 7,
        E_IF_BUSHIDO_LEVEL = 8,
        E_IF_MEDIC_LEVEL = 9,
        E_IF_ALCHEMIST_LEVEL = 10,
        E_IF_BARD_LEVEL = 11,
        E_IF_CURSEMAKER_LEVEL = 12,
        E_IF_TGT_ITEM_1_NUM = 13,
        E_IF_TGT_ITEM_2_NUM = 14,
        E_IF_TGT_ITEM_3_NUM = 15,
        E_IF_TGT_ITEM_4_NUM = 16,
        E_IF_TGT_ITEM_5_NUM = 17,
        E_IF_PARTY_STR = 18,
        E_IF_PARTY_VIT = 19,
        E_IF_PARTY_AGI = 20,
        E_IF_PARTY_ENTRY = 21,
        E_IF_DUNGEON_WALK = 22,
        E_IF_TMP_FLAG_ON = 23,
        E_IF_TMP_FLAG_OFF = 24,
        E_IF_PREV_FLOOR_ENEMY = 25,
        E_IF_VALUE_1 = 26,
        E_IF_VALUE_2 = 27,
        E_IF_VALUE_3 = 28,
        E_IF_VALUE_4 = 29,
        E_IF_VALUE_5 = 30,
        E_IF_SHOP_ITEM_1_NUM = 31,
        E_IF_SHOP_ITEM_2_NUM = 32,
        E_IF_SHOP_ITEM_3_NUM = 33,
        E_IF_SHOP_ITEM_4_NUM = 34,
        E_IF_SHOP_ITEM_5_NUM = 35,
        E_IF_BAD_STATUS_DEAD = 36,
        E_IF_BAD_STATUS_STONE = 37,
        E_IF_BAD_STATUS_SLEEP = 38,
        E_IF_BAD_STATUS_CONFUSION = 39,
        E_IF_BAD_STATUS_TERROR = 40,
        E_IF_BAD_STATUS_POISON = 41,
        E_IF_BAD_STATUS_BLIND = 42,
        E_IF_BAD_STATUS_CURSE = 43,
        E_IF_BAD_STATUS_PARALYSIS = 44,
        E_IF_BAD_STATUS_HEAD = 45,
        E_IF_BAD_STATUS_ARM = 46,
        E_IF_BAD_STATUS_LEG = 47,
        E_IF_BAD_STATUS_DETH_TGT_PLAYER = 48,
        E_IF_ITEM_EMPTY_NUM = 49,
        E_IF_SKILL_LEVEL = 50,
        E_IF_MAP_CORRECT = 51,
        E_IF_ITEM_REPORT_NUM = 52,
        E_IF_ENEMY_REPORT_NUM = 53,
        E_IF_HAS_EQUIP_ITEM = 54,
        E_TARGET_MAX = 55,
    }

    public enum ChainType
    {
        E_IF_CHAIN_AND = 0,
        E_IF_CHAIN_OR = 1,
        E_IF_CHAIN_NONE = 2,
    }

    public enum ValueMath
    {
        E_VALUE_MATH_PLUS = 0,
        E_VALUE_MATH_MINUS = 1,
        E_VALUE_MATH_EQUAL = 2,
        E_VALUE_MATH_MAX = 3,
    }
}
/*


// Namespace: 
public enum EventSelectModule.EVEN_MENU_TYPE // TypeDefIndex: 10801
{
	// Fields
	public int value__; // 0x0
	public const EventSelectModule.EVEN_MENU_TYPE EVEN_MENU_MESS_WINDOW_UNDER = 0;
	public const EventSelectModule.EVEN_MENU_TYPE EVEN_MENU_MESS_WINDOW_CENTER = 1;
	public const EventSelectModule.EVEN_MENU_TYPE EVEN_MENU_YN_WINDOW_CENTER = 2;
	public const EventSelectModule.EVEN_MENU_TYPE EVEN_MENU_YN_WINDOW_UNDER = 3;
	public const EventSelectModule.EVEN_MENU_TYPE EVEN_MENU_THREE_CHOICE_WINDOW_CENTER = 4;
	public const EventSelectModule.EVEN_MENU_TYPE EVEN_MENU_THREE_CHOICE_WINDOW_UNDER = 5;
	public const EventSelectModule.EVEN_MENU_TYPE EVEN_MENU_TYPE_END = 6;
}

// Namespace: 
public enum EventSelectModule.EventSelectResult // TypeDefIndex: 10802
{
	// Fields
	public int value__; // 0x0
	public const EventSelectModule.EventSelectResult E_EVE_SEL_NONE = -1;
	public const EventSelectModule.EventSelectResult E_EVE_SEL_YES = 0;
	public const EventSelectModule.EventSelectResult E_EVE_SEL_NO = 1;
	public const EventSelectModule.EventSelectResult E_EVE_SEL_UP = 2;
	public const EventSelectModule.EventSelectResult E_EVE_SEL_MID = 3;
	public const EventSelectModule.EventSelectResult E_EVE_SEL_DOWN = 4;
	public const EventSelectModule.EventSelectResult E_EVE_SEL_NEXT = 5;
}

// Namespace: 
public enum EventSelectModule.MenuStackInfoMode // TypeDefIndex: 10803
{
	// Fields
	public int value__; // 0x0
	public const EventSelectModule.MenuStackInfoMode MODE_EVENT_WAIT = 0;
	public const EventSelectModule.MenuStackInfoMode MODE_MENU_WAIT = 1;
	public const EventSelectModule.MenuStackInfoMode MODE_MENU_END_PROC_WAIT = 2;
}
 
// Namespace: 
public enum EventIfModule.MathType // TypeDefIndex: 10776
{
    // Fields
    public int value__; // 0x0
public const EventIfModule.MathType E_MATH_01 = 0;
public const EventIfModule.MathType E_MATH_02 = 1;
public const EventIfModule.MathType E_MATH_03 = 2;
public const EventIfModule.MathType E_MATH_04 = 3;
public const EventIfModule.MathType E_MATH_05 = 4;
public const EventIfModule.MathType E_MATH_MAX = 5;
}


*/
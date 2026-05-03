namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Event
{
    public class EventEntry
    {
        // size = 0x18

        // 00 = Event index?
        // 01 = Floor (+29) condition (possibly locationID)
        // 02 = X Coord
        // 03 = Y Coord
        // 04 = Condition related
        // 05 = Condition related
        // 06 = Facing Direction bitflag, "any" = 0x1E
        // 07 = "param 2" condition
        // 08-0B = Required Flag Condition
        // 0C-0F = Event Flag (Not set Condition)
        // 10 = Required Temp Flag (Condition)
        // 11 = Condition
        // 12-13 = Event Index?
        // 14-17 = Script start offset.
        public EventEntry(byte[] blockData)
        {
            event_index = blockData[0x0];
            locationID = (sbyte)blockData[0x1];
            coordX = (sbyte)blockData[0x2];
            coordY = (sbyte)blockData[0x3];
            time_start = blockData[0x4];
            time_end = blockData[0x5];
            direction = blockData[0x6];
            action = blockData[0x7];

            required_flag = BitConverter.ToInt32(blockData, 0x08);
            not_set_flag = BitConverter.ToInt32(blockData, 0x0C);

            required_temp_flag = (sbyte)blockData[0x10];
            not_set_temp_flag = (sbyte)blockData[0x11];
            complex_condition_index = BitConverter.ToUInt16(blockData, 0x12);

            script_offset = BitConverter.ToUInt32(blockData, 0x14);
        }

        public override string ToString()
        {
            return not_set_flag.ToString("X");
        }

        public CheckPlace checkPlace => (CheckPlace)locationID;
        // index in file.
        public byte event_index; // 00
        public sbyte locationID; // 01
        public sbyte coordX; // 02
        public sbyte coordY; // 03
        public byte time_start; // 04
        public byte time_end; // 05

        public EventDirType dirType => (EventDirType)direction;
        public byte direction; // 06
        public EventActionType action_type => (EventActionType)action; // 07
        public byte action; // 07

        public int required_flag;
        public int not_set_flag;
        public sbyte required_temp_flag; // 10
        public sbyte not_set_temp_flag; // 11

        /*
        0= majority of events
        3= used by b11f event for map completion
        4= used by b12f event for map completion
        5= used by clear key get event
        6= used for violet key get event
        7= used by radha hall event
        8= used by radha hall event
        9= used by radha hall event
        10= used by radha hall event
        11= used by radha hall event
        12= used by radha hall event

        100= used by Quest 1
        119= used by Quest 0
        131= used by Quest 3
        132= used by Quest 4
        133= used by Quest 5
        156= used by Quest 15
        167= used by Quest 16
        108= used by Quest 17
        109= used by quest 22
        65280/-1= used by quest 22, 72
        110= used by quest 23
        101= used by quest 25
        126= used by quest 26
        127= used by quest 27
        128= used by quest 28
        168= used by quest 29
        112= used by quest 33
        181= used by quest 34
        113= used by quest 35
        196= used by quest 36
        175= used by quest 37
        176= used by quest 38
        114= used by quest 39
        134= used by quest 40
        135= used by quest 41
        177= used by quest 42
        129= used by quest 43
        178= used by quest 44
        102= used by quest 50
        170= used by quest 56
        103= used by quest 61
        111= used by quest 62
        179= used by quest 63
        104= used by quest 64
        105= used by quest 66
        182= used by quest 68
        183= used by quest 68
        184= used by quest 68
        185= used by quest 68
        186= used by quest 68
        187= used by quest 68
        188= used by quest 68
        106= used by quest 69
        */
        // Event Index related.
        // Complex condition related.
        public ushort complex_condition_index; // 12-13

        public uint script_offset;

        public EventScript script;
    }

    public enum CheckPlace : sbyte
    {
        E_PLACE_NONE = -1,
        E_DUNGEON_COMMON = 0,
        E_SISETU_COMMON = 1,
        E_YADOYA_IN = 2,
        E_YADOYA_TALK = 3,
        E_YADOYA_OUT = 4,
        E_SHOP_IN = 5,
        E_SHOP_TALK = 6,
        E_SHOP_OUT = 7,
        E_TOUTI_IN = 8,
        E_TOUTI_TALK = 9,
        E_TOUTI_OUT = 10,
        E_TOUTI_UKERU = 11,
        E_TOUTI_HOUKOKU = 12,
        E_SAKABA_IN = 13,
        E_SAKABA_TALK = 14,
        E_SAKABA_OUT = 15,
        E_SAKABA_UKERU = 16,
        E_SAKABA_HOUKOKU = 17,
        E_GUILD_IN = 18,
        E_GUILD_TALK = 19,
        E_GUILD_OUT = 20,
        E_HOSPITAL_IN = 21,
        E_HOSPITAL_TALK = 22,
        E_HOSPITAL_OUT = 23,
        E_HIROBA_IN = 24,
        E_HIROBA_OUT = 25,
        E_JUKAI_ENTER_IN = 26,
        E_JUKAI_ENTER_OUT = 27,
        E_BATTLE_END = 28,
        E_DUNGEON_01F = 29,
        E_DUNGEON_02F = 30,
        E_DUNGEON_03F = 31,
        E_DUNGEON_04F = 32,
        E_DUNGEON_05F = 33,
        E_DUNGEON_06F = 34,
        E_DUNGEON_07F = 35,
        E_DUNGEON_08F = 36,
        E_DUNGEON_09F = 37,
        E_DUNGEON_10F = 38,
        E_DUNGEON_11F = 39,
        E_DUNGEON_12F = 40,
        E_DUNGEON_13F = 41,
        E_DUNGEON_14F = 42,
        E_DUNGEON_15F = 43,
        E_DUNGEON_16F = 44,
        E_DUNGEON_17F = 45,
        E_DUNGEON_18F = 46,
        E_DUNGEON_19F = 47,
        E_DUNGEON_20F = 48,
        E_DUNGEON_21F = 49,
        E_DUNGEON_22F = 50,
        E_DUNGEON_23F = 51,
        E_DUNGEON_24F = 52,
        E_DUNGEON_25F = 53,
        E_DUNGEON_26F = 54,
        E_DUNGEON_27F = 55,
        E_DUNGEON_28F = 56,
        E_DUNGEON_29F = 57,
        E_DUNGEON_30F = 58,
        E_SAKABA_HOUKOKU_MAE = 59,
        E_CAMP_BOOK_MAE = 60,
        E_PLACE_MAX = 61,
    }

    public enum EventDirType : byte
    {
        E_EVE_DIR_NONE = 0,
        E_EVE_DIR_EAST = 1,
        E_EVE_DIR_NORTH = 2,
        E_EVE_DIR_WEST = 3,
        E_EVE_DIR_SOUTH = 4,
        E_EVE_DIR_ALL = 5,
        E_EVE_DIR_MAX = 6,
    }

    public enum EventActionType : byte
    {
        NONE = 0,
        SEARCH = 1,
        TALK = 2,
        DOOR = 3,
        UP = 4,
        DOWN = 5,
        MINING = 6,
        PICK = 7,
        CUT = 8,
        RIDE = 9,
        TREASURE = 10,
        HAVE_ITEM = 11,
        FLOOR_CHANGE = 12,
    }



    /*
	 // Namespace: 
public enum EventScript.ScriptTblFunc // TypeDefIndex: 10795
{
	// Fields
	public int value__; // 0x0
	public const EventScript.ScriptTblFunc SetEventProcState = 0;
	public const EventScript.ScriptTblFunc EventProc_MenuOpen_Mess16 = 1;
	public const EventScript.ScriptTblFunc EventProc_MenuClose_Mess = 2;
	public const EventScript.ScriptTblFunc ImageObjIn = 3;
	public const EventScript.ScriptTblFunc ImageObjOut = 4;
	public const EventScript.ScriptTblFunc JumpTargetLabel = 5;
	public const EventScript.ScriptTblFunc EventIfProc = 6;
	public const EventScript.ScriptTblFunc SetEventFlag = 7;
	public const EventScript.ScriptTblFunc SetEventPlayerPos = 8;
	public const EventScript.ScriptTblFunc SetEventAddGold = 9;
	public const EventScript.ScriptTblFunc SetGold = 10;
	public const EventScript.ScriptTblFunc SetItemNo = 11;
	public const EventScript.ScriptTblFunc SetEventItemFg = 12;
	public const EventScript.ScriptTblFunc SetEventAddCharHp = 13;
	public const EventScript.ScriptTblFunc SetEventAddCharTp = 14;
	public const EventScript.ScriptTblFunc GfxFadeSetSpeed = 15;
	public const EventScript.ScriptTblFunc GfxFadeIn = 16;
	public const EventScript.ScriptTblFunc GfxFadeOut = 17;
	public const EventScript.ScriptTblFunc ScriptPointerPlus = 18;
	public const EventScript.ScriptTblFunc SetEventTempFg = 19;
	public const EventScript.ScriptTblFunc SetEventMoveVec = 20;
	public const EventScript.ScriptTblFunc SetEventItemId = 21;
	public const EventScript.ScriptTblFunc SetEventTimeWndDisp = 22;
	public const EventScript.ScriptTblFunc SetEventEnemyAppearDisp = 23;
	public const EventScript.ScriptTblFunc SetEventGetSkillItem = 24;
	public const EventScript.ScriptTblFunc SetEventValueCom = 25;
	public const EventScript.ScriptTblFunc SetEventPlayerDir = 26;
	public const EventScript.ScriptTblFunc ImageObjFree = 27;
	public const EventScript.ScriptTblFunc SetEventEntryId = 28;
	public const EventScript.ScriptTblFunc SetEventSkillId = 29;
	public const EventScript.ScriptTblFunc SetEventImagePrio = 30;
	public const EventScript.ScriptTblFunc FEnemyTgtSetLockFg = 31;
	public const EventScript.ScriptTblFunc SetFloorEnemyKillTrg = 32;
	public const EventScript.ScriptTblFunc SetEventMapFade = 33;
	public const EventScript.ScriptTblFunc SetEventSetShopListFg = 34;
	public const EventScript.ScriptTblFunc EventSetFgOn = 35;
	public const EventScript.ScriptTblFunc EventSetFgOff = 36;
	public const EventScript.ScriptTblFunc EventBgmLoad = 37;
	public const EventScript.ScriptTblFunc EventBgmPlay = 38;
	public const EventScript.ScriptTblFunc EventBgmStop = 39;
	public const EventScript.ScriptTblFunc EventBgmFree = 40;
	public const EventScript.ScriptTblFunc ImageObjCharLoad = 41;
	public const EventScript.ScriptTblFunc ImageObjCharFree = 42;
	public const EventScript.ScriptTblFunc ImageObjItemLoad = 43;
	public const EventScript.ScriptTblFunc ImageObjItemFree = 44;
	public const EventScript.ScriptTblFunc SetEventBattle = 45;
	public const EventScript.ScriptTblFunc SetEventVec = 46;
	public const EventScript.ScriptTblFunc SetEventFEnemyOn = 47;
	public const EventScript.ScriptTblFunc SetEventFEnemyOff = 48;
	public const EventScript.ScriptTblFunc EventSePlay = 49;
	public const EventScript.ScriptTblFunc SetEventSceneChange = 50;
	public const EventScript.ScriptTblFunc ImageObjLoad = 51;
	public const EventScript.ScriptTblFunc SetEventAllCharCure = 52;
	public const EventScript.ScriptTblFunc MapU_DrawAutoMap = 53;
	public const EventScript.ScriptTblFunc SetEventDungeonWalk = 54;
	public const EventScript.ScriptTblFunc ClearFloorEnemyKillTrg = 55;
	public const EventScript.ScriptTblFunc CheckIllustration = 56;
	public const EventScript.ScriptTblFunc FoeSetReviveDay = 57;
	public const EventScript.ScriptTblFunc StopFoeNextTurn = 58;
	public const EventScript.ScriptTblFunc none = 59;
}
	 */




}

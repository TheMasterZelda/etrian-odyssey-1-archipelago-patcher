using static System.Runtime.InteropServices.JavaScript.JSType;

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
        // 10 = Condition
        // 11 = Condition
        // 12-13 = Event Index?
        // 14-17 = Script start offset.
        public EventEntry(byte[] blockData)
        {
            script_offset = BitConverter.ToUInt32(blockData, 0x14);

        }

        public uint script_offset;
    }


    /*
    // Namespace: 
    public enum EventCheck.CheckPlace // TypeDefIndex: 10762
{
	// Fields
	public int value__; // 0x0
    public const EventCheck.CheckPlace E_PLACE_NONE = -1;
    public const EventCheck.CheckPlace E_DUNGEON_COMMON = 0;
    public const EventCheck.CheckPlace E_SISETU_COMMON = 1;
    public const EventCheck.CheckPlace E_YADOYA_IN = 2;
    public const EventCheck.CheckPlace E_YADOYA_TALK = 3;
    public const EventCheck.CheckPlace E_YADOYA_OUT = 4;
    public const EventCheck.CheckPlace E_SHOP_IN = 5;
    public const EventCheck.CheckPlace E_SHOP_TALK = 6;
    public const EventCheck.CheckPlace E_SHOP_OUT = 7;
    public const EventCheck.CheckPlace E_TOUTI_IN = 8;
    public const EventCheck.CheckPlace E_TOUTI_TALK = 9;
    public const EventCheck.CheckPlace E_TOUTI_OUT = 10;
    public const EventCheck.CheckPlace E_TOUTI_UKERU = 11;
    public const EventCheck.CheckPlace E_TOUTI_HOUKOKU = 12;
    public const EventCheck.CheckPlace E_SAKABA_IN = 13;
    public const EventCheck.CheckPlace E_SAKABA_TALK = 14;
    public const EventCheck.CheckPlace E_SAKABA_OUT = 15;
    public const EventCheck.CheckPlace E_SAKABA_UKERU = 16;
    public const EventCheck.CheckPlace E_SAKABA_HOUKOKU = 17;
    public const EventCheck.CheckPlace E_GUILD_IN = 18;
    public const EventCheck.CheckPlace E_GUILD_TALK = 19;
    public const EventCheck.CheckPlace E_GUILD_OUT = 20;
    public const EventCheck.CheckPlace E_HOSPITAL_IN = 21;
    public const EventCheck.CheckPlace E_HOSPITAL_TALK = 22;
    public const EventCheck.CheckPlace E_HOSPITAL_OUT = 23;
    public const EventCheck.CheckPlace E_HIROBA_IN = 24;
    public const EventCheck.CheckPlace E_HIROBA_OUT = 25;
    public const EventCheck.CheckPlace E_JUKAI_ENTER_IN = 26;
    public const EventCheck.CheckPlace E_JUKAI_ENTER_OUT = 27;
    public const EventCheck.CheckPlace E_BATTLE_END = 28;
    public const EventCheck.CheckPlace E_DUNGEON_01F = 29;
    public const EventCheck.CheckPlace E_DUNGEON_02F = 30;
    public const EventCheck.CheckPlace E_DUNGEON_03F = 31;
    public const EventCheck.CheckPlace E_DUNGEON_04F = 32;
    public const EventCheck.CheckPlace E_DUNGEON_05F = 33;
    public const EventCheck.CheckPlace E_DUNGEON_06F = 34;
    public const EventCheck.CheckPlace E_DUNGEON_07F = 35;
    public const EventCheck.CheckPlace E_DUNGEON_08F = 36;
    public const EventCheck.CheckPlace E_DUNGEON_09F = 37;
    public const EventCheck.CheckPlace E_DUNGEON_10F = 38;
    public const EventCheck.CheckPlace E_DUNGEON_11F = 39;
    public const EventCheck.CheckPlace E_DUNGEON_12F = 40;
    public const EventCheck.CheckPlace E_DUNGEON_13F = 41;
    public const EventCheck.CheckPlace E_DUNGEON_14F = 42;
    public const EventCheck.CheckPlace E_DUNGEON_15F = 43;
    public const EventCheck.CheckPlace E_DUNGEON_16F = 44;
    public const EventCheck.CheckPlace E_DUNGEON_17F = 45;
    public const EventCheck.CheckPlace E_DUNGEON_18F = 46;
    public const EventCheck.CheckPlace E_DUNGEON_19F = 47;
    public const EventCheck.CheckPlace E_DUNGEON_20F = 48;
    public const EventCheck.CheckPlace E_DUNGEON_21F = 49;
    public const EventCheck.CheckPlace E_DUNGEON_22F = 50;
    public const EventCheck.CheckPlace E_DUNGEON_23F = 51;
    public const EventCheck.CheckPlace E_DUNGEON_24F = 52;
    public const EventCheck.CheckPlace E_DUNGEON_25F = 53;
    public const EventCheck.CheckPlace E_DUNGEON_26F = 54;
    public const EventCheck.CheckPlace E_DUNGEON_27F = 55;
    public const EventCheck.CheckPlace E_DUNGEON_28F = 56;
    public const EventCheck.CheckPlace E_DUNGEON_29F = 57;
    public const EventCheck.CheckPlace E_DUNGEON_30F = 58;
    public const EventCheck.CheckPlace E_SAKABA_HOUKOKU_MAE = 59;
    public const EventCheck.CheckPlace E_CAMP_BOOK_MAE = 60;
    public const EventCheck.CheckPlace E_PLACE_MAX = 61;
}
    */

    /*
     // Namespace: 
public enum EventCheck.EventDirType // TypeDefIndex: 10763
{
	// Fields
	public int value__; // 0x0
	public const EventCheck.EventDirType E_EVE_DIR_NONE = 0;
	public const EventCheck.EventDirType E_EVE_DIR_EAST = 1;
	public const EventCheck.EventDirType E_EVE_DIR_NORTH = 2;
	public const EventCheck.EventDirType E_EVE_DIR_WEST = 3;
	public const EventCheck.EventDirType E_EVE_DIR_SOUTH = 4;
	public const EventCheck.EventDirType E_EVE_DIR_ALL = 5;
	public const EventCheck.EventDirType E_EVE_DIR_MAX = 6;
}
     */
    /*
     

// Namespace: 
public enum EventSetFuncTbl.CommandId // TypeDefIndex: 10808
{
	// Fields
	public int value__; // 0x0
	public const EventSetFuncTbl.CommandId E_COMID_EVENT = 0;
	public const EventSetFuncTbl.CommandId E_COMID_EOF = 1;
	public const EventSetFuncTbl.CommandId E_COMID_MES_LOAD = 2;
	public const EventSetFuncTbl.CommandId E_COMID_MES_CITY = 3;
	public const EventSetFuncTbl.CommandId E_COMID_MES_DUNJON = 4;
	public const EventSetFuncTbl.CommandId E_COMID_MES_WIN_CLOSE = 5;
	public const EventSetFuncTbl.CommandId E_COMID_CHR_SET = 6;
	public const EventSetFuncTbl.CommandId E_COMID_CHR_IN = 7;
	public const EventSetFuncTbl.CommandId E_COMID_CHR_OUT = 8;
	public const EventSetFuncTbl.CommandId E_COMID_CHR_DEL = 9;
	public const EventSetFuncTbl.CommandId E_COMID_OBJ_SET = 10;
	public const EventSetFuncTbl.CommandId E_COMID_OBJ_IN = 11;
	public const EventSetFuncTbl.CommandId E_COMID_OBJ_OUT = 12;
	public const EventSetFuncTbl.CommandId E_COMID_OBJ_DEL = 13;
	public const EventSetFuncTbl.CommandId E_COMID_BGM_SET = 14;
	public const EventSetFuncTbl.CommandId E_COMID_BGM_ON = 15;
	public const EventSetFuncTbl.CommandId E_COMID_BGM_OFF = 16;
	public const EventSetFuncTbl.CommandId E_COMID_BGM_OUT = 17;
	public const EventSetFuncTbl.CommandId E_COMID_SE_ON = 18;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_THREE = 19;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_1 = 20;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_2 = 21;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_3 = 22;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_YESNO = 23;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_YES = 24;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_NO = 25;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_END = 26;
	public const EventSetFuncTbl.CommandId E_COMID_IF = 27;
	public const EventSetFuncTbl.CommandId E_COMID_IF_TURE = 28;
	public const EventSetFuncTbl.CommandId E_COMID_IF_FALSE = 29;
	public const EventSetFuncTbl.CommandId E_COMID_IF_END = 30;
	public const EventSetFuncTbl.CommandId E_COMID_EV_BATTLE = 31;
	public const EventSetFuncTbl.CommandId E_COMID_EV_ENMON = 32;
	public const EventSetFuncTbl.CommandId E_COMID_EV_ENMOFF = 33;
	public const EventSetFuncTbl.CommandId E_COMID_FLAGON = 34;
	public const EventSetFuncTbl.CommandId E_COMID_FLAGOFF = 35;
	public const EventSetFuncTbl.CommandId E_COMID_P_WARP = 36;
	public const EventSetFuncTbl.CommandId E_COMID_P_SIGHT = 37;
	public const EventSetFuncTbl.CommandId E_COMID_EV_GET_GOLD = 38;
	public const EventSetFuncTbl.CommandId E_COMID_EV_LOST_GOLD = 39;
	public const EventSetFuncTbl.CommandId E_COMID_EV_GET_ITEM = 40;
	public const EventSetFuncTbl.CommandId E_COMID_EV_LOST_ITEM = 41;
	public const EventSetFuncTbl.CommandId E_COMID_EV_GET_HP = 42;
	public const EventSetFuncTbl.CommandId E_COMID_EV_LOST_HP = 43;
	public const EventSetFuncTbl.CommandId E_COMID_EV_GET_TP = 44;
	public const EventSetFuncTbl.CommandId E_COMID_EV_LOST_TP = 45;
	public const EventSetFuncTbl.CommandId E_COMID_DISP_SHAKE = 46;
	public const EventSetFuncTbl.CommandId E_COMID_DISP_FADEOUT = 47;
	public const EventSetFuncTbl.CommandId E_COMID_DISP_FADEIN = 48;
	public const EventSetFuncTbl.CommandId E_COMID_LABEL = 49;
	public const EventSetFuncTbl.CommandId E_COMID_LABEL_DEFINE = 50;
	public const EventSetFuncTbl.CommandId E_COMID_JUMP_LABEL = 51;
	public const EventSetFuncTbl.CommandId E_COMID_WAIT = 52;
	public const EventSetFuncTbl.CommandId E_COMID_EV_START = 53;
	public const EventSetFuncTbl.CommandId E_COMID_EV_END = 54;
	public const EventSetFuncTbl.CommandId E_COMID_EV_START_NUM = 55;
	public const EventSetFuncTbl.CommandId E_COMID_EV_START_NUM_FROM = 56;
	public const EventSetFuncTbl.CommandId E_COMID_EV_START_NUM_TO = 57;
	public const EventSetFuncTbl.CommandId E_COMID_IF_2 = 58;
	public const EventSetFuncTbl.CommandId E_COMID_IF_TURE_2 = 59;
	public const EventSetFuncTbl.CommandId E_COMID_IF_FALSE_2 = 60;
	public const EventSetFuncTbl.CommandId E_COMID_IF_END_2 = 61;
	public const EventSetFuncTbl.CommandId E_COMID_IF_3 = 62;
	public const EventSetFuncTbl.CommandId E_COMID_IF_TURE_3 = 63;
	public const EventSetFuncTbl.CommandId E_COMID_IF_FALSE_3 = 64;
	public const EventSetFuncTbl.CommandId E_COMID_IF_END_3 = 65;
	public const EventSetFuncTbl.CommandId E_COMID_TMP_FLAG_ON = 66;
	public const EventSetFuncTbl.CommandId E_COMID_TMP_FLAG_OFF = 67;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_THREE_2 = 68;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_1_2 = 69;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_2_2 = 70;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_3_2 = 71;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_YESNO_2 = 72;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_YES_2 = 73;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_NO_2 = 74;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_END_2 = 75;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_THREE_3 = 76;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_1_3 = 77;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_2_3 = 78;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_3_3 = 79;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_YESNO_3 = 80;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_YES_3 = 81;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_NO_3 = 82;
	public const EventSetFuncTbl.CommandId E_COMID_SEL_END_3 = 83;
	public const EventSetFuncTbl.CommandId E_COMID_PARTY_MOVE = 84;
	public const EventSetFuncTbl.CommandId E_COMID_SET_TGT_ITEM_1 = 85;
	public const EventSetFuncTbl.CommandId E_COMID_SET_TGT_ITEM_2 = 86;
	public const EventSetFuncTbl.CommandId E_COMID_SET_TGT_ITEM_3 = 87;
	public const EventSetFuncTbl.CommandId E_COMID_SET_TGT_ITEM_4 = 88;
	public const EventSetFuncTbl.CommandId E_COMID_SET_TGT_ITEM_5 = 89;
	public const EventSetFuncTbl.CommandId E_COMID_KEY_WAIT = 90;
	public const EventSetFuncTbl.CommandId E_COMID_TIME_WND_ON = 91;
	public const EventSetFuncTbl.CommandId E_COMID_TIME_WND_OFF = 92;
	public const EventSetFuncTbl.CommandId E_COMID_MINING = 93;
	public const EventSetFuncTbl.CommandId E_COMID_PICK = 94;
	public const EventSetFuncTbl.CommandId E_COMID_CUT = 95;
	public const EventSetFuncTbl.CommandId E_COMID_VALUE_1 = 96;
	public const EventSetFuncTbl.CommandId E_COMID_VALUE_2 = 97;
	public const EventSetFuncTbl.CommandId E_COMID_VALUE_3 = 98;
	public const EventSetFuncTbl.CommandId E_COMID_VALUE_4 = 99;
	public const EventSetFuncTbl.CommandId E_COMID_VALUE_5 = 100;
	public const EventSetFuncTbl.CommandId E_COMID_P_DIR = 101;
	public const EventSetFuncTbl.CommandId E_COMID_SCENE_CHANGE = 102;
	public const EventSetFuncTbl.CommandId E_COMID_OBJ_CROSS_FADE = 103;
	public const EventSetFuncTbl.CommandId E_COMID_OBJ_SET_2 = 104;
	public const EventSetFuncTbl.CommandId E_COMID_OBJ_IN_2 = 105;
	public const EventSetFuncTbl.CommandId E_COMID_OBJ_OUT_2 = 106;
	public const EventSetFuncTbl.CommandId E_COMID_OBJ_DEL_2 = 107;
	public const EventSetFuncTbl.CommandId E_COMID_SET_TGT_PLAYER = 108;
	public const EventSetFuncTbl.CommandId E_COMID_SET_TGT_SKILL_01 = 109;
	public const EventSetFuncTbl.CommandId E_COMID_SET_TGT_SKILL_02 = 110;
	public const EventSetFuncTbl.CommandId E_COMID_SET_TGT_SKILL_03 = 111;
	public const EventSetFuncTbl.CommandId E_COMID_SET_TGT_SKILL_04 = 112;
	public const EventSetFuncTbl.CommandId E_COMID_SET_TGT_SKILL_05 = 113;
	public const EventSetFuncTbl.CommandId E_COMID_OBJ_PRIO = 114;
	public const EventSetFuncTbl.CommandId E_COMID_OBJ_PRIO_2 = 115;
	public const EventSetFuncTbl.CommandId E_COMID_EV_CURE_ALL = 116;
	public const EventSetFuncTbl.CommandId E_COMID_DRAW_MAP = 117;
	public const EventSetFuncTbl.CommandId E_COMID_ENEMY_CREATE_ON = 118;
	public const EventSetFuncTbl.CommandId E_COMID_ENEMY_CREATE_OFF = 119;
	public const EventSetFuncTbl.CommandId E_COMID_SET_FG_FENEMY_KILL = 120;
	public const EventSetFuncTbl.CommandId E_COMID_MAP_FADE_IN = 121;
	public const EventSetFuncTbl.CommandId E_COMID_SET_SHOP_LIST_FG = 122;
	public const EventSetFuncTbl.CommandId E_COMID_SET_DUNGEON_WALK = 123;
	public const EventSetFuncTbl.CommandId E_COMID_EV_GET_HP_NOT_ARRIVE = 124;
	public const EventSetFuncTbl.CommandId E_COMID_EV_LOST_HP_DEAD = 125;
	public const EventSetFuncTbl.CommandId E_COMID_CLEAR_FG_FENEMY_KILL = 126;
	public const EventSetFuncTbl.CommandId E_COMID_CHECK_NEW_ILLUSTRATION = 127;
	public const EventSetFuncTbl.CommandId E_COMID_FENEMY_CLEAR_REVIVEDAY = 128;
	public const EventSetFuncTbl.CommandId E_COMID_FENEMY_RESET_REVIVEDAY = 129;
	public const EventSetFuncTbl.CommandId E_COMID_STOP_FOE_NEXT_TURN = 130;
	public const EventSetFuncTbl.CommandId E_COMID_MAX = 131;
}
     */









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

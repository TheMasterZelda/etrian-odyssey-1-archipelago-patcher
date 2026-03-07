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
    }
}

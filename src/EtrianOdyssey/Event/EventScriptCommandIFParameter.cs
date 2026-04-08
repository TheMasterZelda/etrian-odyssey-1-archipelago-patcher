namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Event
{
    public class EventScriptCommandIFParameter
    {
        public override string ToString()
        {
            return $"{parameter1}|{parameter2}|{parameter3}|{parameter4}";
        }

        public ushort parameter1;
        public ushort parameter2;
        public ushort parameter3;
        public ushort parameter4;
    }
}

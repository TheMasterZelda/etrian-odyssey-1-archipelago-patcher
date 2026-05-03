using System.Text;

namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Event
{
    public class EventScriptCommand
    {
        public EventScriptCommand(EventCommandId commandId, object[] parameters)
        {
            CommandId = commandId;
            Parameters = parameters;
        }

        public string GetParametersAsString(EtrianString[] item_names)
        {
            if (CommandId == EventCommandId.E_COMID_FLAGON ||
                CommandId == EventCommandId.E_COMID_FLAGOFF ||
                CommandId == EventCommandId.E_COMID_TMP_FLAG_ON ||
                CommandId == EventCommandId.E_COMID_TMP_FLAG_OFF)
                return $"0x{(ushort)Parameters[0]:X3}";

            if (CommandId == EventCommandId.E_COMID_EV_GET_ITEM ||
                CommandId == EventCommandId.E_COMID_EV_LOST_ITEM)
                return $"{item_names[(ushort)Parameters[0] - 1]} (0x{(ushort)Parameters[0]:X4})";

            return string.Join('+', Parameters);
        }

        public override string ToString()
        {
            return $"{CommandId}+{Parameters.Length}";
        }

        public EventCommandId CommandId;
        public object[] Parameters;
    }
}

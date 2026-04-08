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

        public string GetParametersAsString()
        {
            if (CommandId == EventCommandId.E_COMID_FLAGON ||
                CommandId == EventCommandId.E_COMID_FLAGOFF ||
                CommandId == EventCommandId.E_COMID_TMP_FLAG_ON ||
                CommandId == EventCommandId.E_COMID_TMP_FLAG_OFF)
                return $"0x{(ushort)Parameters[0]:X4}";


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

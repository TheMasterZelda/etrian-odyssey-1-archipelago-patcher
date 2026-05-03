using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Data
{
    public class EventIndexDataParam
    {
        public EventIndexDataParam(byte[] data)
        {
            event_index_id = BitConverter.ToInt16(data, 0);
            condition = BitConverter.ToInt16(data, 2);
        }

        public short event_index_id;
        public short condition;
    }
}

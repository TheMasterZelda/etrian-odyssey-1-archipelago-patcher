using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Data
{
    public class EventIndexDataMain
    {
        public EventIndexDataMain(byte[] data) 
        {
            event_index_id = BitConverter.ToInt16(data, 0);
            condition = BitConverter.ToInt16(data, 2);
        }

        public short event_index_id;

        // 0 = Always true
        // 1 = Inventory related
        // 2 = Check Exploration data ?? isn't equal 1 or 2 + floor + unknown.
        // 3 = Unused. Map Walk Check.
        // 4 = Unused.
        // 5 = Unused.
        // 6 = Compendium related
        // 7 = Compendium or Codex related.
        // 8 = Mapping Check. Unused.
        // 9 = FOE related (used by quest 68)
        // A = Party related (used for... 3 quests?)
        // B = map validation (used for B11F and B12F/Mission 5)
        public short condition;


        // parameters per condition:

        // 1:
        // 00-03= ItemID
        // 04-07= ItemCount
        // repeat. -1 mean NULL.
        
        // 2:
        // 00-03= Floor Number (0 base)
        // 04-07= Unknown Exploration variable.
        
        // 3: (unused)
        // 00-03= Floor Number (0 base)
        // 04-07= Walk Parameter
        
        // 6:
        // 00-03= Entry Count.
        
        // 7:
        // 00-03= Entry Count.
        
        // 8:
        // 00-03= Mapping Criteria.

        // 9:
        // 00-03= Floor Number (0 base)
        // 04-07= ??? Criteria
        // 08-0B= ?? Criteria
        // 0C-0F= ?? Criteria
        // 10-13= ?? Criteria
        // 14-17= ?? Criteria

        // A:
        // 00-03= Related to Skill ID. (0=0x80,1=0x87,2=0x81,3=0x8A,4=0xB6,5=0x88,6=0x8C)
        // 04-07= Skill Level.

        // B:
        // 00-03= Walk criteria.
        // 04-07= Mapping criteria.
        public byte[][] parameters = new byte[12][];
    }
}

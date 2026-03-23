using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Data
{
    public class GatherItemData
    {
        public GatherItemData(byte[] data, Dictionary<ushort, ItemOther> materials)
        {
            // Taken mostly from Yggdrasil.
            gatherNumber = BitConverter.ToUInt16(data, 0);

            // Padding 0s.
            unknown02 = BitConverter.ToUInt16(data, 2);
            floorNumber = data[4];
            xCoord = data[5];
            yCoord = data[6];
            unknown07 = data[7];
            itemID1 = BitConverter.ToUInt16(data, 8);
            Item1Name = materials[itemID1].name.StringValue;

            itemID2 = BitConverter.ToUInt16(data, 0xA);
            Item2Name = materials[itemID2].name.StringValue;

            itemID3 = BitConverter.ToUInt16(data, 0xC);
            Item3Name = materials[itemID3].name.StringValue;

            itemProbability1 = BitConverter.ToUInt16(data, 0xE);
            itemProbability2 = BitConverter.ToUInt16(data, 0x10);
            itemProbability3 = BitConverter.ToUInt16(data, 0x12);
            unknown14 = BitConverter.ToUInt32(data, 0x14);
            unknown18 = BitConverter.ToUInt32(data, 0x18);
            unknown1C = BitConverter.ToUInt32(data, 0x1C);
        }

        public string Item1Name;
        public string Item2Name;
        public string Item3Name;

        public ushort gatherNumber; // Gathering spot id?
        public ushort unknown02; // Padding 0s.
        public byte floorNumber;
        public byte xCoord;
        public byte yCoord;
        public byte unknown07; // Padding 0s.
        public ushort itemID1;
        public ushort itemID2;
        public ushort itemID3;
        public ushort itemProbability1;
        public ushort itemProbability2;
        public ushort itemProbability3;
        public uint unknown14; // Always -1/FFFFFFFF.
        public uint unknown18; // Always -1/FFFFFFFF.
        public uint unknown1C; // Always -1/FFFFFFFF.
    }
}

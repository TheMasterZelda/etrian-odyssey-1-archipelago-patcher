using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Data
{
    public class FEnemyData0
    {
        public class Coordinate
        {
            public int X; 
            public int Y;

            public override string ToString()
            {
                return $"{X},{Y}";
            }
        }

        public FEnemyData0(byte[] data)
        {
            enemy_id = BitConverter.ToUInt32(data, 0x0);

            unknown_04 = BitConverter.ToUInt32(data, 0x4);

            coord1 = new Coordinate();
            coord2 = new Coordinate();
            coord3 = new Coordinate();
            Coordinate[] coords = new Coordinate[]
            {
                coord1,
                coord2,
                coord3
            };

            for (int i = 0; i < 3; i++)
            {
                coords[i].X = BitConverter.ToInt32(data, 0x08 + i * 8);
                coords[i].Y = BitConverter.ToInt32(data, 0x08 + 4 + i * 8);
            }


            unknown_80 = new int[6];
            unknown_A0 = new int[6];
            for (int i = 0; i < 6; i++)
            {
                unknown_80[i] = BitConverter.ToInt32(data, 0x80 + i * 4);
                unknown_A0[i] = BitConverter.ToInt32(data, 0xA0 + i * 4);
            }

            respawn_timer = data[0xEC];


            everything_else = string.Join(' ', data.Skip(0x20).ToArray().Select(s => s.ToString("X2")));
        }

        // 0x00-0x03
        public uint enemy_id;

        // 0x04-0x07
        public uint unknown_04; // Initial Facing Direction

        // 0x08-0x1F
        public Coordinate coord1;
        public Coordinate coord2;
        public Coordinate coord3;

        // 0x80-0x97
        public int[] unknown_80;

        // 0xA0-0xB7
        public int[] unknown_A0;

        // 0xEC
        public byte respawn_timer;


        public string everything_else;
    }
}

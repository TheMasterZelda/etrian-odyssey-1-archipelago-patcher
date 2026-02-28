using etrian_odyssey_ap_patcher.DataCompression;
using etrian_odyssey_ap_patcher.EtrianOdyssey.MapData;
using System.ComponentModel;
using System.IO.Compression;
using System.Text;

namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Files
{
    public class MapDataFile : BaseFile
    {
        public const string MAGIC_NUMBER = "YGMD";

        public const int MapWidth = 35;
        public const int MapHeight = 30;
        public const int MapDataOffset = 0x10;

        public enum TileTypes : byte
        {
            Nothing = 0x0,
            Floor = 0x1,
            Wall = 0x2,
            [Description("Stairs (Up)")]
            StairsUp = 0x3,
            [Description("Stairs (Down)")]
            StairsDown = 0x4,
            [Description("One-way Shortcut (N)")]
            OneWayShortcutN = 0x5,
            [Description("One-way Shortcut (S)")]
            OneWayShortcutS = 0x6,
            [Description("One-way Shortcut (W)")]
            OneWayShortcutW = 0x7,
            [Description("One-way Shortcut (E)")]
            OneWayShortcutE = 0x8,
            [Description("Door (N-S)")]
            DoorNS = 0x9,
            [Description("Door (W-E)")]
            DoorWE = 0xA,
            [Description("Treasure Chest")]
            TreasureChest = 0xB,
            [Description("Geomagnetic Field")]
            GeomagneticField = 0xC,
            [Description("Conveyor (N)")]
            SandConveyorN = 0xD,
            [Description("Conveyor (S)")]
            SandConveyorS = 0xE,
            [Description("Conveyor (W)")]
            SandConveyorW = 0xF,
            [Description("Conveyor (E)")]
            SandConveyorE = 0x10,
            [Description("F.O.E. Floor")]
            FOEFloor = 0x11, /* not sure, but seems FOE-related */
            [Description("Collapsing Floor")]
            CollapsingFloor = 0x12,
            Water = 0x13,
            Elevator = 0x14,
            [Description("Refreshing Water")]
            RefreshingWater = 0x15,
            [Description("Warp Entrance")]
            WarpEntrance = 0x16,
            Transporter = 0x17,
            [Description("Damaging Floor")]
            DamagingFloor = 0x18,
            [Description("Unknown (0x19)")]
            Unknown0x19 = 0x19,
        };

        public MapDataFile(byte[] fileData, CompressionType compressionType, bool isNestedFile) : base(fileData, compressionType, isNestedFile)
        {
        }
        public MapDataFile(byte[] fileData) : base(fileData) { }


        public override string MagicNumber => MAGIC_NUMBER;

        //public int FloorNumber { get { return int.Parse(System.Text.RegularExpressions.Regex.Match(Path.GetFileNameWithoutExtension(Filename), @"\d+").Value); } }
        //public string FloorName { get { return string.Format("B{0}F", FloorNumber); } }
        public uint Unknown1 { get; private set; }
        public uint Unknown2 { get; private set; }
        public uint Unknown3 { get; private set; }
        public byte[] UnknownBlock { get; private set; }

        public BaseTile[,] Tiles { get; private set; }

        private BaseTile[,] ParseMapData()
        {
            BaseTile[,] mapData = new BaseTile[MapWidth, MapHeight];

            DataStream.Seek(MapDataOffset, SeekOrigin.Begin);

            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    int offset = (int)DataStream.Position;
                    TileTypes typeId = (TileTypes)DataStream.ReadByte();

                    DataStream.Seek(offset, SeekOrigin.Begin);
                    byte[] data = new byte[16];
                    DataStream.Read(data, 0, 16);

                    mapData[x, y] = CreateTileObject(typeId, data, x, y);
                }
            }

            return mapData;
        }

        private static BaseTile CreateTileObject(TileTypes typeId, byte[] tileData, int x, int y)
        {
            switch (typeId)
            {
                case TileTypes.TreasureChest:
                    return new TreasureChestTile(tileData, x, y);
                case TileTypes.Floor:
                case TileTypes.FOEFloor:
                case TileTypes.CollapsingFloor:
                case TileTypes.DamagingFloor:
                    return new FloorTile(tileData, x, y);
                case TileTypes.Nothing:
                case TileTypes.Wall:
                case TileTypes.StairsUp:
                case TileTypes.StairsDown:
                case TileTypes.OneWayShortcutN:
                case TileTypes.OneWayShortcutS:
                case TileTypes.OneWayShortcutW:
                case TileTypes.OneWayShortcutE:
                case TileTypes.DoorNS:
                case TileTypes.DoorWE:
                case TileTypes.GeomagneticField:
                case TileTypes.SandConveyorN:
                case TileTypes.SandConveyorS:
                case TileTypes.SandConveyorW:
                case TileTypes.SandConveyorE:
                case TileTypes.Water:
                case TileTypes.Elevator:
                case TileTypes.RefreshingWater:
                case TileTypes.WarpEntrance:
                case TileTypes.Transporter:
                case TileTypes.Unknown0x19:
                default:
                    return new OtherTile(tileData, x, y); ;
            }
        }

        public override void Parse()
        {
            BinaryReader reader = new BinaryReader(DataStream);

            reader.BaseStream.Seek(4, SeekOrigin.Begin);
            Unknown1 = reader.ReadUInt32();
            Unknown2 = reader.ReadUInt32();
            Unknown3 = reader.ReadUInt32();

            Tiles = ParseMapData();

            int unknownBlockOffset = MapDataOffset + MapWidth * MapHeight * 0x10;
            if (unknownBlockOffset < reader.BaseStream.Length)
            {
                reader.BaseStream.Seek(unknownBlockOffset, SeekOrigin.Begin);
                UnknownBlock = reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position));
            }
        }

        protected override byte[] Save()
        {
            List<byte> rebuilt = new List<byte>();

            rebuilt.AddRange(Encoding.ASCII.GetBytes(MagicNumber));
            rebuilt.AddRange(BitConverter.GetBytes(Unknown1));
            rebuilt.AddRange(BitConverter.GetBytes(Unknown2));
            rebuilt.AddRange(BitConverter.GetBytes(Unknown3));

            // TODO call update on the tiles.

            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    Tiles[x, y].Save();
                    rebuilt.AddRange(Tiles[x, y].TileData);
                }
            }

            rebuilt.AddRange(UnknownBlock);

            return rebuilt.ToArray();
        }
    }
}

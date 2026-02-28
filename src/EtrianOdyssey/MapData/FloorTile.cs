namespace etrian_odyssey_ap_patcher.EtrianOdyssey.MapData
{
    public class FloorTile : BaseTile
    {
        public FloorTile(byte[] tileData, int x, int y) : base(tileData, x, y)
        {
        }

        public uint unknown4;
        public uint unknown3;
        public ushort encounterGroup;
        public ushort unknown2;
        public ushort unknown1;
        public byte dangerIncrement;

        public override void Save()
        {
        }

        protected override void Load()
        {
            dangerIncrement = TileData[1];
            unknown1 = BitConverter.ToUInt16(TileData, 2);
            unknown2 = BitConverter.ToUInt16(TileData, 4);
            encounterGroup = BitConverter.ToUInt16(TileData, 6);
            unknown3 = BitConverter.ToUInt32(TileData, 8);
            unknown4 = BitConverter.ToUInt32(TileData, 12);
        }
    }
}

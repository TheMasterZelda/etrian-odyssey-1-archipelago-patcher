using etrian_odyssey_ap_patcher.EtrianOdyssey.Files;

namespace etrian_odyssey_ap_patcher.EtrianOdyssey.MapData
{
    public abstract class BaseTile
    {
        public readonly int XCoord;
        public readonly int YCoord;
        public byte[] TileData { get; private set; }
        public MapDataFile.TileTypes TileType { get; private set; }

        public BaseTile(byte[] tileData, int x, int y)
        {
            if (tileData.Length != 16)
                throw new Exception("Invalid length tile data.");

            TileData = tileData;
            XCoord = x;
            YCoord = y;

            TileType = (MapDataFile.TileTypes)TileData[0];

            Load();
        }

        protected abstract void Load();

        public abstract void Save();
    }
}

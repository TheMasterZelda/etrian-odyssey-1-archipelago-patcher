namespace etrian_odyssey_ap_patcher.EtrianOdyssey.MapData
{
    public class OtherTile : BaseTile
    {
        public OtherTile(byte[] tileData, int x, int y) : base(tileData, x, y)
        {
        }

        public override void Save()
        {
            // Do nothing.
        }

        protected override void Load()
        {
            // Do nothing.
        }
    }
}

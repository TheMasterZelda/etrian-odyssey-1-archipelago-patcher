using etrian_odyssey_ap_patcher.Util;

namespace etrian_odyssey_ap_patcher.EtrianOdyssey.MapData
{
    public enum TreasureType : byte
    {
        Money = 0x2,
        Item = 0x3,
        AP = 0x4,
        Floor = 0x5,
        Level = 0x6,

    };

    public enum TreasureItemCategory : byte
    {
        Equipment = 0x0,
        Consumables = 0x1,
    }

    public class TreasureChestTile : BaseTile
    {
        public TreasureChestTile(byte[] tileData, int x, int y) : base(tileData, x, y)
        {
        }

        public TreasureType treasureType;
        public byte treasureChestID;
        public TreasureItemCategory treasureItemCategory;
        public ushort treasureItemID;
        public ushort treasureMoney;

        protected override void Load()
        {
            treasureType = (TreasureType)(TileData[15] >> 4);
            treasureChestID = (byte)(TileData[15] & 0xF);

            switch (treasureType)
            {
                case TreasureType.Item:
                    //this.ChangeBrowsableAttribute("TreasureItemCategory", true);
                    //this.ChangeBrowsableAttribute("TreasureItemID", true);
                    treasureItemCategory = (TreasureItemCategory)(TileData[13] >> 4);
                    treasureItemID = BitConverter.ToUInt16(TileData, 12);

                    //this.ChangeBrowsableAttribute("TreasureMoney", false);
                    break;

                case TreasureType.Money:
                    //this.ChangeBrowsableAttribute("TreasureItemCategory", false);
                    //this.ChangeBrowsableAttribute("TreasureItemID", false);

                    //this.ChangeBrowsableAttribute("TreasureMoney", true);
                    treasureMoney = BitConverter.ToUInt16(TileData, 12);
                    break;
            }
        }

        public override void Save()
        {
            TileData[15] = (byte)((Convert.ToByte(treasureType) << 4) | (treasureChestID & 0xF));

            switch (treasureType)
            {
                case TreasureType.Money:
                    ByteUtil.Write(TileData, 12, treasureMoney);
                    break;
                case TreasureType.Item:
                    ByteUtil.Write(TileData, 12, treasureItemID);
                    break;
                case TreasureType.AP:
                case TreasureType.Floor:
                case TreasureType.Level:
                    break;
            }
        }

    }
}

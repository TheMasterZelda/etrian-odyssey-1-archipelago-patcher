using System.Text;

namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Table
{
    public class DataTable : BaseTable
    {
        public const string MAGIC_NUMBER = "TBL1";

        public override string MagicNumber => MAGIC_NUMBER;

        public uint Unknown { get; private set; }
        public uint DataSize { get; private set; }
        public uint EntrySize { get; private set; }

        int numEntries;

        public DataTable(MemoryStream tableFileData, int offset) : base(tableFileData, offset)
        {
        }

        public int[] DataOffsets { get; private set; }
        public byte[][] Data { get; private set; }

        public override byte[] Rebuild()
        {
            List<byte> rebuilt = new List<byte>();

            rebuilt.AddRange(Encoding.ASCII.GetBytes(MagicNumber));
            rebuilt.AddRange(BitConverter.GetBytes(Unknown));
            rebuilt.AddRange(BitConverter.GetBytes(DataSize));
            rebuilt.AddRange(BitConverter.GetBytes(EntrySize));

            foreach (byte[] entryData in Data) rebuilt.AddRange(entryData);
            rebuilt.AddRange(new byte[(YggdrasilHelper.Round(rebuilt.Count, 16) - rebuilt.Count)]);

            return rebuilt.ToArray();
        }

        protected override void Parse(MemoryStream tableFileData, int offset)
        {
            BinaryReader reader = new BinaryReader(tableFileData);

            reader.BaseStream.Seek(offset + 4, SeekOrigin.Begin);
            Unknown = reader.ReadUInt32();
            DataSize = reader.ReadUInt32();
            EntrySize = reader.ReadUInt32();

            numEntries = (int)(DataSize / EntrySize);
            DataOffsets = new int[numEntries];
            Data = new byte[numEntries][];

            for (int i = 0; i < numEntries; i++)
            {
                DataOffsets[i] = (int)(16 + (i * EntrySize));
                Data[i] = new byte[EntrySize];

                reader.BaseStream.Seek(offset + DataOffsets[i], SeekOrigin.Begin);
                Data[i] = reader.ReadBytes((int)EntrySize);
            }
        }
    }
}

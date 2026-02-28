using etrian_odyssey_ap_patcher.DataCompression;
using etrian_odyssey_ap_patcher.EtrianOdyssey.Table;
using System.Text;

namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Files
{
    public class TableFile : BaseFile
    {
        public const string MAGIC_NUMBER = "TBB1";

        public TableFile(byte[] fileData, CompressionType compressionType, bool isNestedFile) : base(fileData, compressionType, isNestedFile)
        {
        }
        public TableFile(byte[] fileData) : base(fileData) { }

        public override string MagicNumber => MAGIC_NUMBER;
        public uint Unknown { get; private set; }
        public uint NumTables { get; private set; }
        public uint FileSize { get; private set; }
        public uint[] TableOffsets { get; private set; }
        public BaseTable[] Tables { get; private set; }

        private BaseTable CreateTableObject(string magicNumber, MemoryStream tableFileData, int offset)
        {
            switch (magicNumber)
            {
                case DataTable.MAGIC_NUMBER:
                    return new DataTable(tableFileData, offset);
                case MessageTable.MAGIC_NUMBER:
                    return new MessageTable(tableFileData, offset);
                default:
                    throw new NotImplementedException();
            }
        }

        public override void Parse()
        {
            BinaryReader reader = new BinaryReader(DataStream);

            reader.BaseStream.Seek(4, SeekOrigin.Begin);
            Unknown = reader.ReadUInt32();
            NumTables = reader.ReadUInt32();
            FileSize = reader.ReadUInt32();

            TableOffsets = new uint[NumTables];
            for (int i = 0; i < NumTables; i++)
            {
                reader.BaseStream.Seek(16 + i * sizeof(uint), SeekOrigin.Begin);
                TableOffsets[i] = reader.ReadUInt32();
            }

            Tables = new BaseTable[NumTables];
            for (int i = 0; i < NumTables; i++)
            {
                string tableSignature = Encoding.ASCII.GetString(DataStream.ToArray(), (int)TableOffsets[i], 4);
                Tables[i] = CreateTableObject(tableSignature, DataStream, (int)TableOffsets[i]);
            }
        }

        protected override byte[] Save()
        {
            List<byte> rebuilt = new List<byte>();

            rebuilt.AddRange(Encoding.ASCII.GetBytes(MagicNumber));
            rebuilt.AddRange(BitConverter.GetBytes(Unknown));
            rebuilt.AddRange(BitConverter.GetBytes(NumTables));
            rebuilt.AddRange(BitConverter.GetBytes(FileSize));

            int tableDataLocation = YggdrasilHelper.Round(((int)(rebuilt.Count + NumTables * sizeof(uint))), 16);

            List<int> tableOffsets = new List<int>();
            List<byte> tableData = new List<byte>();

            int offset = tableDataLocation;
            for (int i = 0; i < NumTables; i++)
            {
                tableData.AddRange(Tables[i].Rebuild());
                tableData.AddRange(new byte[(YggdrasilHelper.Round(tableData.Count, 16) - tableData.Count)]);

                tableOffsets.Add(offset);
                offset = tableDataLocation + tableData.Count;
            }

            foreach (int tableOffset in tableOffsets) rebuilt.AddRange(BitConverter.GetBytes(tableOffset));
            rebuilt.AddRange(new byte[(YggdrasilHelper.Round(rebuilt.Count, 16) - rebuilt.Count)]);

            rebuilt.AddRange(tableData);

            return rebuilt.ToArray();
        }
    }
}

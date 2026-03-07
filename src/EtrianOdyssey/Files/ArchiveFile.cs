using etrian_odyssey_ap_patcher.DataCompression;
using System.Text;

namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Files
{
    public class ArchiveFile : BaseFile
    {
        public const string MAGIC_NUMBER = "FBIN";

        public ArchiveFile(byte[] fileData, CompressionType compressionType, bool isNestedFile) : base(fileData, compressionType, isNestedFile)
        {
        }
        public ArchiveFile(byte[] fileData) : base(fileData) { }

        public override string MagicNumber => MAGIC_NUMBER;
        public uint NumBlocks { get; private set; }
        public uint DataOffset { get; private set; }
        public uint[] BlockLengths { get; private set; }
        public BaseFile[] Blocks { get; private set; }

        private BaseFile CreateFileBlock(byte[] blockData, CompressionType compressionType)
        {
            string fileSignature = Encoding.ASCII.GetString(blockData, 0, 4);

            switch (fileSignature)
            {
                case ArchiveFile.MAGIC_NUMBER:
                    return new ArchiveFile(blockData, compressionType, true);
                case MapDataFile.MAGIC_NUMBER:
                    return new MapDataFile(blockData, compressionType, true);
                case TableFile.MAGIC_NUMBER:
                    return new TableFile(blockData, compressionType, true);
                default:
                    throw new NotImplementedException();
            }
        }

        public override void Parse()
        {
            BinaryReader reader = new BinaryReader(DataStream);

            reader.BaseStream.Seek(4, SeekOrigin.Begin);
            NumBlocks = reader.ReadUInt32();
            DataOffset = reader.ReadUInt32();

            BlockLengths = new uint[NumBlocks];
            for (int i = 0; i < NumBlocks; i++) BlockLengths[i] = reader.ReadUInt32();

            DataStream.Seek(DataOffset, SeekOrigin.Begin);

            Blocks = new BaseFile[NumBlocks];
            uint offset = DataOffset;
            for (int i = 0; i < NumBlocks; i++)
            {
                uint length = BlockLengths[i];
                byte[] data = new byte[length];

                DataStream.Read(data, 0, (int)length);

                CompressionType compressionType;
                data = CompressionHelper.DetectCompressionTypeAndDecompressIfRequired(data, out compressionType);

                DataStream.Seek(offset + BlockLengths[i], SeekOrigin.Begin);

                offset += BlockLengths[i];

                Blocks[i] = CreateFileBlock(data, compressionType);
            }
        }

        protected override byte[] Save()
        {
            List<byte> rebuilt = new List<byte>();

            rebuilt.AddRange(Encoding.ASCII.GetBytes(MagicNumber));
            rebuilt.AddRange(BitConverter.GetBytes(NumBlocks));
            rebuilt.AddRange(BitConverter.GetBytes(DataOffset));

            List<int> blockLengths = new List<int>();
            List<byte> blocks = new List<byte>();

            for (int i = 0; i < NumBlocks; i++)
            {
                byte[] blockData = Blocks[i].GetUpdatedData();

                blocks.AddRange(blockData);

                blockLengths.Add(blockData.Length);
            }

            foreach (int tableOffset in blockLengths) rebuilt.AddRange(BitConverter.GetBytes(tableOffset));
            rebuilt.AddRange(new byte[(YggdrasilHelper.Round(rebuilt.Count, 16) - rebuilt.Count)]);

            rebuilt.AddRange(blocks);

            return rebuilt.ToArray();
        }
    }
}

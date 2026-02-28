using System.Text;

namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Table
{
    public class MessageTable : BaseTable
    {
        public const string MAGIC_NUMBER = "MTBL";

        public MessageTable(MemoryStream tableFileData, int offset) : base(tableFileData, offset)
        {
        }

        public override string MagicNumber => MAGIC_NUMBER;
        public uint Unknown { get; private set; }
        public uint Size { get; private set; }
        public uint NumMessages { get; private set; }
        public uint Unknown2 { get; private set; }
        public uint[] MessageOffsets { get; private set; }

        public EtrianString[] Messages { get; private set; }
        EtrianString[] originalMessages;

        public bool HasChanges { get { return !YggdrasilHelper.CompareElements(originalMessages, Messages, new EtrianString.EtrianStringComparer()); } }

        public override byte[] Rebuild()
        {
            List<byte> rebuilt = new List<byte>();

            rebuilt.AddRange(Encoding.ASCII.GetBytes(MagicNumber));
            rebuilt.AddRange(BitConverter.GetBytes(Unknown));
            rebuilt.AddRange(BitConverter.GetBytes(Size));
            rebuilt.AddRange(BitConverter.GetBytes(NumMessages));
            rebuilt.AddRange(BitConverter.GetBytes(Unknown2));

            int messageDataLocation = YggdrasilHelper.Round(((int)(rebuilt.Count + NumMessages * sizeof(uint))), 16) - 16;

            List<int> messageOffsets = new List<int>();
            List<byte> messageData = new List<byte>();

            int offset = messageDataLocation;
            for (int i = 0; i < NumMessages; i++)
            {
                if (Messages[i].ConvertedString != string.Empty)
                {
                    originalMessages[i] = new EtrianString(Messages[i].RawData);

                    foreach (ushort val in Messages[i].RawData) messageData.AddRange(BitConverter.GetBytes(val));
                    messageData.AddRange(new byte[2]);

                    int padding = YggdrasilHelper.Round(messageData.Count, 16) - messageData.Count;
                    messageData.AddRange(new byte[padding]);

                    messageOffsets.Add(offset);
                    offset = messageDataLocation + messageData.Count;
                }
                else
                {
                    messageOffsets.Add(0);
                }
            }

            foreach (int messageOffset in messageOffsets) rebuilt.AddRange(BitConverter.GetBytes(messageOffset));
            rebuilt.AddRange(new byte[(YggdrasilHelper.Round(rebuilt.Count, 16) - rebuilt.Count)]);

            rebuilt.AddRange(messageData);

            return rebuilt.ToArray();
        }

        protected override void Parse(MemoryStream tableFileData, int offset)
        {
            BinaryReader reader = new BinaryReader(tableFileData);

            reader.BaseStream.Seek(offset + 4, SeekOrigin.Begin);
            Unknown = reader.ReadUInt32();
            Size = reader.ReadUInt32();
            NumMessages = reader.ReadUInt32();
            Unknown2 = reader.ReadUInt32();

            MessageOffsets = new uint[NumMessages];
            for (int i = 0; i < NumMessages; i++)
            {
                reader.BaseStream.Seek(offset + 20 + i * sizeof(uint), SeekOrigin.Begin);
                MessageOffsets[i] = reader.ReadUInt32();
            }

            byte[] fileData = tableFileData.ToArray();

            Messages = new EtrianString[NumMessages];
            originalMessages = new EtrianString[NumMessages];

            for (int i = 0; i < NumMessages; i++)
            {
                if (MessageOffsets[i] == 0)
                    Messages[i] = new EtrianString(string.Empty);
                else
                    Messages[i] = new EtrianString(fileData, (int)(offset + 0x10 + MessageOffsets[i]));

                originalMessages[i] = new EtrianString(Messages[i].RawData);
            }
        }
    }
}

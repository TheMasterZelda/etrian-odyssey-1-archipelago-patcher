using etrian_odyssey_ap_patcher.DataCompression;
using etrian_odyssey_ap_patcher.EtrianOdyssey.Event;
using System.Text;

namespace etrian_odyssey_ap_patcher.EtrianOdyssey.Files
{
    public class EventFile : BaseFile
    {

        // 00-03 = 06 00 00 00
        // 04-07 = number of entries?
        // 08-0B = Unknown, ID? counter?
        // 0C-0F = EE EE EE EE
        // 10-> start of events block
        // each event is 0x18


        private const int EVENT_ENTRY_BLOCK_SIZE = 0x18;
        public uint Unknown00;
        public uint NumEntries;
        public uint Unknown08;
        public uint Unknown0C;

        public List<EventEntry> Events;

        public EventFile(byte[] fileData, CompressionType compressionType, bool isNestedFile) : base(fileData, compressionType, isNestedFile)
        {
        }

        public EventFile(byte[] fileData) : base(fileData)
        {
        }

        public override void Parse()
        {
            BinaryReader reader = new BinaryReader(DataStream);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            Unknown00 = reader.ReadUInt32();
            NumEntries = reader.ReadUInt32();
            Unknown08 = reader.ReadUInt32();
            Unknown0C = reader.ReadUInt32();
            Events = new List<EventEntry>();

            for (int i = 0; i < NumEntries; i++)
            {
                byte[] blockData = reader.ReadBytes(EVENT_ENTRY_BLOCK_SIZE);

                Events.Add(new EventEntry(blockData));
            }

            long bytesLeft = reader.BaseStream.Length - reader.BaseStream.Position;

            byte[] eventScripts = reader.ReadBytes((int)bytesLeft);

            for (int i = 0; i < NumEntries; i++)
            {
                EventScript script = EventScriptParser.ParseEventScript(eventScripts, Events[i].script_offset);
                Events[i].script = script;
            }
        }

        protected override byte[] Save()
        {
            List<byte> rebuilt = new List<byte>();
            rebuilt.AddRange(BitConverter.GetBytes(Unknown00));
            rebuilt.AddRange(BitConverter.GetBytes(NumEntries));
            rebuilt.AddRange(BitConverter.GetBytes(Unknown08));
            rebuilt.AddRange(BitConverter.GetBytes(Unknown0C));

            uint entries_total_size = NumEntries * EVENT_ENTRY_BLOCK_SIZE;
            uint scripts_base_offset = 0x10 + entries_total_size;

            uint[] script_offsets = new uint[NumEntries];

            List<byte> scripts_rebuild = new List<byte>();

            uint current_script_offset = 0;
            // We need to figure out the new script offsets first.
            for (int i = 0; i < NumEntries; i++)
            {
                EventScript script = Events[i].script;

                byte[] script_bytes = EventScriptParser.RebuildScript(script);
                script_offsets[i] = current_script_offset;
                current_script_offset += (uint)script_bytes.Length;
                scripts_rebuild.AddRange(script_bytes);
            }

            for (int i = 0; i < NumEntries; i++)
            {
                EventEntry entry = Events[i];
                entry.script_offset = script_offsets[i];
                rebuilt.AddRange(entry.Save());
            }

            if (rebuilt.Count != scripts_base_offset)
                throw new Exception();

            // Now insert the scripts.
            rebuilt.AddRange(scripts_rebuild);

            return rebuilt.ToArray();
        }
    }
}

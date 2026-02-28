using etrian_odyssey_ap_patcher.Util;

namespace etrian_odyssey_ap_patcher.NitroRom
{
    public class FileAllocationTable
    {
        public class FileAllocationTableEntry
        {
            public int start_address;
            public int end_address;
        }

        public byte[] data;

        public void UpdateData()
        {
            data = new byte[files.Count * 8];

            int offset = 0;
            foreach (FileAllocationTableEntry file in files)
            {
                ByteUtil.Write(data, offset, file.start_address);
                ByteUtil.Write(data, offset + 4, file.end_address);
                offset += 8;
            }
        }

        public List<FileAllocationTableEntry> files = new List<FileAllocationTableEntry>();
    }
}

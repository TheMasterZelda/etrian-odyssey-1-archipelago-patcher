using static etrian_odyssey_ap_patcher.NitroRom.FileAllocationTable;

namespace etrian_odyssey_ap_patcher.NitroRom
{
    public class FileEntry
    {
        public byte[] file_content;

        public string path;

        public FNTFile fnt_file;
        public FileAllocationTableEntry fat_file;
    }
}

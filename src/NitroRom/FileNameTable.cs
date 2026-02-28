namespace etrian_odyssey_ap_patcher.NitroRom
{
    public class FileNameTable
    {
        public byte[] data;

        public FNTRootDirectory content;

        public Dictionary<ushort, FNTDirectory> directories;
        public Dictionary<ushort, FNTFile> files;
    }
}

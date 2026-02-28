namespace etrian_odyssey_ap_patcher.NitroRom
{
    public class Rom
    {
        public Header header;
        public byte[] arm9;
        public byte[] arm7;
        public Overlay[] arm9_overlays;
        public Overlay[] arm7_overlays;
        public byte[] banner;

        public FileAllocationTable fat;
        public FileNameTable fnt;

        public Dictionary<string, FileEntry> files = new Dictionary<string, FileEntry>();
    }
}

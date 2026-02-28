namespace etrian_odyssey_ap_patcher.NitroRom
{
    public abstract class FNTDirectory
    {
        public ushort directory_id;
        public int offset_to_subtable;
        public ushort id_of_first_file;

        public string path;

        public abstract string GetDirectoryName();

        public List<FNTDirectory> sub_directories = new List<FNTDirectory>();
        public List<FNTFile> files = new List<FNTFile>();
    }
}

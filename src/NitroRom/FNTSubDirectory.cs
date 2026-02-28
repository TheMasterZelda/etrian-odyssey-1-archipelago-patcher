namespace etrian_odyssey_ap_patcher.NitroRom
{
    public class FNTSubDirectory : FNTDirectory
    {
        public byte type;
        public byte length;
        public string directory_name;
        public ushort id_of_parent_directory;

        public FNTSubTable sub_table_entry;

        public override string GetDirectoryName()
        {
            return sub_table_entry.directory_name;
        }

        public override string ToString()
        {
            return $"{directory_id}:{sub_table_entry.directory_name}";
        }
    }
}

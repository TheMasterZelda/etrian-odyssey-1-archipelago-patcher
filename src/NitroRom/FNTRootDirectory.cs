namespace etrian_odyssey_ap_patcher.NitroRom
{
    public class FNTRootDirectory : FNTDirectory
    {
        public short total_number_of_directories;

        public override string GetDirectoryName()
        {
            return "";
        }

        public override string ToString()
        {
            return $"{directory_id}:root";
        }
    }
}

namespace etrian_odyssey_ap_patcher.NitroRom
{
    public class FNTFile
    {
        public ushort file_id;
        public byte length;
        public string file_name;

        public override string ToString()
        {
            return $"{file_id}:{file_name}";
        }
    }
}

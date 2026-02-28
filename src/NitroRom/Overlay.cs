namespace etrian_odyssey_ap_patcher.NitroRom
{
    public class Overlay
    {
        public byte[] data;

        public int overlay_id;
        public int ram_address;
        public int ram_size;
        public int bss_size;
        public int static_initialiser_start_address;
        public int static_initialiser_end_address;
        public int file_id; // 0000h..EFFFh)
        public int reserved;
    }
}

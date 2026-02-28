using etrian_odyssey_ap_patcher.Util;
using System.Text;

namespace etrian_odyssey_ap_patcher.NitroRom
{
    public class Header
    {
        public byte[] data;

        public void UpdateData()
        {
            ByteUtil.Write(data, nds_specification.GAME_TITLE, game_title);
            ByteUtil.Write(data, nds_specification.ARM9_ROM_OFFSET, arm9_rom_offset);
            ByteUtil.Write(data, nds_specification.ARM9_ROM_SIZE, arm9_rom_size);
            ByteUtil.Write(data, nds_specification.ARM7_ROM_OFFSET, arm7_rom_offset);
            ByteUtil.Write(data, nds_specification.ARM7_ROM_SIZE, arm7_rom_size);
            ByteUtil.Write(data, nds_specification.FILE_NAME_TABLE_OFFSET, fnt_offset);
            ByteUtil.Write(data, nds_specification.FILE_NAME_TABLE_SIZE, fnt_size);
            ByteUtil.Write(data, nds_specification.FILE_ALLOCATION_TABLE_OFFSET, fat_offset);
            ByteUtil.Write(data, nds_specification.FILE_ALLOCATION_TABLE_SIZE, fat_size);
            ByteUtil.Write(data, nds_specification.ARM9_OVERLAY_ROM_OFFSET, file_arm9_overlay_offset);
            ByteUtil.Write(data, nds_specification.ARM9_OVERLAY_ROM_SIZE, file_arm9_overlay_size);
            ByteUtil.Write(data, nds_specification.ARM7_OVERLAY_ROM_OFFSET, file_arm7_overlay_offset);
            ByteUtil.Write(data, nds_specification.ARM7_OVERLAY_ROM_SIZE, file_arm7_overlay_size);
            ByteUtil.Write(data, nds_specification.BANNER_FILE_ROM_OFFSET, banner_offset);
            ByteUtil.Write(data, nds_specification.TOTAL_USED_ROM_SIZE, total_used_rom_size);
            ByteUtil.Write(data, nds_specification.ROM_HEADER_SIZE, rom_header_size);
            ByteUtil.Write(data, nds_specification.HEADER_CHECKSUM_CRC, header_checksum);
        }

        public string GameTitle
        {
            get { return Encoding.ASCII.GetString(game_title, 0, 12); }
            set { game_title = Encoding.ASCII.GetBytes(value.Substring(0, Math.Min(value.Length, 12)).PadRight(12, '\0')); }
        }

        public byte[] game_title;
        public int arm9_rom_offset;
        public int arm9_rom_size;
        public int arm7_rom_offset;
        public int arm7_rom_size;
        public int fnt_offset;
        public int fnt_size;
        public int fat_offset;
        public int fat_size;
        public int file_arm9_overlay_offset;
        public int file_arm9_overlay_size;
        public int file_arm7_overlay_offset;
        public int file_arm7_overlay_size;
        public int banner_offset;
        public int total_used_rom_size;
        public int rom_header_size; // Normally 4000h
        public int header_checksum; // CRC-16 of [000h-15Dh]
    }
}

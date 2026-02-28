namespace etrian_odyssey_ap_patcher.NitroRom
{
    public static class nds_specification
    {
        // 0x000-0x00B: Game Title.
        public const int GAME_TITLE = 0x0;
        // 0x00C-0x00F: Game Code.

        // 0x010-0x011: Maker Code.
        // 0x012: Main Unit Code.
        // 0x013: Device Type.
        // 0x014: Device Capacity.
        // 0x015-0x01C: Reserved.
        // 0x01D: Specific.
        // 0x01E: ROM version.
        // 0x01F: Reserved.

        public const int ARM9_ROM_OFFSET = 0x020;
        public const int ARM9_ENTRY_ADDRESS = 0x024;
        public const int ARM9_RAM_ADDRESS = 0x028;
        public const int ARM9_ROM_SIZE = 0x02C;

        public const int ARM7_ROM_OFFSET = 0x030;
        public const int ARM7_ENTRY_ADDRESS = 0x034;
        public const int ARM7_RAM_ADDRESS = 0x038;
        public const int ARM7_ROM_SIZE = 0x03C;

        public const int FILE_NAME_TABLE_OFFSET = 0x040; // FNT.
        public const int FILE_NAME_TABLE_SIZE = 0x044; // FNT.
        public const int FILE_ALLOCATION_TABLE_OFFSET = 0x048; // FAT.
        public const int FILE_ALLOCATION_TABLE_SIZE = 0x04C; // FAT.

        public const int ARM9_OVERLAY_ROM_OFFSET = 0x050;
        public const int ARM9_OVERLAY_ROM_SIZE = 0x054;
        public const int ARM7_OVERLAY_ROM_OFFSET = 0x058;
        public const int ARM7_OVERLAY_ROM_SIZE = 0x05C;

        public const int ROM_CONTROL_INFORMATION = 0x060; // 8 bytes.
        public const int BANNER_FILE_ROM_OFFSET = 0x068; // 4 bytes.
        public const int SECURE_AREA_CRC = 0x06C; // 2 bytes.
        public const int ROM_CONTROL_INFORMATION_2 = 0x06E; // 2 bytes.

        // 0x070-0x073: Arm9 Auto Load List Ram Address.
        // 0x074-0x077: Arm7 Auto Load List Ram Address.
        // 0x078-0x07F: ROM information Reserved Region.

        // 0x080-0x083: Application.
        public const int TOTAL_USED_ROM_SIZE = 0x080; // 4 bytes.
        // 0x084-0x087: ROM Header Size
        public const int ROM_HEADER_SIZE = 0x084; // 4 bytes.
        // 0x088-0x08B: Arm9 Module Parameter Address.
        // 0x08C-0x08F: Arm7 Module Parameter Address.

        // 0x090-0x0BF Reserved.

        public const int NINTENDO_LOGO_IMAGE_DATA = 0x0C0;
        public const int NINTENDO_LOGO_IMAGE_DATA_END = 0x15B;
        public const int NINTENDO_LOGO_CRC = 0x15C; // 2 bytes.
        public const int HEADER_CHECKSUM_CRC = 0x15E; // 2 bytes.

        // 0x160-0x17F Reserved.

    }
}

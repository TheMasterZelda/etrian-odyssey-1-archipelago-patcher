using System.Text;
using etrian_odyssey_ap_patcher.Util;
using static etrian_odyssey_ap_patcher.NitroRom.FileAllocationTable;

namespace etrian_odyssey_ap_patcher.NitroRom
{
    public class RomParser
    {
        public RomParser(string filename)
        {
            ndsFile = new MemoryStream(File.ReadAllBytes(filename));
        }

        private readonly MemoryStream ndsFile;

        public byte[] Extract(int offset, int size)
        {
            byte[] buffer = new byte[size];
            ndsFile.Position = offset;
            ndsFile.Read(buffer, 0, size);
            return buffer;
        }

        public byte[] ParseBanner(BinaryReader ndsRom, Header header)
        {
            ushort version = ByteUtil.ReadAsUint16(ndsRom, header.banner_offset);

            int banner_size;
            switch (version)
            {
                case 0x01:
                    banner_size = 0x840 + 0x1C0;
                    break;
                case 0x02:
                    banner_size = 0x940 + 0xC0;
                    break;
                case 0x03:
                    throw new Exception("Unknown banner version's size");
                case 0x103:
                    banner_size = 0x23C0 + 0x40;
                    break;
                default:
                    throw new Exception("Unknown banner version");
            }

            return Extract(header.banner_offset, banner_size);
        }

        public Header ParseHeader(BinaryReader ndsRom)
        {
            Header header = new Header();

            ndsRom.BaseStream.Position = 0;
            header.game_title = ndsRom.ReadBytes(12);
            header.arm9_rom_offset = ByteUtil.ReadAsInt32(ndsRom, nds_specification.ARM9_ROM_OFFSET);
            header.arm9_rom_size = ByteUtil.ReadAsInt32(ndsRom, nds_specification.ARM9_ROM_SIZE);
            header.arm7_rom_offset = ByteUtil.ReadAsInt32(ndsRom, nds_specification.ARM7_ROM_OFFSET);
            header.arm7_rom_size = ByteUtil.ReadAsInt32(ndsRom, nds_specification.ARM7_ROM_SIZE);
            header.fnt_offset = ByteUtil.ReadAsInt32(ndsRom, nds_specification.FILE_NAME_TABLE_OFFSET);
            header.fnt_size = ByteUtil.ReadAsInt32(ndsRom, nds_specification.FILE_NAME_TABLE_SIZE);
            header.fat_offset = ByteUtil.ReadAsInt32(ndsRom, nds_specification.FILE_ALLOCATION_TABLE_OFFSET);
            header.fat_size = ByteUtil.ReadAsInt32(ndsRom, nds_specification.FILE_ALLOCATION_TABLE_SIZE);
            header.file_arm9_overlay_offset = ByteUtil.ReadAsInt32(ndsRom, nds_specification.ARM9_OVERLAY_ROM_OFFSET);
            header.file_arm9_overlay_size = ByteUtil.ReadAsInt32(ndsRom, nds_specification.ARM9_OVERLAY_ROM_SIZE);
            header.file_arm7_overlay_offset = ByteUtil.ReadAsInt32(ndsRom, nds_specification.ARM7_OVERLAY_ROM_OFFSET);
            header.file_arm7_overlay_size = ByteUtil.ReadAsInt32(ndsRom, nds_specification.ARM7_OVERLAY_ROM_SIZE);
            header.banner_offset = ByteUtil.ReadAsInt32(ndsRom, nds_specification.BANNER_FILE_ROM_OFFSET);
            header.total_used_rom_size = ByteUtil.ReadAsInt32(ndsRom, nds_specification.TOTAL_USED_ROM_SIZE);
            header.rom_header_size = ByteUtil.ReadAsInt32(ndsRom, nds_specification.ROM_HEADER_SIZE);
            header.header_checksum = ByteUtil.ReadAsInt32(ndsRom, nds_specification.HEADER_CHECKSUM_CRC);

            ndsRom.BaseStream.Position = 0;
            header.data = ndsRom.ReadBytes(header.rom_header_size);

            return header;
        }

        public FileAllocationTable ParseFAT(BinaryReader ndsRom, Header header)
        {
            FileAllocationTable fat = new FileAllocationTable();

            ndsRom.BaseStream.Position = header.fat_offset;
            fat.data = ndsRom.ReadBytes(header.fat_size);

            for (int i = 0; i < header.fat_size; i += 8)
            {
                int start_address = ByteUtil.ReadAsInt32(ndsRom, header.fat_offset + i);
                int end_address = ByteUtil.ReadAsInt32(ndsRom, header.fat_offset + i + 4);

                fat.files.Add(new FileAllocationTableEntry()
                {
                    start_address = start_address,
                    end_address = end_address
                });
            }

            return fat;
        }

        public FileNameTable ParseFNT(BinaryReader ndsRom, Header header)
        {
            FileNameTable fnt = new FileNameTable();

            ndsRom.BaseStream.Position = header.fnt_offset;
            fnt.data = ndsRom.ReadBytes(header.fnt_size);

            BinaryReader fntData = new BinaryReader(new MemoryStream(fnt.data));

            FNTRootDirectory root = new FNTRootDirectory();
            fnt.content = root;

            root.directory_id = 0xF000;
            root.offset_to_subtable = ByteUtil.ReadAsInt32(fntData, 0);
            root.id_of_first_file = ByteUtil.ReadAsUint16(fntData, 4);
            root.total_number_of_directories = ByteUtil.ReadAsInt16(fntData, 6);
            root.path = "";

            Dictionary<ushort, FNTDirectory> directories = new Dictionary<ushort, FNTDirectory>(root.total_number_of_directories);
            directories.Add(root.directory_id, root);

            // Parse all directory.
            for (ushort directory_id = 1; directory_id < root.total_number_of_directories; directory_id++)
            {
                int offset = directory_id * 8;

                FNTSubDirectory sub_dir = new FNTSubDirectory();
                sub_dir.directory_id = (ushort)(directory_id | 0xF000);
                sub_dir.offset_to_subtable = ByteUtil.ReadAsInt32(fntData, offset);
                sub_dir.id_of_first_file = ByteUtil.ReadAsUint16(fntData, offset + 4);
                sub_dir.id_of_parent_directory = ByteUtil.ReadAsUint16(fntData, offset + 8);

                directories.Add(sub_dir.directory_id, sub_dir);
            }

            Dictionary<ushort, FNTFile> files = new Dictionary<ushort, FNTFile>();

            foreach (FNTDirectory directory in directories.Values)
            {
                int current_offset = directory.offset_to_subtable;
                ushort current_id = directory.id_of_first_file;

                while (true)
                {
                    fntData.BaseStream.Position = current_offset;
                    byte type = fntData.ReadByte();

                    if (type == 0x00)
                        break;

                    bool is_directory = type > 0x80;
                    byte length = (byte)(type & 0x7F);
                    byte[] name_bytes = fntData.ReadBytes(length);
                    string name = Encoding.ASCII.GetString(name_bytes);

                    if (is_directory)
                    {
                        FNTSubTable subdirectory = new FNTSubTable();
                        subdirectory.directory_name = name;
                        subdirectory.length = length;

                        int offset = current_offset + length + 1;
                        ushort sub_directory_id = ByteUtil.ReadAsUint16(fntData, offset);

                        if (directories[sub_directory_id] is FNTSubDirectory)
                            ((FNTSubDirectory)directories[sub_directory_id]).sub_table_entry = subdirectory;

                        directory.sub_directories.Add(directories[sub_directory_id]);

                        current_offset += length + 3;
                        continue;
                    }

                    FNTFile file = new FNTFile();
                    file.file_id = current_id++;
                    file.length = length;
                    file.file_name = name;

                    files.Add(file.file_id, file);
                    directory.files.Add(file);

                    current_offset += length + 1;
                }
            }

            // Determine directory paths.
            SetSubDirectoriesPath(root);

            fnt.directories = directories;
            fnt.files = files;

            return fnt;

            void SetSubDirectoriesPath(FNTDirectory directory)
            {
                foreach (FNTDirectory subdir in directory.sub_directories)
                {
                    subdir.path = @$"{directory.path}/{subdir.GetDirectoryName()}";
                    SetSubDirectoriesPath(subdir);
                }
            }
        }

        public Overlay[] ParseOverlays(BinaryReader ndsRomData, Rom ndsRom, int offset, int size)
        {
            int number_of_overlay = size / 0x20;

            Overlay[] overlays = new Overlay[number_of_overlay];

            for (int i = 0; i < number_of_overlay; i++)
            {
                int ov_base_offset = offset + i * 0x20;

                Overlay overlay = new Overlay();

                overlay.data = Extract(ov_base_offset, 0x20);

                overlay.overlay_id = ByteUtil.ReadAsInt32(ndsRomData, ov_base_offset);
                overlay.ram_address = ByteUtil.ReadAsInt32(ndsRomData, ov_base_offset + 4);
                overlay.ram_size = ByteUtil.ReadAsInt32(ndsRomData, ov_base_offset + 8);
                overlay.bss_size = ByteUtil.ReadAsInt32(ndsRomData, ov_base_offset + 0x0C);
                overlay.static_initialiser_start_address = ByteUtil.ReadAsInt32(ndsRomData, ov_base_offset + 0x10);
                overlay.static_initialiser_end_address = ByteUtil.ReadAsInt32(ndsRomData, ov_base_offset + 0x14);
                overlay.file_id = ByteUtil.ReadAsInt32(ndsRomData, ov_base_offset + 0x18);
                overlay.reserved = ByteUtil.ReadAsInt32(ndsRomData, ov_base_offset + 0x1C);

                overlays[i] = overlay;
            }

            return overlays;
        }

        public Dictionary<string, FileEntry> ParseFiles(Rom rom)
        {
            Dictionary<string, FileEntry> files = new Dictionary<string, FileEntry>();

            foreach (FNTDirectory directory in rom.fnt.directories.Values)
            {
                foreach (FNTFile file in directory.files)
                {
                    FileEntry entry = new FileEntry();
                    entry.fnt_file = file;
                    entry.path = $"{directory.path}/{file.file_name}";

                    FileAllocationTableEntry fat = rom.fat.files[file.file_id];

                    entry.fat_file = fat;
                    entry.file_content = Extract(fat.start_address, fat.end_address - fat.start_address);

                    files.Add(entry.path, entry);
                }
            }

            return files;
        }

        public Rom Parse()
        {
            Rom rom = new Rom();

            BinaryReader binary_reader = new BinaryReader(ndsFile);

            rom.header = ParseHeader(binary_reader);
            rom.fat = ParseFAT(binary_reader, rom.header);
            rom.fnt = ParseFNT(binary_reader, rom.header);

            rom.arm9 = Extract(rom.header.arm9_rom_offset, rom.header.arm9_rom_size);
            rom.arm7 = Extract(rom.header.arm7_rom_offset, rom.header.arm7_rom_size);

            rom.banner = ParseBanner(binary_reader, rom.header);

            rom.arm7_overlays = ParseOverlays(binary_reader, rom, rom.header.file_arm7_overlay_offset, rom.header.file_arm7_overlay_size);
            rom.arm9_overlays = ParseOverlays(binary_reader, rom, rom.header.file_arm9_overlay_offset, rom.header.file_arm9_overlay_size);

            rom.files = ParseFiles(rom);

            return rom;
        }
    }
}

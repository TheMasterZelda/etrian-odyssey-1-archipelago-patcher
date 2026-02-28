using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using etrian_odyssey_ap_patcher.Util;

namespace etrian_odyssey_ap_patcher.NitroRom
{
    public class Packer
    {


        public byte[] PackRomSimple(Rom rom)
        {
            // Handle arm9 changes.

            MemoryStream ndsRom = new MemoryStream();
            ndsRom.Seek(rom.header.arm9_rom_offset, SeekOrigin.Begin);
            ndsRom.Write(rom.arm9);

            ndsRom.Seek(rom.header.file_arm9_overlay_offset, SeekOrigin.Begin);
            //ndsRom.Write(rom.arm9_overlays) TODO

            ndsRom.Seek(rom.header.arm7_rom_offset, SeekOrigin.Begin);
            ndsRom.Write(rom.arm7);
            ndsRom.Seek(rom.header.file_arm7_overlay_offset, SeekOrigin.Begin);
            //ndsRom.Write(rom.arm7_overlays) TODO

            ndsRom.Seek(rom.header.fnt_offset, SeekOrigin.Begin);
            ndsRom.Write(rom.fnt.data);

            int length = 0;
            int length2 = 0;

            foreach (FileEntry file in rom.files.Values)
            {
                ndsRom.Seek(file.fat_file.start_address, SeekOrigin.Begin);
                ndsRom.Write(file.file_content);

                length += file.file_content.Length;

                if (file.fat_file.start_address == file.fat_file.end_address)
                    throw new Exception();

                length2 += file.fat_file.end_address - file.fat_file.start_address;


                while (ndsRom.Position != file.fat_file.end_address)
                    ndsRom.WriteByte(0x00);
            }

            ndsRom.Seek(rom.header.banner_offset, SeekOrigin.Begin);
            ndsRom.Write(rom.banner);

            ndsRom.Seek(rom.header.fat_offset, SeekOrigin.Begin);
            ndsRom.Write(rom.fat.data);

            ndsRom.Seek(0, SeekOrigin.Begin);
            ndsRom.Write(rom.header.data);

            return ndsRom.ToArray();
        }

        public const int align = 0x1FF;

        private int ComputeCrc16(byte[] data, int crc)
        {
            int[] crc_table = new int[]
            {
                0, 0xCC01, 0xD801, 0x1400, 0xF001, 0x3C00, 0x2800, 0xE401, 0xA001, 0x6C00, 0x7800, 0xB401, 0x5000, 0x9C01, 0x8801, 0x4400
            };

            for (int i = 0; i < data.Length; i += 2)
            {
                ushort x = ByteUtil.ReadUint16(data, i);
                for (int j = 0; j < 16; j += 4)
                {
                    int y = crc_table[crc & 0xF];
                    crc >>= 4;
                    crc ^= y;
                    crc ^= crc_table[x >> j & 0xF];
                }
            }

            return crc;
        }

        private int GetAlignPaddingSize(int current_position)
        {
            return -current_position & align;
        }

        public byte[] PackRom(Rom rom)
        {
            // Handle arm9 changes.

            MemoryStream ndsRom = new MemoryStream();

            void AlignOffset()
            {
                ndsRom.Seek(GetAlignPaddingSize(CurrentOffset()), SeekOrigin.Current);
            }

            int CurrentOffset()
            {
                return (int)ndsRom.Position;
            }

            // Reserve the header space.
            ndsRom.Seek(rom.header.rom_header_size, SeekOrigin.Begin);

            // And align.
            AlignOffset();

            rom.header.arm9_rom_offset = CurrentOffset();
            rom.header.arm9_rom_size = rom.arm9.Length;
            ndsRom.Write(rom.arm9);

            AlignOffset();

            // Overlay not handled.
            //ndsRom.Seek(rom.header.file_arm9_overlay_offset, SeekOrigin.Begin);
            //ndsRom.Write(rom.arm9_overlays) TODO

            rom.header.arm7_rom_offset = CurrentOffset();
            rom.header.arm7_rom_size = rom.arm7.Length;
            ndsRom.Write(rom.arm7);

            AlignOffset();

            // Overlay not handled.
            //ndsRom.Seek(rom.header.file_arm7_overlay_offset, SeekOrigin.Begin);
            //ndsRom.Write(rom.arm7_overlays) TODO

            rom.header.fnt_offset = CurrentOffset();
            rom.header.fnt_size = rom.fnt.data.Length;
            ndsRom.Write(rom.fnt.data);

            AlignOffset();

            foreach (FileEntry file in rom.files.Values)
            {
                FileAllocationTable.FileAllocationTableEntry fat_entry = rom.fat.files[file.fnt_file.file_id];
                fat_entry.start_address = CurrentOffset();
                fat_entry.end_address = fat_entry.start_address + file.file_content.Length;
                ndsRom.Write(file.file_content);

                AlignOffset();
            }

            rom.header.banner_offset = CurrentOffset();
            ndsRom.Write(rom.banner);

            AlignOffset();

            rom.fat.UpdateData();

            rom.header.fat_offset = CurrentOffset();
            ndsRom.Write(rom.fat.data);

            AlignOffset();

            rom.header.total_used_rom_size = CurrentOffset();

            rom.header.UpdateData();

            rom.header.header_checksum = ComputeCrc16(rom.header.data.Take(nds_specification.HEADER_CHECKSUM_CRC).ToArray(), 0xFFFF);
            rom.header.UpdateData();

            ndsRom.Seek(0, SeekOrigin.Begin);
            ndsRom.Write(rom.header.data);

            return ndsRom.ToArray();
        }
    }
}

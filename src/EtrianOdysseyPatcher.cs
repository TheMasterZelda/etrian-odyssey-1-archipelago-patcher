using BsDiff;
using etrian_odyssey_ap_patcher.EtrianOdyssey.Files;
using etrian_odyssey_ap_patcher.EtrianOdyssey.MapData;
using etrian_odyssey_ap_patcher.EtrianOdyssey.Table;
using etrian_odyssey_ap_patcher.NitroRom;
using etrian_odyssey_ap_patcher.Util;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace etrian_odyssey_ap_patcher
{
    public class EtrianOdysseyPatcher
    {
        public EtrianOdysseyPatcher(string rom_file)
        {
            RomParser parser = new RomParser(rom_file);

            rom = parser.Parse();
            files = new EtrianOdysseyFiles(rom);
        }

        private readonly Rom rom;
        private readonly EtrianOdysseyFiles files;

        private void PatchRom9Value(int address, byte? value)
        {
            if (!value.HasValue)
                return;

            ByteUtil.Write(rom.arm9, address, value.Value);
        }

        private void PatchRom9Value(int address, uint? value)
        {
            if (!value.HasValue)
                return;

            ByteUtil.Write(rom.arm9, address, value.Value);
        }

        private void PatchRom9Value(int address, bool? value)
        {
            if (!value.HasValue)
                return;

            ByteUtil.Write(rom.arm9, address, (byte)(value.Value ? 1 : 0));
        }

        public void ApplyAPGameTitle()
        {
            rom.header.GameTitle = "EO1AP V1";
        }

        public void ApplyCodePatch()
        {
            MemoryStream old_arm9 = new MemoryStream(rom.arm9);
            MemoryStream new_arm9 = new MemoryStream();
            BinaryPatch.Apply(old_arm9, () => new MemoryStream(Resource.patch), new_arm9);

            rom.arm9 = new_arm9.ToArray();
        }

        public void ApplyAPPatch(Stream apPatch)
        {
            using ZipArchive archive = new ZipArchive(apPatch, ZipArchiveMode.Read);
            ZipArchiveEntry patchEntry = archive.GetEntry("patch");
            StreamReader reader = new StreamReader(patchEntry.Open());
            string rawPatch = reader.ReadToEnd();
            string decodedPatch = Encoding.UTF8.GetString(Convert.FromBase64String(rawPatch));

            var deserializer = new DeserializerBuilder()/*.WithNamingConvention(UnderscoredNamingConvention.Instance)*/.Build();

            SeedPatchData patchData = deserializer.Deserialize<SeedPatchData>(decodedPatch);

            ApplyInitialValues(patchData.InitialValues);
            ApplyTreasureBoxPatch(patchData.TreasureBoxes);


        }

        public void ApplyTreasureBoxPatch(List<SeedPatchTreasureData> treasureDataList)
        {
            foreach (SeedPatchTreasureData treasure_data in treasureDataList)
            {
                PatchSingularTreasure(treasure_data);

            }

            void PatchSingularTreasure(SeedPatchTreasureData treasure_data)
            {
                MapDataFile floorFile = files.GetFloorFile(treasure_data.floor + 1);

                for (int y = 0; y < MapDataFile.MapHeight; y++)
                {
                    for (int x = 0; x < MapDataFile.MapWidth; x++)
                    {
                        if (floorFile.Tiles[x, y].TileType == MapDataFile.TileTypes.TreasureChest)
                        {
                            TreasureChestTile tile = (TreasureChestTile)floorFile.Tiles[x, y];
                            if (tile.treasureChestID == treasure_data.treasure_id)
                            {
                                TreasureType newTreasureType = (TreasureType)treasure_data.treasure_type;
                                tile.treasureType = newTreasureType;

                                switch (newTreasureType)
                                {
                                    case TreasureType.Money:
                                        tile.treasureMoney = (ushort)treasure_data.treasure_value;
                                        break;
                                    case TreasureType.Item:
                                        tile.treasureItemID = (ushort)treasure_data.treasure_value;
                                        break;
                                    case TreasureType.AP:
                                        // TODO.
                                        break;
                                    default:
                                        throw new Exception("Invalid treasure type.");
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ApplyInitialValues(SeedPatchInitialValues initialValues)
        {
            PatchRom9Value(0xDC591, initialValues.level_cap);
            PatchRom9Value(0xDC592, initialValues.floor_limit);

            PatchRom9Value(0xDC594, initialValues.experience_modifier);

            PatchRom9Value(0xDC5A0, initialValues.landsknecht_unlocked);
            PatchRom9Value(0xDC5A1, initialValues.survivalist_unlocked);
            PatchRom9Value(0xDC5A2, initialValues.protected_unlocked);
            PatchRom9Value(0xDC5A3, initialValues.dark_hunter_unlocked);
            PatchRom9Value(0xDC5A4, initialValues.medic_unlocked);
            PatchRom9Value(0xDC5A5, initialValues.alchemist_unlocked);
            PatchRom9Value(0xDC5A6, initialValues.troubadour_unlocked);
            PatchRom9Value(0xDC5A7, initialValues.ronin_unlocked);
            PatchRom9Value(0xDC5A8, initialValues.hexer_unlocked);

            PatchRom9Value(0xDC5B0, initialValues.landsknecht_skills);
            PatchRom9Value(0xDC5B4, initialValues.survivalist_skills);
            PatchRom9Value(0xDC5B8, initialValues.protected_skills);
            PatchRom9Value(0xDC5BC, initialValues.dark_hunter_skills);
            PatchRom9Value(0xDC5C0, initialValues.medic_skills);
            PatchRom9Value(0xDC5C4, initialValues.alchemist_skills);
            PatchRom9Value(0xDC5C8, initialValues.troubadour_skills);
            PatchRom9Value(0xDC5CC, initialValues.ronin_skills);
            PatchRom9Value(0xDC5D0, initialValues.hexer_skills);
        }

        public void ApplyShopTextPatch()
        {
            MessageTable shopMessages = (MessageTable)files.FacilityText.Tables[3];

            // Add the new menu text.
            shopMessages.Messages[0].Update("Buy\r\nSell\r\nReceive Item\r\nTalk\r\nLeave");

            // Shift the menu info down.
            shopMessages.Messages[104].Update(shopMessages.Messages[103].RawData);
            shopMessages.Messages[103].Update(shopMessages.Messages[102].RawData);

            // Add the new menu info.
            shopMessages.Messages[102].Update("Receive pending AP items.");
        }

        public byte[] SavePatchedRom()
        {
            files.UpdateFiles();

            Packer packer = new Packer();
            return packer.PackRom(rom);
        }
    }
}

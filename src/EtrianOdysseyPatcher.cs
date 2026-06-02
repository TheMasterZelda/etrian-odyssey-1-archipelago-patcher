using BsDiff;
using etrian_odyssey_ap_patcher.EtrianOdyssey;
using etrian_odyssey_ap_patcher.EtrianOdyssey.Data;
using etrian_odyssey_ap_patcher.EtrianOdyssey.Event;
using etrian_odyssey_ap_patcher.EtrianOdyssey.Files;
using etrian_odyssey_ap_patcher.EtrianOdyssey.MapData;
using etrian_odyssey_ap_patcher.EtrianOdyssey.Table;
using etrian_odyssey_ap_patcher.NitroRom;
using etrian_odyssey_ap_patcher.Util;
using System.IO.Compression;
using System.Text;
using YamlDotNet.Serialization;

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

        private void PatchRom9Value(int address, byte[] value)
        {
            ByteUtil.Write(rom.arm9, address, value);
        }

        // Version
        public void ApplyAPGameTitle()
        {
            rom.header.GameTitle = "EO1AP V1";
        }

        public void ReplaceArm9(byte[] arm9)
        {
            rom.arm9 = arm9;
        }

        public void AddShinai()
        {
            ushort shinai_item_id = 72;

            var entry = ((DataTable)files.Item.Tables[0]).Data[shinai_item_id - 1];
            var item_names = ((MessageTable)files.ItemName.Tables[0]).Messages;
            var item_info = ((MessageTable)files.ItemInfo.Tables[0]).Messages;
            //var item = new ItemEquipment(entry, item_names);

            // 0A
            entry[0x0A] = 0x04;
            //item.equipment_type = 0x04;
            // 02
            entry[0x02] = 0x00;
            //item.damage_type = 0;
            //item.secondary_damage_type = 0x06;
            // 0B
            entry[0x0B] = 0xFF;
            //item.weapon_speed_modifier = 0xFF;
            // 04-05
            ByteUtil.Write(entry, 0x04, (ushort)10);
            //item.attack_1 = 10;

            // 0x24-0x27
            ByteUtil.Write(entry, 0x24, (uint)80);
            //item.sell_price = 80;
            // 0x20-0x23
            ByteUtil.Write(entry, 0x20, (uint)200);
            //item.buy_price = 200;
            // 0x28
            entry[0x28] = 0x10;
            //item.usable_by = 0x10;
            // 0x29
            entry[0x29] = 0x10;
            //item.usable_by_2_and_unknown = 0x11;
            ((DataTable)files.Item.Tables[0]).Data[shinai_item_id - 1] = entry;
            item_names[shinai_item_id - 1].Update("Shinai");

            string shinai_description = "Bamboo Katana.\r\nPerfect for a trainee.\r\n<!color=0009>ATK<!color=000A>+10";
            item_info[shinai_item_id - 1].Update(shinai_description);
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

            WritePlayerName(patchData.Name);
            ApplyInitialValues(patchData.InitialValues);
            ApplyTreasureBoxPatch(patchData.TreasureBoxes);
            ApplyQuestHintPatch(patchData.QuestHints);

            if (patchData.MinimizeQuestMaterialGrind.GetValueOrDefault(true))
                ApplyMinimizeQuestMaterialGrindPatch();

            if (patchData.ShopUnlockMaterialCostDivider.HasValue)
                ApplyShopUnlockQoLPatch(patchData.ShopUnlockMaterialCostDivider.Value);

            if (patchData.RemoveSkillsRequirements.GetValueOrDefault(false))
                ApplyRemoveSkillsRequirements();

            int effective_mat_sell_value_multiplier = patchData.MaterialSellValueMultiplier.GetValueOrDefault(1);
            if (effective_mat_sell_value_multiplier != 1)
                ApplyMaterialSellValueMultiplier(effective_mat_sell_value_multiplier);
        }

        private void ApplyQuestHintPatch(List<SeedPatchQuestHintData> questHints)
        {
            foreach (SeedPatchQuestHintData quest_hint_data in questHints)
            {
                PatchSingularQuestHint(quest_hint_data);
            }

            void PatchSingularQuestHint(SeedPatchQuestHintData quest_hint_data)
            {
                EtrianString[] quest_descriptions = ((MessageTable)files.BarQuestMess.Tables[0]).Messages;

                string vanilla_description = quest_descriptions[quest_hint_data.quest_id].StringValue;

                string[] lines = vanilla_description.Split("\r\n");

                lines[2] = $"Player: {quest_hint_data.item_player_name}";
                lines[3] = quest_hint_data.item_name.Substring(0, Math.Min(37, quest_hint_data.item_name.Length));

                quest_descriptions[quest_hint_data.quest_id].Update(string.Join("\r\n", lines));
            }
        }

        private void WritePlayerName(string name)
        {
            byte[] player_name_bytes = Encoding.UTF8.GetBytes(name);

            PatchRom9Value(0xDC600, player_name_bytes);
        }

        private void ApplyMaterialSellValueMultiplier(int effective_mat_sell_value_multiplier)
        {
            byte[][] all_data = ((DataTable)files.Item.Tables[1]).Data;

            for (int i = 0; i < all_data.Length; i++)
            {
                byte[] entry = all_data[i];

                var item = new ItemOther(entry, ((MessageTable)files.ItemName.Tables[0]).Messages);

                if (item.name.RawData.Length == 1 && item.name.RawData[0] == 0)
                    continue;

                // Filter materials only.
                if (item.unknown_0F != 0x14 && item.unknown_0F != 0x15)
                    continue;

                item.sell_price = (uint)(item.sell_price * effective_mat_sell_value_multiplier);

                all_data[i] = item.Save();
            }
        }

        private void ApplyRemoveSkillsRequirements()
        {
            List<Class2Skill> class2Skills = new List<Class2Skill>();

            for (int i = 0; i < ((DataTable)files.Class2Skill.Tables[0]).Data.Length; i++)
            {
                byte[] entry = ((DataTable)files.Class2Skill.Tables[0]).Data[i];

                var class2skill = new Class2Skill(entry);

                class2skill.RequiredSkillID1 = 0;
                class2skill.RequiredSkillLevel1 = 0;
                class2skill.RequiredSkillID2 = 0;
                class2skill.RequiredSkillLevel2 = 0;

                ((DataTable)files.Class2Skill.Tables[0]).Data[i] = class2skill.Save();
            }
        }

        public void ApplyShopUnlockQoLPatch(int divider)
        {
            var messages = ((MessageTable)files.ItemName.Tables[0]).Messages;
            for (int i = 0; i < ((DataTable)files.ItemCompound.Tables[0]).Data.Length; i++)
            {
                byte[] entry = ((DataTable)files.ItemCompound.Tables[0]).Data[i];
                var item_compound = new ItemCompound(entry, messages);

                if (item_compound.name.RawData.Length == 1 && item_compound.name.RawData[0] == 0)
                    continue;

                item_compound.material_1_count = ReduceWithRounding(item_compound.material_1_count);
                item_compound.material_2_count = ReduceWithRounding(item_compound.material_2_count);
                item_compound.material_3_count = ReduceWithRounding(item_compound.material_3_count);

                ((DataTable)files.ItemCompound.Tables[0]).Data[i] = item_compound.Save();
            }

            byte ReduceWithRounding(byte original_count)
            {
                if (original_count == 0)
                    return 0;

                return (byte)Math.Max(1, Math.Ceiling(((decimal)original_count) / divider));
            }
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
                                    case TreasureType.Floor:
                                    case TreasureType.Level:
                                    case TreasureType.Class:
                                    case TreasureType.Other:
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
            PatchRom9Value(0xDC5A2, initialValues.protector_unlocked);
            PatchRom9Value(0xDC5A3, initialValues.dark_hunter_unlocked);
            PatchRom9Value(0xDC5A4, initialValues.medic_unlocked);
            PatchRom9Value(0xDC5A5, initialValues.alchemist_unlocked);
            PatchRom9Value(0xDC5A6, initialValues.troubadour_unlocked);
            PatchRom9Value(0xDC5A7, initialValues.ronin_unlocked);
            PatchRom9Value(0xDC5A8, initialValues.hexer_unlocked);

            PatchRom9Value(0xDC5B0, initialValues.landsknecht_skills);
            PatchRom9Value(0xDC5B4, initialValues.survivalist_skills);
            PatchRom9Value(0xDC5B8, initialValues.protector_skills);
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

        public void ApplyTreasureBoxTextPatch()
        {
            MessageTable dungeonMessage = (MessageTable)files.DungeonMess.Tables[0];


            dungeonMessage.Messages[457].Update("Found an item from another\r\ndimension.");
            dungeonMessage.Messages[458].Update("Found an item for this\r\ndimension");

            // For now, don't implement specific messages for each special item types.
            //dungeonMessage.Messages[458].Update("The labyrinth rumbles...\r\n<!806A> floors available!");
            //dungeonMessage.Messages[459].Update("You feel a surge of energy...\r\n<!806A> level cap!");
            //dungeonMessage.Messages[460].Update("You feel a surge of energy...\r\n<!806A> level cap!");
            //"Obtained\r\n<!806A>        en."
            // Add the new menu info.
            //shopMessages.Messages[102].Update("Receive pending AP items.");
        }

        public void ApplyRestCostReductionPatch()
        {
            // Patch the level cost.
            PatchRom9Value(0x38ee8, (byte)1);

            // Patch the minimum level to rest.
            PatchRom9Value(0x38ce8, (byte)5);
            PatchRom9Value(0xb6824, (byte)5);
            PatchRom9Value(0xb7534, (byte)5);
            PatchRom9Value(0xb6c88, (byte)5);
            PatchRom9Value(0xb5e94, (byte)5);

            MessageTable messages = (MessageTable)files.FacilityText.Tables[4];

            messages.Messages[114].Update("Reset skill points in exchange\r\nfor losing 1 level.");
            messages.Messages[215].Update("<!8064>       's level has decreased\r\nby 1 while resting.");
        }

        public void PatchKeysEvents()
        {
            // These events will play at their vanilla location in randomizer despite not obtaining the key from there.

            // 4 is missing.
            EventEntry event_entry = files.EventDun07f.Events[8];
            // Set impossible coords.
            event_entry.coordX = 0;
            event_entry.coordY = 0;

            event_entry = files.EventDun07f.Events[9];
            // Set impossible coords.
            event_entry.coordX = 0;
            event_entry.coordY = 0;

            event_entry = files.EventDun13f.Events[7];
            // Set impossible coords.
            event_entry.coordX = 0;
            event_entry.coordY = 0;
        }

        public void PatchQuest09()
        {
            // EXPLORERS_GUILD_TRIAL
            EventScript script = files.Quest09.Events[2].script;

            EventScriptCommand command = script.Commands[1];

            if (command.CommandId != EventCommandId.E_COMID_TMP_FLAG_ON)
                throw new Exception();

            command.CommandId = EventCommandId.E_COMID_FLAGON;
            command.Parameters[0] = (ushort)0x49C;

            command = new EventScriptCommand(EventCommandId.E_COMID_MES_DUNJON, new object[] { (ushort)6 });
            script.Commands.Insert(1, command);

            command = new EventScriptCommand(EventCommandId.E_COMID_MES_WIN_CLOSE, new object[] { });
            script.Commands.Insert(2, command);
        }

        public void ApplyMinimizeQuestMaterialGrindPatch()
        {
            files.LoadEventFiles();

            // THE_LEATHERSMITHS_FAVOR.
            PatchQuest00();

            // CHEFS_REQUEST_I
            PatchQuest17();

            // FASHIONISTA_I
            PatchQuest22();

            // PRAYER_TO_THE_STARS
            PatchQuest25();

            // CHEFS_REQUEST_II
            PatchQuest33();

            // UNDER_CONSTRUCTION
            PatchQuest39();

            // A_SISTERS_PARTING_GIFT
            PatchQuest44();

            // THE_CRYSTAL_MAIDEN
            PatchQuest61();

            // EMBLEM_OF_LOVE
            PatchQuest62();

            // THE_GOLD_ENTHUSIAST
            PatchQuest63();
            
            files.UpdateEventFiles();
        }

        private void PatchQuest63()
        {
            // THE_GOLD_ENTHUSIAST
            EditScriptMaterialCountParameter(files.Quest63, event_index: 0, command_index: 10);
            EditScriptMaterialCountParameter(files.Quest63, event_index: 3, command_index: 10);
            EditScriptRemoveCommandRange(files.Quest63, event_index: 4, start_index: 5, count: 9);

            EditEventIndexData(179, x => x.parameters[1][0] = 1);
        }

        private void PatchQuest62()
        {
            // EMBLEM_OF_LOVE
            EditScriptMaterialCountParameter(files.Quest62, event_index: 0, command_index: 9);
            EditScriptMaterialCountParameter(files.Quest62, event_index: 3, command_index: 5);
            EditScriptRemoveCommandRange(files.Quest62, event_index: 4, start_index: 5, count: 2);

            EditEventIndexData(111, x => x.parameters[1][0] = 1);
        }

        private void PatchQuest61()
        {
            // THE_CRYSTAL_MAIDEN
            EditScriptMaterialCountParameter(files.Quest61, event_index: 0, command_index: 9);
            // Note: This quest has 2 event with index 2.
            EditScriptMaterialCountParameter(files.Quest61, event_index: 4, command_index: 5);
            EditScriptRemoveCommandRange(files.Quest61, event_index: 5, start_index: 5, count: 2);

            EditEventIndexData(103, x => x.parameters[1][0] = 1);
        }

        private void PatchQuest44()
        {
            // A_SISTERS_PARTING_GIFT
            EditScriptMaterialCountParameter(files.Quest44, event_index: 0, command_index: 9);
            EditScriptMaterialCountParameter(files.Quest44, event_index: 3, command_index: 5);
            EditScriptRemoveCommandRange(files.Quest44, event_index: 4, start_index: 5, count: 9);

            EditEventIndexData(178, x => x.parameters[1][0] = 1);
        }

        private void PatchQuest39()
        {
            // UNDER_CONSTRUCTION
            EditScriptMaterialCountParameter(files.Quest39, event_index: 0, command_index: 10);
            EditScriptMaterialCountParameterItem2(files.Quest39, event_index: 0, command_index: 10);

            // Note: This quest has 2 event with index 2.

            EditScriptMaterialCountParameter(files.Quest39, event_index: 4, command_index: 6);
            EditScriptMaterialCountParameterItem2(files.Quest39, event_index: 4, command_index: 6);

            EditScriptRemoveCommandRange(files.Quest39, event_index: 5, start_index: 8, count: 4);
            EditScriptRemoveCommandRange(files.Quest39, event_index: 5, start_index: 5, count: 2);

            EditEventIndexData(114, x =>
            {
                x.parameters[1][0] = 1;
                x.parameters[3][0] = 1; // To test.
            });
        }

        private void PatchQuest33()
        {
            // CHEFS_REQUEST_II
            EditScriptMaterialCountParameter(files.Quest33, event_index: 0, command_index: 9);
            EditScriptMaterialCountParameter(files.Quest33, event_index: 3, command_index: 6);
            EditScriptRemoveCommandRange(files.Quest33, event_index: 4, start_index: 5, count: 1);

            EditEventIndexData(112, x => x.parameters[1][0] = 1);
        }

        private void PatchQuest25()
        {
            // PRAYER_TO_THE_STARS
            EditScriptMaterialCountParameter(files.Quest25, event_index: 0, command_index: 9);

            // Note: This quest has 2 event with index 2.

            EditScriptMaterialCountParameter(files.Quest25, event_index: 4, command_index: 5);
            EditScriptRemoveCommandRange(files.Quest25, event_index: 5, start_index: 3, count: 4);

            EditEventIndexData(101, x => x.parameters[1][0] = 1);
        }

        private void PatchQuest22()
        {
            // FASHIONISTA_I
            EditScriptMaterialCountParameter(files.Quest22, event_index: 0, command_index: 9);
            // Technically the next event has a bugged event condition.
            EditScriptMaterialCountParameter(files.Quest22, event_index: 3, command_index: 5);
            EditScriptRemoveCommandRange(files.Quest22, event_index: 4, start_index: 5, count: 4);

            EditEventIndexData(109, x => x.parameters[1][0] = 1);
            // todo verify index 65280
        }

        private void PatchQuest17()
        {
            // CHEFS_REQUEST_I
            EditScriptMaterialCountParameter(files.Quest17, event_index: 0, command_index: 8);
            EditScriptMaterialCountParameter(files.Quest17, event_index: 3, command_index: 5);
            EditScriptRemoveCommandRange(files.Quest17, event_index: 4, start_index: 13, count: 2);

            EditEventIndexData(108, x => x.parameters[1][0] = 1);
        }

        private void PatchQuest00()
        {
            // THE_LEATHERSMITHS_FAVOR.
            EditScriptMaterialCountParameter(files.Quest00, event_index: 0, command_index: 9);
            EditScriptMaterialCountParameter(files.Quest00, event_index: 3, command_index: 10);
            EditScriptRemoveCommandRange(files.Quest00, event_index: 4, start_index: 5, count: 6);

            EditEventIndexData(119, x => x.parameters[1][0] = 1);
        }

        private static void EditScriptMaterialCountParameterItem2(EventFile event_file, int event_index, int command_index)
        {
            EventScript script = event_file.Events[event_index].script;

            EventScriptCommandIFParameter ifparam = (EventScriptCommandIFParameter)script.Commands[command_index].Parameters[2];

            if (ifparam.if_target != IfTarget.E_IF_TGT_ITEM_2_NUM)
                throw new Exception();

            ifparam.parameter3 = 1;
        }

        private static void EditScriptMaterialCountParameter(EventFile event_file, int event_index, int command_index)
        {
            EventScript script = event_file.Events[event_index].script;

            EventScriptCommandIFParameter ifparam = (EventScriptCommandIFParameter)script.Commands[command_index].Parameters[1];

            if (ifparam.if_target != IfTarget.E_IF_TGT_ITEM_1_NUM)
                throw new Exception();

            ifparam.parameter3 = 1;
        }

        private static void EditScriptRemoveCommandRange(EventFile event_file, int event_index, int start_index, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (event_file.Events[event_index].script.Commands[start_index].CommandId != EventCommandId.E_COMID_EV_LOST_ITEM)
                    throw new Exception();

                event_file.Events[event_index].script.Commands.RemoveAt(start_index);
            }
        }

        private void EditEventIndexData(int event_index_id, Action<EventIndexDataMain> action)
        {
            byte[][] all_data = ((DataTable)files.EventIndex.Tables[0]).Data;

            List<EventIndexDataMain> event_indexes = new List<EventIndexDataMain>();

            int current_param = 0;
            EventIndexDataMain current_event_index = null;
            int current_event_start_index = 0;

            for (ushort i = 0; i < all_data.Length; i++)
            {
                byte[] data = all_data[i];

                if (current_event_index == null)
                {
                    current_event_start_index = i;
                    current_event_index = new EventIndexDataMain(data);
                    current_param = 0;
                    continue;
                }

                current_event_index.parameters[current_param++] = data;

                if (current_param == 12)
                {
                    if (current_event_index.event_index_id == event_index_id)
                    {
                        action.Invoke(current_event_index);

                        for (int x = 0; x < 12; x++)
                        {
                            all_data[current_event_start_index + x + 1] = current_event_index.parameters[x];
                        }

                        return;
                    }

                    event_indexes.Add(current_event_index);
                    current_event_index = null;
                }
            }
        }

        public byte[] SavePatchedRom()
        {
            files.UpdateFiles();

            Packer packer = new Packer();
            return packer.PackRom(rom);
        }
    }
}

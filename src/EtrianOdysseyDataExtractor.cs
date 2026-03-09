using etrian_odyssey_ap_patcher.EtrianOdyssey;
using etrian_odyssey_ap_patcher.EtrianOdyssey.Data;
using etrian_odyssey_ap_patcher.EtrianOdyssey.Files;
using etrian_odyssey_ap_patcher.EtrianOdyssey.MapData;
using etrian_odyssey_ap_patcher.EtrianOdyssey.Table;
using etrian_odyssey_ap_patcher.NitroRom;
using System.Text;
using System.Xml.Linq;

namespace etrian_odyssey_ap_patcher
{
    public class EtrianOdysseyDataExtractor
    {
        private EtrianOdysseyFiles files;

        public EtrianOdysseyDataExtractor(string rom_file)
        {
            RomParser parser = new RomParser(rom_file);

            Rom rom = parser.Parse();
            files = new EtrianOdysseyFiles(rom);





        }

        private string FormatToConstant(string value)
        {
            if (char.IsDigit(value[0]))
                value = "N" + value;

            return value.Replace(".", "").Replace('-', '_').Replace(' ', '_').ToUpper();
        }

        public void EquipmentData()
        {
            List<ItemEquipment> items = new List<ItemEquipment>();

            foreach (var entry in ((DataTable)files.Item.Tables[0]).Data)
            {
                var itm = new ItemEquipment(entry, ((MessageTable)files.ItemName.Tables[0]).Messages);

                if (itm.name.RawData.Length == 1 && itm.name.RawData[0] == 0)
                    continue;

                items.Add(itm);
            }

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("class EO1EquipmentNames:");

            foreach (var item in items)
            {
                string constantName = FormatToConstant(item.name.StringValue);

                stringBuilder.AppendLine($"    {constantName} = \"{item.name.StringValue}\"");
            }

            stringBuilder.AppendLine();

            stringBuilder.AppendLine();

            var itemPerEquipmentType = items.GroupBy(g => g.EquipmentType).OrderBy(o => o.Key).ToList();

            int id = 2000;

            foreach (IGrouping<EquipmentType, ItemEquipment> equipmentTypeGrouping in itemPerEquipmentType)
            {
                string nameConstant = FormatToConstant(equipmentTypeGrouping.Key.ToString());

                stringBuilder.AppendLine($"{nameConstant}_DATA: list[EO1EquipmentData] = [");

                foreach (ItemEquipment item in equipmentTypeGrouping)
                {
                    string constantName = FormatToConstant(item.name.StringValue);

                    stringBuilder.AppendLine($"    EO1EquipmentData(EO1ItemType.Equipment, 0x{item.item_id.ToString("X3")}, EO1EquipmentNames.{constantName}, {id++}, EO1EquipmentType.{item.EquipmentType.ToString()}), # DmgType: {item.DamageType}, Secondary DmgType: {item.SecondaryDamageType}, ATK: {item.attack_1}, DEF: {item.defense}");
                }
                stringBuilder.AppendLine("]");

                stringBuilder.AppendLine();
            }



            File.WriteAllText("D:\\Projects\\EtrianOdyssey\\DataDump\\item_tbb_0.txt", stringBuilder.ToString());
        }

        public void Class2Skill()
        {
            List<Class2Skill> class2Skills = new List<Class2Skill>();

            foreach (byte[] entry in ((DataTable)files.Class2Skill.Tables[0]).Data)
            {
                class2Skills.Add(new Class2Skill(entry));
            }

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var item in class2Skills)
            {
                stringBuilder.AppendLine($"    EO1Class2SkillData(0x{item.SkillID.ToString("X2")}, 0x{item.RequiredSkillID1.ToString("X2")}, {item.RequiredSkillLevel1}, 0x{item.RequiredSkillID2.ToString("X2")}, {item.RequiredSkillLevel2}),");
            }

            File.WriteAllText("D:\\Projects\\EtrianOdyssey\\DataDump\\class2skill_tbb.txt", stringBuilder.ToString());
        }

        public void PlayerSkillData()
        {
            EtrianString[] skillnames = ((MessageTable)files.PlayerSkillName.Tables[0]).Messages;
            List<PlayerSkillData0> playerSkillData0s = new List<PlayerSkillData0>();

            foreach (byte[] entry in ((DataTable)files.PlayerSkill.Tables[0]).Data)
            {
                var skill = new PlayerSkillData0(entry);
                playerSkillData0s.Add(skill);
            }

            StringBuilder stringBuilder = new StringBuilder();

            for (int classindex = 1; classindex < 10; classindex++)
            {
                Dictionary<ushort, PlayerSkillData1> playerSkillData1s = new Dictionary<ushort, PlayerSkillData1>();

                byte[][] all_data = ((DataTable)files.PlayerSkill.Tables[classindex]).Data;

                for (int i = 0; i < all_data.Length; i++)
                {
                    byte[] entry = all_data[i];

                    var skill = new PlayerSkillData1(entry, skillnames);

                    for (int j = 1; j < 6; j++)
                    {
                        if (i + j >= all_data.Length)
                            break;

                        byte[] sub_entry = all_data[i + j];

                        var skill_sub_entry = new PlayerSkillData1(sub_entry, skillnames);

                        if (skill_sub_entry.SkillID != 0)
                            break;

                        skill.SubEntry.Add(skill_sub_entry);
                    }

                    if (skill.SkillID == 0)
                        continue;

                    skill.Data0Entry = playerSkillData0s[skill.SkillID];

                    playerSkillData1s.Add(skill.SkillID, skill);
                }

                //foreach (var skill in playerSkillData1s.Values)
                //{
                //    string constantName = FormatToConstant("CLASS_" + skill.Name.StringValue);

                //    stringBuilder.AppendLine($"{constantName} = 0x{skill.SkillID.ToString("X2")}");
                //}

                //stringBuilder.AppendLine();

                foreach (var skill in playerSkillData1s.Values)
                {
                    //string constantName = FormatToConstant("CLASS_" + skill.Name.StringValue);

                    stringBuilder.AppendLine($"    0x{skill.SkillID.ToString("X2")}: EO1SkillData(0x{skill.SkillID.ToString("X2")}, \"{skill.Name}\"),");
                }

                //stringBuilder.AppendLine();
                //stringBuilder.AppendLine();
            }

            File.WriteAllText("D:\\Projects\\EtrianOdyssey\\DataDump\\playerskill_tbb_1.txt", stringBuilder.ToString());

            /*
            var t = playerSkillData1s.Values;

            var t0 = t.GroupBy(g => g.Data0Entry.Unknown0).ToList();
            var t1 = t.GroupBy(g => g.Data0Entry.Unknown1).ToList();
            var t2 = t.GroupBy(g => g.Data0Entry.Unknown2).ToList();
            var t3 = t.GroupBy(g => g.Data0Entry.Unknown3).ToList();
            var t4 = t.GroupBy(g => g.Data0Entry.Unknown4).ToList();
            var t5 = t.GroupBy(g => g.Data0Entry.Unknown5).ToList();
            var t6 = t.GroupBy(g => g.Data0Entry.Unknown6).ToList();
            var t7 = t.GroupBy(g => g.Data0Entry.Unknown7).ToList();
            var t8 = t.GroupBy(g => g.Data0Entry.Unknown8).ToList();
            var t9 = t.GroupBy(g => g.Data0Entry.Unknown9).ToList();
            var tA = t.GroupBy(g => g.Data0Entry.UnknownA).ToList();
            var tB = t.GroupBy(g => g.Data0Entry.UnknownB).ToList();


            Dictionary<ushort, Skill2Effect> skill2Effects0 = new Dictionary<ushort, Skill2Effect>();

            foreach (byte[] entry in ((DataTable)files.Skill2Effect.Tables[0]).Data)
            {
                var effect = new Skill2Effect(entry);
                skill2Effects0.Add(effect.Unknown00, effect);
            }

            Dictionary<ushort, Skill2Effect> skill2Effects1 = new Dictionary<ushort, Skill2Effect>();
            foreach (byte[] entry in ((DataTable)files.Skill2Effect.Tables[1]).Data)
            {
                var effect = new Skill2Effect(entry);
                skill2Effects1.Add(effect.Unknown00, effect);
            }

            var campskillinfo = ((MessageTable)files.CampSkillInfo.Tables[0]).Messages;
            var campskillexeinfo = ((MessageTable)files.CampSkillExeInfo.Tables[0]).Messages;
            var campskillcustom = ((MessageTable)files.CampSkillCustomNextMes.Tables[0]).Messages;

            List<Class2Skill> class2Skills = new List<Class2Skill>();


            foreach (byte[] entry in ((DataTable)files.Class2Skill.Tables[0]).Data)
            {
                class2Skills.Add(new Class2Skill(entry));
            }


            List<ushort> dups = new List<ushort>();
            foreach (var item in skill2Effects0)
            {
                if (skill2Effects1.ContainsKey(item.Key))
                    dups.Add(item.Key);
            }

            */
        }

        public void ItemData()
        {
            List<ItemOther> itemOthers = new List<ItemOther>();

            foreach (var entry in ((DataTable)files.Item.Tables[1]).Data)
            {
                var itm = new ItemOther(entry, ((MessageTable)files.ItemName.Tables[0]).Messages);

                if (itm.name.RawData.Length == 1 && itm.name.RawData[0] == 0)
                    continue;

                itemOthers.Add(itm);
            }

            // Remove material, handle it separately.
            itemOthers = itemOthers.Where(w => w.unknown_0F != 0x14 && w.unknown_0F != 0x15).ToList();

            StringBuilder stringBuilder = new StringBuilder();

            //var itemConsumable = itemOthers.Where(w => w.unknown_0E == 0x00 || w.unknown_0E == 0x03).ToList();
            //var itemKey = itemOthers.Where(w => w.unknown_0E != 0x00 && w.unknown_0E != 0x03).ToList();

            //stringBuilder.AppendLine($"class EO1ItemConsumables:");

            /*
            foreach (var item in itemConsumable)
            {
                string constantName = FormatToConstant(item.name.StringValue);

                stringBuilder.AppendLine($"    {constantName} = \"{item.name.StringValue}\"");
            }

            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"class EO1KeyItems:");

            foreach (var item in itemKey)
            {
                string constantName = FormatToConstant(item.name.StringValue);

                stringBuilder.AppendLine($"    {constantName} = \"{item.name.StringValue}\"");
            }
            */
            stringBuilder.AppendLine("class EO1ItemNames:");

            foreach (var item in itemOthers)
            {
                string constantName = FormatToConstant(item.name.StringValue);

                stringBuilder.AppendLine($"    {constantName} = \"{item.name.StringValue}\"");
            }

            stringBuilder.AppendLine();

            stringBuilder.AppendLine("ITEM_TABLE: list[EO1ItemData] = {");

            int id = 1000;

            foreach (var item in itemOthers)
            {
                string constantName = FormatToConstant(item.name.StringValue);

                string itemType;

                if (item.unknown_0E == 0x00 || item.unknown_0E == 0x03)
                    itemType = "Consumable";
                else
                    itemType = "Key"; // Can't differenciates between Key and Quest items.

                stringBuilder.AppendLine($"    EO1ItemData(EO1ItemType.{itemType}, 0x{item.item_id.ToString("X4")}, EO1ItemNames.{constantName}, {id++}),");
            }

            File.WriteAllText("D:\\Projects\\EtrianOdyssey\\DataDump\\item_tbb_1.txt", stringBuilder.ToString());
        }

        public void ItemCompound()
        {
            var messages = ((MessageTable)files.ItemName.Tables[0]).Messages;

            List<ItemCompound> itemCompounds = new List<ItemCompound>();

            foreach (var entry in ((DataTable)files.ItemCompound.Tables[0]).Data)
            {
                var itm = new ItemCompound(entry, messages);

                if (itm.name.RawData.Length == 1 && itm.name.RawData[0] == 0)
                    continue;

                itemCompounds.Add(itm);
            }
        }

        public void ChestData()
        {
            Dictionary<int, List<TreasureChestTile>> chestsByFloors = new Dictionary<int, List<TreasureChestTile>>();

            for (int i = 0; i < 30; i++)
            {
                int floorNumber = i + 1;
                List<TreasureChestTile> chests = GetFloorChests(floorNumber);
                chestsByFloors.Add(floorNumber, chests);
            }

            List<string> output = new List<string>();

            MessageTable itemNames = ((MessageTable)files.ItemName.Tables[0]);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (KeyValuePair<int, List<TreasureChestTile>> chests in chestsByFloors)
            {
                foreach (TreasureChestTile chest in chests.Value)
                {
                    string contentStr = string.Empty;
                    switch (chest.treasureType)
                    {
                        case TreasureType.Money:
                            contentStr = $"{chest.treasureMoney}en";
                            break;
                        case TreasureType.Item:
                            contentStr = $"0x{chest.treasureItemID.ToString("X4")} - {itemNames.Messages[chest.treasureItemID - 1]}";
                            break;
                        case TreasureType.AP:
                        case TreasureType.Floor:
                        case TreasureType.Level:
                        case TreasureType.Class:
                        case TreasureType.Other:
                        default:
                            break;
                    }

                    output.Add($"B{chests.Key}F chest #{chest.treasureChestID} at [{chest.XCoord},{chest.YCoord}]. Content: {chest.treasureType}, {contentStr}");

                    stringBuilder.AppendLine($"    TreasureData({chests.Key}, {chest.treasureChestID}, \"\", TreasureContentType.{chest.treasureType}, {(chest.treasureType == TreasureType.Item ? chest.treasureItemID : chest.treasureMoney)}, EO1Regions.), # B{chests.Key}F At [{chest.XCoord},{chest.YCoord}], {contentStr}");
                }
            }

            //File.WriteAllLines("D:\\Projects\\EtrianOdyssey\\DataDump\\chests.txt", output);
            File.WriteAllText("D:\\Projects\\EtrianOdyssey\\DataDump\\chests.txt", stringBuilder.ToString());
        }

        public void GovernmentMissions()
        {
            byte[][] all_data = ((DataTable)files.GovernmentMissionData.Tables[0]).Data;

            Dictionary<ushort, GovernmentMissionData> missions = new Dictionary<ushort, GovernmentMissionData>();
            for (ushort i = 0; i < all_data.Length; i++)
            {
                byte[] data = all_data[i];

                EtrianString name = ((MessageTable)files.GovernmentMissionName.Tables[0]).Messages[i];

                missions.Add(i, new GovernmentMissionData(data, name));
            }

            StringBuilder stringBuilder = new StringBuilder();

            foreach (GovernmentMissionData mission in missions.Values)
            {
                stringBuilder.AppendLine($"    MissionData(0x{mission.mission_id.ToString("X")}, \"{mission.Name}\", 0x{mission.MissionResultsReportedFlagID.ToString("X2")})");
            }

            File.WriteAllText("D:\\Projects\\EtrianOdyssey\\DataDump\\mission_data.txt", stringBuilder.ToString());
        }

        private List<TreasureChestTile> GetFloorChests(int floorNumber)
        {
            var floorFile = files.GetFloorFile(floorNumber);
            List<TreasureChestTile> chests = new List<TreasureChestTile>();

            for (int y = 0; y < MapDataFile.MapHeight; y++)
            {
                for (int x = 0; x < MapDataFile.MapWidth; x++)
                {
                    if (floorFile.Tiles[x, y].TileType == MapDataFile.TileTypes.TreasureChest)
                    {
                        chests.Add((TreasureChestTile)floorFile.Tiles[x, y]);
                    }
                }
            }

            return chests;
        }

        public void EncounterGroups()
        {
            byte[][] all_data = ((DataTable)files.EnemyData.Tables[0]).Data;
            Dictionary<ushort, EnemyData> enemies = new Dictionary<ushort, EnemyData>();
            for (ushort i = 1; i < all_data.Length; i++)
            {
                byte[] data = all_data[i];

                EtrianString name = ((MessageTable)files.EnemyName.Tables[0]).Messages[i - 1];

                enemies.Add(i, new EnemyData(data, i, name));
            }

            all_data = ((DataTable)files.EncounterData.Tables[0]).Data;

            Dictionary<ushort, EncounterData> encounters = new Dictionary<ushort, EncounterData>();
            for (ushort i = 0; i < all_data.Length; i++)
            {
                byte[] data = all_data[i];
                encounters.Add(i, new EncounterData(data, i, enemies));
            }

            all_data = ((DataTable)files.EncounterData.Tables[1]).Data;
            Dictionary<ushort, EncounterGroup> encounterGroups = new Dictionary<ushort, EncounterGroup>();
            for (ushort i = 0; i < all_data.Length; i++)
            {
                byte[] data = all_data[i];
                encounterGroups.Add(i, new EncounterGroup(data, i, encounters));
            }

            StringBuilder stringBuilder = new StringBuilder();

            foreach (EncounterGroup encounterGroup in encounterGroups.Values)
            {
                stringBuilder.AppendLine($"    EncounterGroupData(0x{encounterGroup.GroupID.ToString("X")}, " +
                    $"0x{encounterGroup.EncounterID1.ToString("X")}, " +
                    $"0x{encounterGroup.EncounterID2.ToString("X")}, " +
                    $"0x{encounterGroup.EncounterID3.ToString("X")}),");
            }

            File.WriteAllText("D:\\Projects\\EtrianOdyssey\\DataDump\\encounter_group_data.txt", stringBuilder.ToString());
        }

        public void EncounterData()
        {
            byte[][] all_data = ((DataTable)files.EnemyData.Tables[0]).Data;
            Dictionary<ushort, EnemyData> enemies = new Dictionary<ushort, EnemyData>();
            for (ushort i = 1; i < all_data.Length; i++)
            {
                byte[] data = all_data[i];

                EtrianString name = ((MessageTable)files.EnemyName.Tables[0]).Messages[i - 1];

                enemies.Add(i, new EnemyData(data, i, name));
            }

            all_data = ((DataTable)files.EncounterData.Tables[0]).Data;

            Dictionary<ushort, EncounterData> encounters = new Dictionary<ushort, EncounterData>();
            for (ushort i = 0; i < all_data.Length; i++)
            {
                byte[] data = all_data[i];
                encounters.Add(i, new EncounterData(data, i, enemies));
            }

            StringBuilder stringBuilder = new StringBuilder();

            foreach (EncounterData encounter in encounters.Values)
            {
                stringBuilder.AppendLine($"    EncounterData(0x{encounter.EncounterID.ToString("X")}, " +
                    $"0x{encounter.EnemiesID[0].ToString("X")}, " +
                    $"0x{encounter.EnemiesID[1].ToString("X")}, " +
                    $"0x{encounter.EnemiesID[2].ToString("X")}, " +
                    $"0x{encounter.EnemiesID[3].ToString("X")}, " +
                    $"0x{encounter.EnemiesID[4].ToString("X")}),");
            }

            File.WriteAllText("D:\\Projects\\EtrianOdyssey\\DataDump\\encounter_data.txt", stringBuilder.ToString());
        }

        public void EnemyData()
        {
            byte[][] all_data = ((DataTable)files.EnemyData.Tables[0]).Data;
            Dictionary<ushort, EnemyData> enemies = new Dictionary<ushort, EnemyData>();
            for (ushort i = 1; i < all_data.Length; i++)
            {
                byte[] data = all_data[i];

                EtrianString name = ((MessageTable)files.EnemyName.Tables[0]).Messages[i - 1];

                enemies.Add(i, new EnemyData(data, i, name));
            }

            StringBuilder stringBuilder = new StringBuilder();


            stringBuilder.AppendLine("class EO1Enemies:");

            foreach (EnemyData enemy in enemies.Values)
            {
                // Skip dummies.
                if (enemy.codex_id == 0)
                    continue;
                string constantName = FormatToConstant(enemy.Name.StringValue);

                stringBuilder.AppendLine($"    {constantName} = 0x{enemy.enemy_id.ToString("X2")}");
            }

            stringBuilder.AppendLine();

            foreach (EnemyData enemy in enemies.Values)
            {
                // Skip dummies.
                if (enemy.codex_id == 0)
                    continue;

                string constantName = FormatToConstant(enemy.Name.StringValue);

                stringBuilder.AppendLine($"    EnemyData(EO1Enemies.{constantName}, \"{enemy.Name}\", {enemy.level}, EnemyType.{enemy.GetEnemyType()}, " +
                    $"0x{enemy.codex_id.ToString("X000")}, 0x{enemy.Drop1ItemID.ToString("X000")}, 0x{enemy.Drop2ItemID.ToString("X000")}, " +
                    $"0x{enemy.Drop3ItemID.ToString("X000")}, DropCondition.{enemy.GetDropCondition()}),");
            }

            File.WriteAllText("D:\\Projects\\EtrianOdyssey\\DataDump\\enemy.txt", stringBuilder.ToString());
        }
    }
}

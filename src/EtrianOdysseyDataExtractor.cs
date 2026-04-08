using etrian_odyssey_ap_patcher.EtrianOdyssey;
using etrian_odyssey_ap_patcher.EtrianOdyssey.Data;
using etrian_odyssey_ap_patcher.EtrianOdyssey.Event;
using etrian_odyssey_ap_patcher.EtrianOdyssey.Files;
using etrian_odyssey_ap_patcher.EtrianOdyssey.MapData;
using etrian_odyssey_ap_patcher.EtrianOdyssey.Table;
using etrian_odyssey_ap_patcher.NitroRom;
using System;
using System.Text;
using System.Xml.Linq;

namespace etrian_odyssey_ap_patcher
{
    public class EtrianOdysseyDataExtractor
    {
        private EtrianOdysseyFiles files;
        private readonly string output_folder;

        public EtrianOdysseyDataExtractor(string rom_file, string outputFolder)
        {
            RomParser parser = new RomParser(rom_file);

            Rom rom = parser.Parse();
            files = new EtrianOdysseyFiles(rom);
            output_folder = outputFolder;


            StringBuilder stringBuilder = new StringBuilder();

            for (int x = 0; x < 0x200; x += 2)
            {
                stringBuilder.AppendLine($"0x{x.ToString("X3")}/0x{(8 * x).ToString("X3")}/{8 * x}");
            }

            OutputFile("numbers.txt", stringBuilder.ToString());

        }

        private void OutputFile(string filename, string content)
        {
            string full_output_path = Path.Combine(output_folder, filename);

            File.WriteAllText(full_output_path, content);
        }

        private string FormatToConstant(string value)
        {
            if (char.IsDigit(value[0]))
                value = "N" + value;

            return value.Replace(".", "").Replace('-', '_').Replace(' ', '_').Replace("\'", "").ToUpper();
        }

        private Dictionary<ushort, ItemOther> GetMaterials()
        {
            List<ItemOther> itemOthers = new List<ItemOther>();

            foreach (var entry in ((DataTable)files.Item.Tables[1]).Data)
            {
                var itm = new ItemOther(entry, ((MessageTable)files.ItemName.Tables[0]).Messages);

                if (itm.name.RawData.Length == 1 && itm.name.RawData[0] == 0)
                    continue;

                itemOthers.Add(itm);
            }

            // Keep only material.
            itemOthers = itemOthers.Where(w => w.unknown_0F == 0x14 || w.unknown_0F == 0x15).ToList();

            return itemOthers.ToDictionary(k => k.item_id, v => v);
        }

        public void ParseEvents()
        {
            Dictionary<string, EventFile> eventFiles = new Dictionary<string, EventFile>();


            for (int i = 0; i < 30; i++)
                LoadFile($"DUN_{(i + 1).ToString("d2")}F");

            //LoadFile("Byouin"); // Hospital
            LoadFile("Guild");
            LoadFile("Hiroba");
            LoadFile("Hospital");
            LoadFile("Jukai");

            for (int i = 0; i < 7; i++)
                LoadFile($"MIS_{i.ToString("d3")}");

            for (int i = 0; i < 93; i++)
            {
                //if (i == 11 || i == 24 || i == 26 || i == 27 || 
                //    i == 28 || i == 39 || i == 43 || i == 50 || 
                //    i == 66 || i == 67 || i == 69 || i == 72 || 
                //    i == 84 || i == 91)
                //    continue;
                LoadFile($"QUE_{i.ToString("d3")}");
            }

            LoadFile("Sakaba");
            LoadFile("Shop");
            LoadFile("Touti");
            LoadFile("Yadoya");

            foreach (KeyValuePair<string, EventFile> eventFile in eventFiles)
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (EventEntry event_entry in eventFile.Value.Events)
                {
                    stringBuilder.AppendLine($"event_index={event_entry.event_index}");
                    stringBuilder.AppendLine($"checkPlace={event_entry.checkPlace}");
                    stringBuilder.AppendLine($"coordX={event_entry.coordX}");
                    stringBuilder.AppendLine($"coordY={event_entry.coordY}");
                    stringBuilder.AppendLine($"unknown condition 04={event_entry.unknown_condition_04}");
                    stringBuilder.AppendLine($"unknown condition 05={event_entry.unknown_condition_05}");
                    stringBuilder.AppendLine($"direction=0x{event_entry.direction:X2}");
                    stringBuilder.AppendLine($"unknown condition 07={event_entry.unknown_condition_07}");
                    stringBuilder.AppendLine($"required_flag=0x{event_entry.required_flag.ToString("X3")}");
                    stringBuilder.AppendLine($"not_set_flag=0x{event_entry.not_set_flag.ToString("X3")}");
                    stringBuilder.AppendLine($"unknown condition 10={event_entry.unknown_condition_10}");
                    stringBuilder.AppendLine($"unknown condition 11={event_entry.unknown_condition_11}");
                    stringBuilder.AppendLine($"unknown 12={event_entry.unknown_12}");
                    stringBuilder.AppendLine($"script_offset={event_entry.script_offset}");
                    stringBuilder.AppendLine("script:");

                    foreach (EventScriptCommand command in event_entry.script.Commands)
                    {
                        string parameters = command.GetParametersAsString();

                        stringBuilder.AppendLine($"{command.CommandId}: {parameters}");
                    }

                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine();
                }

                OutputFile(@$"event\{eventFile.Key}.txt", stringBuilder.ToString());
            }

            void LoadFile(string filename)
            {
                var eventFile = new EventFile(files.GetFile(@$"/Data/Event/{filename}.cmp").file_content);
                eventFiles.Add(filename, eventFile);
            }
        }

        public void GatheringSpot()
        {
            Dictionary<ushort, ItemOther> materials = GetMaterials();

            List<GatherItemData> item_cut = new List<GatherItemData>();
            foreach (var entry in ((DataTable)files.SkillItemCutData.Tables[0]).Data)
            {
                var item = new GatherItemData(entry, materials);
                item_cut.Add(item);
            }

            List<GatherItemData> item_mine = new List<GatherItemData>();
            foreach (var entry in ((DataTable)files.SkillItemMiningData.Tables[0]).Data)
            {
                var item = new GatherItemData(entry, materials);
                item_mine.Add(item);
            }

            List<GatherItemData> item_pick = new List<GatherItemData>();
            foreach (var entry in ((DataTable)files.SkillItemPickData.Tables[0]).Data)
            {
                var item = new GatherItemData(entry, materials);
                item_pick.Add(item);
            }

            StringBuilder stringBuilder = new StringBuilder();

            int unique_id = 0;

            AddList(item_cut, "CHOP");
            AddList(item_mine, "MINE");
            AddList(item_pick, "TAKE");

            OutputFile("gather.txt", stringBuilder.ToString());

            void AddList(List<GatherItemData> list, string gather_type)
            {
                foreach (GatherItemData item in list)
                {
                    string item_1 = FormatToConstant(item.Item1Name);
                    string item_2 = FormatToConstant(item.Item2Name);
                    string item_3 = FormatToConstant(item.Item3Name);

                    stringBuilder.AppendLine($"    EO1GatheringSpotData({unique_id++}, " +
                        $"EO1GatherType.{gather_type}, " +
                        $"{item.gatherNumber}, " +
                        $"{item.floorNumber}, " +
                        $"EO1MaterialID.{item_1}, " +
                        $"EO1MaterialID.{item_2}, " +
                        $"EO1MaterialID.{item_3}, " +
                        $"{item.itemProbability1}, " +
                        $"{item.itemProbability2}, " +
                        $"{item.itemProbability3}, " +
                        $"EO1Regions.), " +
                        $"# X Coord:{item.xCoord}, Y Coord:{item.yCoord}");
                }
            }
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


            stringBuilder.AppendLine("class EO1ItemID:");

            foreach (var item in items)
            {
                string constantName = FormatToConstant(item.name.StringValue);

                stringBuilder.AppendLine($"    {constantName} = 0x{item.item_id.ToString("X4")}");
            }

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

        private class ClassStats
        {
            public ClassStats()
            {
                for (int i = 0; i < 70; i++)
                {
                    HP[i] = 0;
                    TP[i] = 0;
                    STR[i] = 0;
                    VIT[i] = 0;
                    TEC[i] = 0;
                    AGI[i] = 0;
                    LUC[i] = 0;
                }
            }

            private void AddAll(int level, LevelUpData0Sub stats)
            {
                HP[level] += stats.hp;
                TP[level] += stats.tp;
                STR[level] += stats.str;
                VIT[level] += stats.vit;
                TEC[level] += stats.tec;
                AGI[level] += stats.agi;
                LUC[level] += stats.luc;
            }

            public void AddStats(int level, LevelUpData0Sub stats)
            {
                for (int cum_level = level; cum_level < 70; cum_level++)
                {
                    AddAll(cum_level, stats);
                }

            }

            public ushort[] HP { get; set; } = new ushort[70];
            public ushort[] TP { get; set; } = new ushort[70];
            public byte[] STR { get; set; } = new byte[70];
            public byte[] VIT { get; set; } = new byte[70];
            public byte[] TEC { get; set; } = new byte[70];
            public byte[] AGI { get; set; } = new byte[70];
            public byte[] LUC { get; set; } = new byte[70];
        }

        public void ClassLevelStats()
        {
            List<LevelUpData0> levelUpData = new List<LevelUpData0>();

            foreach (byte[] entry in ((DataTable)files.LevelUp.Tables[0]).Data)
            {
                levelUpData.Add(new LevelUpData0(entry));
            }


            List<ClassStats> classStats = new List<ClassStats>();

            for (int i = 0; i < 9; i++)
                classStats.Add(new ClassStats());

            for (int level = 0; level < 70; level++)
            {
                for (int class_id = 0; class_id < 9; class_id++)
                {
                    LevelUpData0 level_up_data = levelUpData[level];
                    LevelUpData0Sub stats = level_up_data.stat_per_class[class_id];

                    classStats[class_id].AddStats(level, stats);
                }
            }

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < 9; i++)
            {
                string class_name = GetClassNameByIndex(i);

                string constantName = FormatToConstant(class_name + "_STATS");
                string classConst = FormatToConstant(class_name);

                ClassStats stats = classStats[i];

                stringBuilder.AppendLine($"{constantName} = EO1ClassStats(EO1Class.{classConst}, " +
                    $"[{FormatStatsInt16(stats.HP)}]," +
                    $"[{FormatStatsInt16(stats.TP)}]," +
                    $"[{FormatStatsInt8(stats.STR)}]," +
                    $"[{FormatStatsInt8(stats.TEC)}]," +
                    $"[{FormatStatsInt8(stats.VIT)}]," +
                    $"[{FormatStatsInt8(stats.AGI)}]," +
                    $"[{FormatStatsInt8(stats.LUC)}]" +
                    $")");
            }

            File.WriteAllText("D:\\Projects\\EtrianOdyssey\\DataDump\\class_stats.txt", stringBuilder.ToString());

            string FormatStatsInt16(ushort[] stat)
            {
                return string.Join(',', stat);
            }
            string FormatStatsInt8(byte[] stat)
            {
                return string.Join(',', stat);
            }

            string GetClassNameByIndex(int classindex)
            {
                switch (classindex + 1)
                {
                    case 1:
                        return "Landsknecht";
                    case 2:
                        return "Survivalist";
                    case 3:
                        return "Protector";
                    case 4:
                        return "Dark Hunter";
                    case 5:
                        return "Ronin";
                    case 6:
                        return "Medic";
                    case 7:
                        return "Alchemist";
                    case 8:
                        return "Troubadour";
                    case 9:
                        return "Hexer";
                    default:
                        throw new NotImplementedException();
                }
            }
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


            Dictionary<int, Dictionary<ushort, PlayerSkillData1>> playerClassSkillData = new Dictionary<int, Dictionary<ushort, PlayerSkillData1>>();
            List<PlayerSkillData1> allSkills = new List<PlayerSkillData1>();
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

                playerClassSkillData.Add(classindex, playerSkillData1s);
                allSkills.AddRange(playerSkillData1s.Values);
            }

            StringBuilder stringBuilder = new StringBuilder();

            // Constants.
            foreach (var classSkillData in playerClassSkillData)
            {
                string class_name = GetClassNameByIndex(classSkillData.Key);

                foreach (var skill in classSkillData.Value.Values)
                {
                    string constantName = FormatToConstant(class_name + "_" + skill.Name.StringValue);

                    stringBuilder.AppendLine($"    {constantName} = 0x{skill.SkillID.ToString("X2")}");
                }
            }

            List<Class2Skill> class2Skills = new List<Class2Skill>();

            foreach (byte[] entry in ((DataTable)files.Class2Skill.Tables[0]).Data)
            {
                class2Skills.Add(new Class2Skill(entry));
            }

            stringBuilder.AppendLine();

            int ap_id = 200;

            // Data.
            foreach (var classSkillData in playerClassSkillData)
            {
                string class_name = GetClassNameByIndex(classSkillData.Key);

                string class_const = FormatToConstant(class_name);

                stringBuilder.AppendLine($"{class_const}_SKILLS: list[EO1SkillData] = [");
                foreach (PlayerSkillData1 skill in classSkillData.Value.Values)
                {
                    int skill_index = -1;
                    int offset = (classSkillData.Key - 1) * 21;

                    for (int i = 0; i < 21; i++)
                    {
                        if (class2Skills[offset + i].SkillID == skill.SkillID)
                        {
                            skill_index = i;
                            break;
                        }
                    }

                    if (skill_index == -1)
                        throw new Exception($"Skill not found {skill.SkillID}.");

                    string constantName = FormatToConstant(class_name + "_" + skill.Name.StringValue);

                    stringBuilder.Append($"    EO1SkillData(EO1Skills.{constantName}, \"{skill.Name}\", {ap_id++}, EO1Class.{class_const}, " +
                        $"{skill_index}, " +
                        $"EO1SkillType.{skill.GetSkillType()}, EO1Element.{skill.SkillAttributes.PrimaryDamageType.ToString()}, " +
                        $"EO1Element.{skill.SkillAttributes.SecondaryDamageType.ToString()}, EO1Ailment.{skill.GetAilment()}, " +
                        $"EO1SkillNeed.{skill.SkillAttributes.SkillNeed}, " +
                        $"EO1SkillMastery.{skill.SkillAttributes.SkillMastery}, " +
                        $"EO1SkillTarget.{skill.SkillAttributes.SkillTarget}, " +
                        $"EO1SkillSide.{skill.SkillAttributes.SkillSide}, " +
                        $"EO1SkillUsage.{skill.SkillAttributes.SkillUsage}, " +
                        $"EO1SkillEffect.{skill.SkillAttributes.SkillEffect}, " +
                        $"[");

                    stringBuilder.Append(GetSkillValueStr(skill));

                    foreach (PlayerSkillData1 sub_entry in skill.SubEntry)
                    {
                        stringBuilder.Append(",");
                        stringBuilder.Append(GetSkillValueStr(sub_entry));
                    }

                    stringBuilder.AppendLine($"]),");
                    skill_index++;
                }
                stringBuilder.AppendLine($"]");
                stringBuilder.AppendLine();
            }


            var t = allSkills.GroupBy(g => g.SkillAttributes.SkillType).ToList();
            var sub = allSkills.SelectMany(s => s.SubEntry).ToList();

            sub.AddRange(allSkills);

            var tt = sub.GroupBy(g => g.ValueType).OrderBy(o => (ushort)(o.Key)).ToList();



            File.WriteAllText("D:\\Projects\\EtrianOdyssey\\DataDump\\playerskill_tbb_1.txt", stringBuilder.ToString());

            string GetSkillValueStr(PlayerSkillData1 skill_data)
            {
                return $"EO1SkillValue(EO1SkillValueType.{skill_data.GetSkillValueType()}," +
                    $"[{string.Join(',', skill_data.ValuePerLevel.Take(10))}])";
            }

            string GetClassNameByIndex(int classindex)
            {
                switch (classindex)
                {
                    case 1:
                        return "Landsknecht";
                    case 2:
                        return "Survivalist";
                    case 3:
                        return "Protector";
                    case 4:
                        return "Dark Hunter";
                    case 5:
                        return "Ronin";
                    case 6:
                        return "Medic";
                    case 7:
                        return "Alchemist";
                    case 8:
                        return "Troubadour";
                    case 9:
                        return "Hexer";
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        /*

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

   Dictionary<ushort, PlayerSkillData1> playerSkillData1s = new Dictionary<ushort, PlayerSkillData1>();
for (int classindex = 1; classindex < 10; classindex++)
{

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

   foreach (PlayerSkillData1 skill in playerSkillData1s.Values)
   {
       //string constantName = FormatToConstant("CLASS_" + skill.Name.StringValue);

       stringBuilder.AppendLine($"    0x{skill.SkillID.ToString("X2")}: EO1SkillData(0x{skill.SkillID.ToString("X2")}, \"{skill.Name}\"),");
   }

   //stringBuilder.AppendLine();
   //stringBuilder.AppendLine();
}

File.WriteAllText("D:\\Projects\\EtrianOdyssey\\DataDump\\playerskill_tbb_1.txt", stringBuilder.ToString());





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
//
}
*/

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

            stringBuilder.AppendLine("class EO1ItemID:");

            foreach (var item in itemOthers)
            {
                string constantName = FormatToConstant(item.name.StringValue);

                stringBuilder.AppendLine($"    {constantName} = 0x{item.item_id.ToString("X4")}");
            }

            stringBuilder.AppendLine();

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

        private HashSet<ushort> GetGatheringMaterials()
        {
            HashSet<ushort> gatheringMaterials = new HashSet<ushort>();
            Dictionary<ushort, ItemOther> gatheringSpot = GetMaterials();

            foreach (var entry in ((DataTable)files.SkillItemCutData.Tables[0]).Data)
            {
                var item = new GatherItemData(entry, gatheringSpot);
                AddGatheringSpotData(item);
            }

            foreach (var entry in ((DataTable)files.SkillItemMiningData.Tables[0]).Data)
            {
                var item = new GatherItemData(entry, gatheringSpot);
                AddGatheringSpotData(item);
            }

            foreach (var entry in ((DataTable)files.SkillItemPickData.Tables[0]).Data)
            {
                var item = new GatherItemData(entry, gatheringSpot);
                AddGatheringSpotData(item);
            }

            return gatheringMaterials;

            void AddGatheringSpotData(GatherItemData item)
            {

                if (item.itemID1 != 0)
                    gatheringMaterials.Add(item.itemID1);
                if (item.itemID2 != 0)
                    gatheringMaterials.Add(item.itemID2);
                if (item.itemID3 != 0)
                    gatheringMaterials.Add(item.itemID3);
            }
        }

        private HashSet<ushort> GetMonsterMaterials()
        {
            byte[][] all_data = ((DataTable)files.EnemyData.Tables[0]).Data;
            HashSet<ushort> monsterMaterials = new HashSet<ushort>();
            for (ushort i = 1; i < all_data.Length; i++)
            {
                byte[] data = all_data[i];

                EtrianString name = ((MessageTable)files.EnemyName.Tables[0]).Messages[i - 1];

                EnemyData enemy_data = new EnemyData(data, i, name);

                // Skip dummies.
                if (enemy_data.codex_id == 0 && enemy_data.enemy_id != 0x7D)
                    continue;

                AddEnemyData(enemy_data);
            }

            return monsterMaterials;

            void AddEnemyData(EnemyData item)
            {

                if (item.Drop1ItemID != 0)
                    monsterMaterials.Add(item.Drop1ItemID);
                if (item.Drop2ItemID != 0)
                    monsterMaterials.Add(item.Drop2ItemID);
                if (item.Drop3ItemID != 0)
                    monsterMaterials.Add(item.Drop3ItemID);
            }
        }

        public void MaterialData()
        {
            // Setup auxiliary data
            HashSet<ushort> gatheringMaterials = GetGatheringMaterials();
            HashSet<ushort> monsterMaterials = GetMonsterMaterials();

            //
            List<ItemOther> itemOthers = new List<ItemOther>();

            foreach (var entry in ((DataTable)files.Item.Tables[1]).Data)
            {
                var itm = new ItemOther(entry, ((MessageTable)files.ItemName.Tables[0]).Messages);

                if (itm.name.RawData.Length == 1 && itm.name.RawData[0] == 0)
                    continue;

                itemOthers.Add(itm);
            }

            // Keep only material.
            itemOthers = itemOthers.Where(w => w.unknown_0F == 0x14 || w.unknown_0F == 0x15).ToList();

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("class EO1MaterialID:");

            foreach (var item in itemOthers)
            {
                string constantName = FormatToConstant(item.name.StringValue);

                stringBuilder.AppendLine($"    {constantName} = 0x{item.item_id.ToString("X4")}");
            }

            //var t = itemOthers.GroupBy(g => g.unknown_0E).ToList();
            //var t2 = itemOthers.GroupBy(g => g.unknown_0F).ToList();

            stringBuilder.AppendLine();

            stringBuilder.AppendLine("COMPENDIUM_TABLE: list[CompendiumData] = [");

            int id = 3000;
            foreach (var item in itemOthers)
            {
                string constantName = FormatToConstant(item.name.StringValue);
                string materialType = GetMaterialType(item.item_id);
                //if (item.unknown_0E == 0x00 || item.unknown_0E == 0x03)
                //    itemType = "Consumable";
                //else
                //    itemType = "Key"; // Can't differenciates between Key and Quest items.

                stringBuilder.AppendLine($"    CompendiumData(EO1MaterialID.{constantName}, " +
                    $"\"{item.name.StringValue}\", {id++}, CompendiumSource.{materialType}),");
                //$"{item.unknown_0E}, " +
                //$"{item.unknown_0F}),");
            }

            stringBuilder.AppendLine("]");

            OutputFile("material.txt", stringBuilder.ToString());

            string GetMaterialType(ushort material_id)
            {
                if (gatheringMaterials.Contains(material_id) && monsterMaterials.Contains(material_id))
                    return "BOTH";

                if (gatheringMaterials.Contains(material_id))
                    return "GATHERING";

                if (monsterMaterials.Contains(material_id))
                    return "MONSTER";

                throw new Exception($"Material {material_id} not obtainable.");
            }
        }

        public void ItemCompound()
        {
            var messages = ((MessageTable)files.ItemName.Tables[0]).Messages;

            List<ItemCompound> itemCompounds = new List<ItemCompound>();
            List<ItemCompound> filteredItemCompounds = new List<ItemCompound>();

            foreach (var entry in ((DataTable)files.ItemCompound.Tables[0]).Data)
            {
                var itm = new ItemCompound(entry, messages);

                if (itm.name.RawData.Length == 1 && itm.name.RawData[0] == 0)
                    continue;

                itemCompounds.Add(itm);

                if (itm.material_1_count == 0)
                    continue;

                filteredItemCompounds.Add(itm);
            }

            StringBuilder stringBuilder = new StringBuilder();

            foreach (ItemCompound compound in filteredItemCompounds)
            {
                string result_item_constant_name = FormatToConstant(compound.name.StringValue);

                string material_1_item_constant_name = FormatToConstant(compound.material_1_item_name?.StringValue ?? "NONE");
                string material_2_item_constant_name = FormatToConstant(compound.material_2_item_name?.StringValue ?? "NONE");
                string material_3_item_constant_name = FormatToConstant(compound.material_3_item_name?.StringValue ?? "NONE");


                stringBuilder.AppendLine($"    EO1ItemCompound(EO1ItemID.{result_item_constant_name}, " +
                    $"{GetConstantOr0(material_1_item_constant_name)}, " +
                    $"{GetConstantOr0(material_2_item_constant_name)}, " +
                    $"{GetConstantOr0(material_3_item_constant_name)}, " +
                    $"{compound.material_1_count}, " +
                    $"{compound.material_2_count}, " +
                    $"{compound.material_3_count}),");
            }

            OutputFile("item_compound_tbb.txt", stringBuilder.ToString());

            string GetConstantOr0(string constantName)
            {
                if (constantName == "NONE")
                    return "0";
                string constant_class_name = "EO1MaterialID";

                return $"{constant_class_name}.{constantName}";
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

                EnemyData enemy_data = new EnemyData(data, i, name);

                // Skip dummies.
                if (enemy_data.codex_id == 0 && enemy_data.enemy_id != 0x7D)
                    continue;

                enemies.Add(i, enemy_data);
            }

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("class EO1Enemies:");

            foreach (EnemyData enemy in enemies.Values)
            {
                string constantName = FormatToConstant(enemy.Name.StringValue);

                stringBuilder.AppendLine($"    {constantName} = 0x{enemy.enemy_id.ToString("X2")}");
            }

            stringBuilder.AppendLine();

            foreach (EnemyData enemy in enemies.Values)
            {
                string constantName = FormatToConstant(enemy.Name.StringValue);

                stringBuilder.AppendLine($"    EnemyData(EO1Enemies.{constantName}, \"{enemy.Name}\", {enemy.level}, EnemyType.{enemy.GetEnemyType()}, " +
                    $"0x{enemy.codex_id.ToString("X000")}, {enemy.HP}, {enemy.STR}, {enemy.VIT}, {enemy.TEC}, " +
                    $"{enemy.CutResistance}, {enemy.BashResistance}, {enemy.StabResistance}, " +
                    $"{enemy.FireResistance}, {enemy.IceResistance}, {enemy.VoltResistance}, " +
                    $"0x{enemy.Drop1ItemID.ToString("X000")}, 0x{enemy.Drop2ItemID.ToString("X000")}, " +
                    $"0x{enemy.Drop3ItemID.ToString("X000")}, DropCondition.{enemy.GetDropCondition()}, {enemy.Item3Chances}),");
            }

            File.WriteAllText("D:\\Projects\\EtrianOdyssey\\DataDump\\enemy.txt", stringBuilder.ToString());
        }

        public void CodexData()
        {
            byte[][] all_data = ((DataTable)files.EnemyData.Tables[0]).Data;
            Dictionary<ushort, EnemyData> enemies = new Dictionary<ushort, EnemyData>();
            MessageTable messageTable = ((MessageTable)files.EnemyName.Tables[0]);
            for (ushort i = 1; i < all_data.Length; i++)
            {
                byte[] data = all_data[i];

                EtrianString name = messageTable.Messages[i - 1];

                EnemyData enemy_data = new EnemyData(data, i, name);

                // Skip dummies.
                if (enemy_data.codex_id == 0 && enemy_data.enemy_id != 0x7D)
                    continue;

                enemies.Add(i, enemy_data);
            }

            List<EnemyData> ordered_enemies = enemies.Values.OrderBy(o => o.codex_id).ToList();


            StringBuilder stringBuilder = new StringBuilder();

            int id = 2000;
            foreach (EnemyData enemy in ordered_enemies)
            {
                string constantName = FormatToConstant(enemy.Name.StringValue);
                stringBuilder.AppendLine($"    CodexData(EO1Enemies.{constantName}, " +
                    $"0x{enemy.codex_id.ToString("X000")}, {id++}),");
            }

            File.WriteAllText("D:\\Projects\\EtrianOdyssey\\DataDump\\codex.txt", stringBuilder.ToString());
        }
    }
}

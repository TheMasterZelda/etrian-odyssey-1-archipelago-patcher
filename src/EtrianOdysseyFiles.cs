using etrian_odyssey_ap_patcher.EtrianOdyssey.Files;
using etrian_odyssey_ap_patcher.NitroRom;

namespace etrian_odyssey_ap_patcher
{
    public class EtrianOdysseyFiles
    {
        private class RomFile<T> where T : BaseFile
        {
            public RomFile(FileEntry entry, T file) { file_entry = entry; base_file = file; }

            public FileEntry file_entry;
            public T base_file;
        }

        private const string DATA_PARAM = "/Data/Param/";
        private Rom rom;

        private readonly RomFile<TableFile> facility_text;
        private readonly RomFile<MapDataFile>[] map_data_files;

        private readonly RomFile<TableFile> item;
        private readonly RomFile<TableFile> item_info;
        private readonly RomFile<TableFile> item_name;
        private readonly RomFile<TableFile> item_ill_info;

        private readonly RomFile<TableFile> encount_data;
        private readonly RomFile<TableFile> enemy_data;
        private readonly RomFile<TableFile> enemy_name;

        private readonly RomFile<TableFile> government_mission_data;
        private readonly RomFile<TableFile> government_mission_name;

        private readonly RomFile<TableFile> player_skill;
        private readonly RomFile<TableFile> player_skill_name;
        private readonly RomFile<TableFile> camp_skill_info;
        private readonly RomFile<TableFile> camp_skill_exe_info;
        private readonly RomFile<TableFile> camp_skill_custom_next_mes;

        private readonly RomFile<TableFile> skill_2_effect;
        private readonly RomFile<TableFile> battle_skill_info;

        private readonly RomFile<TableFile> class_2_skill;

        public EtrianOdysseyFiles(Rom rom)
        {
            this.rom = rom;


            encount_data = LoadTableFile(DATA_PARAM + "EncountData.tbb");
            item = LoadTableFile(DATA_PARAM + "Item.tbb");
            item_info = LoadTableFile(DATA_PARAM + "ItemInfo.cmp");
            item_name = LoadTableFile(DATA_PARAM + "ItemName.cmp");
            item_ill_info = LoadTableFile(DATA_PARAM + "itemIllInfo.cmp");
            facility_text = LoadTableFile(DATA_PARAM + "FacilityText.cmp");

            encount_data = LoadTableFile(DATA_PARAM + "EncountData.tbb");
            enemy_data = LoadTableFile(DATA_PARAM + "EnemyData.tbb");
            enemy_name = LoadTableFile(DATA_PARAM + "EnemyName.mbb");

            government_mission_data = LoadTableFile(DATA_PARAM + "GovernmentMissionData.cmp");
            government_mission_name = LoadTableFile(DATA_PARAM + "GovernmentMissionName.cmp");

            player_skill = LoadTableFile(DATA_PARAM + "PlayerSkill.tbb");
            player_skill_name = LoadTableFile(DATA_PARAM + "PlayerSkillName.mbb");
            camp_skill_info = LoadTableFile(DATA_PARAM + "CampSkillInfo.mbb");
            camp_skill_exe_info = LoadTableFile(DATA_PARAM + "CampSkillExeInfo.mbb");
            camp_skill_custom_next_mes = LoadTableFile(DATA_PARAM + "CampSkillCustomNextMes.mbb");

            skill_2_effect = LoadTableFile(DATA_PARAM + "skill2effect.tbb");

            battle_skill_info = LoadTableFile(DATA_PARAM + "BtlSkillInfo.mbb");

            class_2_skill = LoadTableFile(DATA_PARAM + "Class2Skill.tbb");

            //var t = LoadTableFile(DATA_PARAM + "GovernmentMissionMess.cmp");
            //var tt = LoadTableFile(DATA_PARAM + "GovernmentMissionName.cmp");
            //var ttt = LoadTableFile(DATA_PARAM + "GovernmentMissionPrize.cmp");

            map_data_files = new RomFile<MapDataFile>[30];

            for (int i = 0; i < 30; i++)
            {
                map_data_files[i] = LoadMapDataFile($"/Data/MapDat/Ymd/MapB{(i + 1).ToString("00")}_ymd.cmp"); ;
            }
        }

        public void UpdateFiles()
        {
            UpdateFile(facility_text);

            foreach (RomFile<MapDataFile> file in map_data_files)
            {
                UpdateFile(file);
            }
        }

        private void UpdateFile<T>(RomFile<T> romFile) where T : BaseFile
        {
            romFile.file_entry.file_content = romFile.base_file.GetUpdatedData();
        }

        private RomFile<TableFile> LoadTableFile(string filename)
        {
            FileEntry entry = GetFile(filename);
            TableFile file = new TableFile(entry.file_content);
            return new RomFile<TableFile>(entry, file);
        }

        private RomFile<MapDataFile> LoadMapDataFile(string filename)
        {
            FileEntry entry = GetFile(filename);
            MapDataFile file = new MapDataFile(entry.file_content);
            return new RomFile<MapDataFile>(entry, file);
        }

        private FileEntry GetFile(string filename)
        {
            return rom.files[filename];
        }

        public MapDataFile GetFloorFile(int floorNumber)
        {
            return map_data_files[floorNumber - 1].base_file;
        }

        public TableFile EncounterData => encount_data.base_file;
        public TableFile EnemyData => enemy_data.base_file;
        public TableFile EnemyName => enemy_name.base_file;
        public TableFile GovernmentMissionData => government_mission_data.base_file;
        public TableFile GovernmentMissionName => government_mission_name.base_file;
        public TableFile Item => item.base_file;
        public TableFile ItemInfo => item_info.base_file;
        public TableFile ItemName => item_name.base_file;
        public TableFile ItemIllInfo => item_ill_info.base_file;
        public TableFile FacilityText => facility_text.base_file;

        public TableFile PlayerSkill => player_skill.base_file;
        public TableFile PlayerSkillName => player_skill_name.base_file;
        public TableFile CampSkillInfo => camp_skill_info.base_file;
        public TableFile CampSkillExeInfo => camp_skill_exe_info.base_file;
        public TableFile CampSkillCustomNextMes => camp_skill_custom_next_mes.base_file;

        public TableFile Skill2Effect => skill_2_effect.base_file;
        public TableFile BattleSkillInfo => battle_skill_info.base_file;

        public TableFile Class2Skill => class_2_skill.base_file;

    }
}

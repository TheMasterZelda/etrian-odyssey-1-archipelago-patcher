namespace etrian_odyssey_ap_patcher
{
    public class SeedPatchSkillRequirement
    {
        public int skill_id { get; set; }
        public int required_skill_1_id { get; set; }
        public int required_skill_1_level { get; set; }
        public int required_skill_2_id { get; set; }
        public int required_skill_2_level { get; set; }
    }

    public class SeedPatchRandomizedGameData
    {
        public List<SeedPatchSkillRequirement> skill_requirements { get; set; }
    }
}

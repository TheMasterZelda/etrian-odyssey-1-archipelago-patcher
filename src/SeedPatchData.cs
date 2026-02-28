namespace etrian_odyssey_ap_patcher
{
    /// <summary>
    /// Contains all the seed specific data to patch.
    /// </summary>
    public class SeedPatchData
    {
        public string Version { get; set; }
        public string Seed { get; set; }
        public int Slot { get; set; }
        public string Name { get; set; }
        public SeedPatchInitialValues InitialValues { get; set; }
        public List<SeedPatchTreasureData> TreasureBoxes { get; set; }
    }
}

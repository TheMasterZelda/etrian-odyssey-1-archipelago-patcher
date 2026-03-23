using BsDiff;
using etrian_odyssey_ap_patcher;
using etrian_odyssey_ap_patcher.DataCompression;
using etrian_odyssey_ap_patcher.EtrianOdyssey.Files;
using etrian_odyssey_ap_patcher.NitroRom;

internal class Program
{
    private static void CreatePatch(string oldFile, string newFile, string outputFile)
    {
        var oldFileBytes = File.ReadAllBytes(oldFile);
        var newFileBytes = File.ReadAllBytes(newFile);
        using var outputStream = File.Create(outputFile);
        BinaryPatch.Create(oldFileBytes, newFileBytes, outputStream);
    }

    private static void ApplyPatch()
    {
        using var oldFile = File.OpenRead("oldFile");
        using var newFile = File.Create("newFile");
        BinaryPatch.Apply(oldFile, () => File.OpenRead("patchFile"), newFile);
    }

    private static void DebugPatch()
    {
        EtrianOdysseyPatcher patcher = new EtrianOdysseyPatcher(@"D:\Projects\EtrianOdyssey\Ygg_Unpack\Etrian Odyssey (USA).nds");

        patcher.ReplaceArm9(File.ReadAllBytes(@"D:\Projects\EtrianOdyssey\Git\etrian-odyssey-1-archipelago-patch\arm9.bin"));
        patcher.ApplyShopTextPatch();
        patcher.ApplyTreasureBoxTextPatch();
        // @"D:\Projects\EtrianOdyssey\Git\APWorld\Archipelago\output\AP_14360063531218312718\AP_14360063531218312718_P1_TMZ.apeo1"
        //patcher.ApplyAPPatch(new FileStream(@"D:\Projects\EtrianOdyssey\Git\APWorld\Archipelago\output\AP_22080746147398243852\AP_22080746147398243852_P1_TMZ.apeo1", FileMode.Open));
        patcher.ApplyAPGameTitle();
        patcher.ApplyRestCostReductionPatch();

        // "D:\\Code\\TestRepack.nds"
        File.WriteAllBytes("D:\\Code\\TestRepack.nds", patcher.SavePatchedRom());
    }

    private static void ExecuteExtractor()
    {
        EtrianOdysseyDataExtractor extractor = new EtrianOdysseyDataExtractor(@"D:\Projects\EtrianOdyssey\Ygg_Unpack\Etrian Odyssey (USA).nds", "D:\\Projects\\EtrianOdyssey\\DataDump\\");
        extractor.EnemyData();
        extractor.CodexData();
        extractor.ChestData();
        extractor.GovernmentMissions();
        extractor.EquipmentData();
        extractor.ItemData();
        extractor.MaterialData();
        extractor.GatheringSpot();
        extractor.EncounterData();
        extractor.EncounterGroups();
        extractor.PlayerSkillData();
        extractor.Class2Skill();
        extractor.ItemCompound();
        extractor.ClassLevelStats();

        extractor.ParseEvents();
    }

    private static void ExecutePatcher()
    {
        Console.WriteLine("Please select the vanilla Etrian Odyssey (USA) rom...");

        OpenFileDialog openSourceRom = new OpenFileDialog();
        openSourceRom.Title = "Source ROM Etrian Odyssey (USA)";

        if (openSourceRom.ShowDialog() != DialogResult.OK)
        {
            Console.WriteLine("Patching cancelled.");
            return;
        }

        if (!File.Exists(openSourceRom.FileName))
        {
            Console.WriteLine("Cannot find source rom file.");
            return;
        }

        Console.WriteLine("Please select the Archipelago Patch file...");

        OpenFileDialog openAPPatch = new OpenFileDialog();
        openAPPatch.Title = "Archipelago Patch file";

        if (openAPPatch.ShowDialog() != DialogResult.OK)
        {
            Console.WriteLine("Patching cancelled.");
            return;
        }

        if (!File.Exists(openAPPatch.FileName))
        {
            Console.WriteLine("Cannot find the patch file.");
            return;
        }

        Console.WriteLine("Please select the output file...");

        SaveFileDialog saveOutputRom = new SaveFileDialog();
        saveOutputRom.Title = "Output file";

        if (saveOutputRom.ShowDialog() != DialogResult.OK)
        {
            Console.WriteLine("Patching cancelled.");
            return;
        }

        try
        {
            // @"D:\Projects\EtrianOdyssey\Ygg_Unpack\Etrian Odyssey (USA).nds")
            EtrianOdysseyPatcher patcher = new EtrianOdysseyPatcher(openSourceRom.FileName);

            patcher.ApplyCodePatch();
            patcher.ApplyShopTextPatch();
            patcher.ApplyTreasureBoxTextPatch();
            patcher.ApplyRestCostReductionPatch();
            // @"D:\Projects\EtrianOdyssey\Git\APWorld\Archipelago\output\AP_14360063531218312718\AP_14360063531218312718_P1_TMZ.apeo1"
            patcher.ApplyAPPatch(new FileStream(openAPPatch.FileName, FileMode.Open));
            patcher.ApplyAPGameTitle();

            // "D:\\Code\\TestRepack.nds"
            File.WriteAllBytes(saveOutputRom.FileName, patcher.SavePatchedRom());

            Console.WriteLine("Done patching. Have fun!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An exception occured:");
            Console.WriteLine(ex);
            Console.ReadKey();
            throw;
        }
    }

    [STAThread]
    private static void Main(string[] args)
    {
        //DebugPatch();

        //ExecuteExtractor();

        //CreatePatch("D:\\Code\\original_arm9.bin", "D:\\Code\\patched_arm9.bin", "D:\\Code\\patch.bsdiff");

        ExecutePatcher();

        Console.Read();
    }
}
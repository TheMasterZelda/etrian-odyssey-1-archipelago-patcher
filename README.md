# Etrian Odyssey 1 Archipelago Patcher

This is the project to apply archipelago patch for Etrian Odyssey 1 (DS). The runtime target `windows` specifically only to use the file dialog, nothing else should depend on it.

Additionaly, this project offer the toolset to generate the `.bsdiff` file for the base patch along a data extraction used to generate most of the data used to make the apworld.

## How to Use (Patcher)

Run `etrian_odyssey_ap_patcher.exe`. It will prompt you to select in order:

- The source Etrian Odyssey (DS) rom file.
- The Archipelago patch file (`.apeo1`)
- Output file (recommended to make a new file for each seed.) (Recommended extension: `.nds`)

## How to Use (Extractor)

In `Program.cs`.`Main()`, uncomment the `ExecuteExtractor();` line and comment the `ExecutePatcher();` line. Make sure to set the output directory to the desired location.
Note that this feature was made by me for me, and so has a few things hardcoded - This is planned to be improved over time.

## How to Use (Create Patch)

In `Program.cs`.`Main()`, uncomment the `CreatePatch();` line and comment the `ExecutePatcher();` line.
Set the parameter to the desired values. Once done, run the program.

## How to build

Publish project, zip result.

## Note

To replace the `patch.bsdiff` file, you need to manually delete the `bin` and `obj` folders for some reasons, otherwise the compiler sometimes uses the previous version
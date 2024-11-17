using System;

class Program
{
    #region Get Minecraft Commands
    public static void GetFillCommand(int x1, int z1, int y1, int x2, int z2, int y2, string materialName, List<string> createCommands, List<string> clearCommands, bool isHollow = false)
    {
        if (isHollow)
        {
            createCommands.Add($"/fill {x1} {z1} {y1} {x2} {z2} {y2} {materialName} hollow");
        }
        else
        {
            createCommands.Add($"/fill {x1} {z1} {y1} {x2} {z2} {y2} {materialName}");
        }
        clearCommands.Add($"/fill {x1} {z1} {y1} {x2} {z2} {y2} air");
    }

    public static void GetSetBlockCommand(int x1, int z1, int y1, string materialName, List<string> createCommands, List<string> clearCommands)
    {
        createCommands.Add($"/setblock {x1} {z1} {y1} {materialName}");
        clearCommands.Add($"/fill {x1} {z1} {y1} {x1} {z1} {y1} air");
    }

    public static void GetReplaceItemCommand(int x1, int z1, int y1, string materialName, List<string> createCommands)
    {
        createCommands.Add($"/replaceitem block {x1} {z1} {y1} container.0 {materialName}");
    }

    public static void GetTorchMatrix(int x1, int z1, int y1, List<string> createCommands, List<string> clearCommands, int bigRadius, int spawnSpacing)
    {
        for (int i = 0; i < bigRadius; i++)
        {
            for (int j = 0; j < bigRadius; j++)
            {
                if (i % spawnSpacing == 0 && j % spawnSpacing == 0)
                {
                    GetSetBlockCommand(x1 + i, z1, y1 + j, "minecraft:torch", createCommands, clearCommands);
                    GetSetBlockCommand(x1 + i, z1, y1 - j, "minecraft:torch", createCommands, clearCommands);
                    GetSetBlockCommand(x1 - i, z1, y1 + j, "minecraft:torch", createCommands, clearCommands);
                    GetSetBlockCommand(x1 - i, z1, y1 - j, "minecraft:torch", createCommands, clearCommands);
                }
            }
        }
    }

    public static void GetFillEdgesCommand(int x1, int z1, int y1, string materialName, List<string> createCommands, int radius)
    {
        for (int i = radius; i >= 0; i--)
        {
            if (i == 0)
            {
                createCommands.Add($"/fill {x1 + i} {z1} {y1 + radius} {x1 + i} {z1} {y1 + radius} minecraft:water");
                createCommands.Add($"/fill {x1 + i} {z1} {y1 - radius} {x1 + i} {z1} {y1 - radius} minecraft:water");
                createCommands.Add($"/fill {x1 + radius} {z1} {y1 + i} {x1 + radius} {z1} {y1 + i} minecraft:water");
                createCommands.Add($"/fill {x1 - radius} {z1} {y1 + i} {x1 - radius} {z1} {y1 + i} minecraft:water");
            }
            else
            {
                createCommands.Add($"/fill {x1 + i} {z1} {y1 + radius} {x1 + i} {z1} {y1 + radius} minecraft:water");
                createCommands.Add($"/fill {x1 - i} {z1} {y1 + radius} {x1 - i} {z1} {y1 + radius} minecraft:water");
                createCommands.Add($"/fill {x1 + i} {z1} {y1 - radius} {x1 + i} {z1} {y1 - radius} minecraft:water");
                createCommands.Add($"/fill {x1 - i} {z1} {y1 - radius} {x1 - i} {z1} {y1 - radius} minecraft:water");
                createCommands.Add($"/fill {x1 + radius} {z1} {y1 + i} {x1 + radius} {z1} {y1 + i} minecraft:water");
                createCommands.Add($"/fill {x1 + radius} {z1} {y1 - i} {x1 + radius} {z1} {y1 - i} minecraft:water");
                createCommands.Add($"/fill {x1 - radius} {z1} {y1 + i} {x1 - radius} {z1} {y1 + i} minecraft:water");
                createCommands.Add($"/fill {x1 - radius} {z1} {y1 - i} {x1 - radius} {z1} {y1 - i} minecraft:water");
            }
        }
    }

    public static void GetSidePlatformFills(int x1, int z1, int y1, int buildX, int buildY, bool buildBarsX, string materialName, List<string> createCommands, List<string> createLastCommands, List<string> clearCommands, int platformSize = 6, string centerBlock = "")
    {
        if (buildBarsX)
        {
            for (int i = platformSize; i >= 0; i--)
            {
                int buildYi = (platformSize - i) * buildY;
                GetFillCommand(x1 - i, z1, y1 + buildYi, x1 + i, z1, y1 + buildYi, materialName, createCommands, clearCommands);
            }
        }
        else
        {
            for (int i = platformSize; i >= 0; i--)
            {
                int buildXi = (platformSize - i) * buildX;
                GetFillCommand(x1 + buildXi, z1, y1 + i, x1 + buildXi, z1, y1 - i, materialName, createCommands, clearCommands);
            }
        }
        if (centerBlock != "")
        {
            GetSetBlockCommand(x1, z1, y1, centerBlock, createCommands, clearCommands);
            if (centerBlock == "minecraft:dispenser[facing=up]")
            {
                GetReplaceItemCommand(x1, z1, y1, "minecraft:water_bucket", createLastCommands);
            }
        }
    }

    public static void GetCenterPlatformFills(int x1, int z1, int y1, string materialName, List<string> createCommands, List<string> createLastCommands, List<string> clearCommands, int platformSize = 7, string centerBlock = "", string underCenterBlock = "")
    {
        for (int i = 1; i < 11; i++)
        {
            GetSidePlatformFills(x1, z1 + i * 3, y1, 1, 0, false, materialName, createCommands, createLastCommands, clearCommands, 7);
            GetSidePlatformFills(x1 - 1, z1 + i * 3, y1, -1, 0, false, materialName, createCommands, createLastCommands, clearCommands, platformSize - 1);
            if (centerBlock != "")
            {
                GetSetBlockCommand(x1, z1 + i * 3, y1, centerBlock, createCommands, clearCommands);
                if (centerBlock == "minecraft:dispenser[facing=up]")
                {
                    GetReplaceItemCommand(x1, z1 + i * 3, y1, "minecraft:water_bucket", createLastCommands);
                }
            }
            if (underCenterBlock != "")
            {
                GetSetBlockCommand(x1, (z1 + i * 3) - 1, y1, underCenterBlock, createCommands, clearCommands);
            }
        }
    }

    public static void GetRedStoneNetwork(int x1, int z1, int y1, List<string> createCommands, int radius, string lanternBlock, int lanternSpacing)
    {
        int lanternRows = radius / lanternSpacing;

        for (int i = 0; i <= radius; i++)
        {
            if (i == 0)
            {
                createCommands.Add($"/setblock {x1} {z1} {y1} minecraft:repeater[delay=4,facing=west]");
                createCommands.Add($"/setblock {x1} {z1} {y1 - 1} minecraft:repeater[delay=4,facing=east]");
            }
            else if (i == radius)
            {
                createCommands.Add($"/setblock {x1 - i} {z1} {y1} minecraft:redstone_wire[north=none]");
                createCommands.Add($"/setblock {x1 - i} {z1} {y1 - 1} minecraft:redstone_wire[north=none]");

                createCommands.Add($"/setblock {x1 + i} {z1} {y1 + 1} minecraft:lever[face=floor,facing=north,powered=true]");
                createCommands.Add($"/setblock {x1 + i} {z1} {y1} minecraft:comparator[mode=subtract,facing=south]");
                createCommands.Add($"/setblock {x1 + i} {z1} {y1 - 1} minecraft:redstone_wire[north=none]");
            }
            else
            {
                createCommands.Add($"/setblock {x1 + i} {z1} {y1} minecraft:repeater[delay=4,facing=west]");
                createCommands.Add($"/setblock {x1 + i} {z1} {y1 - 1} minecraft:repeater[delay=4,facing=east]");
                createCommands.Add($"/setblock {x1 - i} {z1} {y1} minecraft:repeater[delay=4,facing=west]");
                createCommands.Add($"/setblock {x1 - i} {z1} {y1 - 1} minecraft:repeater[delay=4,facing=east]");
            }

            if (i % lanternSpacing == 0)
            {
                for (int j = 0; j < lanternRows; j++)
                {
                    createCommands.Add($"/setblock {x1 + i} {z1} {y1 + ((j + 1) * lanternSpacing)} {lanternBlock}");
                    createCommands.Add($"/setblock {x1 + i} {z1} {y1 - ((j + 1) * lanternSpacing)} {lanternBlock}");
                    createCommands.Add($"/setblock {x1 - i} {z1} {y1 - ((j + 1) * lanternSpacing)} {lanternBlock}");
                    createCommands.Add($"/setblock {x1 - i} {z1} {y1 + ((j + 1) * lanternSpacing)} {lanternBlock}");
                }
            }
        }
    }

    public static void GetKillChamberCommands(int x1, int z1, int y1, int killChamberDepth, int radius, int bigRadius, string materialName, List<string> createCommands, List<string> clearCommands)
    {
        int z = z1 - killChamberDepth;

        int x1c = x1 - radius;
        int y1c = y1 - radius;

        int x2c = x1 + radius;
        int y2c = y1 + radius;

        GetFillCommand(x1 + bigRadius, z - 1, y1 + bigRadius, x1 - bigRadius, z + 1, y1 - bigRadius, "air", createCommands, clearCommands);
        GetFillCommand(x1 + bigRadius, z - 1, y1 + bigRadius, x1 - bigRadius, z - 1, y1 - bigRadius, materialName, createCommands, clearCommands);
        GetFillCommand(x1 + bigRadius, z + 1, y1 + bigRadius, x1 - bigRadius, z + 1, y1 + bigRadius, "toms_storage:ts.inventory_cable", createCommands, clearCommands);
        GetFillCommand(x1 + bigRadius, z + 1, y1 + bigRadius, x1 + bigRadius, z + 1, y1 - bigRadius, "toms_storage:ts.inventory_cable", createCommands, clearCommands);
        GetFillCommand(x1 - bigRadius, z + 1, y1 + bigRadius, x1 - bigRadius, z + 1, y1 - bigRadius, "toms_storage:ts.inventory_cable", createCommands, clearCommands);
        GetFillCommand(x1 + bigRadius, z + 1, y1 - bigRadius, x1 - bigRadius, z + 1, y1 - bigRadius, "toms_storage:ts.inventory_cable", createCommands, clearCommands);

        GetSetBlockCommand(x1 + bigRadius, z, y1, $"minecraft:torch", createCommands, clearCommands);
        GetSetBlockCommand(x1 - bigRadius, z, y1, $"minecraft:torch", createCommands, clearCommands);
        GetSetBlockCommand(x1, z, y1 + bigRadius, $"minecraft:torch", createCommands, clearCommands);
        GetSetBlockCommand(x1, z, y1 - bigRadius, $"minecraft:torch", createCommands, clearCommands);

        GetSetBlockCommand(x1 + bigRadius, z, y1 + bigRadius, $"minecraft:torch", createCommands, clearCommands);
        GetSetBlockCommand(x1 - bigRadius, z, y1 + bigRadius, $"minecraft:torch", createCommands, clearCommands);
        GetSetBlockCommand(x1 + bigRadius, z, y1 - bigRadius, $"minecraft:torch", createCommands, clearCommands);
        GetSetBlockCommand(x1 - bigRadius, z, y1 - bigRadius, $"minecraft:torch", createCommands, clearCommands);

        GetSetBlockCommand(x1 + bigRadius / 2, z, y1 + bigRadius / 2, $"minecraft:torch", createCommands, clearCommands);
        GetSetBlockCommand(x1 - bigRadius / 2, z, y1 + bigRadius / 2, $"minecraft:torch", createCommands, clearCommands);
        GetSetBlockCommand(x1 + bigRadius / 2, z, y1 - bigRadius / 2, $"minecraft:torch", createCommands, clearCommands);
        GetSetBlockCommand(x1 - bigRadius / 2, z, y1 - bigRadius / 2, $"minecraft:torch", createCommands, clearCommands);

        GetFillCommand(x1c, z1, y1c, x2c, z1 - killChamberDepth, y2c, materialName, createCommands, clearCommands, true);

        x1c++;
        y1c++;
        x2c--;
        y2c--;

        // Clear top drop in:
        GetFillCommand(x1c, z1, y1c, x2c, z1, y2c, "air", createCommands, clearCommands);

        string[] directions = new string[] { "west", "east", "north", "south" };

        // Chest layer:
        z++;
        GetFillCommand(x1c, z, y1c, x2c, z, y2c, "chest[facing=north]", createCommands, clearCommands);

        GetSetBlockCommand(x1 + radius, z, y1, $"toms_storage:ts.inventory_connector", createCommands, clearCommands);

        GetSetBlockCommand(x1 + (radius + 1), z, y1, $"toms_storage:ts.inventory_cable_connector[facing={directions[0]}]", createCommands, clearCommands);

        GetSetBlockCommand(x1 + (radius + 1), z - 1, y1, $"minecraft:torch", createCommands, clearCommands);
        GetSetBlockCommand(x1 - (radius + 1), z - 1, y1, $"minecraft:torch", createCommands, clearCommands);
        GetSetBlockCommand(x1, z - 1, y1 + (radius + 1), $"minecraft:torch", createCommands, clearCommands);
        GetSetBlockCommand(x1, z - 1, y1 - (radius + 1), $"minecraft:torch", createCommands, clearCommands);

        GetFillCommand(x1 + (radius + 2), z, y1, x1 + bigRadius, z, y1, "toms_storage:ts.inventory_cable", createCommands, clearCommands);

        // Hopper layer:
        z++;
        GetFillCommand(x1c, z, y1c, x2c, z, y2c, "minecraft:hopper[facing=down,enabled=true]", createCommands, clearCommands);

        // Soul campfire layer:
        z++;
        GetFillCommand(x1c, z, y1c, x2c, z, y2c, "minecraft:soul_campfire[lit=true]", createCommands, clearCommands);
    }
    #endregion
    
    #region Generate Mob Farm
    /// <summary>
    /// This command is based on a mob farm of the following style:
    /// https://www.youtube.com/watch?v=AkqP71XH-FU
    /// 
    /// Be careful this command will carve out a massive area destroying everything in the way starting at the point of origin as the center of the top of the mob farm.
    /// 
    /// These cannot be built below z = 50.
    /// 
    /// </summary>
    public static CommandGenerationResults GenerateMobFarm(int x, int z, int y, string interiorMaterial, string exteriorMaterial, string lanternBlock)
    {
        List<string> createCommands = new List<string>();
        List<string> createLastCommands = new List<string>();
        List<string> clearCommands = new List<string>();

        // We first find the point of the base then begin constructing relative to the center of the bottom water floor for the spawner:
        z = z - 40;

        Console.WriteLine("");

        // Determines how far under the MFG we build the kill drop.
        int killChamberDepth = 10;

        // Four corners of the base:
        int xc1 = x - 11;
        int zc1 = z;
        int yc1 = y - 11;

        int xc2 = x - 11;
        int zc2 = z;
        int yc2 = y + 11;

        int xc3 = x + 11;
        int zc3 = z;
        int yc3 = y - 11;

        int xc4 = x + 11;
        int zc4 = z;
        int yc4 = y + 11;

        // Four corners of the wrap base:
        int xc1w = x - 12;
        int zc1w = z;
        int yc1w = y - 12;

        int xc2w = x - 12;
        int zc2w = z;
        int yc2w = y + 12;

        int xc3w = x + 12;
        int zc3w = z;
        int yc3w = y - 12;

        int xc4w = x + 12;
        int zc4w = z;
        int yc4w = y + 12;

        // First clear everything in the way:
        GetFillCommand(xc1 - 2, zc1 - (killChamberDepth + 2), yc1 - 2, xc4 + 2, zc4 + 41, yc4 + 2, "air", createCommands, clearCommands);

        // Generate the grass top layer:
        GetFillCommand(xc1 - 2, z + 38, yc1 - 2, xc4 + 2, zc4 + 40, yc4 + 2, "minecraft:grass_block", createCommands, clearCommands);
        GetTorchMatrix(x, z + 41, y, createCommands, clearCommands, 13, 6);

        // Generate the base floor:
        GetFillCommand(xc1, zc1, yc1, xc4, zc4, yc4, interiorMaterial, createCommands, clearCommands);

        // Generate the walls:
        GetFillCommand(xc1, zc1, yc1, xc2, zc2 + 36, yc2, interiorMaterial, createCommands, clearCommands);
        GetFillCommand(xc1, zc1, yc1, xc3, zc3 + 36, yc3, interiorMaterial, createCommands, clearCommands);
        GetFillCommand(xc4, zc4, yc4, xc2, zc2 + 36, yc2, interiorMaterial, createCommands, clearCommands);
        GetFillCommand(xc4, zc4, yc4, xc3, zc3 + 36, yc3, interiorMaterial, createCommands, clearCommands);

        // Generate the roof floor:
        GetFillCommand(xc1, zc1 + 36, yc1, xc4, zc4 + 36, yc4, interiorMaterial, createCommands, clearCommands);

        // Wrap it:

        // Generate the base floor:
        GetFillCommand(xc1w, zc1w - 1, yc1w, xc4w, zc4w - 1, yc4w, exteriorMaterial, createCommands, clearCommands);

        // Generate the walls:
        GetFillCommand(xc1w, zc1w - 1, yc1w, xc2w, zc2w + 37, yc2w, exteriorMaterial, createCommands, clearCommands);
        GetFillCommand(xc1w, zc1w - 1, yc1w, xc3w, zc3w + 37, yc3w, exteriorMaterial, createCommands, clearCommands);
        GetFillCommand(xc4w, zc4w - 1, yc4w, xc2w, zc2w + 37, yc2w, exteriorMaterial, createCommands, clearCommands);
        GetFillCommand(xc4w, zc4w - 1, yc4w, xc3w, zc3w + 37, yc3w, exteriorMaterial, createCommands, clearCommands);

        // Next build the inner platforms:

        // Four wall midpoints:
        int xc1m = x - 10;
        int yc1m = y;

        int xc2m = x;
        int yc2m = y + 10;

        int xc3m = x + 10;
        int yc3m = y;

        int xc4m = x;
        int yc4m = y - 10;

        GetCenterPlatformFills(x, z + 3, y, interiorMaterial, createCommands, createLastCommands, clearCommands, 7, "minecraft:dispenser[facing=up]", "minecraft:observer[facing=up]");

        // Center Hole:
        GetFillCommand(x - 1, z, y - 1, x + 1, z - 1, y + 1, "air", createCommands, clearCommands);

        // Bottom floor water:
        GetFillEdgesCommand(x, z + 2, y, "water", createLastCommands, 10);

        // Roof top observer:
        GetSetBlockCommand(x, z + 36, y, "minecraft:dispenser[facing=up]", createCommands, clearCommands);
        GetSetBlockCommand(x, z + 35, y, "minecraft:observer[facing=up]", createCommands, clearCommands);
        GetRedStoneNetwork(x, z + 37, y, createCommands, 9, lanternBlock, 4);

        // Build the kill chamber room:
        GetKillChamberCommands(x, z - 1, y, killChamberDepth, 2, 13, exteriorMaterial, createCommands, clearCommands);

        return new CommandGenerationResults { createCommands = createCommands, createLastCommands = createLastCommands, clearCommands = clearCommands };
    }
    #endregion

    #region Generate Mob Farm Grid
    public static List<CommandGenerationResults> GenerateMobFarmGrid(int x, int z, int y, string interiorMaterial, string exteriorMaterial, string lanternBlock, int gridRadius, bool wrapEntireGrid = true)
    {
        int mobFarmSeparationBlocks = 26;
        List<CommandGenerationResults> results = new List<CommandGenerationResults>();
        for (int i = 0; i <= gridRadius; i++)
        {
            for (int j = 0; j <= gridRadius; j++)
            {
                if (i == 0 && j == 0)
                {
                    results.Add(GenerateMobFarm(x, z, y, interiorMaterial, exteriorMaterial, lanternBlock));
                }
                else if (i == 0)
                {
                    results.Add(GenerateMobFarm(x + (mobFarmSeparationBlocks * i), z, y + (mobFarmSeparationBlocks * j), interiorMaterial, exteriorMaterial, lanternBlock));
                    results.Add(GenerateMobFarm(x + (mobFarmSeparationBlocks * i), z, y - (mobFarmSeparationBlocks * j), interiorMaterial, exteriorMaterial, lanternBlock));
                }
                else if (j == 0)
                {
                    results.Add(GenerateMobFarm(x + (mobFarmSeparationBlocks * i), z, y + (mobFarmSeparationBlocks * j), interiorMaterial, exteriorMaterial, lanternBlock));
                    results.Add(GenerateMobFarm(x - (mobFarmSeparationBlocks * i), z, y + (mobFarmSeparationBlocks * j), interiorMaterial, exteriorMaterial, lanternBlock));
                }
                else
                {
                    results.Add(GenerateMobFarm(x + (mobFarmSeparationBlocks * i), z, y + (mobFarmSeparationBlocks * j), interiorMaterial, exteriorMaterial, lanternBlock));
                    results.Add(GenerateMobFarm(x - (mobFarmSeparationBlocks * i), z, y + (mobFarmSeparationBlocks * j), interiorMaterial, exteriorMaterial, lanternBlock));
                    results.Add(GenerateMobFarm(x + (mobFarmSeparationBlocks * i), z, y - (mobFarmSeparationBlocks * j), interiorMaterial, exteriorMaterial, lanternBlock));
                    results.Add(GenerateMobFarm(x - (mobFarmSeparationBlocks * i), z, y - (mobFarmSeparationBlocks * j), interiorMaterial, exteriorMaterial, lanternBlock));
                }
            }
        }
        if (wrapEntireGrid)
        {
            CommandGenerationResults result = new CommandGenerationResults() { createCommands = new List<string>(), createLastCommands = new List<string>(), clearCommands = new List<string>()};
            int gridFullRadius = (mobFarmSeparationBlocks * gridRadius) + (mobFarmSeparationBlocks / 2);
            GetFillCommand(x + gridFullRadius, z + 1, y + gridFullRadius, x + gridFullRadius, z - 52, y - gridFullRadius, exteriorMaterial, result.createCommands, result.clearCommands);
            GetFillCommand(x + gridFullRadius, z + 1, y + gridFullRadius, x - gridFullRadius, z - 52, y + gridFullRadius, exteriorMaterial, result.createCommands, result.clearCommands);
            GetFillCommand(x - gridFullRadius, z + 1, y + gridFullRadius, x - gridFullRadius, z - 52, y - gridFullRadius, exteriorMaterial, result.createCommands, result.clearCommands);
            GetFillCommand(x + gridFullRadius, z + 1, y - gridFullRadius, x - gridFullRadius, z - 52, y - gridFullRadius, exteriorMaterial, result.createCommands, result.clearCommands);
            results.Add(result);
        }
        return results;
    }
    #endregion

    #region Print / Helpers
    public class CommandGenerationResults
    {
        public List<string> createCommands { get; set; }
        public List<string> createLastCommands { get; set; }
        public List<string> clearCommands { get; set; }
    }

    public static void PrintCommands(CommandGenerationResults results, bool printDeletes = false)
    {
        foreach (string command in results.createCommands)
        {
            Console.WriteLine(command);
        }
        Console.WriteLine("");
        foreach (string command in results.createLastCommands)
        {
            Console.WriteLine(command);
        }
        Console.WriteLine("");
        if (printDeletes)
        {
            foreach (string command in results.clearCommands)
            {
                Console.WriteLine(command);
            }
            Console.WriteLine("");
        }
    }

    public static void PrintCommands(List<CommandGenerationResults> results, bool printDeletes = false)
    {
        foreach (var r in results)
        {
            foreach (string command in r.createCommands)
            {
                Console.WriteLine(command);
            }
        }
        Console.WriteLine("");
        foreach (var r in results)
        {
            foreach (string command in r.createLastCommands)
            {
                Console.WriteLine(command);
            }
        }
        Console.WriteLine("");
        if (printDeletes)
        {
            foreach (var r in results)
            {
                foreach (string command in r.clearCommands)
                {
                    Console.WriteLine(command);
                }
            }
            Console.WriteLine("");
        }
    }

    public static void ShowHelp()
    {
        Console.WriteLine("Available Commands:");
        Console.WriteLine("  mfg x z y a b c -    Generates a mob farm.");
        Console.WriteLine("                       x, z, y: Starting point (integers)");
        Console.WriteLine("                       a: Interior material (string, e.g., 'minecraft:black_concrete')");
        Console.WriteLine("                       b: Exterior material (string, e.g., 'minecraft:cut_sandstone')");
        Console.WriteLine("                       c: Lantern block (string, e.g., 'minecraft:sea_lantern')");
        Console.WriteLine("  mfgx x z y a b c r - Generates an network of mob farms wirth radius r.");
        Console.WriteLine("                       x, z, y: Starting point (integers)");
        Console.WriteLine("                       a: Interior material (string, e.g., 'minecraft:black_concrete')");
        Console.WriteLine("                       b: Exterior material (string, e.g., 'minecraft:cut_sandstone')");
        Console.WriteLine("                       c: Lantern block (string, e.g., 'minecraft:sea_lantern')");
        Console.WriteLine("                       r: Radius of the mob farm network, 1 would yield a 3x3 network");
        Console.WriteLine("  help               - Displays this help menu.");
    }
    #endregion

    #region Main
    static void Main(string[] args)
    {
        Console.WriteLine("Minecraft Command Generator");
        Console.WriteLine("Type 'help' to see available commands.");

        bool isDebug = true;
        int commandCount = 0;

        while (true)
        {
            string input = "";

            if (isDebug && commandCount == 0)
            {
                input = "mfgx 269 129 804 minecraft:black_concrete minecraft:cut_sandstone supplementaries:end_stone_lamp 2";
                //input = "mfg 269 129 804 minecraft:black_concrete minecraft:cut_sandstone supplementaries:end_stone_lamp";
            }
            else
            {
                Console.Write("\nEnter your command: ");
                input = Console.ReadLine()?.Trim();
            }

            if (string.IsNullOrEmpty(input)) continue;

            string[] parts = input.Split(' ');

            switch (parts[0].ToLower())
            {
                case "mfg":
                    {
                        if (parts.Length == 7 &&
                            int.TryParse(parts[1], out int x) &&
                            int.TryParse(parts[2], out int z) &&
                            int.TryParse(parts[3], out int y))
                        {
                            string interiorMaterial = parts[4];
                            string exteriorMaterial = parts[5];
                            string lanternBlock = parts[6];
                            PrintCommands(GenerateMobFarm(x, z, y, interiorMaterial, exteriorMaterial, lanternBlock));
                        }
                        else
                        {
                            Console.WriteLine("Invalid syntax for 'mfg'. Use: mfg x z y a b");
                        }
                    }
                    break;
                case "mfgx":
                    {
                        if (parts.Length == 8 &&
                            int.TryParse(parts[1], out int x) &&
                            int.TryParse(parts[2], out int z) &&
                            int.TryParse(parts[3], out int y) &&
                            int.TryParse(parts[7], out int r))
                        {
                            string interiorMaterial = parts[4];
                            string exteriorMaterial = parts[5];
                            string lanternBlock = parts[6];
                            PrintCommands(GenerateMobFarmGrid(x, z, y, interiorMaterial, exteriorMaterial, lanternBlock, r));
                        }
                        else
                        {
                            Console.WriteLine("Invalid syntax for 'mfg'. Use: mfg x z y a b");
                        }
                    }
                    break;
                case "help":
                    ShowHelp();
                    break;

                default:
                    Console.WriteLine("Unknown command. Type 'help' to see available commands.");
                    break;
            }
            commandCount++;
        }
    }
    #endregion
}


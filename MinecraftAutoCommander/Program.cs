using System;
using System.Text;
using static Program;
using TextCopy; // Add TextCopy for clipboard functionality

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
            GetFillCommand(x + gridFullRadius, z, y + gridFullRadius, x + gridFullRadius, z - 40, y - gridFullRadius, exteriorMaterial, result.createCommands, result.clearCommands);
            GetFillCommand(x + gridFullRadius, z, y + gridFullRadius, x - gridFullRadius, z - 40, y + gridFullRadius, exteriorMaterial, result.createCommands, result.clearCommands);
            GetFillCommand(x - gridFullRadius, z, y + gridFullRadius, x - gridFullRadius, z - 40, y - gridFullRadius, exteriorMaterial, result.createCommands, result.clearCommands);
            GetFillCommand(x + gridFullRadius, z, y - gridFullRadius, x - gridFullRadius, z - 40, y - gridFullRadius, exteriorMaterial, result.createCommands, result.clearCommands);

            gridFullRadius++;
            GetFillCommand(x + gridFullRadius, z + 1, y + gridFullRadius, x + gridFullRadius, z - 52, y - gridFullRadius, exteriorMaterial, result.createCommands, result.clearCommands);
            GetFillCommand(x + gridFullRadius, z + 1, y + gridFullRadius, x - gridFullRadius, z - 52, y + gridFullRadius, exteriorMaterial, result.createCommands, result.clearCommands);
            GetFillCommand(x - gridFullRadius, z + 1, y + gridFullRadius, x - gridFullRadius, z - 52, y - gridFullRadius, exteriorMaterial, result.createCommands, result.clearCommands);
            GetFillCommand(x + gridFullRadius, z + 1, y - gridFullRadius, x - gridFullRadius, z - 52, y - gridFullRadius, exteriorMaterial, result.createCommands, result.clearCommands);
            results.Add(result);
        }
        return results;
    }
    #endregion

    #region Build Castle Components
    public class MaterialSet
    {
        public string StoneBricks { get; set; }
        public string StoneSlabs { get; set; }
        public string StoneBrickSlabs { get; set; }
        public string StoneStairs { get; set; }
        public string Glass { get; set; }
        public string ChiseledBricks { get; set; }

        public string Logs { get; set; }
        public string StrippedLogs { get; set; }
        public string Planks { get; set; }
        public string PlankStairs { get; set; }
        public string PlankTrapdoor { get; set; }
        public string PlankSlab { get; set; }
        public string PlankDoor { get; set; }

        public string RoofRock { get; set; }
        public string RoofRockPolished { get; set; }
        public string RoofRockSlab { get; set; }

        public string StoneBricks2 { get; set; }
        public string StoneSlabs2 { get; set; }
        public string StoneBrickSlabs2 { get; set; }

        public string CornerWalls { get; set; }

        public string Ladder { get; set; }

        public string Torch { get; set; }

        public string LanternBlock { get; set; }

        public MaterialSet(string[] materials)
        {
            if (materials.Length != 23)
            {
                throw new ArgumentException("Expected 23 materials in the input.");
            }

            StoneBricks = materials[0];
            StoneSlabs = materials[1];
            StoneBrickSlabs = materials[2];
            StoneStairs = materials[3];
            Glass = materials[4];
            ChiseledBricks = materials[5];

            Logs = materials[6];
            StrippedLogs = materials[7];
            Planks = materials[8];
            PlankStairs = materials[9];
            PlankTrapdoor = materials[10];
            PlankSlab = materials[11];
            PlankDoor = materials[12];

            RoofRock = materials[13];
            RoofRockPolished = materials[14];
            RoofRockSlab = materials[15];

            StoneBricks2 = materials[16];
            StoneSlabs2 = materials[17];
            StoneBrickSlabs2 = materials[18];

            CornerWalls = materials[19];

            Ladder = materials[20];

            Torch = materials[21];

            LanternBlock = materials[22];
        }

        public override string ToString()
        {
            return string.Join("\n", new[]
            {
                $"StoneBricks: {StoneBricks}",
                $"StoneSlabs: {StoneSlabs}",
                $"StoneBrickSlabs: {StoneBrickSlabs}",
                $"StoneStairs: {StoneStairs}",
                $"Glass: {Glass}",
                $"ChiseledBricks: {ChiseledBricks}",
                $"Logs: {Logs}",
                $"StrippedLogs: {StrippedLogs}",
                $"Planks: {Planks}",
                $"PlankStairs: {PlankStairs}",
                $"PlankTrapdoor: {PlankTrapdoor}",
                $"PlankSlab: {PlankSlab}",
                $"PlankDoor: {PlankDoor}",
                $"RoofRock: {RoofRock}",
                $"RoofRockPolished: {RoofRockPolished}",
                $"RoofRockSlab: {RoofRockSlab}",
                $"StoneBricks2: {StoneBricks2}",
                $"StoneSlabs2: {StoneSlabs2}",
                $"StoneBrickSlabs2: {StoneBrickSlabs2}",
                $"CornerWalls: {CornerWalls}",
                $"Ladder: {Ladder}",
                $"Torch: {Torch}",
                $"LanternBlock: {LanternBlock}"
            });
        }
    }

    public static void BuildTower1(int x, int z, int y, MaterialSet materials, List<string> createCommands, List<string> clearCommands, int bonusHeight = 0)
    {
        GetFillCommand(x - 2, z, y, x + 2, z + 7 + bonusHeight, y + 4, materials.StoneBricks, createCommands, clearCommands, true);
        GetFillCommand(x - 1, z, y + 1, x + 1, z, y + 3, materials.StoneBricks2, createCommands, clearCommands, true);

        GetFillCommand(x - 1, z + 7 + bonusHeight, y + 1, x + 1, z + 7 + bonusHeight, y + 3, "air", createCommands, clearCommands, true);
        GetFillCommand(x - 2, z + 8 + bonusHeight, y, x + 2, z + 8 + bonusHeight, y + 4, materials.ChiseledBricks, createCommands, clearCommands, true);
        GetFillCommand(x - 1, z + 8 + bonusHeight, y + 1, x + 1, z + 8 + bonusHeight, y + 3, materials.Planks, createCommands, clearCommands, true);
        GetFillCommand(x, z + 4 + bonusHeight, y, x, z + 6 + bonusHeight, y, "minecraft:iron_bars", createCommands, clearCommands, true);
        GetFillCommand(x - 1, z + 1, y + 1, x - 1, z + 8 + bonusHeight, y + 1, materials.Ladder + "[facing=south]", createCommands, clearCommands, true);

        // Log frame:
        GetFillCommand(x - 2, z + 9 + bonusHeight, y + 0, x - 2, z + 13 + bonusHeight, y + 0, materials.Logs + "[axis=y]", createCommands, clearCommands, true);
        GetFillCommand(x - 2, z + 9 + bonusHeight, y + 4, x - 2, z + 13 + bonusHeight, y + 4, materials.Logs + "[axis=y]", createCommands, clearCommands, true);
        GetFillCommand(x + 2, z + 9 + bonusHeight, y + 0, x + 2, z + 13 + bonusHeight, y + 0, materials.Logs + "[axis=y]", createCommands, clearCommands, true);
        GetFillCommand(x + 2, z + 9 + bonusHeight, y + 4, x + 2, z + 13 + bonusHeight, y + 4, materials.Logs + "[axis=y]", createCommands, clearCommands, true);
        GetFillCommand(x - 1, z + 13 + bonusHeight, y + 0, x + 1, z + 13 + bonusHeight, y + 0, materials.Logs + "[axis=x]", createCommands, clearCommands, true);
        GetFillCommand(x - 1, z + 13 + bonusHeight, y + 4, x + 1, z + 13 + bonusHeight, y + 4, materials.Logs + "[axis=x]", createCommands, clearCommands, true);
        GetFillCommand(x - 2, z + 13 + bonusHeight, y + 1, x - 2, z + 13 + bonusHeight, y + 3, materials.Logs + "[axis=z]", createCommands, clearCommands, true);
        GetFillCommand(x - 2, z + 13 + bonusHeight, y + 1, x + 2, z + 13 + bonusHeight, y + 3, materials.Logs + "[axis=z]", createCommands, clearCommands, true);

        // Lectern wall:
        GetFillCommand(x - 1, z + 9 + bonusHeight, y + 0, x + 1, z + 9 + bonusHeight, y + 0, "minecraft:lectern[facing=south]", createCommands, clearCommands);
        GetFillCommand(x - 1, z + 9 + bonusHeight, y + 4, x + 1, z + 9 + bonusHeight, y + 4, "minecraft:lectern[facing=north]", createCommands, clearCommands);
        GetFillCommand(x - 2, z + 9 + bonusHeight, y + 1, x - 2, z + 9 + bonusHeight, y + 3, "minecraft:lectern[facing=east]", createCommands, clearCommands);
        GetFillCommand(x + 2, z + 9 + bonusHeight, y + 1, x + 2, z + 9 + bonusHeight, y + 3, "minecraft:lectern[facing=west]", createCommands, clearCommands);

        GetFillCommand(x - 1, z + 12 + bonusHeight, y + 0, x - 1, z + 12 + bonusHeight, y + 0, materials.PlankStairs + "[half=top,facing=west]", createCommands, clearCommands);
        GetFillCommand(x + 1, z + 12 + bonusHeight, y + 0, x + 1, z + 12 + bonusHeight, y + 0, materials.PlankStairs + "[half=top,facing=east]", createCommands, clearCommands);
        GetFillCommand(x - 1, z + 12 + bonusHeight, y + 4, x - 1, z + 12 + bonusHeight, y + 4, materials.PlankStairs + "[half=top,facing=west]", createCommands, clearCommands);
        GetFillCommand(x + 1, z + 12 + bonusHeight, y + 4, x + 1, z + 12 + bonusHeight, y + 4, materials.PlankStairs + "[half=top,facing=east]", createCommands, clearCommands);

        GetFillCommand(x - 2, z + 12 + bonusHeight, y + 1, x - 2, z + 12 + bonusHeight, y + 1, materials.PlankStairs + "[half=top,facing=north]", createCommands, clearCommands);
        GetFillCommand(x + 2, z + 12 + bonusHeight, y + 1, x + 2, z + 12 + bonusHeight, y + 1, materials.PlankStairs + "[half=top,facing=north]", createCommands, clearCommands);
        GetFillCommand(x - 2, z + 12 + bonusHeight, y + 3, x - 2, z + 12 + bonusHeight, y + 3, materials.PlankStairs + "[half=top,facing=south]", createCommands, clearCommands);
        GetFillCommand(x + 2, z + 12 + bonusHeight, y + 3, x + 2, z + 12 + bonusHeight, y + 3, materials.PlankStairs + "[half=top,facing=south]", createCommands, clearCommands);

        GetFillCommand(x, z + 12 + bonusHeight, y + 0, x, z + 12 + bonusHeight, y + 0, materials.PlankTrapdoor + "[half=top]", createCommands, clearCommands);
        GetFillCommand(x + 2, z + 12 + bonusHeight, y + 2, x + 2, z + 12 + bonusHeight, y + 2, materials.PlankTrapdoor + "[half=top]", createCommands, clearCommands);
        GetFillCommand(x, z + 12 + bonusHeight, y + 4, x, z + 12 + bonusHeight, y + 4, materials.PlankTrapdoor + "[half=top]", createCommands, clearCommands);
        GetFillCommand(x - 2, z + 12 + bonusHeight, y + 2, x - 2, z + 12 + bonusHeight, y + 2, materials.PlankTrapdoor + "[half=top]", createCommands, clearCommands);

        GetFillCommand(x - 1, z + 11 + bonusHeight, y + 0, x - 1, z + 11 + bonusHeight, y + 0, materials.PlankTrapdoor + "[open=true,facing=east]", createCommands, clearCommands);
        GetFillCommand(x + 1, z + 11 + bonusHeight, y + 0, x + 1, z + 11 + bonusHeight, y + 0, materials.PlankTrapdoor + "[open=true,facing=west]", createCommands, clearCommands);
        GetFillCommand(x - 1, z + 11 + bonusHeight, y + 4, x - 1, z + 11 + bonusHeight, y + 4, materials.PlankTrapdoor + "[open=true,facing=east]", createCommands, clearCommands);
        GetFillCommand(x + 1, z + 11 + bonusHeight, y + 4, x + 1, z + 11 + bonusHeight, y + 4, materials.PlankTrapdoor + "[open=true,facing=west]", createCommands, clearCommands);

        GetFillCommand(x - 2, z + 11 + bonusHeight, y + 1, x - 2, z + 11 + bonusHeight, y + 1, materials.PlankTrapdoor + "[open=true,facing=south]", createCommands, clearCommands);
        GetFillCommand(x + 2, z + 11 + bonusHeight, y + 1, x + 2, z + 11 + bonusHeight, y + 1, materials.PlankTrapdoor + "[open=true,facing=south]", createCommands, clearCommands);
        GetFillCommand(x - 2, z + 11 + bonusHeight, y + 3, x - 2, z + 11 + bonusHeight, y + 3, materials.PlankTrapdoor + "[open=true,facing=north]", createCommands, clearCommands);
        GetFillCommand(x + 2, z + 11 + bonusHeight, y + 3, x + 2, z + 11 + bonusHeight, y + 3, materials.PlankTrapdoor + "[open=true,facing=north]", createCommands, clearCommands);

        GetFillCommand(x - 1, z + 13 + bonusHeight, y - 1, x + 1, z + 13 + bonusHeight, y - 1, materials.PlankStairs + "[half=top,facing=south]", createCommands, clearCommands);
        GetFillCommand(x - 1, z + 13 + bonusHeight, y + 5, x + 1, z + 13 + bonusHeight, y + 5, materials.PlankStairs + "[half=top,facing=north]", createCommands, clearCommands);
        GetFillCommand(x - 3, z + 13 + bonusHeight, y + 1, x - 3, z + 13 + bonusHeight, y + 3, materials.PlankStairs + "[half=top,facing=east]", createCommands, clearCommands);
        GetFillCommand(x + 3, z + 13 + bonusHeight, y + 1, x + 3, z + 13 + bonusHeight, y + 3, materials.PlankStairs + "[half=top,facing=west]", createCommands, clearCommands);

        GetFillCommand(x - 1, z + 14 + bonusHeight, y + 0, x + 1, z + 14 + bonusHeight, y + 0, materials.Planks, createCommands, clearCommands);

        BuildFloors(x, z + 14 + bonusHeight, y + 2, x, z + 14 + bonusHeight, y + 2, "lantern[hanging=true]", createCommands, clearCommands);
        BuildFloors(x, z + 12 + bonusHeight, y + 2, x, z + 12 + bonusHeight, y + 2, "lantern[hanging=true]", createCommands, clearCommands);
        BuildFloors(x, z + 7 + bonusHeight, y + 2, x, z + 7 + bonusHeight, y + 2, "lantern[hanging=true]", createCommands, clearCommands);


        GetFillCommand(x - 1, z + 14 + bonusHeight, y + 4, x + 1, z + 14 + bonusHeight, y + 4, materials.Planks, createCommands, clearCommands);
        GetFillCommand(x - 2, z + 14 + bonusHeight, y + 1, x - 2, z + 14 + bonusHeight, y + 3, materials.Planks, createCommands, clearCommands);
        GetFillCommand(x + 2, z + 14 + bonusHeight, y + 1, x + 2, z + 14 + bonusHeight, y + 3, materials.Planks, createCommands, clearCommands);
        GetFillCommand(x - 1, z + 15 + bonusHeight, y + 1, x + 1, z + 15 + bonusHeight, y + 3, materials.Planks, createCommands, clearCommands);

        GetFillCommand(x + 1, z + 16 + bonusHeight, y + 2, x + 1, z + 16 + bonusHeight, y + 2, materials.PlankStairs + "[facing=west]", createCommands, clearCommands);
        GetFillCommand(x - 1, z + 16 + bonusHeight, y + 2, x - 1, z + 16 + bonusHeight, y + 2, materials.PlankStairs + "[facing=east]", createCommands, clearCommands);
        GetFillCommand(x, z + 16 + bonusHeight, y + 1, x, z + 16 + bonusHeight, y + 1, materials.PlankStairs + "[facing=south]", createCommands, clearCommands);
        GetFillCommand(x, z + 16 + bonusHeight, y + 3, x, z + 16 + bonusHeight, y + 3, materials.PlankStairs + "[facing=north]", createCommands, clearCommands);

        GetFillCommand(x, z + 17 + bonusHeight, y + 2, x, z + 17 + bonusHeight, y + 2, materials.Planks, createCommands, clearCommands);
        GetFillCommand(x, z + 18 + bonusHeight, y + 2, x, z + 18 + bonusHeight, y + 2, materials.Planks.Replace("_planks", "_fence"), createCommands, clearCommands);
    }

    public static void BuildTower2(int x, int z, int y, MaterialSet materials, List<string> createCommands, List<string> clearCommands)
    {

    }

    public static void BuildRoofWrap(int startX, int endX, int startY, int endY, int z, MaterialSet materials, List<string> createCommands, List<string> clearCommands)
    {

        int pillarMod = (startX + startY + 1) % 2;
        for (int i = startX; i <= endX; i++)
        {
            for (int j = startY; j <= endY; j++)
            {
                if ((i == startX || i == endX) || (j == startY || j == endY))
                {
                    if ((i + j) % 2 == pillarMod)
                    {
                        GetFillCommand(i, z, j, i, z + 1, j, materials.RoofRockPolished, createCommands, clearCommands, true);
                    }
                    else
                    {
                        if (!(i == startX || i == endX) || !(j == startY || j == endY))
                        {
                            // slabs:
                            GetFillCommand(i, z, j, i, z, j, materials.StoneBrickSlabs + "[type=top]", createCommands, clearCommands, true);
                        }
                    }
                }
            }
        }
    }

    public static void BuildGatehouse(int x, int z, int y, MaterialSet materials, List<string> createCommands, List<string> clearCommands)
    {
        // Origin of the gate house it right in front of the origin point

        // Build second floor first:
        GetFillCommand(x - 4, (z + 4), y, x + 4, (z + 4) + 6, y + 6, materials.StoneBricks, createCommands, clearCommands, true);
        GetFillCommand(x - 4, z - 1, y, x + 4, z + 4, y + 6, materials.StoneBricks, createCommands, clearCommands, true);
        GetFillCommand(x - 3, z - 1, y, x - 3, z + 4, y + 6, materials.StoneBricks, createCommands, clearCommands, true);
        GetFillCommand(x + 3, z - 1, y, x + 3, z + 4, y + 6, materials.StoneBricks, createCommands, clearCommands, true);
        GetFillCommand(x - 2, z, y, x + 2, z + 3, y + 6, "air", createCommands, clearCommands, true);
        GetFillCommand(x, z + 4, y, x, z + 4, y + 6, materials.StoneBrickSlabs + "[type=top]", createCommands, clearCommands, true);
        GetFillCommand(x, z + 4, y, x, z + 4, y + 6, materials.StoneBrickSlabs + "[type=top]", createCommands, clearCommands, true);
        GetFillCommand(x - 2, z + 3, y, x - 2, z + 3, y + 6, materials.StoneStairs + "[half=top,facing=west]", createCommands, clearCommands, true);
        GetFillCommand(x + 2, z + 3, y, x + 2, z + 3, y + 6, materials.StoneStairs + "[half=top,facing=east]", createCommands, clearCommands, true);

        int z2 = (z + 4);

        // Second floor winds:
        GetFillCommand(x - 3, z2 + 1, y, x - 3, z2 + 3, y, "air", createCommands, clearCommands, true);
        GetFillCommand(x + 3, z2 + 1, y, x + 3, z2 + 3, y, "air", createCommands, clearCommands, true);
        GetFillCommand(x - 1, z2 + 1, y, x + 1, z2 + 4, y, "air", createCommands, clearCommands, true);
        GetFillCommand(x - 1, z2 + 4, y, x - 1, z2 + 4, y, materials.StoneStairs + "[half=top,facing=west]", createCommands, clearCommands, true);
        GetFillCommand(x + 1, z2 + 4, y, x + 1, z2 + 4, y, materials.StoneStairs + "[half=top,facing=east]", createCommands, clearCommands, true);

        int startX = x - 5;
        int endX = x + 5;
        int startY = y - 1;
        int endY = y + 7;
        BuildRoofWrap(startX, endX, startY, endY, z2 + 6, materials, createCommands, clearCommands);

        BuildFloors(x + 2, z2 + 6 - 1, y - 1, x + 2, z2 + 6 - 1, y - 1, "lantern[hanging=true]", createCommands, clearCommands);
        BuildFloors(x + 2, z2 + 6 + 2, y - 1, x + 2, z2 + 6 + 2, y - 1, "lantern[hanging=false]", createCommands, clearCommands);
        BuildFloors(x - 2, z2 + 6 - 1, y - 1, x - 2, z2 + 6 - 1, y - 1, "lantern[hanging=true]", createCommands, clearCommands);
        BuildFloors(x - 2, z2 + 6 + 2, y - 1, x - 2, z2 + 6 + 2, y - 1, "lantern[hanging=false]", createCommands, clearCommands);
        BuildFloors(x, z2 + 6 - 2, y, x, z2 + 6 - 2, y, "lantern[hanging=true]", createCommands, clearCommands);

        BuildFloors(x + 1, z + 3, y, x + 1, z + 3, y, "lantern[hanging=true]", createCommands, clearCommands);
        BuildFloors(x - 1, z + 3, y, x - 1, z + 3, y, "lantern[hanging=true]", createCommands, clearCommands);
    }

    public static void BuildWalls(int x, int z, int y, int x2, int z2, int y2, int height, MaterialSet materials, List<string> createCommands, List<string> clearCommands)
    {
        GetFillCommand(x, z, y, x2, z2 + (height - 1), y2, materials.StoneBricks, createCommands, clearCommands, true);
    }

    public static void BuildFloors(int x, int z, int y, int x2, int z2, int y2, string material, List<string> createCommands, List<string> clearCommands)
    {
        GetFillCommand(x, z, y, x2, z2, y2, material, createCommands, clearCommands, true);
    }

    /// <summary>
    /// Building a castle based on the following template:
    /// https://www.youtube.com/watch?v=a4_qLnYZuls
    /// </summary>
    public static CommandGenerationResults BuildCastle(int x, int z, int y, MaterialSet materials)
    {
        List<string> createCommands = new List<string>();
        List<string> clearCommands = new List<string>();

        // clear the basement and upper floors:
        //BuildFloors(x + 15, z, y, x - 15, z + 30, y + 30, "air", createCommands, clearCommands);

        // Gatehouse: 9x7, 5 blocks tall, 5x4 tall clearing top and bottom:
        BuildGatehouse(x, z, y, materials, createCommands, clearCommands);

        BuildWalls(x - 5, z, y + 2, x - 6, z, y + 2, 7, materials, createCommands, clearCommands);
        GetFillCommand(x - 5, z + 6, y + 1, x - 5, z + 7, y + 1, materials.RoofRockPolished, createCommands, clearCommands, true);
        GetFillCommand(x - 6, z + 6, y + 1, x - 6, z + 6, y + 1, materials.StoneBrickSlabs + "[type=top]", createCommands, clearCommands, true);

        BuildWalls(x + 5, z, y + 2, x + 6, z, y + 2, 7, materials, createCommands, clearCommands);
        GetFillCommand(x + 5, z + 6, y + 1, x + 5, z + 7, y + 1, materials.RoofRockPolished, createCommands, clearCommands, true);
        GetFillCommand(x + 6, z + 6, y + 1, x + 6, z + 6, y + 1, materials.StoneBrickSlabs + "[type=top]", createCommands, clearCommands, true);

        BuildWalls(x - 7, z, y + 3, x - 9, z, y + 3, 7, materials, createCommands, clearCommands);
        GetFillCommand(x - 7, z + 6, y + 2, x - 7, z + 7, y + 2, materials.RoofRockPolished, createCommands, clearCommands, true);
        GetFillCommand(x - 8, z + 6, y + 2, x - 8, z + 6, y + 2, materials.StoneBrickSlabs + "[type=top]", createCommands, clearCommands, true);
        GetFillCommand(x - 9, z + 6, y + 2, x - 9, z + 7, y + 2, materials.RoofRockPolished, createCommands, clearCommands, true);

        BuildWalls(x + 7, z, y + 3, x + 9, z, y + 3, 7, materials, createCommands, clearCommands);
        GetFillCommand(x + 7, z + 6, y + 2, x + 7, z + 7, y + 2, materials.RoofRockPolished, createCommands, clearCommands, true);
        GetFillCommand(x + 8, z + 6, y + 2, x + 8, z + 6, y + 2, materials.StoneBrickSlabs + "[type=top]", createCommands, clearCommands, true);
        GetFillCommand(x + 9, z + 6, y + 2, x + 9, z + 7, y + 2, materials.RoofRockPolished, createCommands, clearCommands, true);

        BuildWalls(x - 10, z, y + 4, x - 11, z, y + 4, 7, materials, createCommands, clearCommands);
        GetFillCommand(x - 10, z + 6, y + 3, x - 10, z + 6, y + 3, materials.StoneBrickSlabs + "[type=top]", createCommands, clearCommands, true);
        GetFillCommand(x - 11, z + 6, y + 3, x - 11, z + 7, y + 3, materials.RoofRockPolished, createCommands, clearCommands, true);

        BuildWalls(x + 10, z, y + 4, x + 11, z, y + 4, 7, materials, createCommands, clearCommands);
        GetFillCommand(x + 10, z + 6, y + 3, x + 10, z + 6, y + 3, materials.StoneBrickSlabs + "[type=top]", createCommands, clearCommands, true);
        GetFillCommand(x + 11, z + 6, y + 3, x + 11, z + 7, y + 3, materials.RoofRockPolished, createCommands, clearCommands, true);

        BuildTower1(x + 14, z, y + 1, materials, createCommands, clearCommands, 5);
        BuildTower1(x - 14, z, y + 1, materials, createCommands, clearCommands, 5);

        BuildWalls(x + 14, z, y + 6, x + 14, z, y + 26, 7, materials, createCommands, clearCommands);
        BuildWalls(x - 14, z, y + 6, x - 14, z, y + 26, 7, materials, createCommands, clearCommands);

        int modFinder = (y + 6) % 2;
        int modFinderLantern = (y + 6) % 4;
        for (int i = y + 6; i <= y + 26; i++)
        {
            if ((i % 2) == modFinder)
            {
                GetFillCommand(x + 15, z + 6, i, x + 15, z + 7, i, materials.RoofRockPolished, createCommands, clearCommands, true);
                GetFillCommand(x - 15, z + 6, i, x - 15, z + 7, i, materials.RoofRockPolished, createCommands, clearCommands, true);
                if ((i % 4) == modFinderLantern)
                {
                    BuildFloors(x + 15, z + 5, i, x + 15, z + 5, i, "lantern[hanging=true]", createCommands, clearCommands);
                    BuildFloors(x + 15, z + 8, i, x + 15, z + 8, i, "lantern[hanging=false]", createCommands, clearCommands);
                    BuildFloors(x - 15, z + 5, i, x - 15, z + 5, i, "lantern[hanging=true]", createCommands, clearCommands);
                    BuildFloors(x - 15, z + 8, i, x - 15, z + 8, i, "lantern[hanging=false]", createCommands, clearCommands);
                }
            }
            else
            {
                GetFillCommand(x + 15, z + 6, i, x + 15, z + 6, i, materials.StoneBrickSlabs + "[type=top]", createCommands, clearCommands, true);
                GetFillCommand(x - 15, z + 6, i, x - 15, z + 6, i, materials.StoneBrickSlabs + "[type=top]", createCommands, clearCommands, true);
            }
        }

        BuildTower1(x + 14, z, y + 27, materials, createCommands, clearCommands, 5);
        BuildTower1(x - 14, z, y + 27, materials, createCommands, clearCommands, 5);

        BuildWalls(x + 11, z, y + 29, x - 11, z, y + 29, 7, materials, createCommands, clearCommands);

        modFinder = (x - 11) % 2;
        modFinderLantern = (x - 11) % 4;
        for (int i = x - 11; i <= x + 11; i++)
        {
            if ((i % 2) == modFinder)
            {
                GetFillCommand(i, z + 6, y + 30, i, z + 7, y + 30, materials.RoofRockPolished, createCommands, clearCommands, true);
                if ((i % 4) == modFinderLantern)
                {
                    BuildFloors(i, z + 5, y + 30, i, z + 5, y + 30, "lantern[hanging=true]", createCommands, clearCommands);
                    BuildFloors(i, z + 8, y + 30, i, z + 8, y + 30, "lantern[hanging=false]", createCommands, clearCommands);
                }
            }
            else
            {
                GetFillCommand(i, z + 6, y + 30, i, z + 6, y + 30, materials.StoneBrickSlabs + "[type=top]", createCommands, clearCommands, true);
            }
        }

        // building the floors:
        BuildFloors(x + 14, z - 1, y + 7, x - 14, z - 1, y + 29, materials.RoofRockPolished, createCommands, clearCommands);
        BuildFloors(x + 14, z - 1, y + 7, x - 14, z - 1, y + 29, materials.RoofRockPolished, createCommands, clearCommands);

        BuildFloors(x + 14, z - 1, y + 4, x + 5, z - 1, y + 29, materials.RoofRockPolished, createCommands, clearCommands);
        BuildFloors(x - 14, z - 1, y + 4, x - 5, z - 1, y + 29, materials.RoofRockPolished, createCommands, clearCommands);

        BuildFloors(x + 14, z + 5, y + 5, x + 4, z + 5, y + 27, materials.StoneBricks, createCommands, clearCommands);
        BuildFloors(x - 14, z + 5, y + 5, x - 4, z + 5, y + 27, materials.StoneBricks, createCommands, clearCommands);

        BuildFloors(x + 13, z + 5, y + 13, x - 13, z + 5, y + 28, materials.StoneBricks, createCommands, clearCommands);

        BuildFloors(x + 3, z, y + 7, x + 3, z + 6, y + 12, materials.Logs + "[axis=y]", createCommands, clearCommands);
        BuildFloors(x - 3, z, y + 7, x - 3, z + 6, y + 12, materials.Logs + "[axis=y]", createCommands, clearCommands);

        BuildFloors(x - 4, z + 5, y + 3, x - 9, z + 5, y + 5, materials.StoneBricks, createCommands, clearCommands);
        BuildFloors(x + 4, z + 5, y + 3, x + 9, z + 5, y + 5, materials.StoneBricks, createCommands, clearCommands);

        BuildFloors(x + 2, z, y + 7, x - 2, z, y + 7, materials.StoneStairs + "[facing=south]", createCommands, clearCommands);
        BuildFloors(x + 2, z + 1, y + 8, x - 2, z + 1, y + 8, materials.StoneStairs + "[facing=south]", createCommands, clearCommands);
        BuildFloors(x + 2, z + 2, y + 9, x - 2, z + 2, y + 9, materials.StoneStairs + "[facing=south]", createCommands, clearCommands);
        BuildFloors(x + 2, z + 3, y + 10, x - 2, z + 3, y + 10, materials.StoneStairs + "[facing=south]", createCommands, clearCommands);
        BuildFloors(x + 2, z + 4, y + 11, x - 2, z + 4, y + 11, materials.StoneStairs + "[facing=south]", createCommands, clearCommands);
        BuildFloors(x + 2, z + 5, y + 12, x - 2, z + 5, y + 12, materials.StoneStairs + "[facing=south]", createCommands, clearCommands);

        BuildFloors(x + 13, z, y + 6, x + 13, z, y + 6, materials.StoneStairs + "[facing=north]", createCommands, clearCommands);
        BuildFloors(x + 13, z + 1, y + 5, x + 13, z + 2, y + 5, "air", createCommands, clearCommands);

        BuildFloors(x + 12, z, y + 8, x + 13, z, y + 8, materials.StoneStairs + "[facing=south]", createCommands, clearCommands);
        BuildFloors(x + 12, z + 1, y + 9, x + 13, z + 1, y + 9, materials.StoneStairs + "[facing=south]", createCommands, clearCommands);
        BuildFloors(x + 12, z + 2, y + 10, x + 13, z + 2, y + 10, materials.StoneStairs + "[facing=south]", createCommands, clearCommands);
        BuildFloors(x + 12, z + 3, y + 11, x + 13, z + 3, y + 11, materials.StoneStairs + "[facing=south]", createCommands, clearCommands);
        BuildFloors(x + 12, z + 4, y + 12, x + 13, z + 4, y + 12, materials.StoneStairs + "[facing=south]", createCommands, clearCommands);
        BuildFloors(x + 12, z + 5, y + 13, x + 13, z + 5, y + 13, materials.StoneStairs + "[facing=south]", createCommands, clearCommands);
        BuildFloors(x + 12, z + 5, y + 8, x + 13, z + 5, y + 12, "air", createCommands, clearCommands);

        BuildFloors(x - 13, z, y + 6, x - 13, z, y + 6, materials.StoneStairs + "[facing=north]", createCommands, clearCommands);
        BuildFloors(x - 13, z + 1, y + 5, x - 13, z + 2, y + 5, "air", createCommands, clearCommands);

        BuildFloors(x - 12, z, y + 8, x - 13, z, y + 8, materials.StoneStairs + "[facing=south]", createCommands, clearCommands);
        BuildFloors(x - 12, z + 1, y + 9, x - 13, z + 1, y + 9, materials.StoneStairs + "[facing=south]", createCommands, clearCommands);
        BuildFloors(x - 12, z + 2, y + 10, x - 13, z + 2, y + 10, materials.StoneStairs + "[facing=south]", createCommands, clearCommands);
        BuildFloors(x - 12, z + 3, y + 11, x - 13, z + 3, y + 11, materials.StoneStairs + "[facing=south]", createCommands, clearCommands);
        BuildFloors(x - 12, z + 4, y + 12, x - 13, z + 4, y + 12, materials.StoneStairs + "[facing=south]", createCommands, clearCommands);
        BuildFloors(x - 12, z + 5, y + 13, x - 13, z + 5, y + 13, materials.StoneStairs + "[facing=south]", createCommands, clearCommands);
        BuildFloors(x - 12, z + 5, y + 8, x - 13, z + 5, y + 12, "air", createCommands, clearCommands);

        BuildFloors(x - 4, z + 4, y + 8, x - 4, z + 4, y + 8, "lantern[hanging=true]", createCommands, clearCommands);
        BuildFloors(x + 4, z + 4, y + 8, x + 4, z + 4, y + 8, "lantern[hanging=true]", createCommands, clearCommands);

        for (int i = 0; i <= 4; i++)
        {
            BuildFloors(x - 4, z + 4, y + 8 + (4 * i), x - 4, z + 4, y + 8 + (4 * i), "lantern[hanging=true]", createCommands, clearCommands);
            BuildFloors(x + 4, z + 4, y + 8 + (4 * i), x + 4, z + 4, y + 8 + (4 * i), "lantern[hanging=true]", createCommands, clearCommands);

            BuildFloors(x - 8, z + 4, y + 8 + (4 * i), x - 8, z + 4, y + 8 + (4 * i), "lantern[hanging=true]", createCommands, clearCommands);
            BuildFloors(x + 8, z + 4, y + 8 + (4 * i), x + 8, z + 4, y + 8 + (4 * i), "lantern[hanging=true]", createCommands, clearCommands);
        }

        BuildFloors(x - 3, z + 7, y + 7, x - 3, z + 7, y + 7, "lantern[hanging=false]", createCommands, clearCommands);
        BuildFloors(x + 3, z + 7, y + 7, x + 3, z + 7, y + 7, "lantern[hanging=false]", createCommands, clearCommands);
        BuildFloors(x - 3, z + 7, y + 12, x - 3, z + 7, y + 12, "lantern[hanging=false]", createCommands, clearCommands);
        BuildFloors(x + 3, z + 7, y + 12, x + 3, z + 7, y + 12, "lantern[hanging=false]", createCommands, clearCommands);

        // Building out the basement:
        BuildFloors(x + 3, z, y + 12, x + 3, z + 4, y + 12, "air", createCommands, clearCommands);
        BuildFloors(x - 3, z, y + 12, x - 3, z + 4, y + 12, "air", createCommands, clearCommands);
        BuildFloors(x + 5, z, y + 13, x + 5, z + 4, y + 13, materials.Logs + "[axis=y]", createCommands, clearCommands);
        BuildFloors(x - 5, z, y + 13, x - 5, z + 4, y + 13, materials.Logs + "[axis=y]", createCommands, clearCommands);
        BuildFloors(x + 5, z, y + 14, x + 5, z + 4, y + 28, materials.StoneBricks, createCommands, clearCommands);
        BuildFloors(x - 5, z, y + 14, x - 5, z + 4, y + 28, materials.StoneBricks, createCommands, clearCommands);
        BuildFloors(x + 6, z, y + 13, x + 13, z + 4, y + 13, materials.StoneBricks, createCommands, clearCommands);
        BuildFloors(x - 6, z, y + 13, x - 13, z + 4, y + 13, materials.StoneBricks, createCommands, clearCommands);
        BuildFloors(x + 5, z, y + 19, x - 5, z + 4, y + 19, materials.StoneBricks, createCommands, clearCommands);

        BuildFloors(x + 1, z, y + 19, x - 1, z + 3, y + 19, "minecraft:oak_trapdoor[open=true]", createCommands, clearCommands);
        BuildFloors(x, z, y + 19, x, z + 1, y + 19, "air", createCommands, clearCommands);
        BuildFloors(x, z, y + 19, x, z, y + 19, materials.PlankDoor + "[half=lower]", createCommands, clearCommands);
        BuildFloors(x, z + 1, y + 19, x, z + 1, y + 19, materials.PlankDoor + "[half=upper]", createCommands, clearCommands);

        BuildFloors(x + 5, z, y + 15, x + 5, z + 3, y + 17, "minecraft:oak_trapdoor[open=true,facing=west]", createCommands, clearCommands);
        BuildFloors(x - 5, z, y + 15, x - 5, z + 3, y + 17, "minecraft:oak_trapdoor[open=true,facing=east]", createCommands, clearCommands);

        BuildFloors(x + 5, z, y + 16, x + 5, z + 1, y + 16, "air", createCommands, clearCommands);
        BuildFloors(x - 5, z, y + 16, x - 5, z + 1, y + 16, "air", createCommands, clearCommands);

        BuildFloors(x + 5, z, y + 16, x + 5, z, y + 16, materials.PlankDoor + "[half=lower,facing=west]", createCommands, clearCommands);
        BuildFloors(x + 5, z + 1, y + 16, x + 5, z + 1, y + 16, materials.PlankDoor + "[half=upper,facing=west]", createCommands, clearCommands);

        BuildFloors(x - 5, z, y + 16, x - 5, z, y + 16, materials.PlankDoor + "[half=lower]", createCommands, clearCommands);
        BuildFloors(x - 5, z + 1, y + 16, x - 5, z + 1, y + 16, materials.PlankDoor + "[half=upper]", createCommands, clearCommands);

        BuildFloors(x + 4, z, y + 20, x - 4, z + 3, y + 28, "minecraft:chest", createCommands, clearCommands);
        BuildFloors(x + 4, z + 4, y + 20, x - 4, z + 4, y + 28, "air", createCommands, clearCommands);

        BuildFloors(x + 4, z - 1, y + 20, x + 4, z - 1, y + 20, materials.LanternBlock, createCommands, clearCommands);
        BuildFloors(x - 4, z - 1, y + 20, x - 4, z - 1, y + 20, materials.LanternBlock, createCommands, clearCommands);
        BuildFloors(x + 4, z - 1, y + 28, x + 4, z - 1, y + 28, materials.LanternBlock, createCommands, clearCommands);
        BuildFloors(x - 4, z - 1, y + 28, x - 4, z - 1, y + 28, materials.LanternBlock, createCommands, clearCommands);

        // Top floor housing area:
        BuildFloors(x + 5, z + 6, y + 18, x - 5, z + 16, y + 28, materials.StoneBricks, createCommands, clearCommands);

        int startX = x - 6;
        int endX = x + 6;
        int startY = y + 17;
        int endY = y + 29;
        BuildRoofWrap(startX, endX, startY, endY, z + 16, materials, createCommands, clearCommands);

        BuildFloors(x + 3, z + 18, y + 17, x + 3, z + 18, y + 17, "lantern[hanging=false]", createCommands, clearCommands);
        BuildFloors(x - 3, z + 18, y + 17, x - 3, z + 18, y + 17, "lantern[hanging=false]", createCommands, clearCommands);
        BuildFloors(x + 3, z + 15, y + 17, x + 3, z + 15, y + 17, "lantern[hanging=true]", createCommands, clearCommands);
        BuildFloors(x - 3, z + 15, y + 17, x - 3, z + 15, y + 17, "lantern[hanging=true]", createCommands, clearCommands);

        BuildFloors(x + 3, z + 18, y + 29, x + 3, z + 18, y + 29, "lantern[hanging=false]", createCommands, clearCommands);
        BuildFloors(x - 3, z + 18, y + 29, x - 3, z + 18, y + 29, "lantern[hanging=false]", createCommands, clearCommands);
        BuildFloors(x + 3, z + 15, y + 29, x + 3, z + 15, y + 29, "lantern[hanging=true]", createCommands, clearCommands);
        BuildFloors(x - 3, z + 15, y + 29, x - 3, z + 15, y + 29, "lantern[hanging=true]", createCommands, clearCommands);

        BuildFloors(x + 2, z + 15, y + 20, x + 2, z + 15, y + 20, "lantern[hanging=true]", createCommands, clearCommands);
        BuildFloors(x - 2, z + 15, y + 20, x - 2, z + 15, y + 20, "lantern[hanging=true]", createCommands, clearCommands);
        BuildFloors(x - 2, z + 15, y + 26, x - 2, z + 15, y + 26, "lantern[hanging=true]", createCommands, clearCommands);
        BuildFloors(x + 2, z + 15, y + 26, x + 2, z + 15, y + 26, "lantern[hanging=true]", createCommands, clearCommands);


        BuildFloors(x + 4, z + 6, y + 19, x - 4, z + 6, y + 27, materials.Logs, createCommands, clearCommands);
        BuildFloors(x + 3, z + 6, y + 20, x - 3, z + 6, y + 26, materials.ChiseledBricks, createCommands, clearCommands);
        BuildFloors(x + 2, z + 6, y + 21, x - 2, z + 6, y + 25, materials.Planks, createCommands, clearCommands);

        BuildFloors(x + 1, z + 7, y + 18, x - 1, z + 10, y + 18, "minecraft:oak_trapdoor[open=true]", createCommands, clearCommands);
        BuildFloors(x, z + 7, y + 18, x, z + 8, y + 18, "air", createCommands, clearCommands);
        BuildFloors(x, z + 7, y + 18, x, z + 7, y + 18, materials.PlankDoor + "[half=lower]", createCommands, clearCommands);
        BuildFloors(x, z + 8, y + 18, x, z + 8, y + 18, materials.PlankDoor + "[half=upper]", createCommands, clearCommands);

        BuildFloors(x + 5, z + 6, y + 17, x - 5, z + 6, y + 17, materials.StoneStairs + "[facing=south]", createCommands, clearCommands);


        // Command terminal at the very last:
        BuildFloors(x, z + 4, y + 27, x, z + 7, y + 27, "minecraft:chest", createCommands, clearCommands);
        BuildFloors(x, z + 8, y + 27, x, z + 8, y + 27, "toms_storage:ts.inventory_connector", createCommands, clearCommands);
        BuildFloors(x, z + 8, y + 26, x, z + 8, y + 26, "toms_storage:ts.crafting_terminal[facing=south]", createCommands, clearCommands);

        BuildFloors(x, z + 6, y + 26, x, z + 7, y + 26, materials.StoneBricks, createCommands, clearCommands);
        BuildFloors(x, z + 8, y + 27, x, z + 9, y + 27, materials.StoneBricks, createCommands, clearCommands);
        BuildFloors(x, z + 6, y + 28, x, z + 8, y + 28, materials.StoneBricks, createCommands, clearCommands);

        BuildFloors(x + 1, z + 6, y + 27, x + 1, z + 8, y + 27, materials.StoneBricks, createCommands, clearCommands);
        BuildFloors(x - 1, z + 6, y + 27, x - 1, z + 8, y + 27, materials.StoneBricks, createCommands, clearCommands);

        return new CommandGenerationResults() { createCommands = createCommands, clearCommands = clearCommands };
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
        StringBuilder clipboardBuilder = new StringBuilder();
        StringBuilder consoleBuilder = new StringBuilder();

        if (results.createCommands != null)
        {
            foreach (string command in results.createCommands)
            {
                consoleBuilder.AppendLine(command);
                clipboardBuilder.AppendLine(command);
            }
            consoleBuilder.AppendLine("");
        }
        if (results.createLastCommands != null)
        {
            foreach (string command in results.createLastCommands)
            {
                consoleBuilder.AppendLine(command);
                clipboardBuilder.AppendLine(command);
            }
            consoleBuilder.AppendLine("");
        }
        if (printDeletes && results.clearCommands != null)
        {
            foreach (string command in results.clearCommands)
            {
                consoleBuilder.AppendLine(command);
            }
            consoleBuilder.AppendLine("");
        }

        Console.WriteLine(consoleBuilder.ToString());
        // Copy only the relevant commands to the clipboard (excluding clearCommands)
        ClipboardService.SetText(clipboardBuilder.ToString());
    }

    public static void PrintCommands(List<CommandGenerationResults> results, bool printDeletes = false)
    {
        StringBuilder clipboardBuilder = new StringBuilder();
        StringBuilder consoleBuilder = new StringBuilder();

        foreach (var r in results)
        {
            if (r.createCommands != null)
            {
                foreach (string command in r.createCommands)
                {
                    consoleBuilder.AppendLine(command);
                    clipboardBuilder.AppendLine(command);
                }
            }
        }
        consoleBuilder.AppendLine("");
        foreach (var r in results)
        {
            if (r.createLastCommands != null)
            {
                foreach (string command in r.createLastCommands)
                {
                    consoleBuilder.AppendLine(command);
                    clipboardBuilder.AppendLine(command);
                }
            }
        }
        consoleBuilder.AppendLine("");
        if (printDeletes)
        {
            foreach (var r in results)
            {
                if (r.clearCommands != null)
                {
                    foreach (string command in r.clearCommands)
                    {
                        consoleBuilder.AppendLine(command);
                    }
                }
            }
            consoleBuilder.AppendLine("");
        }

        Console.WriteLine(consoleBuilder.ToString());
        // Copy only the relevant commands to the clipboard (excluding clearCommands)
        ClipboardService.SetText(clipboardBuilder.ToString());
    }

    public static void ShowHelp()
    {
        Console.WriteLine("Available Commands:");
        Console.WriteLine("  mfg x z y a b c -    Generates a mob farm.");
        Console.WriteLine("                       x, z, y: Starting point (integers)");
        Console.WriteLine("                       a: Interior material (string, e.g., 'minecraft:black_concrete')");
        Console.WriteLine("                       b: Exterior material (string, e.g., 'minecraft:cut_sandstone')");
        Console.WriteLine("                       c: Lantern block (string, e.g., 'minecraft:sea_lantern')");
        Console.WriteLine("  mfgx x z y a b c r - Generates a network of mob farms with radius r.");
        Console.WriteLine("                       x, z, y: Starting point (integers)");
        Console.WriteLine("                       a: Interior material (string, e.g., 'minecraft:black_concrete')");
        Console.WriteLine("                       b: Exterior material (string, e.g., 'minecraft:cut_sandstone')");
        Console.WriteLine("                       c: Lantern block (string, e.g., 'minecraft:sea_lantern')");
        Console.WriteLine("                       r: Radius of the mob farm network, 1 would yield a 3x3 network");
        Console.WriteLine();
        Console.WriteLine("  buildcastle x z y material1 material2 ... material23 - Builds a castle at the given");
        Console.WriteLine("                       coordinates using the specified materials.");
        Console.WriteLine("                       x, z, y: Starting point (integers)");
        Console.WriteLine("                       The materials correspond to the following castle components:");
        Console.WriteLine("                       - Stone: stonebricks, stoneslabs, stonebrickslabs, stonestairs,");
        Console.WriteLine("                         glass, chiseledbricks");
        Console.WriteLine("                       - Wood: logs, strippedlogs, planks, plankstairs, planktrapdoor,");
        Console.WriteLine("                         plankslab, plankdoor");
        Console.WriteLine("                       - Roof: roofrock, roofrockpolished, roofrockslab");
        Console.WriteLine("                       - Additional Stone: stonebricks2, stoneslabs2, stonebrickslabs2");
        Console.WriteLine("                       - Decorations: cornerwalls, ladder, torch, lanternblock");
        Console.WriteLine();
        Console.WriteLine("                       Example:");
        Console.WriteLine("                       buildcastle 269 130 804 minecraft:stone_bricks minecraft:stone_slab minecraft:stone_brick_slab minecraft:stone_stairs minecraft:glass_pane minecraft:chiseled_stone_bricks minecraft:spruce_log minecraft:stripped_spruce_log minecraft:spruce_planks minecraft:spruce_stairs minecraft:spruce_trapdoor minecraft:spruce_slab minecraft:spruce_door minecraft:andesite minecraft:polished_andesite minecraft:andesite_slab minecraft:cobblestone minecraft:cobblestone_slab minecraft:stone_brick_stairs minecraft:cobblestone_wall minecraft:ladder minecraft:torch minecraft:glowstone");
        Console.WriteLine();
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
                //input = "mfgx 269 129 804 minecraft:black_concrete minecraft:cut_sandstone supplementaries:end_stone_lamp 2";
                //input = "mfg 269 129 804 minecraft:black_concrete minecraft:cut_sandstone supplementaries:end_stone_lamp";

                input = "buildcastle 269 130 804 minecraft:stone_bricks minecraft:stone_slab minecraft:stone_brick_slab minecraft:stone_stairs minecraft:glass_pane minecraft:chiseled_stone_bricks minecraft:spruce_log minecraft:stripped_spruce_log minecraft:spruce_planks minecraft:spruce_stairs minecraft:spruce_trapdoor minecraft:spruce_slab minecraft:spruce_door minecraft:andesite minecraft:polished_andesite minecraft:andesite_slab minecraft:cobblestone minecraft:cobblestone_slab minecraft:stone_brick_stairs minecraft:cobblestone_wall minecraft:ladder minecraft:torch minecraft:glowstone";

                //input = "buildcastle 304 130 804 minecraft:stone_bricks minecraft:stone_slab minecraft:stone_brick_slab minecraft:stone_stairs minecraft:glass_pane minecraft:chiseled_stone_bricks minecraft:spruce_log minecraft:stripped_spruce_log minecraft:spruce_planks minecraft:spruce_stairs minecraft:spruce_trapdoor minecraft:spruce_slab minecraft:spruce_door minecraft:andesite minecraft:polished_andesite minecraft:andesite_slab minecraft:cobblestone minecraft:cobblestone_slab minecraft:stone_brick_stairs minecraft:cobblestone_wall minecraft:ladder minecraft:torch minecraft:glowstone";
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
                case "buildcastle":
                    {
                        if (parts.Length == 27 &&
                            int.TryParse(parts[1], out int x) &&
                            int.TryParse(parts[2], out int z) &&
                            int.TryParse(parts[3], out int y))
                        {
                            try
                            {
                                var materials = new MaterialSet(parts[4..]);
                                Console.WriteLine("Materials set successfully.");
                                Console.WriteLine(materials);
                                Console.WriteLine("Building castle...");
                                Console.WriteLine("");
                                PrintCommands(BuildCastle(x, z, y, materials), true);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error setting materials or building castle: {ex.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid syntax for 'buildcastle'. Use: buildcastle x z y material1 material2 ... material23");
                        }
                    }
                    break;
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


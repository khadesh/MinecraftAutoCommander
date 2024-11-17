# MinecraftAutoCommander

This program is designed to allow someone to easily generate large amounts of Minecraft content programmatically.

The console has a 'help' command to find all available commands and how to use them.

Help menu:

Available Commands:
  mfg x z y a b c -    Generates a mob farm.
                       x, z, y: Starting point (integers)
                       a: Interior material (string, e.g., 'minecraft:black_concrete')
                       b: Exterior material (string, e.g., 'minecraft:cut_sandstone')
                       c: Lantern block (string, e.g., 'minecraft:sea_lantern')
  mfgx x z y a b c r - Generates an network of mob farms wirth radius r.
                       x, z, y: Starting point (integers)
                       a: Interior material (string, e.g., 'minecraft:black_concrete')
                       b: Exterior material (string, e.g., 'minecraft:cut_sandstone')
                       c: Lantern block (string, e.g., 'minecraft:sea_lantern')
                       r: Radius of the mob farm network, 1 would yield a 3x3 network
  help               - Displays this help menu.

Example commands:
* mfgx 269 129 804 minecraft:black_concrete minecraft:cut_sandstone supplementaries:end_stone_lamp 2
* The above command will generate a grid of 5x5 mob farms, it uses the toms simple storage mod to create a network for pulling the items from hopper chests at the bottom.
* mfg 269 129 804 minecraft:black_concrete minecraft:cut_sandstone supplementaries:end_stone_lamp
* The above command will generate a single auto generated mob farm, it uses the toms simple storage mod to create a network for pulling the items from hopper chests at the bottom.

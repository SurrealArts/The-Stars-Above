
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using SubworldLibrary;
using StarsAbove.Buffs;
using StarsAbove.Projectiles.Otherworld;
using StarsAbove.Buffs.SubworldModifiers;
using StarsAbove.Subworlds;

namespace StarsAbove.Items.Placeable
{

    public class CelestriadRoot : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Celestriad Root");
			

			/* Tooltip.SetDefault("" +
				"A sapling from the heart of the universe" +
				"\nCan be used as a crafting bench to create the Stellaglyph and its upgrades" +
				"" +
				""); */
			//
			//
			//"\n[c/D32C2C:Modded chests from mods added after world generation may cease to open once entering a subworld]" +
			//"\n[c/D32C2C:Mods which allow global auto-use may cause issues upon usage]" +
			//"\n[c/D32C2C:Mods which 'cull' projectiles (anti-lag mods) will cause issues]");
			ItemID.Sets.SortingPriorityBossSpawns[Item.type] = 13; // This helps sort inventory know this is a boss summoning item.

		}

		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.CelestriadRoot>(), 0);

			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 1;
			Item.rare = ModContent.GetInstance<Systems.StellarRarity>().Type; // Custom Rarity
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = SoundID.Item44;
			Item.consumable = true;
			Item.noUseGraphic = false; Item.ResearchUnlockCount = 0;

		}


		public override void AddRecipes()
		{
			
		}
	}
}
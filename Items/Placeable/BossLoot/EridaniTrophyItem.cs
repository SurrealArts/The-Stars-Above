using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace StarsAbove.Items.Placeable.BossLoot
{
	public class EridaniTrophyItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Arbitration Trophy");

			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults()
		{
			// Vanilla has many useful methods like these, use them! This substitutes setting Item.createTile and Item.placeStyle aswell as setting a few values that are common across all placeable items
			Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.BossLoot.EridaniTrophy>());

			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 99;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.buyPrice(0, 1);
		}
	}
}
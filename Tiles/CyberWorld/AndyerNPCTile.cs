using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Enums;
using System;
using ReLogic.Content;
using Terraria.Localization;
using StarsAbove.Utilities;
using StarsAbove.Systems;

namespace StarsAbove.Tiles.CyberWorld
{
    // Common code for a Master Mode boss relic
    // Contains comments for optional Item.placeStyle handling if you wish to add more relics but use the same tile type (then it would be wise to name this class something more generic like BossRelic)
    // And in case of wanting to add more relics but not wanting to go the optional way, scroll down to the bottom of the file
    public class AndyerNPCTile : ModTile
	{
		public const int FrameWidth = 18 * 3;
		public const int FrameHeight = 18 * 4;
		public const int HorizontalFrames = 1;
		public const int VerticalFrames = 1; // Optional: Increase this number to match the amount of relics you have on your extra sheet, if you choose to go the Item.placeStyle way

		public Asset<Texture2D> RelicTexture;

		// Every relic has its own extra floating part, should be 50x50. Optional: Expand this sheet if you want to add more, stacked vertically
		// If you do not go the optional way, and you extend from this class, you can override this to point to a different texture

		// All relics use the same pedestal texture, this one is copied from vanilla
		public override string Texture => "StarsAbove/Tiles/CyberWorld/AndyerNPCTile";
		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				

			}
			else
			{

			}



			base.NearbyEffects(i, j, closer);
		}
		public override bool RightClick(int i, int j)
		{
			//Main.LocalPlayer.GetModPlayer<CelestialCartographyPlayer>().CelestialCartographyActive = true;
			return true;
		}
		public override void MouseOver(int i, int j)
		{
			int whoAmI = 0;
			Player player = Main.player[whoAmI];
			player.noThrow = 2;
			//player.cursorItemIconEnabled = true;
			//player.cursorItemIconID = Mod.Find<ModItem>("StellaglyphTier1").Type;
		}
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
			return true;
            return false;
            
        }
        public override void Load()
		{
			if (!Main.dedServ)
			{
				// Cache the extra texture displayed on the pedestal
			}
		}

		public override void Unload()
		{
			// Unload the extra texture displayed on the pedestal
		}

		public override void SetStaticDefaults()
		{
			//Main.tileShine[Type] = 400; // Responsible for golden particles
			Main.tileFrameImportant[Type] = true; // Any multitile requires this
			TileID.Sets.InteractibleByNPCs[Type] = true; // Town NPCs will palm their hand at this tile

			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x4); // Relics are 3x4
			TileObjectData.newTile.LavaDeath = false; // Does not break when lava touches it
			TileObjectData.newTile.DrawYOffset = 2; // So the tile sinks into the ground
			TileObjectData.newTile.Direction = TileObjectDirection.None;
			TileObjectData.newTile.StyleHorizontal = false; // Based on how the alternate sprites are positioned on the sprite (by default, true)

			// Optional: If you decide to make your tile utilize different styles through Item.placeStyle, you need these, aswell as the code in SetDrawPositions
			// TileObjectData.newTile.StyleWrapLimitVisualOverride = 2;
			// TileObjectData.newTile.StyleMultiplier = 2;
			// TileObjectData.newTile.StyleWrapLimit = 2;
			// TileObjectData.newTile.styleLineSkipVisualOverride = 0;

			// Register an alternate tile data with flipped direction
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile); // Copy everything from above, saves us some code
			TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight; // Player faces to the right
			TileObjectData.addAlternate(1);

			// Register the tile data itself
			TileObjectData.addTile(Type);

			
			AddMapEntry(new Color(0, 185, 255),  CreateMapEntryName());
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			// This code here infers the placeStyle the tile was placed with. Only required if you go the Item.placeStyle approach. You just need Item.NewItem otherwise
			// The placeStyle calculated here corresponds to whatever placeStyle you specified on your items that place this tile (Either through Item.placeTile or Item.DefaultToPlacableTile)
			int placeStyle = frameX / FrameWidth;

			int itemType = ModContent.ItemType<Items.Placeable.Stellaglyphs.StellaglyphTier1>();
			switch (placeStyle)
			{
				case 0:
					//itemType = ModContent.ItemType<Items.Placeable.CelestriadRoot>();
					break;
					// Optional: Add more cases here
			}

			if (itemType > 0)
			{
				// Spawn the item
			}
		}

		public override bool CreateDust(int i, int j, ref int type)
		{
			return false;
		}

		public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
		{
			// Only required If you decide to make your tile utilize different styles through Item.placeStyle

			// This preserves its original frameX/Y which is required for determining the correct texture floating on the pedestal, but makes it draw properly
			tileFrameX %= FrameWidth; // Clamps the frameX
			tileFrameY %= FrameHeight * 2; // Clamps the frameY (two horizontally aligned place styles, hence * 2)
		}

		public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
		{
			// Since this tile does not have the hovering part on its sheet, we have to animate it ourselves
			// Therefore we register the top-left of the tile as a "special point"
			// This allows us to draw things in SpecialDraw
			if (drawData.tileFrameX % FrameWidth == 0 && drawData.tileFrameY % FrameHeight == 0)
			{
				Main.instance.TilesRenderer.AddSpecialLegacyPoint(i, j);
			}
		}

		float rotation;
		public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
		{
			// This is lighting-mode specific, always include this if you draw tiles manually
			Vector2 offScreen = new Vector2(Main.offScreenRange);
			if (Main.drawToScreen)
			{
				offScreen = Vector2.Zero;
			}

			// Take the tile, check if it actually exists
			Point p = new Point(i, j);
			Tile tile = Main.tile[p.X, p.Y];
			if (tile == null || !tile.HasTile)
			{
				return;
			}
			rotation += 0.01f;
			if(rotation > 360)
            {
				rotation = 0;
            }

			
		}
	}
}
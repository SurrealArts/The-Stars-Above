using Microsoft.Xna.Framework;
using StarsAbove.Buffs.VirtuesEdge;
using StarsAbove.Effects;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
namespace StarsAbove.Projectiles.VirtuesEdge
{
    public class VirtueVFXVoid : ModProjectile
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Virtue's Edge");     //The English name of the projectile
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 170;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 3;        //The recording mode
			//DrawOffsetX = 40;
			//DrawOriginOffsetY = 81;
		}

		public override void SetDefaults() {
			Projectile.width = 10;               //The width of projectile hitbox
			Projectile.height = 10;              //The height of projectile hitbox
			Projectile.aiStyle = 1;             //The ai style of the projectile, please reference the source code of Terraria
			Projectile.friendly = false;         //Can the projectile deal damage to enemies?
			Projectile.hostile = false;         //Can the projectile deal damage to the player?
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			Projectile.timeLeft = 120;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			Projectile.alpha = 255;             //The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
			Projectile.light = 0.5f;            //How much light emit around the projectile
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.tileCollide = false;          //Can the projectile collide with tiles?
			Projectile.extraUpdates = 0;            //Set to above 0 if you want the projectile to update multiple time in a frame
			Projectile.hide = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}
		int direction;//0 is right 1 is left
		float rotationStrength = 0.1f;
		bool firstSpawn = true;
		double deg;
		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{

			behindNPCsAndTiles.Add(index);

		}
		public override void AI()
		{
			
			Player player = Main.player[Projectile.owner];
			if (firstSpawn)
            {
				Projectile.ai[1] = MathHelper.ToDegrees((float)Math.Atan2(Main.MouseWorld.Y-player.Center.Y,Main.MouseWorld.X-player.Center.X) - 30);
				Projectile.ai[1] -= 3;
				firstSpawn = false;
            }
			if (player.dead && !player.active)
			{
				Projectile.Kill();
			}
			
			rotationStrength = 0.5f;
			


			deg = Projectile.ai[1] += 1.5f + rotationStrength;

			//Spawn the black hole.

			if (Projectile.timeLeft == 70)
			{
				player.AddBuff(BuffType<CelestialVoidBuff>(), 480);

				player.GetModPlayer<WeaponPlayer>().BlackHolePosition = Projectile.Center;
				player.GetModPlayer<StarsAbovePlayer>().activateBlackHoleShockwaveEffect = true;
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, Mod.Find<ModProjectile>("CelestialVoidVFX").Type, 0, 0f, 0, 0);

				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, Mod.Find<ModProjectile>("CelestialVoidVFX2").Type, 0, 0f, 0, 0);
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, Mod.Find<ModProjectile>("CelestialVoidBack").Type, 0, 0f, 0, 0);

				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, Mod.Find<ModProjectile>("CelestialVoid").Type, Projectile.damage, 0f, player.whoAmI, 0);

			}

			double rad = deg * (Math.PI / 180);
			double dist = 190;

			/*Position the player based on where the player is, the Sin/Cos of the angle times the /
            /distance for the desired distance away from the player minus the projectile's width   /
            /and height divided by two so the center of the projectile is at the right place.     */
			Projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
			Projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;

			//Projectile.rotation = Vector2.Normalize(Main.player[Projectile.owner].Center - Projectile.Center).ToRotation() + MathHelper.ToRadians(0f);
			//Projectile.rotation = MathHelper.ToRadians(90f);
			Projectile.rotation = MathHelper.ToRadians(0f);

		}

		public override bool PreDraw(ref Color lightColor)
		{
			default(LargeBlueTrail).Draw(Projectile);

			return true;
		}

	}
}

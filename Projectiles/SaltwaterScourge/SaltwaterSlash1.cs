using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace StarsAbove.Projectiles.SaltwaterScourge
{
    public class SaltwaterSlash1 : ModProjectile
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Saltwater Scourge");     //The English name of the projectile
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;        //The recording mode
			//DrawOffsetX = 40;
			//DrawOriginOffsetY = 81;
		}

		public override void SetDefaults() {
			Projectile.width = 170;               //The width of projectile hitbox
			Projectile.height = 170;              //The height of projectile hitbox
			Projectile.aiStyle = 1;             //The ai style of the projectile, please reference the source code of Terraria
			Projectile.friendly = true;         //Can the projectile deal damage to enemies?
			Projectile.hostile = false;         //Can the projectile deal damage to the player?
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = -1;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			Projectile.timeLeft = 18;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			Projectile.alpha = 0;             //The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
			Projectile.light = 0.5f;            //How much light emit around the projectile
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.tileCollide = false;          //Can the projectile collide with tiles?
			Projectile.extraUpdates = 0;            //Set to above 0 if you want the projectile to update multiple time in a frame

			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}
		int direction;//0 is right 1 is left
		float rotationStrength = 0.1f;
		bool firstSpawn = true;
		double deg;
		public override void AI()
		{
			
			Player player = Main.player[Projectile.owner];
			if (firstSpawn)
            {
				Projectile.ai[1] = MathHelper.ToDegrees((float)Math.Atan2(Main.MouseWorld.Y-player.Center.Y,Main.MouseWorld.X-player.Center.X) - 30);
				firstSpawn = false;
            }
			if (player.dead && !player.active)
			{
				Projectile.Kill();
			}
			if (Projectile.timeLeft > 12)
			{
				rotationStrength += 0.9f;
				Projectile.scale += 0.03f;
			}
			else
			{
				Projectile.scale -= 0.03f;
				rotationStrength -= 1.2f;
				if (rotationStrength < -12f)
				{
					rotationStrength = -12f;
				}
				Projectile.alpha += 12;
			}



			deg = Projectile.ai[1] += 12f + rotationStrength;

			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (player.Center - Projectile.Center).ToRotation() + MathHelper.PiOver2);

			double rad = deg * (Math.PI / 180);
			double dist = 48;

			/*Position the player based on where the player is, the Sin/Cos of the angle times the /
            /distance for the desired distance away from the player minus the projectile's width   /
            /and height divided by two so the center of the projectile is at the right place.     */
			Projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
			Projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;

			Projectile.rotation = Vector2.Normalize(Main.player[Projectile.owner].Center - Projectile.Center).ToRotation() + MathHelper.ToRadians(0f);

			
			
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			for (int d = 0; d < 8; d++)
			{
				Dust.NewDust(target.Center, 0, 0, 219, Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5), 150, default(Color), 0.4f);

			}
			Player player = Main.player[Projectile.owner];

			target.AddBuff(BuffID.OnFire, 120);
			player.AddBuff(BuffID.Swiftness, 240);

			if (!target.active)
			{
				int k = Item.NewItem(target.GetSource_DropAsItem(), (int)target.position.X, (int)target.position.Y, target.width, target.height, ItemID.SilverCoin, 15, false);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(21, -1, -1, null, k, 1f);
				}
			}

			base.OnHitNPC(target, damage, knockback, crit);
		}


	}
}

﻿
using Microsoft.Xna.Framework;
using StarsAbove.Buffs.BlackSilence;
using StarsAbove.Buffs.CatalystMemory;
using StarsAbove.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace StarsAbove.Projectiles.BuryTheLight
{
    public class BuryTheLightSheathe : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Aurum Sheath");
			Main.projFrames[Projectile.type] = 2;
		}

		public override void SetDefaults()
		{
			
			AIType = 0;
			
			Projectile.width = 70;
			Projectile.height = 170;
			Projectile.minion = false;
			Projectile.minionSlots = 0f;
			Projectile.timeLeft = 240;
			Projectile.penetrate = -1;
			Projectile.hide = false;
			Projectile.alpha = 0;
			Projectile.netImportant = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;

			DrawOriginOffsetY = 20;
		}
		bool firstSpawn = true;
		int newOffsetY;
		float spawnProgress;
		bool dustSpawn = true;

		float rotationStrength = 0.1f;
		double deg;

		bool startSound = true;
		bool endSound = false;

		public override bool PreAI()
        {
			Player player = Main.player[Projectile.owner];
			return true;
		}
		public override void AI()
		{
			Player projOwner = Main.player[Projectile.owner];
			Projectile.scale = 1f;

			if (firstSpawn)
			{
				
				firstSpawn = false;
			}
			

			if (Projectile.spriteDirection == 1)
			{//Adjust when facing the other direction

				Projectile.ai[1] = 280;
			}
			else
			{

				Projectile.ai[1] = 260;
			}



			deg = Projectile.ai[1];
			double rad = deg * (Math.PI / 180);
			double dist = 10;

			/*Position the player based on where the player is, the Sin/Cos of the angle times the /
            /distance for the desired distance away from the player minus the projectile's width   /
            /and height divided by two so the center of the projectile is at the right place.     */
			Projectile.position.X = projOwner.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
			Projectile.position.Y = projOwner.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;

			

			if (Projectile.spriteDirection == 1)
			{//Adjust when facing the other direction

				Projectile.rotation = Vector2.Normalize(Main.player[Projectile.owner].Center - Projectile.Center).ToRotation() + MathHelper.ToRadians(180f);
			}
			else
			{

				Projectile.rotation = Vector2.Normalize(Main.player[Projectile.owner].Center - Projectile.Center).ToRotation() + MathHelper.ToRadians(0f);
			}
			
			Projectile.ai[0]++;

			if ((projOwner.dead && !projOwner.active) || !projOwner.GetModPlayer<WeaponPlayer>().BuryTheLightHeld)
			{//Disappear when player dies
				Projectile.Kill();
			}
			

			if(projOwner.ownedProjectileCounts[ProjectileType<BuryTheLightSwing1>()] >= 1 || projOwner.ownedProjectileCounts[ProjectileType<BuryTheLightSwing2>()] >= 1)
            {

				Projectile.frame = 1;
            }
			else
            {
				//Arms will hold the weapon.
				/*projOwner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (projOwner.Center -
					new Vector2(Projectile.Center.X + (projOwner.velocity.X * 0.05f), Projectile.Center.Y + (projOwner.velocity.Y * 0.05f))
					).ToRotation() + MathHelper.PiOver2);*/
				projOwner.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, (projOwner.Center -
					new Vector2(Projectile.Center.X + (projOwner.velocity.X * 0.05f) + 5, Projectile.Center.Y + (projOwner.velocity.Y * 0.05f))
					).ToRotation() + MathHelper.PiOver2);
				Projectile.alpha -= 40;

				//projOwner.heldProj = Projectile.whoAmI;//The projectile draws in front of the player.
				Projectile.frame = 0;
            }

			
			

			


			Projectile.timeLeft = 10;//The prjoectile doesn't time out.





			//Orient projectile
			Projectile.direction = projOwner.direction;
			Projectile.spriteDirection = Projectile.direction;
			//Projectile.rotation += projOwner.velocity.X * 0.05f; //Rotate in the direction of the user when moving


			

			//Cap alpha
			/*if (Projectile.alpha < 0)
			{
				Projectile.alpha = 0;
			}
			if (Projectile.alpha > 255)
			{
				Projectile.alpha = 255;
			}*/
			if (spawnProgress > 1f)
			{
				spawnProgress = 1f;
			}
			if (spawnProgress < 0f)
			{
				spawnProgress = 0f;
			}//Capping variables (there is 100% a better way to do this!)
		}
		private void Visuals()
		{
			

			

			
		}
		public override void Kill(int timeLeft)
		{
			

		}

	}
}

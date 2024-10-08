﻿using Microsoft.Xna.Framework;
using StarsAbove.Buffs;
using StarsAbove.Buffs.StellarNovas;
using StarsAbove.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace StarsAbove.Projectiles.StellarNovas.GuardiansLight
{
    public class NovaBombExplosion : ModProjectile
	{
		public override void SetStaticDefaults() {
			
		}

		public override void SetDefaults() {
			Projectile.width = 650;
			Projectile.height = 650;
			Projectile.aiStyle = 0;
			Projectile.timeLeft = 1;
			Projectile.penetrate = -1;
			Projectile.scale = 1f;
			Projectile.alpha = 255;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			
			Projectile.tileCollide = false;
			Projectile.hide = true;
			Projectile.netUpdate = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;

		}

		
		public override void AI() {
			Player projOwner = Main.player[Projectile.owner];
			Lighting.AddLight(Projectile.Center, TorchID.Purple);


		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.AddBuff(BuffID.ShadowFlame, 60 * 10);
            base.OnHitNPC(target, hit, damageDone);
        }
        public override void OnKill(int timeLeft)
		{
			Player projOwner = Main.player[Projectile.owner];
			projOwner.GetModPlayer<StarsAbovePlayer>().screenShakeTimerGlobal = -90;
			//Boom
			SoundEngine.PlaySound(StarsAboveAudio.SFX_VoidExplosion, Projectile.Center);
			
			
			for (int d = 0; d < 50; d++)
			{
				Dust.NewDust(Projectile.Center, 0, 0, DustID.GemAmethyst, 0f + Main.rand.Next(-10, 10), 0f + Main.rand.Next(-5, 5), 150, default(Color), 1.5f);
			}
			for (int d = 0; d < 54; d++)
			{
				Dust.NewDust(Projectile.Center, 0, 0, DustID.FireworkFountain_Pink, 0f + Main.rand.Next(-25, 25), 0f + Main.rand.Next(-25, 25), 150, default(Color), 1.5f);
			}
			for (int d = 0; d < 54; d++)
			{
				Dust.NewDust(Projectile.Center, 0, 0, DustID.Shadowflame, 0f + Main.rand.Next(-25, 25), 0f + Main.rand.Next(-25, 25), 150, default(Color), 2.5f);
			}
			float dustAmount = 120f;
			for (int i = 0; (float)i < dustAmount; i++)
			{
				Vector2 spinningpoint5 = Vector2.UnitX * 0f;
				spinningpoint5 += -Vector2.UnitY.RotatedBy((float)i * ((float)Math.PI * 2f / dustAmount)) * new Vector2(4f, 4f);
				spinningpoint5 = spinningpoint5.RotatedBy(Projectile.velocity.ToRotation());
				int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemAmethyst);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].position = Projectile.Center + spinningpoint5;
				Main.dust[dust].velocity = Projectile.velocity * 0f + spinningpoint5.SafeNormalize(Vector2.UnitY) * 30f;
			}
			for (int i = 0; (float)i < dustAmount; i++)
			{
				Vector2 spinningpoint5 = Vector2.UnitX * 0f;
				spinningpoint5 += -Vector2.UnitY.RotatedBy((float)i * ((float)Math.PI * 2f / dustAmount)) * new Vector2(4f, 4f);
				spinningpoint5 = spinningpoint5.RotatedBy(Projectile.velocity.ToRotation());
				int dust = Dust.NewDust(Projectile.Center, 0, 0, DustID.GemAmethyst);
				Main.dust[dust].scale = 2f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].position = Projectile.Center + spinningpoint5;
				Main.dust[dust].velocity = Projectile.velocity * 0f + spinningpoint5.SafeNormalize(Vector2.UnitY) * 40f;
			}
			
			Vector2 position = Main.rand.NextVector2FromRectangle(Projectile.Hitbox);
			ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
			
			for (int i = 0; (float)i < 40; i++)
			{
				particleOrchestraSettings.PositionInWorld = (new Vector2(Projectile.Center.X + Main.rand.Next(-420, 420), Projectile.Center.Y + Main.rand.Next(-420, 420)));

				ParticleOrchestrator.RequestParticleSpawn(false, ParticleOrchestraType.PrincessWeapon, particleOrchestraSettings, Projectile.owner);

			}
			base.OnKill(timeLeft);
        }
    }
}

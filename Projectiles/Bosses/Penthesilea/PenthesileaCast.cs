﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarsAbove.Buffs.CarianDarkMoon;
using Terraria;using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace StarsAbove.Projectiles.Bosses.Penthesilea
{
	public class PenthesileaCast : ModProjectile
	{
		public override void SetStaticDefaults() {
			// DisplayName.SetDefault("Tsukiyomi, the First Starfarer");
			Main.projFrames[Projectile.type] = 1;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;        //The recording mode
		}

		public override void SetDefaults() {
			Projectile.width = 72;
			Projectile.height = 120;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.scale = 1f;
			Projectile.alpha = 0;
			Projectile.timeLeft = 160;
			Projectile.hide = false;
			Projectile.ownerHitCheck = false;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
		}
        public override Color? GetAlpha(Color lightColor)
        {
            //return Color.White;
            return new Color(255, 255, 255, 0) * (1f - Projectile.alpha / 255f);
        }

        public override void AI() {

			//DrawOffsetX = -34;
			//DrawOriginOffsetY = -16;

			Projectile.timeLeft = 10;
			Projectile.ai[0]--;
			if(Projectile.ai[0] <= 0)
            {
				Projectile.Kill();
            }
			//Do something based on AI? Arcipluvian Arcana?
		}

	}
}

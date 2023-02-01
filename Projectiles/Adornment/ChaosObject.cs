using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;

namespace StarsAbove.Projectiles.Adornment
{
	public class ChaosObject : ModProjectile
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Adornment of the Chaotic God");     //The English name of the projectile
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 3;        //The recording mode
		}

		public override void SetDefaults() {
			Projectile.width = 36;               //The width of projectile hitbox
			Projectile.height = 36;              //The height of projectile hitbox
			Projectile.aiStyle = 0;             //The ai style of the projectile, please reference the source code of Terraria
			Projectile.friendly = false;         //Can the projectile deal damage to enemies?
			Projectile.hostile = false;         //Can the projectile deal damage to the player?
			Projectile.DamageType = DamageClass.Summon;           //Is the projectile shoot by a ranged weapon?
			Projectile.penetrate = 1;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			Projectile.timeLeft = 600;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			Projectile.alpha = 0;             //The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
			Projectile.light = 0.5f;            //How much light emit around the projectile
			Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			Projectile.tileCollide = false;          //Can the projectile collide with tiles?
			Projectile.extraUpdates = 1;            //Set to above 0 if you want the projectile to update multiple time in a frame
		}
		private float travelRadians;//Thank you to absoluteAquarian for this code!
		public Vector2 MidPoint;
		public float Radius;
		public bool RotateClockwise;
		public Vector2 InitialDirection;
		public Vector2 Target;
		

		public override void AI()
        {
			//

			travelRadians += (RotateClockwise ? 1 : -1) * MathHelper.ToRadians(0.3f * 6f);

			Projectile.Center = MidPoint + InitialDirection.RotatedBy(travelRadians) * Radius;

			
			float rotationsPerSecond = 2f;
			
			Projectile.rotation += (RotateClockwise ? 1 : -1) * MathHelper.ToRadians(rotationsPerSecond * 6f);



			if (Vector2.Distance(Projectile.Center, Target) < 5f)
            {
				
				Projectile.Kill();
            }

			base.AI();
        }


        public override bool OnTileCollide(Vector2 oldVelocity) {
			//If collide with tile, reduce the penetrate.
			//So the projectile can reflect at most 5 times
			Projectile.penetrate--;
			if (Projectile.penetrate <= 0) {
				Projectile.Kill();
			}
			else {
				Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
				SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
				if (Projectile.velocity.X != oldVelocity.X) {
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y) {
					Projectile.velocity.Y = -oldVelocity.Y;
				}
			}
			return false;
		}

		public override bool PreDraw(ref Color lightColor) {
			//Redraw the projectile with the color not influenced by light
			//Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Width() * 0.5f, Projectile.height * 0.5f);
			//for (int k = 0; k < Projectile.oldPos.Length; k++) {
			//	Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
			//	Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
			//	Main.EntitySpriteDraw((Texture2D)TextureAssets.Projectile[Projectile.type], drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			//}
			default(Effects.WhiteTrail).Draw(Projectile);
			return true;
		}
		
		public override void Kill(int timeLeft)
		{
			Player projOwner = Main.player[Projectile.owner];
			// This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
			Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
			
			// Play explosion sound
			
			// Large Smoke Gore spawn
			

		}
	}
}
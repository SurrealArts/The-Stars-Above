using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StarsAbove.Systems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace StarsAbove.Projectiles.Magic.Ozma
{
    public class OzmaAttack5 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ozma Ascendant");     //The English name of the projectile
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;        //The recording mode
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 200;               //The width of projectile hitbox
            Projectile.height = 200;              //The height of projectile hitbox
            Projectile.aiStyle = 0;             //The ai style of the projectile, please reference the source code of Terraria
            Projectile.friendly = false;         //Can the projectile deal damage to enemies?
            Projectile.hostile = false;         //Can the projectile deal damage to the player?
            Projectile.scale = 1f;
            Projectile.penetrate = -1;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
            Projectile.timeLeft = 20;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.alpha = 255;             //The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
            Projectile.light = 2f;            //How much light emit around the projectile
            Projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
            Projectile.tileCollide = false;          //Can the projectile collide with tiles?
            Projectile.extraUpdates = 0;            //Set to above 0 if you want the projectile to update multiple time in a frame
            Projectile.hide = false;
        }
        float rotationSpeed = 10f;
        float expandStrength = 0f;


        public override void AI()
        {






            Projectile.alpha -= 20;
            expandStrength += 0.001f;
            Projectile.scale += 0.001f;



            float rotationsPerSecond = rotationSpeed;
            rotationSpeed = 0.4f;
            bool rotateClockwise = true;
            //The rotation is set here

            Projectile.rotation += (rotateClockwise ? 1 : -1) * MathHelper.ToRadians(rotationsPerSecond * 6f);

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Lighting.AddLight(new Vector2(Projectile.Center.X, Projectile.Center.Y), 81 * 0.001f, 194 * 0.001f, 58 * 0.001f);//Vanilla code adapted
            for (int i = 0; i < 1; i++)
            {
                int num7 = 16;
                float num9 = 6f;
                float num8 = (float)(Math.Cos(Main.GlobalTimeWrappedHourly % 2.40000009536743 / 2.40000009536743 * 6.28318548202515) / 5 + 0.5);
                float amount1 = 0.5f;
                float num10 = 0.0f;
                float addY = 0f;
                float addHeight = 0f;
                SpriteEffects spriteEffects = SpriteEffects.None;
                Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
                Vector2 vector2_3 = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Width() / 2, Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Height() / 1 / 2);
                Vector2 position1 = Projectile.Center - Main.screenPosition - new Vector2(texture.Width, texture.Height / 1) * Projectile.scale / 2f + vector2_3 * Projectile.scale + new Vector2(0.0f, addY + addHeight + 0);
                Color color2 = new Color(255, 255, 255, 150);
                Rectangle r = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Frame(1, 1, 0, 0);
                for (int index2 = 0; index2 < num7; ++index2)
                {
                    Color newColor2 = color2;
                    Color color3 = Projectile.GetAlpha(newColor2) * (0.85f - num8);
                    Vector2 position2 = new Vector2(Projectile.Center.X, Projectile.Center.Y) + ((float)(index2 / (double)num7 * 6.28318548202515) + Projectile.rotation + num10).ToRotationVector2() * (float)(4.0 * (double)num8 + 2.0) - Main.screenPosition - new Vector2(texture.Width, texture.Height / 1) * Projectile.scale / 2f + vector2_3 * Projectile.scale + new Vector2(0.0f, addY + addHeight + 0);
                    Main.spriteBatch.Draw((Texture2D)Terraria.GameContent.TextureAssets.Projectile[Projectile.type], position2, new Microsoft.Xna.Framework.Rectangle?(r), color3, Projectile.rotation, vector2_3, Projectile.scale * (1.05f + expandStrength), spriteEffects, 0.0f);
                }
            }
            return base.PreDraw(ref lightColor);
        }
        public override void OnKill(int timeLeft)
        {
            // This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
            //Collision.HitTiles(Projectile.position + Projectile.velocity, Projectile.velocity, Projectile.width, Projectile.height);
            Player projOwner = Main.player[Projectile.owner];
            projOwner.GetModPlayer<StarsAbovePlayer>().screenShakeTimerGlobal = -90;
            // Play explosion sound
            SoundEngine.PlaySound(StarsAboveAudio.SFX_HolyStab, Projectile.Center);
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, Mod.Find<ModProjectile>("fastRadiate").Type, 0, 0f, 0, 0);
            for (int i = 0; i < 100; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.Center, 0, 0, DustID.LifeDrain, Main.rand.NextFloat(-15, 15), Main.rand.NextFloat(-15, 15), 100, default, 1f);
                Main.dust[dustIndex].noGravity = true;

                dustIndex = Dust.NewDust(Projectile.Center, 0, 0, 91, Main.rand.NextFloat(-10, 10), Main.rand.NextFloat(-10, 10), 100, default, 0.6f);
                Main.dust[dustIndex].velocity *= 3f;
            }



        }
        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
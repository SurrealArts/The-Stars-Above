
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace StarsAbove.Projectiles.Celestial.BlackSilence
{
    public class ZelkovaSlash3 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Gloves of the Black Silence");     //The English name of the projectile
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;        //The recording mode
                                                                        //DrawOffsetX = 40;
                                                                        //DrawOriginOffsetY = 81;
        }

        public override void SetDefaults()
        {
            Projectile.width = 120;               //The width of projectile hitbox
            Projectile.height = 120;              //The height of projectile hitbox
            Projectile.aiStyle = 1;             //The ai style of the projectile, please reference the source code of Terraria
            Projectile.friendly = true;         //Can the projectile deal damage to enemies?
            Projectile.hostile = false;         //Can the projectile deal damage to the player?
            Projectile.DamageType = GetInstance<Systems.CelestialDamageClass>();
            Projectile.penetrate = -1;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
            Projectile.timeLeft = 25;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
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
        double rad;
        public override void AI()
        {

            Player player = Main.player[Projectile.owner];
            Player p = Main.player[Projectile.owner];
            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (player.Center - Projectile.Center).ToRotation() + MathHelper.PiOver2);

            //player.itemTime = 10;
            //player.itemAnimation = 10;
            if (firstSpawn)
            {
                Projectile.ai[1] = MathHelper.ToDegrees((float)Math.Atan2(Main.MouseWorld.Y - player.Center.Y, Main.MouseWorld.X - player.Center.X) + 30);
                //Projectile.ai[1] = 180;
                firstSpawn = false;
            }
            //projectile.timeLeft = 10;

            //if (player.dead && !player.active || player.GetModPlayer<StarsAbovePlayer>().kroniicHeld < 0)
            if (player.dead && !player.active)
            {
                Projectile.Kill();
            }
            //projectile.spriteDirection = projectile.velocity.X > 0 ? 1 : -1;

            if (Projectile.timeLeft > 20)
            {
                rotationStrength += 0.8f;
                Projectile.scale += 0.01f;
            }
            else
            {
                Projectile.scale -= 0.01f;
                rotationStrength -= 1.2f;
                if (rotationStrength < -12f)
                {
                    rotationStrength = -12f;
                }
                Projectile.alpha += 5;
            }



            deg = Projectile.ai[1] -= 12f + rotationStrength; //The degrees, you can multiply projectile.ai[1] to make it orbit faster, may be choppy depending on the value



            rad = deg * (Math.PI / 180); //Convert degrees to radians
            double dist = 48; //Distance away from the player

            /*Position the player based on where the player is, the Sin/Cos of the angle times the /
            /distance for the desired distance away from the player minus the projectile's width   /
            /and height divided by two so the center of the projectile is at the right place.     */
            Projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
            Projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - Projectile.height / 2;



            //Increase the counter/angle in degrees by 1 point, you can change the rate here too, but the orbit may look choppy depending on the value




            //float rotationsPerSecond = rotationSpeed;
            //rotationSpeed -= 1f;
            //bool rotateClockwise = true;
            //The rotation is set here
            Projectile.rotation = Vector2.Normalize(Main.player[Projectile.owner].Center - Projectile.Center).ToRotation() + MathHelper.ToRadians(0f);
            //projectile.rotation = projectile.velocity.ToRotation();


        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            for (int d = 0; d < 8; d++)
            {
                Dust.NewDust(target.Center, 0, 0, 219, Main.rand.NextFloat(-5, 5), Main.rand.NextFloat(-5, 5), 150, default, 0.4f);

            }
            Player player = Main.player[Projectile.owner];
            if (target.CanBeChasedBy() && !target.SpawnedFromStatue)
            {
                Rectangle textPos = new Rectangle((int)player.position.X, (int)player.position.Y - 20, player.width, player.height);
                CombatText.NewText(textPos, new Color(49, 234, 63, 240), $"{Math.Min(damageDone / 50, 5)}", false, false);
                player.statLife += Math.Min(damageDone / 50, 5);
            }




        }


    }
}

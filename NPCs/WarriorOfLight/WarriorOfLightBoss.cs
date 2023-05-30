
using Microsoft.Xna.Framework;
using StarsAbove.NPCs.OffworldNPCs;
//using StarsAbove.Projectiles.Bosses.WarriorOfLight;
using StarsAbove.Projectiles.Bosses.Vagrant;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using static Terraria.ModLoader.ModContent;

using static StarsAbove.NPCs.AttackLibrary.AttackLibrary;
using StarsAbove.Buffs.Boss;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using StarsAbove.Buffs;
using StarsAbove.Utilities;

namespace StarsAbove.NPCs.WarriorOfLight
{
	[AutoloadBossHead]

	public class WarriorOfLightBoss : ModNPC
	{

		private int portalFrame
		{
			get => (int)NPC.localAI[0];
			set => NPC.localAI[0] = value;
		}
		public static readonly int arenaWidth = (int)(1.2f * 1000);
		public static readonly int arenaHeight = (int)(1.2f * 600);

		


		// Our texture is 36x36 with 2 pixels of padding vertically, so 38 is the vertical spacing.
		// These are for our benefit and the numbers could easily be used directly in the code below, but this is how we keep code organized.
		private enum Frame
		{
			Empty,
			Idle1,
			Idle2,
			Idle3,
			Idle4,
			Idle5,
		}

		// These are reference properties. One, for example, lets us write AI_State as if it's NPC.ai[0], essentially giving the index zero our own name.
		// Here they help to keep our AI code clear of clutter. Without them, every instance of "AI_State" in the AI code below would be "npc.ai[0]", which is quite hard to read.
		// This is all to just make beautiful, manageable, and clean code.
		public ref float AI_State => ref NPC.ai[0];
		public ref float AI_Timer => ref NPC.ai[1];

		public ref float AI_RotationNumber => ref NPC.ai[2];//Where the boss is in its rotation.
		public ref float AI_CastTimerMax => ref NPC.ai[3];//Continually ticks down from the value given by the attack; at zero, the cast is finished.
		public ref float AI_CastTimer => ref NPC.localAI[3];//Continually ticks down from the value given by the attack; at zero, the cast is finished.


		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("WarriorOfLight, the Burnished King");
			
			Main.npcFrameCount[NPC.type] = 6; // make sure to set this for your modnpcs.

			// Specify the debuffs it is immune to
			/*NPCID.Sets.DebuffImmunitySets.Add(Type, new NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[] {
					BuffID.Poisoned // This NPC will be immune to the Poisoned debuff.
				}
			});*/
			
			
			NPCID.Sets.MPAllowedEnemies[NPC.type] = true;
			// By default enemies gain health and attack if hardmode is reached. this NPC should not be affected by that
			NPCID.Sets.DontDoHardmodeScaling[Type] = true;
			// Enemies can pick up coins, let's prevent it for this NPC
			NPCID.Sets.CantTakeLunchMoney[Type] = true;
			// Automatically group with other bosses
			NPCID.Sets.BossBestiaryPriority.Add(Type);

			NPCID.Sets.NPCBestiaryDrawModifiers bestiaryData = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true // Hides this NPC from the bestiary
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, bestiaryData);

		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			
			

		}
		public override void SetDefaults()
		{
			NPC.boss = true;
			NPC.lifeMax = 514000;
			NPC.damage = 0;
			NPC.defense = 35;
			NPC.knockBackResist = 0f;
			NPC.width = 200;
			NPC.height = 200;
			NPC.scale = 1f;
			NPC.npcSlots = 1f;
			NPC.aiStyle = -1;
			NPC.lavaImmune = true;
			NPC.noGravity = false;
			NPC.noTileCollide = false;
			NPC.value = 0f;
			DrawOffsetY = 0;

			NPC.HitSound = SoundID.NPCHit54;
			//NPC.DeathSound = SoundID.NPCDeath52;


			Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Boss/WarriorOfLight/ToTheEdgeInstrumental");
			SpawnModBiomes = new int[1] { ModContent.GetInstance<Biomes.SeaOfStarsBiome>().Type };
			NPC.netAlways = true;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0f;
		}
		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
		{
			NPC.lifeMax = (int)(NPC.lifeMax * bossAdjustment * balance);
			//NPC.defense *= numPlayers * 5;
		}
		public override void BossLoot(ref string name, ref int potionType)
        {
			
			potionType = ItemID.None;


			base.BossLoot(ref name, ref potionType);
        }
        public override bool CheckDead()
		{
			if (NPC.ai[0] != (float)ActionState.Dying) //If the boss is defeated, but the death animation hasn't played yet, play the death animation.
			{
				NPC.ai[0] = (float)ActionState.Dying; //Flag boss as "dying"
				NPC.damage = 0; //Disable contact damage
				NPC.life = NPC.lifeMax; //HP set to max
				NPC.dontTakeDamage = true; //Invulnerable
				NPC.netUpdate = true; //Sync to clients
				return false; //Boss isn't dead yet!
			}
			return true;
		}

		public override void AI()
        {
			if (!Main.dedServ)
			{
				portalFrame++;
				portalFrame %= 6 * Main.projFrames[ProjectileID.PortalGunGate];
			}
			var modPlayer = Main.LocalPlayer.GetModPlayer<StarsAbovePlayer>();
            var bossPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            bossPlayer.WarriorOfLightBarActive = true;
            NPC.velocity *= 0.98f; //So the dashes don't propel the boss away

            Player P = Main.player[NPC.target];//THIS IS THE BOSS'S MAIN TARGET
            FindTargetPlayer();
            IfAllPlayersAreDead();
            if (NPC.ai[0] == (float)ActionState.Dying)
            {
                DeathAnimation();//The boss is in its dying animation. No other AI code will run.

                return;
            }
            switch (AI_State)
            {
                case (float)ActionState.Spawning:
                    SpawnAnimation();
                    break;
                case (float)ActionState.PersistentCast:
                    PersistentCast();
                    break;
                case (float)ActionState.Casting:
                    Casting();
                    break;
                case (float)ActionState.Idle:
                    Idle();
                    break;
            }
			//if AI_Timer is less than 120, it isn't casting- let's try changing phases here
			if(AI_Timer < 120 && AI_State == (float)ActionState.Idle)
            {

            }
            else if (AI_Timer >= 120) //An attack is active.
            {

                //Attacks begin here.
                if (AI_RotationNumber == 0)
                {
                    //
                    IvoryStake1(P, NPC);
                    return;
                }
				else if (AI_RotationNumber == 1)
				{
					//
					RightwardRend(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 2)
				{
					//
					OuterAgony(P, NPC);
					return;
				}
				else
                {
                    AI_RotationNumber = 0;
                    return;
                }


            }
			//if (Main.netMode != NetmodeID.Server && Main.myPlayer == Main.LocalPlayer.whoAmI) { Main.NewText(Language.GetTextValue($"Rotation Number {AI_RotationNumber}"), 220, 100, 247); }
			//if (Main.netMode != NetmodeID.Server && Main.myPlayer == Main.LocalPlayer.whoAmI) { Main.NewText(Language.GetTextValue($"Timer {AI_Timer}"), 220, 100, 247); }
			//if (Main.netMode != NetmodeID.Server && Main.myPlayer == Main.LocalPlayer.whoAmI) { Main.NewText(Language.GetTextValue($"State {AI_State}"), 220, 100, 247); }
			
			//Animate the border.
			
		}

        private void FindTargetPlayer()
        {
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest(true);
            }
        }

        private void IfAllPlayersAreDead()
        {
            if (Main.player[NPC.target].dead)
            {
                //Leave if all players are dead.
                Vector2 vector8 = new Vector2(NPC.Center.X, NPC.Center.Y);
                for (int d = 0; d < 100; d++)
                {
                    Dust.NewDust(vector8, 0, 0, 269, 0f + Main.rand.Next(-40, 40), 0f + Main.rand.Next(-40, 40), 150, default(Color), 1.5f);
                }
                for (int d = 0; d < 65; d++)
                {
                    Dust.NewDust(vector8, 0, 0, 21, 0f + Main.rand.Next(-45, 45), 0f + Main.rand.Next(-45, 45), 150, default(Color), 1.5f);
                }
                for (int d = 0; d < 35; d++)
                {
                    Dust.NewDust(vector8, 0, 0, 50, 0f + Main.rand.Next(-45, 45), 0f + Main.rand.Next(-45, 45), 150, default(Color), 1.5f);
                }
                for (int d = 0; d < 35; d++)
                {
                    Dust.NewDust(vector8, 0, 0, 55, 0f + Main.rand.Next(-45, 45), 0f + Main.rand.Next(-45, 45), 150, default(Color), 1.5f);
                }
                NPC.active = false;


            }
        }

        // Here in FindFrame, we want to set the animation frame our npc will use depending on what it is doing.
        // We set npc.frame.Y to x * frameHeight where x is the xth frame in our spritesheet, counting from 0. For convenience, we have defined a enum above.
        public override void FindFrame(int frameHeight)
		{
			// This makes the sprite flip horizontally in conjunction with the npc.direction.
			//NPC.spriteDirection = NPC.direction;

			//If a projectile that replaces the boss sprite appears, hide the boss sprite.
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile other = Main.projectile[i];

				if (other.active && (other.type == ModContent.ProjectileType<WarriorOfLightSwordSprite>()
					|| other.type == ModContent.ProjectileType<WarriorOfLightSwordAttackSprite>()
					|| other.type == ModContent.ProjectileType<WarriorOfLightLoseSwordSprite>()
					|| other.type == ModContent.ProjectileType<WarriorOfLightCastSprite>()
					|| other.type == ModContent.ProjectileType<VagrantSwordSprite>()
					&& other.alpha < 1))
					
				{
					if(other.alpha > 1)
                    {
						if (NPC.frame.Y == (int)Frame.Empty)
						{
							NPC.frame.Y = (int)Frame.Idle1 * frameHeight;
						}
					}
					else
                    {
						NPC.frame.Y = (int)Frame.Empty * frameHeight;
					}
					
					//NPC.alpha = 250;
					return;
				}
			}
			
			//NPC.alpha = 0;
			//NPC.alpha -= 100;//The sprite should appear again after being replaced.
			// For the most part, our animation matches up with our states.
			switch (AI_State)
			{
				case (float)ActionState.Idle:
					NPC.frameCounter++;
					if (NPC.frameCounter < 10)
					{
						NPC.frame.Y = (int)Frame.Idle1 * frameHeight;
					}
					else if (NPC.frameCounter < 20)
					{
						NPC.frame.Y = (int)Frame.Idle2 * frameHeight;
					}
					else if (NPC.frameCounter < 30)
					{
						NPC.frame.Y = (int)Frame.Idle3 * frameHeight;
					}
					else if (NPC.frameCounter < 40)
					{
						NPC.frame.Y = (int)Frame.Idle4 * frameHeight;
					}
					else if (NPC.frameCounter < 50)
					{
						NPC.frame.Y = (int)Frame.Idle5 * frameHeight;
					}
					else if (NPC.frameCounter < 60)
					{
						NPC.frame.Y = (int)Frame.Idle6 * frameHeight;
					}
					else
					{
						NPC.frameCounter = 0;
					}

					break;
				case (float)ActionState.Casting:
					NPC.frameCounter++;
					if (NPC.frameCounter < 10)
					{
						NPC.frame.Y = (int)Frame.Idle1 * frameHeight;
					}
					else if (NPC.frameCounter < 20)
					{
						NPC.frame.Y = (int)Frame.Idle2 * frameHeight;
					}
					else if (NPC.frameCounter < 30)
					{
						NPC.frame.Y = (int)Frame.Idle3 * frameHeight;
					}
					else if (NPC.frameCounter < 40)
					{
						NPC.frame.Y = (int)Frame.Idle4 * frameHeight;
					}
					else if (NPC.frameCounter < 50)
					{
						NPC.frame.Y = (int)Frame.Idle5 * frameHeight;
					}
					else if (NPC.frameCounter < 60)
					{
						NPC.frame.Y = (int)Frame.Idle6 * frameHeight;
					}
					else
					{
						NPC.frameCounter = 0;
					}
					break;
			}
		}

		public override bool? CanFallThroughPlatforms()
		{


			return false;
		}
		private void DeathAnimation()
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<StarsAbovePlayer>();
			Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/silence");
			NPC.dontTakeDamage = true;
			
			NPC.ai[1] += 1f; // increase our death timer.
							 //npc.velocity = Vector2.UnitY * npc.velocity.Length();
			NPC.velocity.X *= 0.95f; // lose inertia
									 //if expert mode, boss flies up and new phase starts?
									 //npc.velocity.Y = npc.velocity.Y - 0.02f;
			if (NPC.velocity.Y < 0.1f)
			{
				NPC.velocity.Y = NPC.velocity.Y + 0.01f;
			}
			if (NPC.velocity.Y > 0.1f)
			{
				NPC.velocity.Y = NPC.velocity.Y - 0.01f;
			}
			
			if (Main.rand.NextBool(5) && NPC.ai[1] < 20f)
			{
				
				// This dust spawn adapted from the Pillar death code in vanilla.
				for (int dustNumber = 0; dustNumber < 3; dustNumber++)
				{
					Dust dust = Main.dust[Dust.NewDust(NPC.Left, NPC.width, NPC.height / 2, DustID.FireworkFountain_Red, 0f, 0f, 0, default(Color), 0.4f)];
					dust.position = NPC.Center + Vector2.UnitY.RotatedByRandom(4.1887903213500977) * new Vector2(NPC.width, NPC.height) * 0.8f * (0.8f + Main.rand.NextFloat() * 0.2f);
					dust.velocity.X = 0f;
					dust.velocity.Y = -Math.Abs(dust.velocity.Y - (float)dustNumber + NPC.velocity.Y - 4f) * 3f;
					dust.noGravity = true;
					dust.fadeIn = 1f;
					dust.scale = 1f + Main.rand.NextFloat() + (float)dustNumber * 0.3f;
				}
			}

			if (NPC.ai[1] >= 20f)
			{
				//Main.LocalPlayer.GetModPlayer<BossPlayer>().WarriorOfLightCutsceneProgress = 10;
				for (int d = 0; d < 305; d++)
				{
					Dust.NewDust(NPC.Center, 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-45, 45), 0f + Main.rand.Next(-45, 45), 150, default(Color), 1.5f);
				}

				//SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_TheGodsWillNotBeWatching, NPC.Center);
				/*
				if (!NPC.AnyNPCs(NPCType<WarriorOfLightBossPhase2>()))
				{
					int index = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<WarriorOfLightBossPhase2>(), NPC.whoAmI);
					int index2 = NPC.NewNPC(null, (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<WarriorOfLightPhase2WallsNPC>(), NPC.whoAmI);


					if (Main.netMode == NetmodeID.Server && index < Main.maxNPCs)
					{
						NetMessage.SendData(MessageID.SyncNPC, number: index);
					}
					if (Main.netMode == NetmodeID.Server && index < Main.maxNPCs)
					{
						NetMessage.SendData(MessageID.SyncNPC, number: index2);
					}
				}
				/*
				

				DownedBossSystem.downedWarriorOfLight = true;
				*/
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
				}
				

				NPC.life = 0;
				NPC.HitEffect(0, 0);
				NPC.checkDead(); // This will trigger ModNPC.CheckDead the second time, causing the real death.

				
			}
			return;
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{/*
		  * 
			// Do NOT misuse the ModifyNPCLoot and OnKill hooks: the former is only used for registering drops, the latter for everything else
			//Chance for a Prism
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Prisms.BurnishedPrism>(), 4));

			// Add the treasure bag using ItemDropRule.BossBag (automatically checks for expert mode)
			//npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<VagrantBossBag>()));

			// Trophies are spawned with 1/10 chance
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.BossLoot.WarriorOfLightTrophyItem>(), 10));

			// ItemDropRule.MasterModeCommonDrop for the relic
			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Placeable.BossLoot.WarriorOfLightBossRelicItem>()));

			// ItemDropRule.MasterModeDropOnAllPlayers for the pet
			//npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<MinionBossPetItem>(), 4));

			// All our drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
			LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
			LeadingConditionRule ExpertRule = new LeadingConditionRule(new Conditions.IsExpert());

			// Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
			// Boss masks are spawned with 1/7 chance
			//notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<MinionBossMask>(), 7));

			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Prisms.BurnishedPrism>(), 4));

			// Finally add the leading rule
			npcLoot.Add(ExpertRule);
			npcLoot.Add(notExpertRule);*/
			npcLoot.RemoveWhere(rule => true);
		}
		
		private void SpawnAnimation()
		{
			
			//Once the spawn animation is done, change to ActionState.Idle

			//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
			//Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSlamSprite>(), 0, 0, Main.myPlayer);
			
			for (int i = 0; i < Main.maxPlayers; i++)
			{
				Player player = Main.player[i];
				if (player.active && player.Distance(NPC.Center) < 300)
				{
					player.velocity = Vector2.Normalize(NPC.Center - player.Center) * -10f;
				}
			}


			NPC.position.X = Main.player[NPC.target].Center.X - 80;
			NPC.position.Y = Main.player[NPC.target].position.Y-160;
			NPC.netUpdate = true;
			//SoundEngine.PlaySound(StarsAboveAudio.WarriorOfLight_WarriorOfLightIntroQuote, NPC.Center);
			for (int d = 0; d < 130; d++)
			{
				Dust.NewDust(NPC.Center, 0, 0, 0, 0f + Main.rand.Next(-30, 30), 0f + Main.rand.Next(-30, 30), 150, default(Color), 1.5f);
			}
			for (int d = 0; d < 144; d++)
			{
				Dust.NewDust(NPC.Center, 0, 0, 0, 0f + Main.rand.Next(-35, 35), 0f + Main.rand.Next(-35, 35), 150, default(Color), 1.5f);
			}
			for (int d = 0; d < 126; d++)
			{
				Dust.NewDust(NPC.Center, 0, 0, 133, 0f + Main.rand.Next(-36, 36), 0f + Main.rand.Next(-36, 36), 150, default(Color), 1.5f);
			}
			for (int d = 0; d < 130; d++)
			{
				Dust.NewDust(NPC.Center, 0, 0, 7, 0f + Main.rand.Next(-33, 33), 0f + Main.rand.Next(-33, 33), 150, default(Color), 1.5f);
			}
			for (int d = 0; d < 140; d++)
			{
				Dust.NewDust(NPC.Center, 0, 0, 269, 0f + Main.rand.Next(-33, 33), 0f + Main.rand.Next(-33, 33), 150, default(Color), 1.5f);
			}
			for (int d = 0; d < 150; d++)
			{
				Dust.NewDust(NPC.Center, 0, 0, 78, 0f + Main.rand.Next(-35, 35), 0f + Main.rand.Next(-35, 35), 150, default(Color), 1.5f);
			}

			AI_State = (float)ActionState.Idle;
		}
		private void Idle()
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();
			modPlayer.NextAttack = "";
			NPC.direction = (Main.player[NPC.target].Center.X < NPC.Center.X).ToDirectionInt();//Face the target.

			AI_Timer++;//The boss's rotation timer ticks upwards.


		}
		private void Casting()
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();
			NPC.localAI[3]++;
			NPC.direction = (Main.player[NPC.target].Center.X < NPC.Center.X).ToDirectionInt();//Face the target.
			modPlayer.CastTime = (int)NPC.localAI[3];
			modPlayer.CastTimeMax = (int)NPC.ai[3];
		}
		private void PersistentCast()
		{
			var modPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

		}
		//Draw the portal
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			int portalWidth = 48;
			int portalDepth = 18;
			Color color = new Color(181, 43, 43);
			int centerX = (int)NPC.Center.X;
			int centerY = (int)NPC.Center.Y;
			Main.instance.LoadProjectile(ProjectileID.PortalGunGate);
			for (int x = centerX - arenaWidth / 2; x < centerX + arenaWidth / 2; x += portalWidth)
			{
				int frameNum = (portalFrame / 6 + x / portalWidth) % Main.projFrames[ProjectileID.PortalGunGate];
				Rectangle frame = new Rectangle(0, frameNum * (portalWidth + 2), portalDepth, portalWidth);
				Vector2 drawPos = new Vector2(x + portalWidth / 2, centerY - arenaHeight / 2) - Main.screenPosition;
				spriteBatch.Draw((Texture2D)TextureAssets.Projectile[ProjectileID.PortalGunGate], drawPos, frame, color, (float)-Math.PI / 2f, new Vector2(portalDepth / 2, portalWidth / 2), 1f, SpriteEffects.None, 0f);
				drawPos.Y += arenaHeight;
				spriteBatch.Draw((Texture2D)TextureAssets.Projectile[ProjectileID.PortalGunGate], drawPos, frame, color, (float)Math.PI / 2f, new Vector2(portalDepth / 2, portalWidth / 2), 1f, SpriteEffects.None, 0f);
			}
			for (int y = centerY - arenaHeight / 2; y < centerY + arenaHeight / 2; y += portalWidth)
			{
				int frameNum = (portalFrame / 6 + y / portalWidth) % Main.projFrames[ProjectileID.PortalGunGate];
				Rectangle frame = new Rectangle(0, frameNum * (portalWidth + 2), portalDepth, portalWidth);
				Vector2 drawPos = new Vector2(centerX - arenaWidth / 2, y + portalWidth / 2) - Main.screenPosition;
				spriteBatch.Draw((Texture2D)TextureAssets.Projectile[ProjectileID.PortalGunGate], drawPos, frame, color, (float)Math.PI, new Vector2(portalDepth / 2, portalWidth / 2), 1f, SpriteEffects.None, 0f);
				drawPos.X += arenaWidth;
				spriteBatch.Draw((Texture2D)TextureAssets.Projectile[ProjectileID.PortalGunGate], drawPos, frame, color, 0f, new Vector2(portalDepth / 2, portalWidth / 2), 1f, SpriteEffects.None, 0f);
			}
		}
	}
}
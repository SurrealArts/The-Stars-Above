
using Microsoft.Xna.Framework;
using StarsAbove.NPCs.OffworldNPCs;
using StarsAbove.Projectiles.Bosses.Nalhaun;
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
using StarsAbove.Items.BossBags;

namespace StarsAbove.NPCs.Nalhaun
{
	[AutoloadBossHead]

	public class NalhaunBossPhase2 : ModNPC
	{

		public int AttackTimer = 120;

		private enum Frame
		{
			Empty,
			Idle1,
			Idle2,
			Idle3,
			Idle4,
			Idle5,
			Idle6,

			Prep1,
			Prep2,
			Prep3,
			Prep4,
			Prep5,
			Prep6,

			Defeat

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
			// DisplayName.SetDefault("Nalhaun, Ruler of the Hollow World");
			
			Main.npcFrameCount[NPC.type] = 7; // make sure to set this for your modnpcs.

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
			var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{ // Influences how the NPC looks in the Bestiary
				CustomTexturePath = "StarsAbove/Bestiary/Nalhaun_Bestiary", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
				Position = new Vector2(0f, 0f),
				PortraitScale = 1f,
				PortraitPositionXOverride = 0f,
				PortraitPositionYOverride = 0f
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);

		}
		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {

				new FlavorTextBestiaryInfoElement($"Mods.StarsAbove.Bestiary.{Name}")
				});

		}
		public override void SetDefaults()
		{
			NPC.boss = true;
			NPC.lifeMax = 155000;
			NPC.damage = 20;
			NPC.defense = 15;
			NPC.knockBackResist = 0f;
			NPC.width = 160;
			NPC.height = 160;
			NPC.scale = 1f;
			NPC.npcSlots = 1f;
			NPC.aiStyle = -1;
			NPC.lavaImmune = true;
			NPC.noGravity = true;
			NPC.noTileCollide = false;
			DrawOffsetY = 42;

			NPC.HitSound = SoundID.NPCHit54;
			NPC.DeathSound = SoundID.NPCDeath52;

			NPC.value = Item.buyPrice(0, 20, 75, 45);

			Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/TheMightOfTheHellblade");
			SpawnModBiomes = new int[1] { ModContent.GetInstance<Biomes.SeaOfStarsBiome>().Type };
			NPC.netAlways = true;
		}
		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			return false;
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

			potionType = ItemID.GreaterHealingPotion;


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
			
			var modPlayer = Main.LocalPlayer.GetModPlayer<StarsAbovePlayer>();
            var bossPlayer = Main.LocalPlayer.GetModPlayer<BossPlayer>();

            bossPlayer.NalhaunBarActive = true;
            NPC.velocity *= 0.98f; //So the dashes don't propel the boss away

			for (int i = 0; i < Main.maxPlayers; i++)
			{
				Player player = Main.player[i];
				if (player.active)
				{
					//If boss is in phase 2...
					if (NPC.localAI[0] == 1)
					{
						player.AddBuff(BuffType<Buffs.Boss.MonarchPresence>(), 10);

					}
				}
			}

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
			if(Main.expertMode)
            {
				AttackTimer = 100;
            }
            if (AI_Timer >= AttackTimer) //An attack is active.
            {

                //Attacks begin here.
                if (AI_RotationNumber == 0)
                {
					//
					ArsLaevateinn(P, NPC);
                    return;
                }
				else if (AI_RotationNumber == 1)
				{
					//
					DelayedOuterAgony(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 2)
				{
					//
					BladeworkStrong3(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 3)
				{
					//
					IvoryStake2(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 4)
				{
					//
					VelvetApogee(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 5)
				{
					//
					Apostasy(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 6)
				{
					//
					Bladework1(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 7)
				{
					//
					CarrionCall2(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 8)
				{
					//
					OuterAgony(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 9)
				{
					//
					Bladework3(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 10)
				{
					//
					DelayedLeftwardRend(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 11)
				{
					//
					Transplacement(P, NPC);
					return;
				}
				else if(AI_RotationNumber == 12)
				{
					//
					RightwardRend(P, NPC);
					return;
				}
				else if(AI_RotationNumber == 13)
				{
					//
					VelvetAzimuth(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 14)
				{
					//
					DelayedInnerAgony(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 15)
				{
					BladeworkStrong2(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 16)
				{
					IvoryStake2(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 17)
				{
					VelvetApogee(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 18)
				{
					BladeworkStrong4(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 19)
				{
					MonarchFeint(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 20)
				{
					FakeInnerAgony(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 21)
				{
					Bladework3(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 22)
				{
					DelayedInnerAgony(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 23)
				{
					Transplacement(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 24)
				{
					VelvetApogee(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 25)
				{
					CarrionCall2(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 26)
				{
					DelayedRightwardRend(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 27)
				{
					Transplacement(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 28)
				{
					MonarchFeint(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 29)
				{
					FakeInnerAgony(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 30)
				{
					Transplacement(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 31)
				{
					Bladework3(P, NPC);
					return;
				}
				else if(AI_RotationNumber == 32)
				{
					Apostasy2(P, NPC);
					return;
				}
				else if(AI_RotationNumber == 33)
				{
					Transplacement(P, NPC);
					return;
				}
				else if(AI_RotationNumber == 34)
				{
					
					DelayedRightwardRend(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 35)
				{
					BladeworkStrong1(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 36)
				{
					BladeworkStrong1(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 37)
				{
					DelayedLeftwardRend(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 38)
				{
					BladeworkStrong2(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 39)
				{
					Transplacement(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 40)
				{
					IvoryStake1(P, NPC);
					return;
				}
				else if (AI_RotationNumber == 41)
				{
					CarrionCall(P, NPC);
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

				if (other.active && (other.type == ModContent.ProjectileType<NalhaunSwordSprite>()
					|| other.type == ModContent.ProjectileType<NalhaunLoseSwordSprite>()
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
					if(NPC.HasBuff(BuffType<NalhaunSword>()))
                    {
						if (NPC.frameCounter < 10)
						{
							NPC.frame.Y = (int)Frame.Prep1 * frameHeight;
						}
						else if (NPC.frameCounter < 20)
						{
							NPC.frame.Y = (int)Frame.Prep2 * frameHeight;
						}
						else if (NPC.frameCounter < 30)
						{
							NPC.frame.Y = (int)Frame.Prep3 * frameHeight;
						}
						else if (NPC.frameCounter < 40)
						{
							NPC.frame.Y = (int)Frame.Prep4 * frameHeight;
						}
						else if (NPC.frameCounter < 50)
						{
							NPC.frame.Y = (int)Frame.Prep5 * frameHeight;
						}
						else if (NPC.frameCounter < 60)
						{
							NPC.frame.Y = (int)Frame.Prep6 * frameHeight;
						}
						else
						{
							NPC.frameCounter = 0;
						}
					}
					else
                    {
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
					}
					
				break;
				case (float)ActionState.Casting:
					NPC.frameCounter++;

					if (NPC.HasBuff(BuffType<NalhaunSword>()))
					{
						if (NPC.frameCounter < 10)
						{
							NPC.frame.Y = (int)Frame.Prep1 * frameHeight;
						}
						else if (NPC.frameCounter < 20)
						{
							NPC.frame.Y = (int)Frame.Prep2 * frameHeight;
						}
						else if (NPC.frameCounter < 30)
						{
							NPC.frame.Y = (int)Frame.Prep3 * frameHeight;
						}
						else if (NPC.frameCounter < 40)
						{
							NPC.frame.Y = (int)Frame.Prep4 * frameHeight;
						}
						else if (NPC.frameCounter < 50)
						{
							NPC.frame.Y = (int)Frame.Prep5 * frameHeight;
						}
						else if (NPC.frameCounter < 60)
						{
							NPC.frame.Y = (int)Frame.Prep6 * frameHeight;
						}
						else
						{
							NPC.frameCounter = 0;
						}
					}
					else
					{
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
			if (Main.rand.NextBool(5) && NPC.ai[1] < 420f)
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

			if (NPC.ai[1] >= 480f)
			{
				for (int d = 0; d < 305; d++)
				{
					Dust.NewDust(NPC.Center, 0, 0, DustID.FireworkFountain_Red, 0f + Main.rand.Next(-65, 65), 0f + Main.rand.Next(-65, 65), 150, default(Color), 1.5f);
				}
				SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_NalhaunDeathQuote, NPC.Center);

				DownedBossSystem.downedNalhaun = true;
				if (Main.netMode == NetmodeID.Server)
				{
					NetMessage.SendData(MessageID.WorldData); // Immediately inform clients of new world state.
				}
				modPlayer.NalhaunActive = false;
				modPlayer.NalhaunBarActive = false;

				NPC.life = 0;
				NPC.HitEffect(0, 0);
				NPC.checkDead(); // This will trigger ModNPC.CheckDead the second time, causing the real death.

				
			}
			return;
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			// Do NOT misuse the ModifyNPCLoot and OnKill hooks: the former is only used for registering drops, the latter for everything else
			//Chance for a Prism
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Prisms.BurnishedPrism>(), 4));

			// Add the treasure bag using ItemDropRule.BossBag (automatically checks for expert mode)
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<NalhaunBossBag>()));

			// Trophies are spawned with 1/10 chance
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Placeable.BossLoot.NalhaunTrophyItem>(), 10));

			// ItemDropRule.MasterModeCommonDrop for the relic
			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<Items.Placeable.BossLoot.NalhaunBossRelicItem>()));

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
			npcLoot.Add(notExpertRule);
		}
		public override void OnKill()
		{
			NPC.SetEventFlagCleared(ref DownedBossSystem.downedNalhaun, -1);

		}
		private void SpawnAnimation()
		{
			
			//Once the spawn animation is done, change to ActionState.Idle

			//Sprite animation. Easier to work with, because it's not tied to the main sprite sheet.
			//Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, 0, 0, ModContent.ProjectileType<VagrantSlamSprite>(), 0, 0, Main.myPlayer);
			
			
			
			//NPC.position.X = Main.player[NPC.target].position.X;
			//NPC.position.Y = Main.player[NPC.target].position.Y-160;
			//SoundEngine.PlaySound(StarsAboveAudio.Nalhaun_NalhaunIntroQuote, NPC.Center);
			NPC.netUpdate = true;
			if(Main.netMode == NetmodeID.SinglePlayer)
			{
				NPC.dontTakeDamage = true;

			}
			for (int i = 0; i < Main.maxPlayers; i++)
			{
				Player player = Main.player[i];
				if (player.active && player.Distance(NPC.Center) < 1000)
					player.AddBuff(BuffType<Invincibility>(), 10);

			}
			//Boss spawn timer.
			NPC.localAI[0]++;
			if (NPC.localAI[0] >= 400)
			{
				NPC.dontTakeDamage = false;

				NPC.netUpdate = true;

				AI_State = (float)ActionState.Idle;
			}
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
		
	}
}
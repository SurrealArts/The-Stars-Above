﻿using Terraria;using Terraria.ID;
using Terraria.ModLoader;

namespace StarsAbove.Buffs.TagDamage
{
    public class ApalistikTagDamage : ModBuff
    {
		public override void SetStaticDefaults()
		{
			// This allows the debuff to be inflicted on NPCs that would otherwise be immune to all debuffs.
			// Other mods may check it for different purposes.
			BuffID.Sets.IsATagBuff[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<ApalistikDebuffNPC>().marked = true;
		}
	}

	public class ApalistikDebuffNPC : GlobalNPC
	{
		// This is required to store information on entities that isn't shared between them.
		public override bool InstancePerEntity => true;

		public bool marked;

		public override void ResetEffects(NPC npc)
		{
			marked = false;
		}

		// TODO: Inconsistent with vanilla, increasing damage AFTER it is randomised, not before. Change to a different hook in the future.
		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
		{
			// Only player attacks should benefit from this buff, hence the NPC and trap checks.
			if (marked && !projectile.npcProj && !projectile.trap && (projectile.minion || ProjectileID.Sets.MinionShot[projectile.type]))
			{
				modifiers.FlatBonusDamage += 5;
			}
		}
	}
}


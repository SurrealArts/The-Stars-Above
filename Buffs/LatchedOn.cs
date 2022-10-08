﻿using Terraria;
using Terraria.ModLoader;

namespace StarsAbove.Buffs
{
    public class LatchedOn : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Latched On");
            Description.SetDefault("Spider is on you");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
        }

        public override void Update(Player player, ref int buffIndex)
        {
            //player.GetModPlayer<StarsAbovePlayer>().NanitePlague = true;
        }

        
        public override void Update(NPC npc, ref int buffIndex)
        {
            //npc.velocity *= 0;
            
            //npc.GetGlobalNPC<StarsAboveGlobalNPC>().Petrified = true;
        }
        public override bool ReApply(NPC npc, int time, int buffIndex)
        {
            
            return base.ReApply(npc, time, buffIndex);
        }
    }
}

﻿using Terraria;
using Terraria.ModLoader;

namespace StarsAbove.Buffs
{
    public class SoulSapping : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Soul Sapping");
            Description.SetDefault("Nalhaun is draining your soul; if the Soul Gauge drops to zero, you'll die");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true; //Add this so the nurse doesn't remove the buff when healing
        }
        
        public override void Update(Player player, ref int buffIndex)
        {
           

            
            
        }

    }
}

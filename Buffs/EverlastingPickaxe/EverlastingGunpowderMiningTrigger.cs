﻿using Terraria;
using Terraria.ModLoader;

namespace StarsAbove.Buffs.EverlastingPickaxe
{
    public class EverlastingGunpowderMiningTrigger : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gunpowder Trigger");
            Description.SetDefault("");
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
            Main.persistentBuff[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            
        }
    }
}

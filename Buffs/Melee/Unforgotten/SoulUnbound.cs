﻿using Terraria;
using Terraria.ModLoader;

namespace StarsAbove.Buffs.Melee.Unforgotten
{
    public class SoulUnbound : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Soul Unbound");
            // Description.SetDefault("You have let go of your mortal form, gaining Movement Speed");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; //Add this so the nurse doesn't remove the buff when healing
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed *= 4f;
        }

        public override bool RightClick(int buffIndex)
        {
            return false;
        }
    }
}

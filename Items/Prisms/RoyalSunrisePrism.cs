using Microsoft.Xna.Framework;
using StarsAbove.Systems.Items;
using StarsAbove.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace StarsAbove.Items.Prisms
{
    public class RoyalSunrisePrism : StellarPrism
    {
        public override int SetBonus { get; set; } = (int)ItemPrismSystem.MajorSetBonuses.RoyalSunrise;
        public override string SetBonusSimpleName { get; set; } = "RoyalSunrise";
        public override string SetBonusName { get; set; } = LangHelper.GetTextValue("StellarNova.StellarPrisms.RoyalSunrise.Name");
        public override string SetBonusDescription1 { get; set; } = LangHelper.GetTextValue("StellarNova.StellarPrisms.RoyalSunrise.SetBonus1");
        public override string SetBonusDescription2 { get; set; } = LangHelper.GetTextValue("StellarNova.StellarPrisms.RoyalSunrise.SetBonus2");
        public override string SetBonusDescription3 { get; set; } = LangHelper.GetTextValue("StellarNova.StellarPrisms.RoyalSunrise.SetBonus3");

        public override string FlavorTooltip { get; set; } = LangHelper.GetTextValue("StellarNova.StellarPrisms.RoyalSunrise.FlavorTooltip");

    }
}
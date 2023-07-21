using Terraria;
using Terraria.ModLoader;

namespace StarsAbove.Prefixes
{
	// This class serves as an example for declaring item 'prefixes', or 'modifiers' in other words.
	public class NovaPrefix1 : ModPrefix
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Bright");
		}
		// We declare a custom *virtual* property here, so that another type, ExampleDerivedPrefix, could override it and change the effective power for itself.
		public virtual float Power => 0f;

		// Change your category this way, defaults to PrefixCategory.Custom. Affects which items can get this prefix.
		public override PrefixCategory Category => PrefixCategory.AnyWeapon;

		// See documentation for vanilla weights and more information.
		// In case of multiple prefixes with similar functions this can be used with a switch/case to provide different chances for different prefixes
		// Note: a weight of 0f might still be rolled. See CanRoll to exclude prefixes.
		// Note: if you use PrefixCategory.Custom, actually use ModItem.ChoosePrefix instead.
		public override float RollChance(Item item)
		{
			return 0f;
		}

		// Determines if it can roll at all.
		// Use this to control if a prefix can be rolled or not.
		public override bool CanRoll(Item item)
		{
			if(DownedBossSystem.downedVagrant)
            {
				return true;
			}
			else
            {
				return false;
            }
			
		}

		// Use this function to modify these stats for items which have this prefix:
		// Damage Multiplier, Knockback Multiplier, Use Time Multiplier, Scale Multiplier (Size), Shoot Speed Multiplier, Mana Multiplier (Mana cost), Crit Bonus.
		public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
		{
			
		}

		// Modify the cost of items with this modifier with this function.
		public override void ModifyValue(ref float valueMult)
		{
			valueMult *= 1f + 0.05f * Power;
		}

		// This is used to modify most other stats of items which have this modifier.
		public override void Apply(Item item)
		{
			
		}
	}
	public class NovaPrefix2 : ModPrefix
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Glowing");
		}
		// We declare a custom *virtual* property here, so that another type, ExampleDerivedPrefix, could override it and change the effective power for itself.
		public virtual float Power => 0f;

		// Change your category this way, defaults to PrefixCategory.Custom. Affects which items can get this prefix.
		public override PrefixCategory Category => PrefixCategory.AnyWeapon;

		// See documentation for vanilla weights and more information.
		// In case of multiple prefixes with similar functions this can be used with a switch/case to provide different chances for different prefixes
		// Note: a weight of 0f might still be rolled. See CanRoll to exclude prefixes.
		// Note: if you use PrefixCategory.Custom, actually use ModItem.ChoosePrefix instead.
		public override float RollChance(Item item)
		{
			return 0f;
		}

		// Determines if it can roll at all.
		// Use this to control if a prefix can be rolled or not.
		public override bool CanRoll(Item item)
		{
			if (DownedBossSystem.downedVagrant)
			{
				return true;
			}
			else
			{
				return false;
			}

		}

		// Use this function to modify these stats for items which have this prefix:
		// Damage Multiplier, Knockback Multiplier, Use Time Multiplier, Scale Multiplier (Size), Shoot Speed Multiplier, Mana Multiplier (Mana cost), Crit Bonus.
		public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
		{

		}

		// Modify the cost of items with this modifier with this function.
		public override void ModifyValue(ref float valueMult)
		{
			valueMult *= 1f + 0.05f * Power;
		}

		// This is used to modify most other stats of items which have this modifier.
		public override void Apply(Item item)
		{

		}
	}
	public class NovaPrefix3 : ModPrefix
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Luminous");
		}
		// We declare a custom *virtual* property here, so that another type, ExampleDerivedPrefix, could override it and change the effective power for itself.
		public virtual float Power => 0f;

		// Change your category this way, defaults to PrefixCategory.Custom. Affects which items can get this prefix.
		public override PrefixCategory Category => PrefixCategory.AnyWeapon;

		// See documentation for vanilla weights and more information.
		// In case of multiple prefixes with similar functions this can be used with a switch/case to provide different chances for different prefixes
		// Note: a weight of 0f might still be rolled. See CanRoll to exclude prefixes.
		// Note: if you use PrefixCategory.Custom, actually use ModItem.ChoosePrefix instead.
		public override float RollChance(Item item)
		{
			return 0f;
		}

		// Determines if it can roll at all.
		// Use this to control if a prefix can be rolled or not.
		public override bool CanRoll(Item item)
		{
			if (DownedBossSystem.downedVagrant)
			{
				return true;
			}
			else
			{
				return false;
			}

		}

		// Use this function to modify these stats for items which have this prefix:
		// Damage Multiplier, Knockback Multiplier, Use Time Multiplier, Scale Multiplier (Size), Shoot Speed Multiplier, Mana Multiplier (Mana cost), Crit Bonus.
		public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
		{

		}

		// Modify the cost of items with this modifier with this function.
		public override void ModifyValue(ref float valueMult)
		{
			valueMult *= 1f + 0.05f * Power;
		}

		// This is used to modify most other stats of items which have this modifier.
		public override void Apply(Item item)
		{

		}
	}
	public class NovaPrefix4 : ModPrefix
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Radiant");
		}
		// We declare a custom *virtual* property here, so that another type, ExampleDerivedPrefix, could override it and change the effective power for itself.
		public virtual float Power => 0f;

		// Change your category this way, defaults to PrefixCategory.Custom. Affects which items can get this prefix.
		public override PrefixCategory Category => PrefixCategory.AnyWeapon;

		// See documentation for vanilla weights and more information.
		// In case of multiple prefixes with similar functions this can be used with a switch/case to provide different chances for different prefixes
		// Note: a weight of 0f might still be rolled. See CanRoll to exclude prefixes.
		// Note: if you use PrefixCategory.Custom, actually use ModItem.ChoosePrefix instead.
		public override float RollChance(Item item)
		{
			return 0f;
		}

		// Determines if it can roll at all.
		// Use this to control if a prefix can be rolled or not.
		public override bool CanRoll(Item item)
		{
			if (DownedBossSystem.downedVagrant)
			{
				return true;
			}
			else
			{
				return false;
			}

		}

		// Use this function to modify these stats for items which have this prefix:
		// Damage Multiplier, Knockback Multiplier, Use Time Multiplier, Scale Multiplier (Size), Shoot Speed Multiplier, Mana Multiplier (Mana cost), Crit Bonus.
		public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
		{

		}

		// Modify the cost of items with this modifier with this function.
		public override void ModifyValue(ref float valueMult)
		{
			valueMult *= 1f + 0.05f * Power;
		}

		// This is used to modify most other stats of items which have this modifier.
		public override void Apply(Item item)
		{

		}
	}
	public class BadNovaPrefix1 : ModPrefix
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Bleak");
		}
		// We declare a custom *virtual* property here, so that another type, ExampleDerivedPrefix, could override it and change the effective power for itself.
		public virtual float Power => 0f;

		// Change your category this way, defaults to PrefixCategory.Custom. Affects which items can get this prefix.
		public override PrefixCategory Category => PrefixCategory.AnyWeapon;

		// See documentation for vanilla weights and more information.
		// In case of multiple prefixes with similar functions this can be used with a switch/case to provide different chances for different prefixes
		// Note: a weight of 0f might still be rolled. See CanRoll to exclude prefixes.
		// Note: if you use PrefixCategory.Custom, actually use ModItem.ChoosePrefix instead.
		public override float RollChance(Item item)
		{
			return 0f;
		}

		// Determines if it can roll at all.
		// Use this to control if a prefix can be rolled or not.
		public override bool CanRoll(Item item)
		{
			if (DownedBossSystem.downedVagrant)
			{
				return true;
			}
			else
			{
				return false;
			}

		}

		// Use this function to modify these stats for items which have this prefix:
		// Damage Multiplier, Knockback Multiplier, Use Time Multiplier, Scale Multiplier (Size), Shoot Speed Multiplier, Mana Multiplier (Mana cost), Crit Bonus.
		public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
		{

		}

		// Modify the cost of items with this modifier with this function.
		public override void ModifyValue(ref float valueMult)
		{
			valueMult *= 1f - 0.05f * Power;
		}

		// This is used to modify most other stats of items which have this modifier.
		public override void Apply(Item item)
		{

		}
	}
	public class BadNovaPrefix2 : ModPrefix
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Lightless");
		}
		// We declare a custom *virtual* property here, so that another type, ExampleDerivedPrefix, could override it and change the effective power for itself.
		public virtual float Power => 0f;

		// Change your category this way, defaults to PrefixCategory.Custom. Affects which items can get this prefix.
		public override PrefixCategory Category => PrefixCategory.AnyWeapon;

		// See documentation for vanilla weights and more information.
		// In case of multiple prefixes with similar functions this can be used with a switch/case to provide different chances for different prefixes
		// Note: a weight of 0f might still be rolled. See CanRoll to exclude prefixes.
		// Note: if you use PrefixCategory.Custom, actually use ModItem.ChoosePrefix instead.
		public override float RollChance(Item item)
		{
			return 0f;
		}

		// Determines if it can roll at all.
		// Use this to control if a prefix can be rolled or not.
		public override bool CanRoll(Item item)
		{
			if (DownedBossSystem.downedVagrant)
			{
				return true;
			}
			else
			{
				return false;
			}

		}

		// Use this function to modify these stats for items which have this prefix:
		// Damage Multiplier, Knockback Multiplier, Use Time Multiplier, Scale Multiplier (Size), Shoot Speed Multiplier, Mana Multiplier (Mana cost), Crit Bonus.
		public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
		{

		}

		// Modify the cost of items with this modifier with this function.
		public override void ModifyValue(ref float valueMult)
		{
			valueMult *= 1f - 0.05f * Power;
		}

		// This is used to modify most other stats of items which have this modifier.
		public override void Apply(Item item)
		{

		}
	}

}
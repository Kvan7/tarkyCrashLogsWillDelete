using System.Collections.Generic;
using System.ComponentModel;
using BepInEx.Configuration;
using EFT;

namespace kvan.RaidSkillInfo.Helpers
{
	public enum ToastsState
	{
		[Description("All Skills")]
		All,
		[Description("Specific Skills (use advanced config)")]
		SpecificSkills,
		[Description("No Skills")]
		None
	};
	internal static class MyConfig
	{
		public static ConfigEntry<ToastsState> EnableToasts;
		public static ConfigEntry<bool> ShowTimeRemaining;
		public static ConfigEntry<int> RefreshTime;
		public static Dictionary<ESkillId, ConfigEntry<bool>> SkillToasts = new Dictionary<ESkillId, ConfigEntry<bool>>();

		public static void InitializeConfig(ConfigFile Config)
		{
			// Normal config
			ShowTimeRemaining = Config.Bind(
				"General",
				"Show Reset Time Remaining",
				true,
				new ConfigDescription("Show the time remaining until the skill's fatigue resets on the skills screen, updates based on Refresh Time(see advanced)",
				null,
				new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 20 })
			);
			RefreshTime = Config.Bind(
				"General",
				"Refresh Time",
				1,
				new ConfigDescription("Increase if noticing performance issues",
				new AcceptableValueRange<int>(0, 300),
				new ConfigurationManagerAttributes { IsAdvanced = true, ShowRangeAsPercent = false, Order = 19 })
			);

			EnableToasts = Config.Bind(
				"Notifications",
				"Enable Fatigue Reset notifications",
				ToastsState.All,
				new ConfigDescription("Whether to get notification toasts when a skill's fatigue expires",
				null,
				new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 100 }));

			// Initialize skill toast settings
			InitializeSkillToasts(Config);
		}

		// Initializes various skill toasts based on the provided ConfigFile.
		public static void InitializeSkillToasts(ConfigFile Config)
		{
			SkillToasts[ESkillId.Endurance] = Config.Bind("Notifications", "Show Endurance Toasts", true, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 65 }));
			SkillToasts[ESkillId.Strength] = Config.Bind("Notifications", "Show Strength Toasts", true, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 64 }));
			SkillToasts[ESkillId.Vitality] = Config.Bind("Notifications", "Show Vitality Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 63 }));
			SkillToasts[ESkillId.Health] = Config.Bind("Notifications", "Show Health Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 62 }));
			SkillToasts[ESkillId.StressResistance] = Config.Bind("Notifications", "Show StressResistance Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 61 }));
			SkillToasts[ESkillId.Metabolism] = Config.Bind("Notifications", "Show Metabolism Toasts", true, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 60 }));
			SkillToasts[ESkillId.Immunity] = Config.Bind("Notifications", "Show Immunity Toasts", true, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 59 }));
			SkillToasts[ESkillId.Perception] = Config.Bind("Notifications", "Show Perception Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 58 }));
			SkillToasts[ESkillId.Intellect] = Config.Bind("Notifications", "Show Intellect Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 57 }));
			SkillToasts[ESkillId.Attention] = Config.Bind("Notifications", "Show Attention Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 56 }));
			SkillToasts[ESkillId.Charisma] = Config.Bind("Notifications", "Show Charisma Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 55 }));
			SkillToasts[ESkillId.Memory] = Config.Bind("Notifications", "Show Memory Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 54 }));
			SkillToasts[ESkillId.MagDrills] = Config.Bind("Notifications", "Show MagDrills Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 53 }));
			SkillToasts[ESkillId.Pistol] = Config.Bind("Notifications", "Show Pistol Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 52 }));
			SkillToasts[ESkillId.Revolver] = Config.Bind("Notifications", "Show Revolver Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 51 }));
			SkillToasts[ESkillId.SMG] = Config.Bind("Notifications", "Show SMG Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 50 }));
			SkillToasts[ESkillId.Assault] = Config.Bind("Notifications", "Show Assault Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 49 }));
			SkillToasts[ESkillId.Shotgun] = Config.Bind("Notifications", "Show Shotgun Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 48 }));
			SkillToasts[ESkillId.Sniper] = Config.Bind("Notifications", "Show Sniper Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 47 }));
			SkillToasts[ESkillId.LMG] = Config.Bind("Notifications", "Show LMG Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 46 }));
			SkillToasts[ESkillId.HMG] = Config.Bind("Notifications", "Show HMG Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 45 }));
			SkillToasts[ESkillId.Launcher] = Config.Bind("Notifications", "Show Launcher Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 44 }));
			SkillToasts[ESkillId.AttachedLauncher] = Config.Bind("Notifications", "Show AttachedLauncher Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 43 }));
			SkillToasts[ESkillId.Throwing] = Config.Bind("Notifications", "Show Throwing Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 42 }));
			SkillToasts[ESkillId.Misc] = Config.Bind("Notifications", "Show Misc Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 41 }));
			SkillToasts[ESkillId.Melee] = Config.Bind("Notifications", "Show Melee Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 40 }));
			SkillToasts[ESkillId.DMR] = Config.Bind("Notifications", "Show DMR Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 39 }));
			SkillToasts[ESkillId.DrawMaster] = Config.Bind("Notifications", "Show DrawMaster Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 38 }));
			SkillToasts[ESkillId.AimMaster] = Config.Bind("Notifications", "Show AimMaster Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 37 }));
			SkillToasts[ESkillId.RecoilControl] = Config.Bind("Notifications", "Show RecoilControl Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 36 }));
			SkillToasts[ESkillId.TroubleShooting] = Config.Bind("Notifications", "Show TroubleShooting Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 35 }));
			SkillToasts[ESkillId.Sniping] = Config.Bind("Notifications", "Show Sniping Toasts", true, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 34 }));
			SkillToasts[ESkillId.CovertMovement] = Config.Bind("Notifications", "Show CovertMovement Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 33 }));
			SkillToasts[ESkillId.ProneMovement] = Config.Bind("Notifications", "Show ProneMovement Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 32 }));
			SkillToasts[ESkillId.FirstAid] = Config.Bind("Notifications", "Show FirstAid Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 31 }));
			SkillToasts[ESkillId.FieldMedicine] = Config.Bind("Notifications", "Show FieldMedicine Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 30 }));
			SkillToasts[ESkillId.Surgery] = Config.Bind("Notifications", "Show Surgery Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 29 }));
			SkillToasts[ESkillId.LightVests] = Config.Bind("Notifications", "Show LightVests Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 28 }));
			SkillToasts[ESkillId.HeavyVests] = Config.Bind("Notifications", "Show HeavyVests Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 27 }));
			SkillToasts[ESkillId.WeaponModding] = Config.Bind("Notifications", "Show WeaponModding Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 26 }));
			SkillToasts[ESkillId.AdvancedModding] = Config.Bind("Notifications", "Show AdvancedModding Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 25 }));
			SkillToasts[ESkillId.NightOps] = Config.Bind("Notifications", "Show NightOps Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 24 }));
			SkillToasts[ESkillId.SilentOps] = Config.Bind("Notifications", "Show SilentOps Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 23 }));
			SkillToasts[ESkillId.Lockpicking] = Config.Bind("Notifications", "Show Lockpicking Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 22 }));
			SkillToasts[ESkillId.Search] = Config.Bind("Notifications", "Show Search Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 21 }));
			SkillToasts[ESkillId.WeaponTreatment] = Config.Bind("Notifications", "Show WeaponTreatment Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 20 }));
			SkillToasts[ESkillId.Freetrading] = Config.Bind("Notifications", "Show Freetrading Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 19 }));
			SkillToasts[ESkillId.Auctions] = Config.Bind("Notifications", "Show Auctions Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 18 }));
			SkillToasts[ESkillId.Cleanoperations] = Config.Bind("Notifications", "Show Cleanoperations Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 17 }));
			SkillToasts[ESkillId.Barter] = Config.Bind("Notifications", "Show Barter Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 16 }));
			SkillToasts[ESkillId.Shadowconnections] = Config.Bind("Notifications", "Show Shadowconnections Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 15 }));
			SkillToasts[ESkillId.Taskperformance] = Config.Bind("Notifications", "Show Taskperformance Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 14 }));
			SkillToasts[ESkillId.BearAssaultoperations] = Config.Bind("Notifications", "Show BearAssaultoperations Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 13 }));
			SkillToasts[ESkillId.BearAuthority] = Config.Bind("Notifications", "Show BearAuthority Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 12 }));
			SkillToasts[ESkillId.BearAksystems] = Config.Bind("Notifications", "Show BearAksystems Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 11 }));
			SkillToasts[ESkillId.BearHeavycaliber] = Config.Bind("Notifications", "Show BearHeavycaliber Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 10 }));
			SkillToasts[ESkillId.BearRawpower] = Config.Bind("Notifications", "Show BearRawpower Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 9 }));
			SkillToasts[ESkillId.UsecArsystems] = Config.Bind("Notifications", "Show UsecArsystems Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 8 }));
			SkillToasts[ESkillId.UsecDeepweaponmodding] = Config.Bind("Notifications", "Show UsecDeepweaponmodding Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 7 }));
			SkillToasts[ESkillId.UsecLongrangeoptics] = Config.Bind("Notifications", "Show UsecLongrangeoptics Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 6 }));
			SkillToasts[ESkillId.UsecNegotiations] = Config.Bind("Notifications", "Show UsecNegotiations Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 5 }));
			SkillToasts[ESkillId.UsecTactics] = Config.Bind("Notifications", "Show UsecTactics Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 4 }));
			SkillToasts[ESkillId.AimDrills] = Config.Bind("Notifications", "Show AimDrills Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 3 }));
			SkillToasts[ESkillId.HideoutManagement] = Config.Bind("Notifications", "Show HideoutManagement Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 2 }));
			SkillToasts[ESkillId.Crafting] = Config.Bind("Notifications", "Show Crafting Toasts", false, new ConfigDescription("", null, new ConfigurationManagerAttributes { IsAdvanced = true, Order = 1 }));
		}
	}
}
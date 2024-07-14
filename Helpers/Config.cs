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
		public static Dictionary<ESkillId, ConfigEntry<bool>> SkillToasts = new Dictionary<ESkillId, ConfigEntry<bool>>();

		public static void InitializeConfig(ConfigFile Config)
		{
			// Normal config
			EnableToasts = Config.Bind(
				"General",
				"Enable Fatigue Reset notifications ",
				ToastsState.All,
				new ConfigDescription("Whether to get notification toasts when a skill's fatigue expires",
				null,
				new ConfigurationManagerAttributes { IsAdvanced = false, ShowRangeAsPercent = false, Order = 0 }));

			// Initialize skill toast settings
			InitializeSkillToasts(Config);
		}


		// Initializes various skill toasts based on the provided ConfigFile.
		public static void InitializeSkillToasts(ConfigFile Config)
		{
			SkillToasts[ESkillId.Endurance] = Config.Bind("General", "Show Endurance Toasts", true, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Strength] = Config.Bind("General", "Show Strength Toasts", true, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Vitality] = Config.Bind("General", "Show Vitality Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Health] = Config.Bind("General", "Show Health Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.StressResistance] = Config.Bind("General", "Show StressResistance Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Metabolism] = Config.Bind("General", "Show Metabolism Toasts", true, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Immunity] = Config.Bind("General", "Show Immunity Toasts", true, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Perception] = Config.Bind("General", "Show Perception Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Intellect] = Config.Bind("General", "Show Intellect Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Attention] = Config.Bind("General", "Show Attention Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Charisma] = Config.Bind("General", "Show Charisma Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Memory] = Config.Bind("General", "Show Memory Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.MagDrills] = Config.Bind("General", "Show MagDrills Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Pistol] = Config.Bind("General", "Show Pistol Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Revolver] = Config.Bind("General", "Show Revolver Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.SMG] = Config.Bind("General", "Show SMG Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Assault] = Config.Bind("General", "Show Assault Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Shotgun] = Config.Bind("General", "Show Shotgun Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Sniper] = Config.Bind("General", "Show Sniper Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.LMG] = Config.Bind("General", "Show LMG Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.HMG] = Config.Bind("General", "Show HMG Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Launcher] = Config.Bind("General", "Show Launcher Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.AttachedLauncher] = Config.Bind("General", "Show AttachedLauncher Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Throwing] = Config.Bind("General", "Show Throwing Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Misc] = Config.Bind("General", "Show Misc Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Melee] = Config.Bind("General", "Show Melee Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.DMR] = Config.Bind("General", "Show DMR Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.DrawMaster] = Config.Bind("General", "Show DrawMaster Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.AimMaster] = Config.Bind("General", "Show AimMaster Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.RecoilControl] = Config.Bind("General", "Show RecoilControl Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.TroubleShooting] = Config.Bind("General", "Show TroubleShooting Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Sniping] = Config.Bind("General", "Show Sniping Toasts", true, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.CovertMovement] = Config.Bind("General", "Show CovertMovement Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.ProneMovement] = Config.Bind("General", "Show ProneMovement Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.FirstAid] = Config.Bind("General", "Show FirstAid Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.FieldMedicine] = Config.Bind("General", "Show FieldMedicine Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Surgery] = Config.Bind("General", "Show Surgery Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.LightVests] = Config.Bind("General", "Show LightVests Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.HeavyVests] = Config.Bind("General", "Show HeavyVests Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.WeaponModding] = Config.Bind("General", "Show WeaponModding Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.AdvancedModding] = Config.Bind("General", "Show AdvancedModding Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.NightOps] = Config.Bind("General", "Show NightOps Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.SilentOps] = Config.Bind("General", "Show SilentOps Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Lockpicking] = Config.Bind("General", "Show Lockpicking Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Search] = Config.Bind("General", "Show Search Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.WeaponTreatment] = Config.Bind("General", "Show WeaponTreatment Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Freetrading] = Config.Bind("General", "Show Freetrading Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Auctions] = Config.Bind("General", "Show Auctions Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Cleanoperations] = Config.Bind("General", "Show Cleanoperations Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Barter] = Config.Bind("General", "Show Barter Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Shadowconnections] = Config.Bind("General", "Show Shadowconnections Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Taskperformance] = Config.Bind("General", "Show Taskperformance Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.BearAssaultoperations] = Config.Bind("General", "Show BearAssaultoperations Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.BearAuthority] = Config.Bind("General", "Show BearAuthority Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.BearAksystems] = Config.Bind("General", "Show BearAksystems Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.BearHeavycaliber] = Config.Bind("General", "Show BearHeavycaliber Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.BearRawpower] = Config.Bind("General", "Show BearRawpower Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.UsecArsystems] = Config.Bind("General", "Show UsecArsystems Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.UsecDeepweaponmodding] = Config.Bind("General", "Show UsecDeepweaponmodding Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.UsecLongrangeoptics] = Config.Bind("General", "Show UsecLongrangeoptics Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.UsecNegotiations] = Config.Bind("General", "Show UsecNegotiations Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.UsecTactics] = Config.Bind("General", "Show UsecTactics Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.AimDrills] = Config.Bind("General", "Show AimDrills Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.HideoutManagement] = Config.Bind("General", "Show HideoutManagement Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
			SkillToasts[ESkillId.Crafting] = Config.Bind("General", "Show Crafting Toasts", false, new ConfigDescription(null, null, new ConfigurationManagerAttributes { IsAdvanced = true }));
		}
	}
}
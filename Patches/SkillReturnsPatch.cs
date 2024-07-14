using SPT.Reflection.Patching;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using EFT;
using EFT.UI;
using kvan.RaidSkillInfo.Controllers;
using kvan.RaidSkillInfo.Helpers;
using System;

namespace kvan.RaidSkillInfo.Patches
{
	internal class BuffIconPatch : ModulePatch
	{
		// Dictionary to store the original effectiveness for each skill ID
		private static readonly Dictionary<ESkillId, float> originalEffectivenessMap = new Dictionary<ESkillId, float>();

		protected override MethodBase GetTargetMethod()
		{
			return AccessTools.Method(typeof(SkillPanel), nameof(SkillPanel.method_1));
		}

		[PatchPrefix]
		static bool Prefix(SkillPanel __instance)
		{
			if (!Utils.InRaid())
			{
				return true;
			}

			SkillClass skill = (SkillClass)AccessTools.Field(typeof(SkillPanel), "skillClass").GetValue(__instance);
			ESkillId skillId = skill.Id;

			if (SkillDeterminer.SpecificSkills.Contains(skillId))
			{
				// Store the original effectiveness value in the dictionary
				if (!originalEffectivenessMap.ContainsKey(skillId))
				{
					originalEffectivenessMap[skillId] = skill.Effectiveness;
				}

				// Apply the temporary modification
				AccessTools.Field(typeof(SkillClass), "float_3").SetValue(skill, 1f);
			}
			return true;
		}

		[PatchPostfix]
		static void Postfix(SkillPanel __instance)
		{
			if (!Utils.InRaid())
			{
				return;
			}
			SkillClass skill = (SkillClass)AccessTools.Field(typeof(SkillPanel), "skillClass").GetValue(__instance);
			ESkillId skillId = skill.Id;

			if (SkillDeterminer.SpecificSkills.Contains(skillId))
			{
				// Restore the original effectiveness value from the dictionary
				if (originalEffectivenessMap.TryGetValue(skillId, out float originalEffectiveness))
				{
					AccessTools.Field(typeof(SkillClass), "float_3").SetValue(skill, originalEffectiveness);
					originalEffectivenessMap.Remove(skillId); // Clean up the dictionary
				}
			}
		}
	}

	internal class BuffTooltipPatch : ModulePatch
	{
		// Dictionary to store the original effectiveness for each skill ID
		private static readonly Dictionary<ESkillId, float> originalEffectivenessMap = new Dictionary<ESkillId, float>();

		protected override MethodBase GetTargetMethod()
		{
			return AccessTools.Method(typeof(SkillTooltip), nameof(SkillTooltip.Show), new Type[] { typeof(SkillClass) });
		}

		[PatchPrefix]
		static bool Prefix(SkillClass skill)
		{
			if (!Utils.InRaid())
			{
				return true;
			}

			ESkillId skillId = skill.Id;

			if (SkillDeterminer.SpecificSkills.Contains(skillId))
			{
				// Store the original effectiveness value in the dictionary
				if (!originalEffectivenessMap.ContainsKey(skillId))
				{
					originalEffectivenessMap[skillId] = skill.Effectiveness;
				}

				// Apply the temporary modification
				AccessTools.Field(typeof(SkillClass), "float_3").SetValue(skill, 1f);
			}
			return true;
		}

		[PatchPostfix]
		static void Postfix(SkillClass skill)
		{
			if (!Utils.InRaid())
			{
				return;
			}
			ESkillId skillId = skill.Id;

			if (SkillDeterminer.SpecificSkills.Contains(skillId))
			{
				// Restore the original effectiveness value from the dictionary
				if (originalEffectivenessMap.TryGetValue(skillId, out float originalEffectiveness))
				{
					AccessTools.Field(typeof(SkillClass), "float_3").SetValue(skill, originalEffectiveness);
					originalEffectivenessMap.Remove(skillId); // Clean up the dictionary
				}
			}
		}
	}
}

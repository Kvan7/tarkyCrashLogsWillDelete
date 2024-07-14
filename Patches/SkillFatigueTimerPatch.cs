using SPT.Reflection.Patching;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using EFT;
using EFT.UI;
using kvan.RaidSkillInfo.Controllers;
using kvan.RaidSkillInfo.Helpers;
using System;
using UnityEngine;
using TMPro;

namespace kvan.RaidSkillInfo.Patches
{
	internal class SkillFatigueTimerPatch : ModulePatch
	{
		public static readonly Dictionary<ESkillId, float> TimeRemaining = new Dictionary<ESkillId, float>();
		protected override MethodBase GetTargetMethod()
		{
			return AccessTools.Method(typeof(SkillPanel), nameof(SkillPanel.Show));
		}

		[PatchPostfix]
		static void Postfix(SkillPanel __instance)
		{
			if (!Utils.InRaid())
			{
				return;
			}
			if (!MyConfig.ShowTimeRemaining.Value)
			{
				return;
			}

			// Add in new text
			Transform buffsContainer = AccessTools.Field(typeof(SkillPanel), "_buffsContainer").GetValue(__instance) as Transform;

			// Create a new TextMeshProUGUI element
			var fatigueTimerText = new GameObject("BuffCountText").AddComponent<TextMeshProUGUI>();
			fatigueTimerText.transform.SetParent(buffsContainer, false);

			// Set the font size, color, and alignment
			fatigueTimerText.fontSize = 16;
			fatigueTimerText.color = Color.red;
			fatigueTimerText.alignment = TextAlignmentOptions.Center;

			// Get the skill class to determine the number of buffs
			var skillClass = AccessTools.Field(typeof(SkillPanel), "skillClass").GetValue(__instance) as SkillClass;
			if (skillClass != null)
			{
				if (TimeRemaining.TryGetValue(skillClass.Id, out float timeRemaining))
				{
					fatigueTimerText.text = timeRemaining >= 0 && timeRemaining < 1e5f ? timeRemaining.ToString("F0") : string.Empty;
				}
			}

			// Position the text element as the first or last sibling
			fatigueTimerText.transform.SetAsLastSibling(); // or use SetAsLastSibling() if you want it at the end
		}
	}


	internal class SkillFatigueTimerTooltipPatch : ModulePatch
	{
		protected override MethodBase GetTargetMethod()
		{
			return AccessTools.Method(typeof(SkillTooltip), nameof(SkillTooltip.Show), new Type[] { typeof(SkillClass) });
		}

		[PatchPostfix]
		static void Postfix(SkillTooltip __instance, SkillClass skill)
		{
			// Utils.LogMessage("POSTFIX RUNNY");
			if (!Utils.InRaid())
			{
				return;
			}
			// Utils.LogMessage("IN Raid");

			if (skill != null && SkillFatigueTimerPatch.TimeRemaining.TryGetValue(skill.Id, out float timeRemaining))
			{
				TextMeshProUGUI tooltipDescription = AccessTools.Field(typeof(SkillTooltip), "_description").GetValue(__instance) as TextMeshProUGUI;

				if (timeRemaining >= 0 && timeRemaining < 1e5f)
				{
					tooltipDescription.text += $"\n<color=#C40000FF>Time remaining: {timeRemaining:F0} seconds</color>";
				}

				// tooltipDescription.text = "BANANA MEow";
				Utils.LogMessage("Tooltip updated");
			}
			else
			{
				Utils.LogError("Tooltip not updated, value false");
			}
		}
	}
}
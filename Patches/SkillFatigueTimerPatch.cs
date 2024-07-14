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

			// Get the _effectivenessDown GameObject
			GameObject effectivenessDownObject = AccessTools.Field(typeof(SkillPanel), "_effectivenessDown").GetValue(__instance) as GameObject;

			if (effectivenessDownObject == null)
			{
				Debug.LogError("_effectivenessDown GameObject not found");
				return;
			}

			// Create a new TextMeshProUGUI element
			var fatigueTimerText = new GameObject("FatigueTimerText").AddComponent<TextMeshProUGUI>();
			fatigueTimerText.transform.SetParent(effectivenessDownObject.transform, false);

			// Set the font size, color, and alignment
			fatigueTimerText.fontSize = 14;
			fatigueTimerText.color = Color.red;
			fatigueTimerText.alignment = TextAlignmentOptions.Center;
			RectTransform rectTransform = fatigueTimerText.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = new Vector2(-20, 0);

			// Get the skill class to determine the time remaining
			var skillClass = AccessTools.Field(typeof(SkillPanel), "skillClass").GetValue(__instance) as SkillClass;
			if (skillClass != null)
			{
				if (TimeRemaining.TryGetValue(skillClass.Id, out float timeRemaining))
				{
					fatigueTimerText.text = timeRemaining >= 0 && timeRemaining < 1e5f ? timeRemaining.ToString("F0") : string.Empty;
				}
			}

			// Optionally position the text element as the first or last sibling
			fatigueTimerText.transform.SetAsLastSibling(); // or use SetAsFirstSibling() if you want it at the start
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
			if (!Utils.InRaid())
			{
				return;
			}

			if (skill != null && SkillFatigueTimerPatch.TimeRemaining.TryGetValue(skill.Id, out float timeRemaining))
			{
				TextMeshProUGUI tooltipDescription = AccessTools.Field(typeof(SkillTooltip), "_description").GetValue(__instance) as TextMeshProUGUI;

				if (timeRemaining >= 0 && timeRemaining < 1e5f)
				{
					tooltipDescription.text += $"\n<color=#C40000FF>Time remaining: {timeRemaining:F0} seconds</color>";
				}
			}
		}
	}
}
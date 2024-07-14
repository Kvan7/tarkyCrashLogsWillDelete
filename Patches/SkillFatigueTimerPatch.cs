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
			fatigueTimerText.fontSize = 14;
			fatigueTimerText.color = Color.white;
			fatigueTimerText.alignment = TextAlignmentOptions.Center;

			// Get the skill class to determine the number of buffs
			var skillClass = AccessTools.Field(typeof(SkillPanel), "skillClass").GetValue(__instance) as SkillClass;
			if (skillClass != null)
			{
				if (TimeRemaining.TryGetValue(skillClass.Id, out float timeRemaining))
				{
					fatigueTimerText.text = timeRemaining >= 0 && timeRemaining < 1e5f ? $"[{timeRemaining:F0}]" : string.Empty;
				}
			}

			// Position the text element as the first or last sibling
			fatigueTimerText.transform.SetAsLastSibling(); // or use SetAsLastSibling() if you want it at the end
		}
	}
}
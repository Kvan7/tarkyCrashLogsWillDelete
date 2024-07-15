using SPT.Reflection.Patching;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using EFT;
using EFT.UI;
using kvan.RaidSkillInfo.Helpers;
using System;
using UnityEngine;
using TMPro;
using System.Linq;

namespace kvan.RaidSkillInfo.Patches
{
	public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
	{
		public event Action<TKey, TValue> OnValueChanged;

		new public TValue this[TKey key]
		{
			get => base[key];
			set
			{
				base[key] = value;
				OnValueChanged?.Invoke(key, value);
			}
		}
	}

	internal class SkillFatigueTimerPatch : ModulePatch
	{
		public static readonly ObservableDictionary<ESkillId, float> TimeRemaining = new ObservableDictionary<ESkillId, float>();
		private static readonly Dictionary<ESkillId, TextMeshProUGUI> fatigueTimerTexts = new Dictionary<ESkillId, TextMeshProUGUI>();

		protected override MethodBase GetTargetMethod()
		{
			return AccessTools.Method(typeof(SkillPanel), nameof(SkillPanel.Show));
		}

		public SkillFatigueTimerPatch()
		{
			TimeRemaining.OnValueChanged += UpdateFatigueTimerText;
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

			// Get the skill class to determine the skill ID
			var skillClass = AccessTools.Field(typeof(SkillPanel), "skillClass").GetValue(__instance) as SkillClass;
			if (skillClass == null) return;

			// Create a new TextMeshProUGUI element
			var fatigueTimerText = new GameObject("FatigueTimerText").AddComponent<TextMeshProUGUI>();
			fatigueTimerText.transform.SetParent(effectivenessDownObject.transform, false);

			// Set the font size, color, and alignment
			fatigueTimerText.fontSize = 14;
			fatigueTimerText.color = Color.red;
			fatigueTimerText.alignment = TextAlignmentOptions.Center;
			RectTransform rectTransform = fatigueTimerText.GetComponent<RectTransform>();
			rectTransform.anchoredPosition = new Vector2(-20, 0);

			// Add to the dictionary
			fatigueTimerTexts[skillClass.Id] = fatigueTimerText;

			// Position the text element as the first or last sibling
			fatigueTimerText.transform.SetAsLastSibling(); // or use SetAsFirstSibling() if you want it at the start

			// Update the text with the initial time
			if (TimeRemaining.TryGetValue(skillClass.Id, out float timeRemaining))
			{
				UpdateFatigueTimerText(skillClass.Id, timeRemaining);
			}
		}

		private static void UpdateFatigueTimerText(ESkillId skillId, float timeRemaining)
		{
			if (fatigueTimerTexts.TryGetValue(skillId, out var fatigueTimerText))
			{
				// Update the text with the remaining time
				fatigueTimerText.text = timeRemaining >= 0 && timeRemaining < 1e5f ? timeRemaining.ToString("F0") : string.Empty;
			}

			// Update the tooltip if it is showing the same skill
			if (SkillFatigueTimerTooltipPatch.currentSkill != null && SkillFatigueTimerTooltipPatch.currentSkill.Id == skillId)
			{
				SkillFatigueTimerTooltipPatch.UpdateTooltipDescription(skillId, timeRemaining);
			}
		}
	}

	internal class SkillFatigueTimerTooltipPatch : ModulePatch
	{
		public static SkillClass currentSkill;
		public static TextMeshProUGUI tooltipDescription;

		protected override MethodBase GetTargetMethod()
		{
			return AccessTools.Method(typeof(SkillTooltip), nameof(SkillTooltip.Show), new Type[] { typeof(SkillClass) });
		}

		public SkillFatigueTimerTooltipPatch()
		{
			SkillFatigueTimerPatch.TimeRemaining.OnValueChanged += (skillId, timeRemaining) =>
			{
				if (currentSkill != null && currentSkill.Id == skillId)
				{
					UpdateTooltipDescription(skillId, timeRemaining);
				}
			};
		}

		[PatchPostfix]
		static void Postfix(SkillTooltip __instance, SkillClass skill)
		{
			if (!Utils.InRaid())
			{
				return;
			}

			currentSkill = skill;

			tooltipDescription = AccessTools.Field(typeof(SkillTooltip), "_description").GetValue(__instance) as TextMeshProUGUI;

			if (skill != null && SkillFatigueTimerPatch.TimeRemaining.TryGetValue(skill.Id, out float timeRemaining))
			{
				UpdateTooltipDescription(skill.Id, timeRemaining);
			}
		}

		public static void UpdateTooltipDescription(ESkillId skillId, float timeRemaining)
		{
			if (tooltipDescription == null)
			{
				return;
			}
			string newLine = $"\n<color=#C40000FF>Time remaining: {timeRemaining:F0} seconds</color>";
			string[] lines = tooltipDescription.text.Split('\n');

			if (lines.Length > 0 && lines[lines.Length - 1].StartsWith("<color=#C40000FF>Time remaining:"))
			{
				if (!MyConfig.ShowTimeRemaining.Value)
				{
					// Since last line has our text, means user changed setting while we had added to it.
					// Need to remove it then
					lines = lines.Take(lines.Length - 1).ToArray();
				}
				else if (timeRemaining >= 0 && timeRemaining < 1e5f)
				{
					lines[lines.Length - 1] = newLine.Trim();
				}
				else
				{
					lines[lines.Length - 1] = string.Empty;
				}
				tooltipDescription.text = string.Join("\n", lines);
			}
			else
			{
				// No line with our text, add it
				if (MyConfig.ShowTimeRemaining.Value && timeRemaining >= 0 && timeRemaining < 1e5f)
				{
					tooltipDescription.text += newLine;
				}
			}
		}
	}
}
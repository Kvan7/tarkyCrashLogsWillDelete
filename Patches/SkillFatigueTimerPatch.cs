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
using System.Collections;
using System.Collections.ObjectModel;

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

			// Remove old TextMeshProUGUI element if it exists
			if (fatigueTimerTexts.TryGetValue(skillClass.Id, out var existingText))
			{
				GameObject.Destroy(existingText.gameObject);
				fatigueTimerTexts.Remove(skillClass.Id);
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

			// Add to the dictionary
			fatigueTimerTexts[skillClass.Id] = fatigueTimerText;

			// Position the text element as the first or last sibling
			fatigueTimerText.transform.SetAsLastSibling(); // or use SetAsFirstSibling() if you want it at the start

			// Update the text element immediately
			UpdateFatigueTimerText(skillClass.Id, TimeRemaining.ContainsKey(skillClass.Id) ? TimeRemaining[skillClass.Id] : 0f);
		}

		private static void UpdateFatigueTimerText(ESkillId skillId, float timeRemaining)
		{
			if (fatigueTimerTexts.TryGetValue(skillId, out var fatigueTimerText))
			{
				fatigueTimerText.text = timeRemaining >= 0 && timeRemaining < 1e5f ? timeRemaining.ToString("F0") : string.Empty;
			}
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

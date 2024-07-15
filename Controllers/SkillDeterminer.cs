using EFT;
using EFT.Communications;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using kvan.RaidSkillInfo.Helpers;
using Comfort.Common;
using EFT.UI;
using kvan.RaidSkillInfo.Patches;

namespace kvan.RaidSkillInfo.Controllers
{
	public class SkillDeterminer : MonoBehaviour
	{
		private SkillManager SkillManager => Utils.GetActiveSkillManager();
		private float elapsedTime = 0f;

		private bool lastInRaid = false;
		SkillClass[] SkillClasses => SkillManager.DisplayList;
		private static Dictionary<ESkillId, SkillPanel> skillPanelCache = new Dictionary<ESkillId, SkillPanel>();

		public static HashSet<ESkillId> SpecificSkills = new HashSet<ESkillId>();

		bool Ready()
		{   //Singleton is a type of a type that means only one instance of it can exist per scene

			if (!Utils.InRaid())
			{
				if (lastInRaid)
				{
					ClearCache();
				}
				lastInRaid = false;
				return false;
			}
			return true;
		}

		void Update()
		{
			// return if not in raid
			if (!Ready())
			{
				return;
			}

			if (SkillManager == null)
			{
				return;
			}
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= MyConfig.RefreshTime.Value)
			{
				CheckSkillFatigue();
				elapsedTime = 0f; // Reset the elapsed time
			}
		}

		void CheckSkillFatigue()
		{
			if (SkillManager == null)
			{
				return;
			}
			if (!lastInRaid || SkillClasses.IsNullOrEmpty())
			{
				UpdateCache();
				lastInRaid = true;
			}

			if (SkillClasses.IsNullOrEmpty())
			{
				// Utils.LogError("No Skills found");
				return;
			}
			SkillClasses.ExecuteForEach(CheckIndividualSkill);
		}

		private static void CheckIndividualSkill(SkillClass skill)
		{
			// Get private float
			float fatigueTime = (float)AccessTools.Field(typeof(SkillClass), "float_4").GetValue(skill);
			ESkillId skillId = skill.Id;
			float timeRemaining = fatigueTime - Time.time;
			if (skill.Effectiveness >= 1f)
			{
				return;
			}
			if (timeRemaining < 0)
			{
				// Send toast if enabled
				if ((MyConfig.EnableToasts.Value == ToastsState.All
						|| (
							MyConfig.EnableToasts.Value == ToastsState.SpecificSkills
							&& MyConfig.SkillToasts.ContainsKey(skillId)
							&& MyConfig.SkillToasts[skillId].Value
							))
					&& !SpecificSkills.Contains(skillId)
					)
				{
					NotificationManagerClass.DisplayMessageNotification($"{skill.Id} fatigue reset", ENotificationDurationType.Default);
				}
				// Add to skill list
				SpecificSkills.Add(skillId);
			}
			else
			{
				// remove from skill list
				SpecificSkills.Remove(skillId);
			}
			if (MyConfig.ShowTimeRemaining.Value)
			{
				SkillFatigueTimerPatch.TimeRemaining[skillId] = timeRemaining;
			}
		}

		public List<SkillPanel> GetSkillPanelsBySkillClasses(SkillClass[] skillClasses)
		{
			List<SkillPanel> matchingPanels = new List<SkillPanel>();
			HashSet<SkillClass> skillClassSet = new HashSet<SkillClass>(skillClasses);

			// Find all SkillPanel instances
			foreach (SkillPanel panel in Resources.FindObjectsOfTypeAll<SkillPanel>())
			{
				if (AccessTools.Field(typeof(SkillPanel), "skillClass").GetValue(panel) is SkillClass skillClass
																						&& skillClassSet.Contains(skillClass))
				{
					matchingPanels.Add(panel);
				}
			}
			return matchingPanels;
		}

		private void UpdateCache()
		{
			skillPanelCache = GetSkillPanelsBySkillClasses(SkillClasses)
								.ToDictionary(RetrieveValue.GetSkillId, panel => panel);
		}

		private void ClearCache()
		{
			skillPanelCache.Clear();
			SpecificSkills = new HashSet<ESkillId>();
		}
	}
}
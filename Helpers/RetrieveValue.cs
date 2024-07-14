using EFT;
using EFT.UI;
using HarmonyLib;

namespace kvan.RaidSkillInfo.Helpers
{
	public static class RetrieveValue
	{

		public static ESkillId GetSkillId(SkillClass skillClass)
		{
			return skillClass.Id;
		}

		public static ESkillId GetSkillId(SkillPanel skillPanel)
		{
			return ((SkillClass)AccessTools.Field(typeof(SkillPanel), "skillClass").GetValue(skillPanel)).Id;
		}

	}
}
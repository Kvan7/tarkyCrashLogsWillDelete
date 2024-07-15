using Comfort.Common;
using EFT;

namespace kvan.RaidSkillInfo.Helpers
{
	public static class Utils
	{
		// https://github.com/CJ-SPT/Skills-Extended/blob/af18c2c9f22732af0cc0f5d9fa88368555024fd4/Plugin/Helpers/Utils.cs#L38
		// If the player is in the gameworld, use the main players skillmanager
		public static SkillManager GetActiveSkillManager()
		{
			if (Singleton<GameWorld>.Instance?.MainPlayer != null)
			{
				return Singleton<GameWorld>.Instance.MainPlayer.Skills;
			}
			// else if (Plugin.Session != null)
			// {
			// 	UsecRifleBehaviour.isSubscribed = false;
			// 	return ClientAppUtils.GetMainApp()?.GetClientBackEndSession()?.Profile?.Skills;
			// }

			return null;
		}

		public static bool InRaid()
		{
			string location = Singleton<GameWorld>.Instance?.MainPlayer?.Location ?? string.Empty;
			return !location.IsNullOrEmpty() && location != "hideout";
		}


		#region Logging
		public static void LogInfo(string message)
		{
			Plugin.LogSource.LogInfo(message);
		}
		public static void LogDebug(string message)
		{
			Plugin.LogSource.LogDebug(message);
		}
		public static void LogMessage(string message)
		{
			Plugin.LogSource.LogMessage(message);
		}
		public static void LogWarning(string message)
		{
			Plugin.LogSource.LogWarning(message);
		}
		public static void LogError(string message)
		{
			Plugin.LogSource.LogError(message);
		}
		#endregion
	}
}
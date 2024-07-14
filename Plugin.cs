using BepInEx;
using BepInEx.Logging;
using kvan.RaidSkillInfo.Patches;
using UnityEngine;
using kvan.RaidSkillInfo.Controllers;
using BepInEx.Configuration;
using EFT;

namespace kvan.RaidSkillInfo
{
	[BepInPlugin("kvan.RaidSkillInfo", "kvan-RaidSkillInfo", "0.0.1")]
	public class Plugin : BaseUnityPlugin
	{
		public static ManualLogSource LogSource;
		public GameObject Hook;


		internal static ConfigEntry<bool> EnableToasts;
		internal static ConfigEntry<ESkillId> EnabledSkills;

		//BaseUnityPlugin inherits MonoBehaviour, so you can use base unity functions like Awake() and Update()
#pragma warning disable IDE0051
		private void Awake() //Awake() will run once when your plugin loads
#pragma warning restore IDE0051
		{
			//we save the Logger to our LogSource variable so we can use it anywhere, such as in our patches via Plugin.LogSource.LogInfo(), etc.
			LogSource = Logger;
			EnableToasts = Config.Bind("General", "Enable Skill Notifications", true, "Whether to get notification toasts when a skill's fatigue expires");
			//uncomment the line below and replace "PatchClassName" with the class name you gave your patch. Patches must be enabled like this to work.
			new BuffIconPatch().Enable();
			new BuffTooltipPatch().Enable();

			Hook = new GameObject("PluginHook"); // create a new gameobject instance along with its name
			Hook.AddComponent<SkillDeterminer>(); // mount mono script to hook using the class name of your mono script
			DontDestroyOnLoad(Hook); // add hook to DontDestroyOnLoad which will add it as a constant object into the game

			LogSource.LogMessage("plugin loaded!");
		}
	}
}

using BepInEx;
using BepInEx.Logging;
using kvan.RaidSkillInfo.Patches;
using UnityEngine;
using kvan.RaidSkillInfo.Controllers;
using BepInEx.Configuration;
using EFT;
using DrakiaXYZ.VersionChecker;
using System;
using kvan.RaidSkillInfo.Helpers;

namespace kvan.RaidSkillInfo
{
	[BepInPlugin("kvan.RaidSkillInfo", "kvan-RaidSkillInfo", "0.0.2")]
	public class Plugin : BaseUnityPlugin
	{
		public const int TarkovVersion = 30626;
		public static ManualLogSource LogSource;
		public GameObject Hook;



		//BaseUnityPlugin inherits MonoBehaviour, so you can use base unity functions like Awake() and Update()
#pragma warning disable IDE0051
		private void Awake() //Awake() will run once when your plugin loads
#pragma warning restore IDE0051
		{
			if (!VersionChecker.CheckEftVersion(Logger, Info, Config))
			{
				throw new Exception("Invalid EFT Version");
			}
			MyConfig.InitializeConfig(Config);
			//we save the Logger to our LogSource variable so we can use it anywhere, such as in our patches via Plugin.LogSource.LogInfo(), etc.
			LogSource = Logger;
			//uncomment the line below and replace "PatchClassName" with the class name you gave your patch. Patches must be enabled like this to work.
			new BuffIconPatch().Enable();
			new BuffTooltipPatch().Enable();
			new SkillFatigueTimerPatch().Enable();
			new SkillFatigueTimerTooltipPatch().Enable();

			Hook = new GameObject("PluginHook"); // create a new gameobject instance along with its name
			Hook.AddComponent<SkillDeterminer>(); // mount mono script to hook using the class name of your mono script
			DontDestroyOnLoad(Hook); // add hook to DontDestroyOnLoad which will add it as a constant object into the game

			LogSource.LogMessage("plugin loaded!");
		}
	}
}

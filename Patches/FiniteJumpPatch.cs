using SPT.Reflection.Patching;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EFT;
using EFT.Communications;
using System.Runtime.CompilerServices;

//this example patch will limit the number of jumps you can do to 3, and log whether or not your jump was successful

namespace kvan.RaidSkillInfo.Patches
{
	internal class FiniteJumpPatch : ModulePatch //we must inherit ModulePatch for our patch to work.
	{
		protected override MethodBase GetTargetMethod()
		{
			//methods are patched by targeting both their class name and the name of the method itself.
			//the example in this patch is the Jump() method in the Player class
			return AccessTools.Method(typeof(Player), nameof(Player.Jump));
		}

		public static int maxJumps = 3;
		public static int completedJumps = 0;

		[PatchPrefix]
		static bool Prefix()
		{
			//code here will run BEFORE original code is executed.
			//if 'true' is returned, the original code will still run.
			//if 'false' is returned, the original code will be skipped.

			if (completedJumps <= maxJumps)
			{
				// completedJumps++;
				int remainingJumps = maxJumps - completedJumps++;

				//here we are using that LogSource variable we set up in the Plugin.cs file.
				string msg = $"You jumped! You have {remainingJumps} jumps left!";
				Plugin.LogSource.LogWarning(msg);

				NotificationManagerClass.DisplayMessageNotification(msg, ENotificationDurationType.Default);

				return true; //we return true to run the original code and jump!
			}
			else
			{
				Plugin.LogSource.LogError("You have no jumps left!");
				NotificationManagerClass.DisplayMessageNotification("You have no jumps left!", ENotificationDurationType.Default);
				return false; //we are out of jumps, so we return false, preventing the original code, and thus, the jump.
			}
		}

		[PatchPrefix]
		static void Postfix()
		{
			//code here will run AFTER the original code is executed.
		}

		//don't forget to add 'new FiniteJumpPatch().Enable();' to the Awake() method of your Plugin.cs script to enable this patch.
	}
}
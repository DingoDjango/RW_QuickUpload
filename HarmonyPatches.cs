using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Verse;
using RimWorld;
using System.Reflection;

namespace Quick_Upload
{
	[StaticConstructorOnStartup]
	public static class HarmonyPatches
	{
		static HarmonyPatches()
		{
#if DEBUG
			HarmonyInstance.DEBUG = true;
#endif

			Harmony harmony = new Harmony("dingo.quickupload");

			harmony.PatchAll(Assembly.GetExecutingAssembly());
		}

		[HarmonyPatch(typeof(Dialog_MessageBox))]
		[HarmonyPatch("InteractionDelayExpired")]
		[HarmonyPatch(MethodType.Getter)]
		public static class Patch_Dialog_MessageBox
		{
			// Negate 6-second cooldown when uploading mods
			public static void Postfix(ref bool __result)
			{
				__result = true;
			}
		}

		[HarmonyPatch(typeof(ModMetaData))]
		[HarmonyPatch(nameof(ModMetaData.GetWorkshopName))]
		public static class Patch_ModMetaData_WorkshopTitle
		{
			// Automatically adds a version tag when uploading to Workshop
			public static void Postfix(ModMetaData __instance, ref string __result)
			{
				if (Settings.TagWorkshopTitle)
				{
					int majorVer = 1;
					int minorVer = 2;

					List<Version> supportedVersions = __instance.SupportedVersionsReadOnly;

					foreach(Version version in supportedVersions)
					{
						if (version.Major >= majorVer && version.Minor > minorVer)
						{
							majorVer = version.Major;
							minorVer = version.Minor;
						}
					}

					string prefix = "[" + majorVer + "." + minorVer + "] "; // Returns "[1.0] ", "[0.19] "

					__result = prefix + __result; // Example result: "[1.0] Dingo's Modding Mod"
				}
			}
		}
	}
}

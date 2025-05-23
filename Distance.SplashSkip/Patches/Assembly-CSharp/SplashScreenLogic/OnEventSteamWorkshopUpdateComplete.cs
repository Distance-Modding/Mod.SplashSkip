﻿using Events.SteamWorkshop;
using HarmonyLib;

namespace Distance.SplashSkip.Patches
{
	/// <summary>
	/// Patch to perform half of the <see cref="Mod.SkipSplashAnimation"/> functionality.
	/// <para/>
	/// This stops the splash animations from being created.
	/// </summary>
	[HarmonyPatch(typeof(SplashScreenLogic), nameof(SplashScreenLogic.OnEventSteamWorkshopUpdateComplete))]
	internal static class SplashScreenLogic__OnEventSteamWorkshopUpdateComplete
	{

		internal static bool Prefix(SplashScreenLogic __instance, Complete.Data data)
		{
			__instance.fadeTimer_ = 0f;
			
			// Skipping this block will skip the REFRACT splash animation (and any other splash animations that could be defined).
			if (!Mod.SkipSplashAnimation.Value)
			{
				PlayCrossplatformMovie component = __instance.foregroundPanel_.GetComponent<PlayCrossplatformMovie>();
				component.Play();
				__instance.foregroundPanel_.GetComponentInChildren<UITexture>().mainTexture = component.GetTexture();
			}

			__instance.waitingForSteamWorshop_ = false;

			// Skip the original method. We've performed everything it would have.
			return false;
		}

	}
}

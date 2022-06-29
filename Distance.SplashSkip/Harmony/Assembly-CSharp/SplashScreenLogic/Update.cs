using HarmonyLib;
using System;
using UnityEngine;

namespace Distance.SplashSkip.Harmony
{
	/// <summary>
	/// Patch to perform <see cref="ConfigurationLogic.SkipIdleMenu"/> functionality,
	/// and half of the <see cref="ConfigurationLogic.SkipSplashAnimation"/> functionality.
	/// <para/>
	/// This ends splash animation fading ASAP, and changes the variable that determines which menu to boot into.
	/// </summary>
	[HarmonyPatch(typeof(SplashScreenLogic), nameof(SplashScreenLogic.Update))]
	internal static class SplashScreenLogic__Update
	{

		internal static bool Prefix(SplashScreenLogic __instance)
		{
			// We still want to wait for the Steam Workshop, since we're still gathering subscribed level info:
			//  i.e. Author names, ratings, etc.
			if (__instance.dontUpdate_ || __instance.waitingForSteamWorshop_)
			{
				// Skip the original method. It would just return after performing the same check again anyway.
				return false;
			}

			// Boot into Main Menu instead of Idle Menu.
			bool skipIdle = Mod.Instance.Config.SkipIdleMenu;
			if (skipIdle && G.Sys.GameManager_.openOnMainMenuInit_ == GameManager.OpenOnMainMenuInit.FancyIdleMenu)
			{
				G.Sys.GameManager_.openOnMainMenuInit_ = GameManager.OpenOnMainMenuInit.None;
			}


			// Immediately end the splash screen animation fading.
			if (Mod.Instance.Config.SkipSplashAnimation)
			{
				// Setting these fields will cause this next Update loop to finish immediately.
				// We use `splashScreens_.Length - 1` for `currentIndex_` because that value will
				//  be incremented before a checking for equality with `splashScreens_.Length`.
				__instance.allowSkip_ = true;
				__instance.fadeTimer_ = 0f;
				__instance.currentIndex_ = (__instance.splashScreens_.Length - 1);
				__instance.currentState_ = SplashScreenLogic.FadeState.FadingOut;

				// Fallthrough to the original method. The original logic will end the fading on this update.
				// Alternatively we could set the fields above to the state where the fading is done and call the following.
				//G.Sys.GameManager_.GoToMainMenu(GameManager.OpenOnMainMenuInit.UsePrevious);
				return true;
			}


			// Fallthrough to the original method, no behavior has been changed.
			return true;
		}

	}
}

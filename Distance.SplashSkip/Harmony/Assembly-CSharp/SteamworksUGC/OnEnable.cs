using Events;
using Events.GameLogic;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Distance.SplashSkip.Harmony
{
	/// <summary>
	/// Patch to perform <see cref="ConfigurationLogic.SkipWorkshopSubscriptions"/> functionality.
	/// <para/>
	/// This bypasses the branch for adding/removing new subscriptions, and goes straight to final setup.
	/// Final setup involves grabbing Workshop information such as author usernames, ratings, etc.
	/// </summary>
	[HarmonyPatch(typeof(SteamworksUGC), nameof(SteamworksUGC.OnEnable))]
	internal static class SteamworksUGC__OnEnable
	{

		internal static bool Prefix(SteamworksUGC __instance)
		{
			if (!SteamworksManager.IsSteamBuild_)
			{
				// Skip the original method. It would just return after performing the same check again anyway.
				return false;
			}

			__instance.storedPublishedFileIDs_ = WorkshopLevelInfos.LoadOrCreate();
			StaticEvent<Skip.Data>.Subscribe(new StaticEvent<Skip.Data>.Delegate(__instance.OnEventSkip));
			
			// Call the later branch in order to skip adding/removing newly-subscribed/unsubscribed workshop levels.
			bool skipChecks = Mod.Instance.Config.SkipWorkshopSubscriptions;
			if (!skipChecks && SteamworksUGC.IsOnline_ && ApplicationEx.LoadedLevelName_ == "SplashScreens")
			{
				__instance.DoNextFrame(delegate
				{
					__instance.UpdateSubscribedWorkshopLevels();
				});
			}
			else
			{
				__instance.DoNextFrame(new Action(__instance.MarkWorkshopLevelsUpdateAsFinished));
				__instance.DoNextFrame(new Action(__instance.BroadcastWorkshopUpdateComplete));
			}

			// Skip the original method. We've already performed everything it would have.
			return false;
		}

	}
}

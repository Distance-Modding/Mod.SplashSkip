using Centrifuge.Distance.Game;
using Centrifuge.Distance.GUI.Controls;
using Centrifuge.Distance.GUI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Reactor.API.Attributes;
using Reactor.API.Interfaces.Systems;
using Reactor.API.Logging;
using Reactor.API.Runtime.Patching;
using UnityEngine;

namespace Distance.SplashSkip
{
	/// <summary>
	/// The mod's main class containing its entry point
	/// </summary>
	[ModEntryPoint("com.github.trigger-segfault/Distance.SplashSkip")]
	public sealed class Mod : MonoBehaviour
	{
		public const string Name = "SplashSkip";
		public const string FullName = "Distance." + Name;
		public const string FriendlyName = "Splash Skip";


		public static Mod Instance { get; private set; }

		public IManager Manager { get; private set; }

		public Log Logger { get; private set; }

		public ConfigurationLogic Config { get; private set; }

		/// <summary>
		/// Method called as soon as the mod is loaded.
		/// WARNING:	Do not load asset bundles/textures in this function
		///				The unity assets systems are not yet loaded when this
		///				function is called. Loading assets here can lead to
		///				unpredictable behaviour and crashes!
		/// </summary>
		public void Initialize(IManager manager)
		{
			// Do not destroy the current game object when loading a new scene
			DontDestroyOnLoad(this);

			Instance = this;
			Manager = manager;

			Logger = LogManager.GetForCurrentAssembly();
			Config = gameObject.AddComponent<ConfigurationLogic>();

			Logger.Info(Mod.Name + ": Initializing...");

			try
			{
				CreateSettingsMenu();
			}
			catch (Exception ex)
			{
				Logger.Error(Mod.Name + ": Error during CreateSettingsMenu()");
				Logger.Exception(ex);
				throw;
			}

			try
			{
				RuntimePatcher.AutoPatch();
			}
			catch (Exception ex)
			{
				Logger.Error(Mod.Name + ": Error during RuntimePatcher.AutoPatch()");
				Logger.Exception(ex);
				throw;
			}

			Logger.Info(Mod.Name + ": Initialized!");
		}

		private void CreateSettingsMenu()
		{
			//Dictionary<string, StartupMenu> startupMenuDictionary = StartupMenuEx.GetSupportedMenus().ToDictionary(
			//	(menu) => StartupMenuEx.GetMenuName(menu));

			MenuTree settingsMenu = new MenuTree("menu.mod." + Mod.Name.ToLower(), Mod.FriendlyName + " Settings");

			settingsMenu.CheckBox(MenuDisplayMode.MainMenu,
				"setting:skip_splash_animation",
				"SKIP SPLASH ANIMATION",
				() => Config.SkipSplashAnimation,
				(value) => Config.SkipSplashAnimation = value,
				description: "Skip the REFRACT splash screen animation.");

			settingsMenu.CheckBox(MenuDisplayMode.MainMenu,
				"setting:skip_workshop_subscriptions",
				"SKIP WORKSHOP SUBSCRIPTIONS",
				() => Config.SkipWorkshopSubscriptions,
				(value) => Config.SkipWorkshopSubscriptions = value,
				description: "Skip startup checks for newly-subscribed/unsubscribed Steam Workshop levels.");

			settingsMenu.CheckBox(MenuDisplayMode.MainMenu,
				"setting:skip_idle_menu",
				"SKIP IDLE MENU",
				() => Config.SkipIdleMenu,
				(value) => Config.SkipIdleMenu = value,
				description: "Skip the 'press-any-key' Idle menu and boot into the Main Menu.");


			/*settingsMenu.ListBox<StartupMenu>(MenuDisplayMode.MainMenu,
				"setting:startup_mode",
				"STARTUP MENU MODE",
				() => Config.StartupMode,
				(value) => Config.StartupMode = value,
				startupMenuDictionary,
				description: "The initial menu or mode to boot into.");*/


			Menus.AddNew(MenuDisplayMode.MainMenu, settingsMenu,
				(Mod.FriendlyName + " Settings").ToUpper(),
				"Settings for faster game startup.");
		}
	}
}




using Reactor.API.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Distance.SplashSkip
{
	public class ConfigurationLogic : MonoBehaviour
	{
		#region Properties

		private const string SkipSplashAnimation_ID = "startup.skip_splash_animation";
		public bool SkipSplashAnimation
		{
			get => Get<bool>(SkipSplashAnimation_ID);
			set => Set(SkipSplashAnimation_ID, value);
		}

		private const string SkipWorkshopSubscriptions_ID = "startup.skip_workshop_subscriptions";
		public bool SkipWorkshopSubscriptions
		{
			get => Get<bool>(SkipWorkshopSubscriptions_ID);
			set => Set(SkipWorkshopSubscriptions_ID, value);
		}

		private const string SkipIdleMenu_ID = "startup.skip_idle_menu";
		public bool SkipIdleMenu
		{
			get => Get<bool>(SkipIdleMenu_ID);
			set => Set(SkipIdleMenu_ID, value);
		}

		// Future plans:
		//  Boot into the Main Menu or one of the other modes, like Sprint, Level Editor, etc.
		//  NOTE: This will replace `SkipIdleMenu`.
		/*private const string StartupMode_ID = "startup.mode";
		public StartupMenu StartupMode
		{
			get => Get<StartupMenu>(StartupMode_ID);
			set => Set(StartupMode_ID, value);
		}*/


		#endregion

		internal Settings Config;

		public event Action<ConfigurationLogic> OnChanged;

		private void Load()
		{
			Config = new Settings("Config");// Mod.FullName);
		}

		public void Awake()
		{
			Load();

			// Assign default settings (if not already assigned).
			Get(SkipSplashAnimation_ID, true);
			Get(SkipWorkshopSubscriptions_ID, true);
			Get(SkipIdleMenu_ID, true);
			//Get(StartupMode_ID, StartupMenu.Main_Menu);

			// Save settings, and any defaults that may have been added.
			Save();
		}

		public T Get<T>(string key, T @default = default)
		{
			return Config.GetOrCreate(key, @default);
		}

		public void Set<T>(string key, T value)
		{
			Config[key] = value;
			Save();
		}

		public void Save()
		{
			Config?.Save();
			OnChanged?.Invoke(this);
		}
	}
}

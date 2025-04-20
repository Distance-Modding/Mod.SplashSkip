using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace Distance.SplashSkip
{
	/// <summary>
	/// The mod's main class containing its entry point
	/// </summary>
	[BepInPlugin(modGUID, modName, modVersion)]
	public sealed class Mod : BaseUnityPlugin
	{
		//Mod Details
		private const string modGUID = "Distance.SplashSkip";
		private const string modName = "Splash Skip";
		private const string modVersion = "1.0.0";

		//Config Entry Strings
		public static string SkipSplashKey = "Skip Splash Animation";
		public static string SkipWorkshopKey = "Skip Workshop Subscriptions";
		public static string SkipIdleKey = "Skip Idle Menu";

		//Config Entries
		public static ConfigEntry<bool> SkipSplashAnimation { get; set; }
		public static ConfigEntry<bool> SkipWorkshopSubscriptions { get; set; }
		public static ConfigEntry<bool> SkipIdleMenu { get; set; }

		//Other
		private static readonly Harmony harmony = new Harmony(modGUID);
        public static ManualLogSource Log = new ManualLogSource(modName);
        public static Mod Instance;

		/// <summary>
		/// Method called as soon as the mod is loaded.
		/// WARNING:	Do not load asset bundles/textures in this function
		///				The unity assets systems are not yet loaded when this
		///				function is called. Loading assets here can lead to
		///				unpredictable behaviour and crashes!
		/// </summary>
		public void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}

			Log = BepInEx.Logging.Logger.CreateLogSource(modGUID);


			//Config Setup
			SkipSplashAnimation = Config.Bind("General",
				SkipSplashKey,
				true,
				new ConfigDescription("Skip the REFRACT splash screen animation."));

			SkipWorkshopSubscriptions = Config.Bind("General",
				SkipWorkshopKey,
				true,
				new ConfigDescription("Skip startup checks for newly-subscribed/unsubscribed Steam Workshop levels."));

			SkipIdleMenu = Config.Bind("General",
				SkipIdleKey,
				true,
				new ConfigDescription("Skip the 'press-any-key' Idle menu and boot into the Main Menu."));

			Log.LogInfo(modName + ": Initializing...");
			harmony.PatchAll();
			Log.LogInfo(modName + ": Initialized!");
		}
	}
}




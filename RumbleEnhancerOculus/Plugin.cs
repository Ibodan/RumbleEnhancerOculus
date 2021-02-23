extern alias HMLib;
using System;
using System.Linq;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using HarmonyLib;
using IPALogger = IPA.Logging.Logger;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace CustomHapticFeedback
{
	[Plugin(RuntimeOptions.SingleStartInit)]
	internal class Plugin
	{
		public static byte[] CutClip;
		public static byte[] MissCutClip;
		public static byte[] UIClip;
		public static byte[] BombClip;
		public static byte[] ClashClip;
		public static byte[] ObstacleClip;

		public static IPALogger logger;
		public static GenericUpdater updater = null;

		private Harmony harmony;
		
		[Init]
		public Plugin(IPALogger logger, Config config)
		{
			Plugin.logger = logger;
			PluginConfig.Instance = config.Generated<PluginConfig>();

			Plugin.logger.Debug("CustomHapticFeedback Initialized, using settings:");
			PluginConfig.Instance.LogValues();
		}

		[OnStart]
		public void OnApplicationStart()
		{
			harmony = new Harmony("ibodan.beatsaber.CustomHapticFeedback");
			harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
			logger.Debug("Harmony patches applied.");

			var deviceName = UnityEngine.XR.XRSettings.loadedDeviceName;
			HMLib::PersistentSingleton<MyHapticFeedbackController>.instance.InjectDriver(
				deviceName == "Oculus" ?
				(HapticFeedbackDriver)new OculusHapticFeedbackDriver() :
				(HapticFeedbackDriver)new SteamHapticFeedbackDriver());
			logger.Debug("Haptic feedback driver injected for: " + deviceName);

			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (updater == null)
			{
				GameObject obj = new GameObject("CustomHapticFeedbackUpdater");
				updater = obj.AddComponent<GenericUpdater>();
				GameObject.DontDestroyOnLoad(obj);
				updater.OnUpdate += HMLib::PersistentSingleton<MyHapticFeedbackController>.instance.OnUpdate;
				logger.Debug("Updater ready");
			}
		}

		[OnExit]
		public void OnApplicationQuit()
		{
		}

		public static void LoadConfig(PluginConfig config)
		{
			Func<string, byte[]> createHapticsClip = (string strPattern) =>
			{
				var pattern = strPattern.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => byte.Parse(s)).ToArray();

				if (pattern.Count() == 0) pattern = new byte[] { 9, 9, 0, 0, 0 };

				var clip = new byte[pattern.Count()];
				for (int i = 0; i < clip.Length; i++)
				{
					clip[i] = pattern[i];
				}
				return clip;
			};

			CutClip = createHapticsClip(config.CutClip);
			MissCutClip = createHapticsClip(config.MissCutClip);
			BombClip = createHapticsClip(config.BombClip);
			UIClip = createHapticsClip(config.UIClip);
			ClashClip = createHapticsClip(config.SaberClashClip);
			ObstacleClip = createHapticsClip(config.ObstacleClip);
		}
	}

	internal class GenericUpdater: MonoBehaviour
	{
		public delegate void UpdateEventHandler();
		public event UpdateEventHandler OnUpdate;
		public void LateUpdate()
		{
			OnUpdate?.Invoke();
		}
	}
}

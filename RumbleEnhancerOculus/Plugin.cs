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

		public static float[] OculusBiasTable;
		public static float[] SteamBiasTable;

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
			Func<string, byte[]> decodeHapticsClip = (string strPattern) =>
			{
				return strPattern.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => byte.Parse(s)).ToArray();
			};

			Func<string, float[]> decodeBiasTable = (string strPattern) =>
			{
				var pattern = strPattern.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => float.Parse(s)).ToArray();
				if (pattern.Count() != 10) pattern = new float[] { 0.0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 1f };
				return pattern;
			};

			CutClip = decodeHapticsClip(config.CutClip);
			MissCutClip = decodeHapticsClip(config.MissCutClip);
			BombClip = decodeHapticsClip(config.BombClip);
			UIClip = decodeHapticsClip(config.UIClip);
			ClashClip = decodeHapticsClip(config.SaberClashClip);
			ObstacleClip = decodeHapticsClip(config.ObstacleClip);

			OculusBiasTable = decodeBiasTable(config.OculusBiasTable);
			SteamBiasTable = decodeBiasTable(config.SteamBiasTable);
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

using System;
using System.Linq;
using IPA;
using IPA.Config;
using Harmony;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

namespace RumbleEnhancerOculus
{
	public class Plugin : IBeatSaberPlugin
	{
		public static OVRHapticsClip CutClip;
		public static OVRHapticsClip MissCutClip;
		public static OVRHapticsClip UIClip;
		public static OVRHapticsClip BombClip;
		public static OVRHapticsClip ClashClip;
		public static OVRHapticsClip ObstacleClip;

		internal static IPALogger logger;
		
		public void Init(IPALogger logger, [Config.Prefer("json")] IConfigProvider configProvider)
		{
			IPA.Logging.StandardLogger.PrintFilter = IPA.Logging.Logger.LogLevel.All;
			Plugin.logger = logger;
			var config = configProvider.MakeLink<PluginConfig>((p, v) =>
			{
				if (v.Value._fresh)
				{
					Plugin.logger.Debug("Generating settings file.");
					v.Value._fresh = false;
					p.Store(v.Value);
				}
			});
			Plugin.logger.Debug("RumbleEnhancerOculus Initialized, using settings:");
			config.Value.LogValues();

			CutClip = createHapticsClip(config.Value.CutClip);
			MissCutClip = createHapticsClip(config.Value.MissCutClip);
			BombClip = createHapticsClip(config.Value.BombClip);
			UIClip = createHapticsClip(config.Value.UIClip);
			ClashClip = createHapticsClip(config.Value.SaberClashClip);
			ObstacleClip = createHapticsClip(config.Value.ObstacleClip);
		}

		private OVRHapticsClip createHapticsClip(string strPattern)
		{
			var pattern = strPattern.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => byte.Parse(s)).ToArray();

			if (pattern.Count() == 0) pattern = new byte[] { 255, 255, 0, 0, 0, 0 };

			var clip = new OVRHapticsClip(pattern.Count());
			for (int i = 0; i < clip.Capacity; i++)
			{
				clip.WriteSample(pattern[i]);
			}
			return clip;
		}

		public void OnApplicationStart()
		{
			SharedCoroutineStarter.instance.StartCoroutine(Patch());
		}

		private IEnumerator Patch()
		{
			yield return new WaitForSecondsRealtime(0.2f);
			var harmony = HarmonyInstance.Create("HapticTest");
			harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
			logger.Debug("Harmony patches applied now.");
		}

		public void OnApplicationQuit()
		{
		}

		public void OnLevelWasLoaded(int level)
		{
		}

		public void OnLevelWasInitialized(int level)
		{
		}

		public void OnUpdate()
		{
		}

		public void OnFixedUpdate()
		{
		}

		public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
		{
		}

		public void OnSceneUnloaded(Scene scene)
		{
		}

		public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
		{
		}
	}
}

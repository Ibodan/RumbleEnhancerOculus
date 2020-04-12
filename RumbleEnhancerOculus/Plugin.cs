using System;
using System.Linq;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using HarmonyLib;
using UnityEngine;
using System.Collections;
using IPALogger = IPA.Logging.Logger;

namespace RumbleEnhancerOculus
{
	[Plugin(RuntimeOptions.SingleStartInit)]
	internal class Plugin
	{
		public static OVRHapticsClip CutClip;
		public static OVRHapticsClip MissCutClip;
		public static OVRHapticsClip UIClip;
		public static OVRHapticsClip BombClip;
		public static OVRHapticsClip ClashClip;
		public static OVRHapticsClip ObstacleClip;

		public static IPALogger logger;

		private Harmony harmony;
		
		[Init]
		public Plugin(IPALogger logger, Config config)
		{
			Plugin.logger = logger;
			PluginConfig.Instance = config.Generated<PluginConfig>();

			Plugin.logger.Debug("RumbleEnhancerOculus Initialized, using settings:");
			PluginConfig.Instance.LogValues();
		}

		[OnStart]
		public void OnApplicationStart()
		{
			harmony = new Harmony("ibodan.beatsaber.RumbleEnhancerOculus");
			harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
			logger.Debug("Harmony patches applied now.");
		}

		[OnExit]
		public void OnApplicationQuit()
		{
		}

		public static void LoadConfig(PluginConfig config)
		{
			Func<string, OVRHapticsClip> createHapticsClip = (string strPattern) =>
			{
				var pattern = strPattern.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => byte.Parse(s)).ToArray();

				if (pattern.Count() == 0) pattern = new byte[] { 255, 255, 0, 0, 0, 0 };

				var clip = new OVRHapticsClip(pattern.Count());
				for (int i = 0; i < clip.Capacity; i++)
				{
					clip.WriteSample(pattern[i]);
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
}

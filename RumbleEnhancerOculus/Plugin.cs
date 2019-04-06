using System;
using System.Linq;
using IllusionPlugin;
using Harmony;
using UnityEngine;
using System.Collections;

namespace RumbleEnhancerOculus
{
	public class Plugin : IPlugin
	{
		public static OVRHapticsClip CutClip;
		public static OVRHapticsClip MissCutClip;
		public static OVRHapticsClip UIClip;
		public static OVRHapticsClip BombClip;
		public static OVRHapticsClip ClashClip;
		public static OVRHapticsClip ObstacleClip;
		public static float CutRumbleDuration;

		public string Name => "RumbleEnhancerOculus";
		public string Version => "1.0.6";

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

		public static void Log(string s)
		{
			Console.WriteLine("[RumbleEnhancerOculus] " + s);
		}

		public void OnApplicationStart()
		{
			SharedCoroutineStarter.instance.StartCoroutine(Patch());

			CutClip =      createHapticsClip(ModPrefs.GetString(Name, "CutClip", "0,0,0,0,255,255,255,0,255,255,255,0,255,255,0,255,255,0,200,200,0,200,200,0,120,120,0,90,90,0,90,90", true));
			MissCutClip =  createHapticsClip(ModPrefs.GetString(Name, "MissCutClip", "0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255", true));
			BombClip =     createHapticsClip(ModPrefs.GetString(Name, "BombClip", "0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255", true));
			UIClip =       createHapticsClip(ModPrefs.GetString(Name, "UIClip",         "80,100,0,0,0,0", true));
			ClashClip =    createHapticsClip(ModPrefs.GetString(Name, "SaberClashClip", "45,90,135,180,0,0,0,0,0,0", true));
			ObstacleClip = createHapticsClip(ModPrefs.GetString(Name, "ObstacleClip",   "255,255,255,0,255,255,255,0", true));
			CutRumbleDuration = 0.0f;
		}

		private IEnumerator Patch()
		{
			yield return new WaitForSecondsRealtime(0.2f);
			var harmony = HarmonyInstance.Create("HapticTest");
			harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());
			Console.WriteLine("[RumbleEnhancerOculus] patch applied now.");
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
	}
}

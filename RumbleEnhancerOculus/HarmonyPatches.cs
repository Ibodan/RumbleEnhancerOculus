extern alias HMLib;
using HarmonyLib;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.XR;

namespace CustomHapticFeedback
{
	[HarmonyPatch(typeof(NoteCutCoreEffectsSpawner))]
	[HarmonyPatch("SpawnNoteCutEffect")]
	public static class NoteCutEffectSpawnerSpawnNoteCutEffectPatch
	{
		static void Prefix(Vector3 pos, NoteController noteController, NoteCutInfo noteCutInfo)
		{
			XRNode node = noteCutInfo.saberType == SaberType.SaberA ? XRNode.LeftHand : XRNode.RightHand;

			if (noteCutInfo.allIsOK)
			{
				HMLib::PersistentSingleton<MyHapticFeedbackController>.instance.Rumble(node, Plugin.CutClip);
			}
			else
			{
				HMLib::PersistentSingleton<MyHapticFeedbackController>.instance.Rumble(node, Plugin.MissCutClip);
			}
		}
	}

	[HarmonyPatch(typeof(NoteCutCoreEffectsSpawner))]
	[HarmonyPatch("SpawnBombCutEffect")]
	public static class NoteCutEffectSpawnerSpawnBombCutEffectPatch
	{
		static void Prefix(Vector3 pos, NoteController noteController, NoteCutInfo noteCutInfo)
		{
			XRNode node = noteCutInfo.saberType == SaberType.SaberA ? XRNode.LeftHand : XRNode.RightHand;
			HMLib::PersistentSingleton<MyHapticFeedbackController>.instance.Rumble(node, Plugin.BombClip);
		}
	}

	[HarmonyPatch(typeof(HMLib::HapticFeedbackController))]
	[HarmonyPatch("PlayHapticFeedback")]
	public static class HapticFeedbackControllerContinuousRumblePatch
	{
		static bool Prefix(XRNode node, HMLib.Libraries.HM.HMLib.VR.HapticPresetSO hapticPreset)
		{
			var stack = new StackTrace(2, false);
			var typename = stack.GetFrame(0).GetMethod().DeclaringType.Name;
			if (typename.StartsWith("SaberCl")) // SaberClashEffect
			{
				HMLib::PersistentSingleton<MyHapticFeedbackController>.instance.ContinuousRumble(node, Plugin.ClashClip);
			}
			else if (typename.StartsWith("SiraObsta")) // SiraObstacleSaberSparkleEffectManager from "SiraUtil"
			{
				HMLib::PersistentSingleton<MyHapticFeedbackController>.instance.ContinuousRumble(node, Plugin.ObstacleClip);
			}
			else if (typename.StartsWith("ObstacleS")) // ObstacleSaberSparkleEffectManager
			{
				HMLib::PersistentSingleton<MyHapticFeedbackController>.instance.ContinuousRumble(node, Plugin.ObstacleClip);
			}
			else if (typename.StartsWith("VRInputMo")) // VRInputModule
			{
				HMLib::PersistentSingleton<MyHapticFeedbackController>.instance.Rumble(node, Plugin.UIClip);
			}
			return false;
		}
	}
}

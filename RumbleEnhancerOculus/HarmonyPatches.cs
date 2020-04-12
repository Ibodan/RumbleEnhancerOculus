extern alias HMLib;
using HarmonyLib;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.XR;

namespace RumbleEnhancerOculus
{
	[HarmonyPatch(typeof(NoteCutEffectSpawner))]
	[HarmonyPatch("SpawnNoteCutEffect")]
	public static class NoteCutEffectSpawnerSpawnNoteCutEffectPatch
	{
		static void Prefix(Vector3 pos, INoteController noteController, NoteCutInfo noteCutInfo)
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

	[HarmonyPatch(typeof(NoteCutEffectSpawner))]
	[HarmonyPatch("SpawnBombCutEffect")]
	public static class NoteCutEffectSpawnerSpawnBombCutEffectPatch
	{
		static void Prefix(Vector3 pos, INoteController noteController, NoteCutInfo noteCutInfo)
		{
			XRNode node = noteCutInfo.saberType == SaberType.SaberA ? XRNode.LeftHand : XRNode.RightHand;
			HMLib::PersistentSingleton<MyHapticFeedbackController>.instance.Rumble(node, Plugin.BombClip);
		}
	}

	[HarmonyPatch(typeof(HMLib::HapticFeedbackController))]
	[HarmonyPatch("ContinuousRumble")]
	public static class HapticFeedbackControllerContinuousRumblePatch
	{
		static bool Prefix(XRNode node)
		{
			var stack = new StackTrace(2, false);
			var typename = stack.GetFrame(0).GetMethod().DeclaringType.Name;
			if (typename == "SaberClashEffect")
			{
				HMLib::PersistentSingleton<MyHapticFeedbackController>.instance.ContinuousRumble(node, Plugin.ClashClip);
			}
			else
			{
				HMLib::PersistentSingleton<MyHapticFeedbackController>.instance.ContinuousRumble(node, Plugin.ObstacleClip);
			}
			return false;
		}
	}

	[HarmonyPatch(typeof(HMLib::HapticFeedbackController))]
	[HarmonyPatch("Rumble")]
	public static class HapticFeedbackControllerRumblePatch
	{
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			return new List<CodeInstruction>() { new CodeInstruction(OpCodes.Ret) };
		}
	}

	[HarmonyPatch(typeof(HMLib::OculusVRHelper))]
	[HarmonyPatch("TriggerHapticPulse")]
	public static class OculusVRHelperTriggerHapticPulsePatch
	{
		static bool Prefix(XRNode node, float strength)
		{
			//var stack = new StackTrace(3, false);
			//if (stack.GetFrame(0).GetMethod().DeclaringType.Name == "VRInputModule")
			if (strength == 0.25f)
			{
				HMLib::PersistentSingleton<MyHapticFeedbackController>.instance.Rumble(node, Plugin.UIClip);
				return false;
			}
			return true;
		}
	}
}

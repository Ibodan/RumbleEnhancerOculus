using Harmony;
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
		static void Prefix(Vector3 pos, NoteController noteController, NoteCutInfo noteCutInfo)
		{
			XRNode node = noteCutInfo.saberType == Saber.SaberType.SaberA ? XRNode.LeftHand : XRNode.RightHand;

			if (noteCutInfo.allIsOK)
			{
				PersistentSingleton<MyHapticFeedbackController>.instance.Rumble(node, Plugin.CutClip, Plugin.CutRumbleDuration);
			}
			else
			{
				PersistentSingleton<MyHapticFeedbackController>.instance.Rumble(node, Plugin.MissCutClip, Plugin.CutRumbleDuration);
			}
		}
	}

	[HarmonyPatch(typeof(NoteCutEffectSpawner))]
	[HarmonyPatch("SpawnBombCutEffect")]
	public static class NoteCutEffectSpawnerSpawnBombCutEffectPatch
	{
		static void Prefix(Vector3 pos, NoteController noteController, NoteCutInfo noteCutInfo)
		{
			XRNode node = noteCutInfo.saberType == Saber.SaberType.SaberA ? XRNode.LeftHand : XRNode.RightHand;
			PersistentSingleton<MyHapticFeedbackController>.instance.Rumble(node, Plugin.BombClip, Plugin.CutRumbleDuration);
		}
	}

	[HarmonyPatch(typeof(HapticFeedbackController))]
	[HarmonyPatch("ContinuousRumble")]
	public static class HapticFeedbackControllerContinuousRumblePatch
	{
		static void Prefix(XRNode node)
		{
			var stack = new StackTrace(2, false);
			var typename = stack.GetFrame(0).GetMethod().DeclaringType.Name;
			if (typename == "SaberClashEffect")
			{
				PersistentSingleton<MyHapticFeedbackController>.instance.ContinuousRumble(node, Plugin.ClashClip);
			}
			else
			{
				PersistentSingleton<MyHapticFeedbackController>.instance.ContinuousRumble(node, Plugin.ObstacleClip);
			}
		}
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			return new List<CodeInstruction>() { new CodeInstruction(OpCodes.Ret) };
		}
	}

	[HarmonyPatch(typeof(HapticFeedbackController))]
	[HarmonyPatch("Rumble")]
	public static class HapticFeedbackControllerRumblePatch
	{
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			return new List<CodeInstruction>() { new CodeInstruction(OpCodes.Ret) };
		}
	}

	[HarmonyPatch(typeof(OculusVRHelper))]
	[HarmonyPatch("TriggerHapticPulse")]
	public static class OculusVRHelperTriggerHapticPulsePatch
	{
		static void Prefix(XRNode node, float strength)
		{
			//var stack = new StackTrace(3, false);
			//if (stack.GetFrame(0).GetMethod().DeclaringType.Name == "VRInputModule")
			if (strength == 0.25f)
			{
				PersistentSingleton<MyHapticFeedbackController>.instance.Rumble(node, Plugin.UIClip, 0f);
			}
		}
		static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			return new List<CodeInstruction>() { new CodeInstruction(OpCodes.Ret) };
		}
	}
}

using System;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

namespace CustomHapticFeedback
{
	interface HapticFeedbackDriver
	{
		void TriggerHapticPulse(XRNode node, byte strength);
		void StopHapticPulse(XRNode node);
	}
	
	public class OculusHapticFeedbackDriver : HapticFeedbackDriver
	{
		public void TriggerHapticPulse(XRNode node, byte strength)
		{
			OVRPlugin.SetControllerVibration(node == XRNode.LeftHand ? 1u : 2u, 1.0f, (float)(strength / 9.0));
		}

		public void StopHapticPulse(XRNode node)
		{
			OVRPlugin.SetControllerVibration(node == XRNode.LeftHand ? 1u : 2u, 0, 0);
		}
	}

	public class SteamHapticFeedbackDriver : HapticFeedbackDriver
	{
		private readonly float[] biasTable = new float[] {
			0.0f,
			0.025f,// 0.0212f,
			0.06f,//35 0.0603f,
			0.095f,//35 0.103f,
			0.13f,//35 0.1401f,
			0.165f,//35 0.1655  0.0875
			0.2f, //35 0.18      0.175
			0.25f,//50 0.215
			0.50f,
			1.0f
		};

		public void TriggerHapticPulse(XRNode node, byte strength)
		{
			float pulseDuration = Time.smoothDeltaTime * 1000000.0f;
			float bias = biasTable[Math.Min(9, (int)strength)];
			float F = pulseDuration * bias * (strength / 9.0f);
			CVRSystem system = OpenVR.System;
			ETrackedControllerRole unDeviceType = (node == XRNode.LeftHand) ? ETrackedControllerRole.LeftHand : ETrackedControllerRole.RightHand;
			uint trackedDeviceIndexForControllerRole = system.GetTrackedDeviceIndexForControllerRole(unDeviceType);
			system.TriggerHapticPulse(trackedDeviceIndexForControllerRole, 0u, (char)F);
		}

		public void StopHapticPulse(XRNode node)
		{
		}
	}
}

using System;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

namespace CustomHapticFeedback
{
	interface HapticFeedbackDriver {
		void TriggerHapticPulse(XRNode node, byte strength);
	}
	
	public class OculusHapticFeedbackDriver : HapticFeedbackDriver {
		private int idleFrames = 0; 
		public void TriggerHapticPulse(XRNode node, byte strength) {
			if (strength == 0) {
				idleFrames++;
				OVRPlugin.SetControllerVibration(node == XRNode.LeftHand ? 1u : 2u, 0.2f, (idleFrames < 500) ? 0.001f : 0);
				return;
			}
			idleFrames = 0;
			OVRPlugin.SetControllerVibration(node == XRNode.LeftHand ? 1u : 2u, 0.2f, Plugin.OculusBiasTable[Math.Min(9, (int)strength)]);
		}
	}

	public class SteamHapticFeedbackDriver : HapticFeedbackDriver {
		public void TriggerHapticPulse(XRNode node, byte strength) {
			if (strength == 0) return;

			float pulseDuration = Time.smoothDeltaTime * 1000000.0f;
			float biasedStrength = Plugin.SteamBiasTable[Math.Min(9, (int)strength)];
			float F = pulseDuration * biasedStrength;
			CVRSystem system = OpenVR.System;
			ETrackedControllerRole unDeviceType = (node == XRNode.LeftHand) ? ETrackedControllerRole.LeftHand : ETrackedControllerRole.RightHand;
			uint trackedDeviceIndexForControllerRole = system.GetTrackedDeviceIndexForControllerRole(unDeviceType);
			system.TriggerHapticPulse(trackedDeviceIndexForControllerRole, 0u, (char)F);
		}
	}
}

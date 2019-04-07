using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace RumbleEnhancerOculus
{
	class MyHapticFeedbackController : PersistentSingleton<MyHapticFeedbackController>
	{
		private const float _kSampleHz = 320.0f;

		private struct CRumbleKey
		{
			public XRNode node;
			public OVRHapticsClip clip;
		}

		private class CRumbleValue
		{
			public bool ping = false;
			public bool finished = true;
			public WaitForSecondsRealtime waiter = null;

			public CRumbleValue(OVRHapticsClip clip)
			{
				waiter = new WaitForSecondsRealtime(clip.Count / _kSampleHz);
			}
		}

		private Dictionary<CRumbleKey, CRumbleValue> _contRumbleStatuses = new Dictionary<CRumbleKey, CRumbleValue>();

		private void TriggerHapticFeedback(XRNode node, OVRHapticsClip clip)
		{
			OVRHaptics.OVRHapticsChannel channel = (node != XRNode.LeftHand) ? OVRHaptics.RightChannel : OVRHaptics.LeftChannel;
			channel.Mix(clip);
		}

		public void Rumble(XRNode node, OVRHapticsClip clip)
		{
			TriggerHapticFeedback(node, clip);
		}

		public void ContinuousRumble(XRNode node, OVRHapticsClip clip)
		{
			CRumbleKey key;
			key.node = node;
			key.clip = clip;
			if (_contRumbleStatuses.ContainsKey(key) == false)
			{
				_contRumbleStatuses.Add(key, new CRumbleValue(clip));
			}
			if (_contRumbleStatuses[key].finished)
			{
				SharedCoroutineStarter.instance.StartCoroutine(ContinuousRumbleCoroutine(key));
			}
			_contRumbleStatuses[key].ping = true;
		}

		private System.Collections.IEnumerator ContinuousRumbleCoroutine(CRumbleKey statusKey)
		{
			var node = statusKey.node;
			var clip = statusKey.clip;
			_contRumbleStatuses[statusKey].finished = false;
			do
			{
				_contRumbleStatuses[statusKey].ping = false;
				TriggerHapticFeedback(node, clip);
				yield return _contRumbleStatuses[statusKey].waiter;
			}
			while (_contRumbleStatuses[statusKey].ping);
			_contRumbleStatuses[statusKey].finished = true;
		}
	}
}

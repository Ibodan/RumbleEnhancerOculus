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
		}

		private struct CWaiterKey
		{
			public XRNode node;
			public float duration;
		}

		private Dictionary<CRumbleKey, CRumbleValue> _contRumbleStatuses = new Dictionary<CRumbleKey, CRumbleValue>();

		private Dictionary<CWaiterKey, CustomYieldInstruction> _waiterCache = new Dictionary<CWaiterKey, CustomYieldInstruction>();

		private CustomYieldInstruction GetWaiterFor(XRNode node, float duration)
		{
			CWaiterKey key;
			key.node = node;
			key.duration = duration;
			
			if (_waiterCache.ContainsKey(key) == false)
			{
				_waiterCache.Add(key, new WaitForSecondsRealtime(duration));
			}
			return _waiterCache[key];
		}

		private void TriggerHapticFeedback(XRNode node, OVRHapticsClip clip)
		{
			OVRHaptics.OVRHapticsChannel channel = (node != XRNode.LeftHand) ? OVRHaptics.RightChannel : OVRHaptics.LeftChannel;
			channel.Mix(clip);
		}

		public void Rumble(XRNode node, OVRHapticsClip clip, float duration)
		{
			if (duration < clip.Count / _kSampleHz)
			{
				TriggerHapticFeedback(node, clip);
				return;
			}

			SharedCoroutineStarter.instance.StartCoroutine(RumbleCoroutine(node, clip, duration));
		}

		public void ContinuousRumble(XRNode node, OVRHapticsClip clip)
		{
			CRumbleKey key;
			key.node = node;
			key.clip = clip;
			if (_contRumbleStatuses.ContainsKey(key) == false)
			{
				_contRumbleStatuses.Add(key, new CRumbleValue());
			}
			if (_contRumbleStatuses[key].finished)
			{
				SharedCoroutineStarter.instance.StartCoroutine(ContinuousRumbleCoroutine(key));
			}
			_contRumbleStatuses[key].ping = true;
		}

		private System.Collections.IEnumerator RumbleCoroutine(XRNode node, OVRHapticsClip clip, float duration)
		{
			float clipDuration = clip.Count / _kSampleHz;
			var waiter = GetWaiterFor(node, clipDuration);
			do
			{
				TriggerHapticFeedback(node, clip);
				yield return waiter;
				duration -= clipDuration;
			}
			while (duration > 0);
		}

		private System.Collections.IEnumerator ContinuousRumbleCoroutine(CRumbleKey statusKey)
		{
			var node = statusKey.node;
			var clip = statusKey.clip;
			float clipDuration = clip.Count / _kSampleHz;
			var waiter = GetWaiterFor(statusKey.node, clipDuration);
			_contRumbleStatuses[statusKey].finished = false;
			do
			{
				_contRumbleStatuses[statusKey].ping = false;
				TriggerHapticFeedback(node, clip);
				yield return waiter;
			}
			while (_contRumbleStatuses[statusKey].ping);
			_contRumbleStatuses[statusKey].finished = true;
		}
	}
}

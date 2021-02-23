extern alias HMLib;
using System.Collections.Generic;
using UnityEngine.XR;
using System;

namespace CustomHapticFeedback
{
	class MyHapticFeedbackController : HMLib::PersistentSingleton<MyHapticFeedbackController>
	{
		private struct ContRumbleKey
		{
			public XRNode node;
			public byte[] clip;
		}

		private class ContRumbleValue
		{
			public bool ping = false;
			public bool playing { get { return cursor < length; } }
			public int cursor = 0;
			private readonly int length = 0;

			public ContRumbleValue(byte[] clip)
			{
				length = clip.Length;
				cursor = length;
			}
		}

		private Dictionary<ContRumbleKey, ContRumbleValue> contRumbles = new Dictionary<ContRumbleKey, ContRumbleValue>();

		private HapticFeedbackChannel rightChannel = null;
		private HapticFeedbackChannel leftChannel = null;

		public void InjectDriver(HapticFeedbackDriver driver)
		{
			if (rightChannel != null) return;

			rightChannel = new HapticFeedbackChannel(XRNode.RightHand, driver);
			leftChannel = new HapticFeedbackChannel(XRNode.LeftHand, driver);
		}

		public void Rumble(XRNode node, byte[] clip)
		{
			TriggerHapticFeedback(node, clip);
		}

		public void ContinuousRumble(XRNode node, byte[] clip)
		{
			ContRumbleKey key;
			key.node = node;
			key.clip = clip;
			if (contRumbles.ContainsKey(key) == false)
			{
				contRumbles.Add(key, new ContRumbleValue(clip));
			}
			contRumbles[key].ping = true;
		}

		private void TriggerHapticFeedback(XRNode node, byte[] clip)
		{
			HapticFeedbackChannel channel = (node == XRNode.RightHand) ? rightChannel : leftChannel;
			channel.Mix(clip);
		}

		public void OnUpdate()
		{
			foreach (KeyValuePair<ContRumbleKey, ContRumbleValue> i in contRumbles)
			{
				if (i.Value.ping)
				{
					if (!i.Value.playing)
					{
						i.Value.cursor = 0;
					}
					if (i.Value.cursor % 2 == 0 && i.Value.playing)
					{
						i.Value.ping = false;
						TriggerHapticFeedback(i.Key.node, i.Key.clip.range(i.Value.cursor, 2));
					}
				}
			}

			rightChannel.Update();
			leftChannel.Update();

			foreach (KeyValuePair<ContRumbleKey, ContRumbleValue> i in contRumbles)
			{
				if (i.Value.playing) { i.Value.cursor++; }
			}
		}
	}

	public static class Extension
	{
		public static byte[] range(this byte[] src, int position, int length)
		{
			length = Math.Min(src.Length - position, length);
			if (length <= 0) { return new byte[0];  }

			byte[] ret = new byte[length];
			for(int i = 0; i < length; i++)
			{
				ret[i] = src[position + i];
			}
			return ret;
		}
	}
}

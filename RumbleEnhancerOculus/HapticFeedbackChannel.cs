extern alias HMLib;
using System;
using UnityEngine.XR;

namespace CustomHapticFeedback
{
	class HapticFeedbackChannel
	{
		private HapticFeedbackDriver driver;
		private XRNode node;

		private const int bufferMask = 0xff;
		private byte[] buffer;
		private int bufferCursor = 0;

		public HapticFeedbackChannel(XRNode node, HapticFeedbackDriver driver)
		{
			this.node = node;
			this.driver = driver;
			buffer = new byte[bufferMask + 1];
		}

		public void Mix(byte[] clip)
		{
			int writeCursor = bufferCursor;
			for (int i = 0; i < clip.Length; i++)
			{
				buffer[writeCursor] =  Math.Max(buffer[writeCursor], clip[i]);
				writeCursor = (writeCursor + 1) & bufferMask;  
			}
		}

		public void Update()
		{
			driver.TriggerHapticPulse(node, buffer[bufferCursor]);
			buffer[bufferCursor] = 0;
			bufferCursor = (bufferCursor + 1) & bufferMask;
		}
	}
}

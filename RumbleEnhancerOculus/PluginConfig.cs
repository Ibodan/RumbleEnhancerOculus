using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumbleEnhancerOculus
{
	internal class PluginConfig
	{
		public string CutClip = "0,0,0,0,255,255,255,0,255,255,255,0,255,255,0,255,255,0,200,200,0,200,200,0,120,120,0,90,90,0,90,90";
		public string MissCutClip = "0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255";
		public string BombClip = "0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255";
		public string UIClip = "80,100,0,0,0,0";
		public string SaberClashClip = "45,90,135,180,0,0,0,0,0,0";
		public string ObstacleClip = "255,255,255,0,255,255,255,0";
		public bool _fresh = true;

		public void LogValues()
		{
			Plugin.logger?.Debug($" CutClip:        {CutClip}");
			Plugin.logger?.Debug($" MissCutClip:    {MissCutClip}");
			Plugin.logger?.Debug($" BombClip:       {BombClip}");
			Plugin.logger?.Debug($" UIClip:         {UIClip}");
			Plugin.logger?.Debug($" SaberClashClip: {SaberClashClip}");
			Plugin.logger?.Debug($" ObstacleClip:   {ObstacleClip}");
		}
	}
}
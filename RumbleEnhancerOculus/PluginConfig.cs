using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]

namespace RumbleEnhancerOculus
{
	
	public class PluginConfig
	{
		public static PluginConfig Instance { get; set; }

		public string CutClip { get; set; } = "0,0,0,0,255,255,255,0,255,255,255,0,255,255,0,255,255,0,200,200,0,200,200,0,120,120,0,90,90,0,90,90";
		public string MissCutClip { get; set; } = "0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255";
		public string BombClip { get; set; } = "0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255";
		public string UIClip { get; set; } = "80,100,0,0,0,0";
		public string SaberClashClip { get; set; } = "45,90,135,180,0,0,0,0,0,0";
		public string ObstacleClip { get; set; } = "255,255,255,0,255,255,255,0";

		public virtual void Changed()
		{

		}

		public virtual void OnReload()
		{
			Plugin.LoadConfig(this);
		}

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
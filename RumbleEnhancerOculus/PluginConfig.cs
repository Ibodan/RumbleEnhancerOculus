using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]

namespace CustomHapticFeedback
{	
	public class PluginConfig
	{
		public static PluginConfig Instance { get; set; }

		public string CutClip { get; set; } = "9,9,0,9,9,0,0,4,4,3,3,2,2,2,1,1,1,1,1";
		public string MissCutClip { get; set; } = "9,9,9,0,9,9,9,0,9,9,0,9,9,0,5,0,8,0,4,0,7,0,2,0,5,0,4,0,2,0,2,0,1";
		public string BombClip { get; set; } = "9,9,9,0,9,9,9,0,9,9,0,9,9,0,5,0,8,0,4,0,7,0,2,0,5,0,4,0,2,0,2,0,1";
		public string UIClip { get; set; } = "1";
		public string SaberClashClip { get; set; } = "1,1,0,0,7,3,0,9,8,4,0,7,4,0";
		public string ObstacleClip { get; set; } = "1,2,3,3,2,1,0,0,0,2,3,2,2,3,2,0,0";
		public string OculusStrengthRange { get; set; } = "0.3, 1.0";
		public string SteamStrengthRange { get; set; } = "0.012, 0.26";

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
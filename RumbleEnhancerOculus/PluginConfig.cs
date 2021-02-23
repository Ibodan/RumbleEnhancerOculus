using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]

namespace CustomHapticFeedback
{	
	public class PluginConfig
	{
		public static PluginConfig Instance { get; set; }

		public string CutClip { get; set; } = "9,9,0,9,9,0,0,6,4,3,3,2,2,2,1,2,1,1,1";
		public string MissCutClip { get; set; } = "9,9,9,0,9,9,9,0,9,9,0,9,9,0,5,0,8,0,4,0,7,0,2,0,5,0,4,0,2,0,2,0,1";
		public string BombClip { get; set; } = "9,9,9,0,9,9,9,0,9,9,0,9,9,0,5,0,8,0,4,0,7,0,2,0,5,0,4,0,2,0,2,0,1";
		public string UIClip { get; set; } = "2";
		public string SaberClashClip { get; set; } = "1,1,0,0,1,1,0,5,4,0,0,0,0,9,9,8,0";
		public string ObstacleClip { get; set; } = "1,1,1,6,8,2,2,3,3,3,5,6";

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
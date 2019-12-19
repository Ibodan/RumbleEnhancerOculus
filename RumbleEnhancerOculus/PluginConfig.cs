using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumbleEnhancerOculus
{
    internal class PluginConfig
    {
        internal static readonly string DefaultCutClip = "0,0,0,0,255,255,255,0,255,255,255,0,255,255,0,255,255,0,200,200,0,200,200,0,120,120,0,90,90,0,90,90";
        internal static readonly string DefaultMissCutClip = "0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255";
        internal static readonly string DefaultBombClip = "0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255,0,0,0,0,255,255,255,0,255,255,255";
        internal static readonly string DefaultUIClip = "80,100,0,0,0,0";
        internal static readonly string DefaultSaberClashClip = "45,90,135,180,0,0,0,0,0,0";
        internal static readonly string DefaultObstacleClip = "255,255,255,0,255,255,255,0";

        public bool RegenerateConfig = true;

        public string CutClip { get; set; }
        public string MissCutClip { get; set; }
        public string BombClip { get; set; }
        public string UIClip { get; set; }
        public string SaberClashClip { get; set; }
        public string ObstacleClip { get; set; }

        public PluginConfig(bool fillDefaults = false)
        {
            if (fillDefaults)
            {
                RegenerateConfig = false;
                CutClip = DefaultCutClip;
                MissCutClip = DefaultMissCutClip;
                BombClip = DefaultBombClip;
                UIClip = DefaultUIClip;
                SaberClashClip = DefaultSaberClashClip;
                ObstacleClip = DefaultObstacleClip;
            }
            else
                RegenerateConfig = true;
        }

        public void LogSettings()
        {
            Plugin.logger?.Debug($"  CutClip:        {CutClip}");
            Plugin.logger?.Debug($"  MissCutClip:    {MissCutClip}");
            Plugin.logger?.Debug($"  BombClip:       {BombClip}");
            Plugin.logger?.Debug($"  UIClip:         {UIClip}");
            Plugin.logger?.Debug($"  SaberClashClip: {SaberClashClip}");
            Plugin.logger?.Debug($"  ObstacleClip:   {ObstacleClip}");

        }
    }
}

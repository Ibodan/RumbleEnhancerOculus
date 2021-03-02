## CustomHapticFeedback (formerly RumbleEnhancerOculus)

BeatSaber haptic feedback system overhaul

This plugin enables you to customize haptic feedbacks (controller rumbling patterns) of various type of interaction with sabers in the game.
(cutting blocks, cutting blocks on wrong direction, cutting bombs, clashing sabers, touching obstacles and touching UI object)

### Settings

Once you play the game with the plugin installed, editable setting file is created as `UserData/CustomHapticFeedback.json`.

`CutClip`, `MissCutClip`, `BombClip`, `UIClip`, `SaberClashClip`, `ObstacleClip` are patterns played on each events in the game.
You can modify these patterns for feeling different tactile feedback from different events.
- Comma separated 0-9 strength. One item is for a signal of one tick. (ex. 1/90 = 0.01111secs for 90Hz frame rate)
- No fool-proofs implemented. The plugin ignores any problem of your modification of a pattern. Make sure by yourself.

`--BiasTable` parameters are for calibration purpose. Default values are calibrated for Oculus Quest 2 on each SDK.
How to calibrate
1. Change `UIClip` pattern to many 9s. Adjust right most value on a target SDKs bias table, find the minimum value it can keep making maximum rumble.
2. Change `UIClip` pattern to many 1s. Adjust second value from left, find the minimum value it can make minimum rumble.
3. Linearly interpolate rest of values.

### For users of former plugin "RumbleEnhancerOculus"

- If you have one already installed, you must manually remove RumbleEnhancerOculus.dll from your Plugins folder.
- Rumble patterns for former plugin obviously generates different haptic feedbacks because of update frequency difference, so you have to re-design new ones.

### Background

Oculus Quest 2 makes it easy to test both SteamVR and Oculus SDK with one device (by connecting PC with Virtual Desktop / Oculus Link). 
Makes me to decide to make this plugin compatible with both SDKs. And it leads the whole plugin name to be changed. 
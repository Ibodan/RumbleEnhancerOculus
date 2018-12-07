## RumbleEnhancerOculus

BeatSaber haptic feedback system overhaul for oculus touch controller

With this plugin, you can change haptic textures separtely for all type of events so far in game.
Though this plugin realizes things by whole different approach from original RumbleEnhancer, I choose this name. I don't want people to confused.

### Notice

- You need **Harmony library installed**. Install with ModManager beforehand. 
- This is only **for Oculus Touch controller**. Nothing should be changed with Vibe or other controllers.


### Settings

Section named `RumbleEnhancerOculus` in `UserData/modprefs.ini` is the place for it.

`CutClip`, `MissCutClip`, `BombClip`, `UIClip`, `SaberClashClip`, `ObstacleClip` are the vibration patterns played on each events in game. 

- Comma separated 0-255 strength. One item is for a signal of 1/320 = 3.125msecs
- At least 6 items required for `SaberClashClip` and `ObstacleClip`.
- No foolproofs implemented. A wrongly parsed pattern still be played, but it may not as expected.

`NoteCutRumbleDuration` is a msecs for rumbling on each note cuts.

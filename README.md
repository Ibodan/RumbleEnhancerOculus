## RumbleEnhancerOculus

BeatSaber haptic feedback system overhaul for Oculus Touch controller

With this plugin, you can change haptic textures separately for all type of events so far in game.
Though this plugin realizes things by whole different approach from original RumbleEnhancer, I choose this name. I don't want people to be confused. The name also means it is alternative of RumbleEnhancer. Meaningless to install both.

### Notice

- This is only **for Oculus Touch controller**. Nothing should be changed with Vibe or other controllers.

### Settings

Once you play the game with the plugin installed, setting file is created as `UserData/RumbleEnhancerOculus.json`.

`CutClip`, `MissCutClip`, `BombClip`, `UIClip`, `SaberClashClip`, `ObstacleClip` are the vibration patterns played on each events in game and you can modify these patterns for feeling different haptic touch for these different events.  

- Comma separated 0-255 strength. One item is for a signal of 1/320 = 0.003125secs
- At least 6 items are required for `SaberClashClip` and `ObstacleClip`.
- No fool-proofs implemented. The plugin ignores any problem of your modification of a pattern. Make sure by yourself.

# Endless Runner

A 2D endless runner built in Unity. You control a goblin scout that runs forever while
obstacles come at you from the right. One hit ends the run, and your score is the number
of obstacles you survive.

## Gameplay

### Objective

Survive as long as possible. Every obstacle that passes behind you adds one point to the
counter, so a longer run means a higher score. There is no finish line — the run only ends
when you hit something.

### Controls

| Action | Keys |
| --- | --- |
| Jump | `Space` (rebindable — see below) |
| Start / Restart | `Enter`, or the on-screen **Play** / **Retry** button |
| Audio settings | **Settings** button (pauses the game while open) |

The jump key is stored in the `PlayerData` asset (`Assets/Data/PlayerData.asset`) along with
jump speed and gravity scale, so it can be changed in the Inspector without touching code.

Jumping only works while you are standing on the ground, so you cannot chain jumps in mid-air.
A jump is committed the moment you leave the floor: you have to read the next obstacle before
you press the key, not after.

### Game flow

The game moves through three states:

1. **Ready** — the title screen is up, obstacles are cleared, and the player is frozen.
   Press `Enter` or **Play** to begin.
2. **Playing** — obstacles spawn and scroll left, the speed climbs over time, and the score
   goes up as you clear obstacles.
3. **Game Over** — the moment the player collides with an obstacle, spawning stops, every
   obstacle freezes in place, and the Game Over panel appears. Press `Enter` or **Retry** to
   start a fresh run with the score and speed reset.

### Obstacles

Two obstacle types are drawn at random from the spawner, one every 2 seconds:

- **Floor obstacle** — sits on the ground. Jump over it.
- **Sky obstacle** — hangs above the ground, leaving a gap underneath. Stay on the ground and
  run through the gap; jumping into it is what kills you.

Because the two types ask for opposite reactions, the run is a reading exercise rather than a
rhythm one — you cannot jump on a fixed beat and survive.

Obstacles are recycled from an object pool (5 instances per type, grown on demand) instead of
being created and destroyed, and a despawner trigger off the left edge of the screen returns
them to the pool.

### Difficulty ramp

The scroll speed is what makes the game harder, not the obstacle layout:

| Setting | Value |
| --- | --- |
| Starting speed | 8 |
| Speed gained | +1 |
| Every | 10 seconds |
| Speed cap | 18 |

Speed increases in discrete steps, so the difficulty jumps rather than creeping. Once it
reaches 18 it stays there, and the run becomes a test of consistency at a fixed pace. The
current speed is shown on screen next to the obstacle count, so you can see the next step
coming.

### Scoring

A point is awarded when an obstacle's trailing edge passes the player's leading edge — you are
credited for clearing it, not for it leaving the screen. Each obstacle scores once per run.
The counter is displayed as `Obstacles: {value}` in the HUD and resets to zero on every restart.

### Audio

Music loops during play and stops on game over, where a lose sound plays instead. Jumps and
landings each have their own effect. Three volume sliders — **Master**, **Music**, and
**Effects** — are routed through an Audio Mixer and saved with `PlayerPrefs`, so your levels
persist between sessions. Opening the settings panel sets the time scale to zero, which pauses
the run and blocks input until you close it.

## Project layout

```
Assets/
├── Art/            Sprites, music, and SFX
├── Audio/          RunnerAudio mixer
├── Data/           PlayerData asset (jump key, jump speed, gravity)
├── Prefabs/        FloorObstacle, SkyObstacle
├── Scenes/         Game.unity
└── Scripts/
    ├── Globals/       GameManager — state machine, score, speed ramp
    ├── Player/        Jump input and ground check, landing particles
    ├── Obstacles/     Spawner, pooling, movement, scoring, despawn
    ├── SO/            ScriptableObject definitions (PlayerData)
    ├── UI/            HUD and panels
    └── Audio/         Mixer control and volume settings UI
```

## Running the project

Open the project in Unity, load `Assets/Scenes/Game.unity`, and press Play. Input uses the
Unity Input System package.

## Credits

### Art

- Player sprite — *Goblin Scout Silhouette* by
  [zneeke](https://zneeke.itch.io/goblin-scout-silhouette)
- Obstacle sprites — *Abyssal Planes Asset Pack 32x32* by
  [deepdivegamestudio](https://deepdivegamestudio.itch.io/abyssal-planes-asset-pack-32x32)

### Audio

- Lose sound — *080047_Lose_Funny_Retro_Video Game* by
  [freesound_community](https://pixabay.com/users/freesound_community-46691455/), via Pixabay

# Simple Shooty Prototype

Simple Shooty is a shoot-em-up game prototype where you control a character in an abstract world, eliminating enemies, collecting weapons, and reaching the goal to win. The game features auto-targeting and auto-shooting mechanics, as well as weapon upgrades and coin collection.

## Gameplay

- Use touch controls to move your character.(Can tounch and move anywhere in screen) [ Can be played with mouse and keyboard[Arrow Keys] on Unity Editor.]
- The character automatically aims and shoots at nearby enemies.
- Enemies target you and move towards your character.
- Collect different weapons to change your firepower.
- Losing occurs when an enemy touches you. Retry button appears.
- Winning occurs when you reach the goal. Retry button appears.

## Level Design

- The game has a single level with manageable stages of enemies.
- Various shapes and paths create distinct sections of the level.
- Average completion time is around 30 seconds.
- The level is designed for replayability, with different strategies.

## Ads/Special Moments Capturing

Moments for creating ads include:

- Picking up powerful weapons with increased rate of fire.
- Defeating a large number of enemies quickly.
- Being overwhelmed and defeated by enemies.

## Project Structure

- Controllers: Scripts for player movement, shooting, and enemy AI.
- Managers: Scripts for game state, UI, audio, and other systems.
- Game: Scripts for game logic, level design, enemy spawning, and systems.
- Utils: Utility scripts used by other scripts.

## Additional Features

- [Not Implemented] Optional: "HYPER" mode with increased rate of fire.
- Save/Load: Saving and loading audio settings (demo purpose).
- Audio: Background music and sound effects for shooting, enemy death, etc.
- UI: Simple UI with a health bar.
- Object Pooling: Efficient object pooling for bullets, enemies, and particles (optimization).
- Particle Effects: Visual feedback with particle effects for bullets and actions.

## Assets

This project uses the following assets:

- Kwalee Assets for the test prototype.
- Sound effects.
- Background Music.
- UI Images.

## Build

- Tested on Android Platform and Unity Editor.
- Developed Unity Version: 2022.3.4f1.0.14531.
- Tested Android Version: 12 & 13.

# Soulslike-Combat

An attempt to create a small boss fight sequence with souls-like combat.

Everything in this repo is a showcase of the game functions, not a final product.

## Motivation ##
I love Bloodborne and Dark Souls series, as a game developer myself, I would like to learn as much as possible on how AAA games like those are made.

Here is a prototype of how I try to implement their combat system and AI.

So here we are, this is where I make a prototype of a mini boss sequence of Souls-like game, I will attempt to make it look as close as possible to any souls-like games.

I use Finite State Machine design pattern to code the player and Behaviour Tree to code the AI human boss. For a better approach to camera, I will be using Cinemachine and its StateDrivenCamera to do make it a bit more cinematic.

## Controls ##
*Support both Gamepad and KBM*

```
Move: WASD
Run (Toggle): L-Shift
Jump: Space
Roll: F
Attack: Left Click
Special Attack: Right Click
Block (Hold): R
Parry: Q
Lock On: E
```

## AI ##
### Behaviour Tree ###
> To view full tree, please head to **Prefabs\Maria**, on Inspector, look for *Behaviour Tree script* and select *Open Behaviour Tree*
![Untitled](https://github.com/nmkha16/Soulslike-Combat/assets/91834063/e5ef411d-5097-49b0-ad65-5d068619324a)

#### AI Gameplay Showcase ####
> Human AI can see, strafe, run, attack player like Dark Souls human boss.

https://github.com/nmkha16/Soulslike-Combat/assets/91834063/a5733e51-da85-4432-a5fc-699d4f23b98e

### Player ###
> Designed using FSM for a flexible control.

#### Parry and Riposte Function Showcase ####
> Parry has 20 frames, any hit land on those frame will initiate success pary, enemy who get parried open up to a riposte.

https://github.com/nmkha16/Soulslike-Combat/assets/91834063/b9caf7a8-14b2-4ac6-b453-4a58d0ff166e

#### Basic Combat Gameplay ####
https://github.com/nmkha16/Soulslike-Combat/assets/91834063/8d37adef-9612-40c3-8a6a-cfc4c69dbabb

## List of Third-Party Assets ##

Behaviour Tree with Graph Editor taken from [UniBT](https://github.com/yoshidan/UniBT).

Animations and Models taken from [Mixamo](https://mixamo.com), [Souls-like Essential Animations](https://assetstore.unity.com/packages/3d/animations/souls-like-essential-animations-178889).

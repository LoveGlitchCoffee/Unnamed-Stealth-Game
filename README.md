# Unnamed-Stealth-Game

##About
The game was made originally as part of UoB Games Development Society's Reborn Game Jam. After the jam, I continued to work on it as an individual project.


## Main Features
The game mainly features path-finding AI. The AI guards will find their way to your position if they see you.

The path-finding search algorithm implemented is breadth first search.

*Breadth first search was chosen because the branching factor is generally small. Implementing A* search would make the game more inefficient due to uses of hash table structure (Dictionary)

## Sub-features
### Occlusion ray-casting
The AI has a cone of vision. The cone of vision uses ray-casting to detect game objects. It is also used to simulate occlusion i.e. Cannot see an enemy through an object.
### Camera effects
There are scripts to control the camera. The camera's default position is to the side the player is facing. If the player moves, the camera's transform will linearly interpolate to its new position, rather than snap.
The camera is also manually controlled to show items of interest, this also use linear interpolation

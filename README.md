# Unity RogueLite game

- Unity3D 5.3
- C# 5.0

It is the videogame I designed for the [Two Weeks Game 7](https://twg7.blogspot.ru) game jam. It is dungeon crawler game with dynamic procedural level generation. Project do not use any assets and made from the scratch.

To reduce number of draw calls and event receivers, procedural levels are baked into a single game objects with generated meth and texture atlas (Assets/Scripts/map.cs).

Level generation algorithm spawns random rooms, depending on level description. Rooms are connected with hallways. TO generate hallways, Astar algorithm is used to generate path from each room to each room. Adjusting the weight of empty tiles, as far as existing hallways, rooms and walls tiles can tune the level connectivity flexibly. After level mesh is generated, level is decorated with game objects and NPCs according to the each level settings.

Game objects (player, enemies, power-ups, projectiles, etc.) interaction uses Unity3D event system to increase performance. Each type of enemies have it's own AI (state machine) to make gameplay more interesting and challenging.

Play online (WebGL wersion) at [Github.io](http://hotkeym.github.io/caveexplorer/index.html)
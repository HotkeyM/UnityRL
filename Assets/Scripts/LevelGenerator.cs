using System;
using System.Collections.Generic;

using UnityEngine;

class Room 
{

}

public enum Tile {FLOOR, WALL, VOID};

public interface IMapGenerator
{
    //enum Tile {FLOOR, WALL, VOID};

    Tile[][] tiles
    {
        get;
    }

    

    void Generate();
    
    
}

public class LevelManager: MonoBehaviour
{

}



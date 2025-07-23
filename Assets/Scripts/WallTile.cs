using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTile : Tile
{
    protected override void Awake()
    {
        tileType = TileType.Wall;
    }
}

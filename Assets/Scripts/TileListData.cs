using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileListData", menuName = "ScriptableObjects/TileListData", order = 1)]
public class TileListData : ScriptableObject
{
    public List<TileConfig> tileConfigList;

    public TileConfig GetTileConfig(TileType tileType)
    {
        foreach (var tileConfig in tileConfigList)
        {
            if (tileConfig.tileType == tileType)
            {
                return tileConfig;
            }
        }
        return null;
    }
}

[Serializable]
public class TileConfig
{
    public TileType tileType;
    public Gradient gradient;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager 
{
    public static event Action<LevelData> OnInitLevel;
    public static void InitLevel(LevelData levelData)
    {
        OnInitLevel?.Invoke(levelData);
    }

    public static event Action<int> OnLevelChanged;
    public static void LevelChanged(int level)
    {
        OnLevelChanged?.Invoke(level);
    }

    public static event Action<int> OnMovesChanged;
    public static void MovesChanged(int moves)
    {
        OnMovesChanged?.Invoke(moves);
    }

    public static event Action OnTileClick;
    public static void TileClick()
    {
        OnTileClick?.Invoke();
    }

    public static event Action OnWon;
    public static void Won()
    {
        OnWon?.Invoke();
    }

    public static event Action OnLost;
    public static void Lost()
    {
        OnLost?.Invoke();
    }

    public static event Action OnRemoveTile;
    public static void RemoveTile()
    {
        OnRemoveTile?.Invoke();
    }
}

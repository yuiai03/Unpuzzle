using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int currentLevel;
    public int CurrentLevel
    {
        get { return currentLevel; }
        set
        {
            currentLevel = value;
            EventManager.LevelChanged(currentLevel);
        }
    }

    private int currentMoves;
    public int CurrentMoves
    {
        get { return currentMoves; }
        set
        {
            currentMoves = value;
            if (currentMoves <= 0)
            {
                currentMoves = 0;
                EventManager.Lost();
            }
            EventManager.MovesChanged(currentMoves);
        }
    }

    public LevelManager CurrentLevelManager { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        InitNewLevel();
    }
    private void OnEnable()
    {
        EventManager.OnInitLevel += OnInitLevel;
        EventManager.OnTileClick += OnTileClick;
    }

    private void OnDisable()
    {
        EventManager.OnInitLevel -= OnInitLevel;
        EventManager.OnTileClick -= OnTileClick;

    }

    private void OnInitLevel(LevelData levelData)
    {
        CurrentLevel = levelData.levelNumber;
        CurrentMoves = levelData.moves;
    }

    private void OnTileClick()
    {
        CurrentMoves--;
    }

    public void InitNewLevel()
    {
        currentLevel++;
        if(CurrentLevelManager) Destroy(CurrentLevelManager.gameObject);

        var prefab = Resources.Load<LevelManager>($"Prefabs/Level{currentLevel}");
        if (!prefab) return;

        var levelManager = Instantiate(prefab);
        CurrentLevelManager = levelManager;
        var camera = Camera.main;
        var newPos = levelManager.CameraPos;
        if (!camera) return;
        camera.transform.position = new Vector3(newPos.x, newPos.y, camera.transform.position.z);
    }
    public void InitCurrentLevel()
    {
        if (CurrentLevelManager) Destroy(CurrentLevelManager.gameObject);

        var prefab = Resources.Load<LevelManager>($"Prefabs/Level{currentLevel}");
        if (!prefab) return;

        var levelManager = Instantiate(prefab);
        CurrentLevelManager = levelManager;
    }
}

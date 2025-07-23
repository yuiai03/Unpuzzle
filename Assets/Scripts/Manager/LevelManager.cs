using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float cameraSize = 10;
    [SerializeField] private Vector2 cameraPos;
    [SerializeField] private GameObject tileHolder;
    [SerializeField] private LevelData levelData;
    private List<Tile> tileList = new List<Tile>();

    private void Start()
    {
        Init();
    }
    private void Init() {
        if (!levelData) return;
        EventManager.InitLevel(levelData);

        if (!tileHolder) return;
        foreach (Transform child in tileHolder.transform)
        {
            Tile tile = child.GetComponent<Tile>();
            if (tile && tile.GetTileType() != TileType.Wall) tileList.Add(tile);
        }
    }
    public bool CheckPos(Vector2Int pos)
    {
        var tempPos = pos;
        foreach (Tile tile in tileList)
        {
            if (tile.CurrentPos == tempPos) return false;
        }
        return true;
    }

    public Vector2Int TargetPos(Vector2Int _currentPos, Vector2 _direction)
    {
        Vector2 direction = _direction;
        Vector2Int currentPos = _currentPos;
        Vector2Int targetPos = _currentPos;

        for (int i = 0; i < 20; i++)
        {
            Vector2Int newPos = targetPos + Vector2Int.RoundToInt(direction);
            bool isEmpty = CheckPos(newPos);

            if (!isEmpty) break;
            else targetPos = newPos;
        }
        return targetPos;
    }
    public void RemoveToList(Tile tile)
    {
        List<Tile> tempList = new List<Tile>(tileList);
        tempList.Remove(tile);
        tileList = tempList;

        EventManager.RemoveTile();
        if (tileList.Count == 0) EventManager.Won();
    }

    public List<Tile> GetTileListToDirection(Vector2Int _currentPos, Vector2 _direction)
    {
        Vector2 direction = _direction;
        Vector2Int currentPos = _currentPos;
        Vector2Int targetPos = _currentPos;

        List<Tile> tilesInDirection = new List<Tile>();
        Tile currentTile = tileList.Find(t => t.CurrentPos == currentPos);
        tilesInDirection.Add(currentTile);

        for (int i = 0; i < 10; i++)
        {
            Vector2Int newPos = targetPos + Vector2Int.RoundToInt(direction);
            bool isEmpty = CheckPos(newPos);
            if (!isEmpty)
            {
                targetPos = newPos;
                Tile tile = tileList.Find(t => t.CurrentPos == targetPos);

                if (tile != null) tilesInDirection.Add(tile);
            }
            else break;
        }
        return tilesInDirection;
    }

    public Vector2 CameraPos => cameraPos;
    public float CameraSize => cameraSize;
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] protected TileType tileType;
    
    public Vector2Int CurrentPos { get; private set; }
    protected SpriteRenderer spriteRenderer;
    protected TrailRenderer trailRenderer;
    private Coroutine shakeCoroutine;
    private Tween shakeTween;
    private GameManager gameManager => GameManager.Instance;
    
    protected virtual void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer) spriteRenderer.sprite = Resources.Load<Sprite>($"Sprites/{tileType}");
    }
    
    protected virtual void OnEnable()
    {
        transform.DOKill();
    }

    protected virtual void Start()
    {
        Init();
    }
    
    private void Init()
    {
        CurrentPos = new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y);
        if (trailRenderer)
        {
            var tileListData = Resources.Load<TileListData>("Datas/TileListData");
            if (!tileListData || tileType == TileType.Hide) return;

            trailRenderer.colorGradient = tileListData.GetTileConfig(tileType)?.gradient;
        }
    }

    public void OnClick()
    {
        if (!CanAction()) return;
        Action();
        EventManager.TileClick();
    }

    public Vector2 GetMoveDirection()
    {
        switch (tileType)
        {
            case TileType.Left:
                return Vector2.left;
            case TileType.Right:
                return Vector2.right;
            case TileType.Up:
                return Vector2.up;
            case TileType.Down:
                return Vector2.down;
            default:
                return Vector2.zero;
        }
    }

    private void ShakeAction()
    {
        var tileList = gameManager.CurrentLevelManager
            .GetTileListToDirection(CurrentPos, GetMoveDirection());
        if (tileList.Count == 0) return;

        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(ShakeCoroutine(tileList));
    }

    private IEnumerator ShakeCoroutine(List<Tile> tileList)
    {
        foreach (Tile tile in tileList)
        {
            Vector2 moveDirection = GetMoveDirection();
            Vector3 shakeDirection = new Vector3(moveDirection.x, moveDirection.y, 0) * 0.1f;
            tile.transform.localPosition = new Vector3(tile.CurrentPos.x, tile.CurrentPos.y, 0);

            if (tile.shakeTween != null && tile.shakeTween.IsActive()) tile.shakeTween.Kill();
            tile.shakeTween = tile.transform.DOPunchPosition(shakeDirection, 0.2f, 5, 0.5f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    tile.transform.localPosition = new Vector3(tile.CurrentPos.x, tile.CurrentPos.y, 0);
                });
            
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void MoveOutAndDestroy(Vector2Int _targetPos, bool canDestroy)
    {
        Vector3 targetPos = new Vector3(_targetPos.x, _targetPos.y, 0);
        if(canDestroy) GameManager.Instance.CurrentLevelManager.RemoveToList(this);

        transform.DOMove(targetPos, canDestroy ? 0.7f : 0.3f)
            .SetEase(Ease.InQuad)
            .OnComplete(() =>
            {
                if(canDestroy)
                {
                    Destroy(gameObject);
                }
                else
                {
                    CurrentPos = _targetPos;
                    ShakeAction();
                }
            });
    }

    public void Action()
    {
        Vector2 moveDirection = GetMoveDirection();
        if (moveDirection == Vector2.zero) return;

        var targetPos = gameManager.CurrentLevelManager.TargetPos(CurrentPos, moveDirection);
        if(CurrentPos == targetPos)
        {
            ShakeAction();
        }
        else
        {
            MoveOutAndDestroy(targetPos, CanDestroy(targetPos) ? true : false);
        }
    }

    private bool CanDestroy(Vector2Int _targetPos)
    {
        return (Mathf.Abs(_targetPos.x) + Mathf.Abs(CurrentPos.x) >= 20) 
            || (Mathf.Abs(_targetPos.y) + Mathf.Abs(CurrentPos.y) >= 20);
    }

    public bool CanAction() => gameManager.CurrentMoves > 0 && gameManager.CanAction && tileType != TileType.Hide;
    
    public TileType GetTileType() => tileType;
}

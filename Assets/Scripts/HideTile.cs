using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HideTile : Tile
{
    [Header("Hide Tile")]
    [SerializeField] int waitTurn;
    [SerializeField] private TileType trueTileType;
    [SerializeField] private TextMeshPro turnText;

    protected override void Awake()
    {
        base.Awake();
        tileType = TileType.Hide;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        EventManager.OnRemoveTile += OnRemoveTile;
    }
    private void OnDisable()
    {
        EventManager.OnRemoveTile -= OnRemoveTile;
    }

    protected override void Start()
    {
        base.Start();
        SetTurnText(waitTurn);
    }

    private void OnRemoveTile()
    {
        waitTurn--;
        if (waitTurn <= 0)
        {
            tileType = trueTileType;
            turnText.gameObject.SetActive(false);
            spriteRenderer.sprite = Resources.Load<Sprite>($"Sprites/{tileType}");
            var tileListData = Resources.Load<TileListData>("Datas/TileListData");
            trailRenderer.colorGradient = tileListData.GetTileConfig(tileType)?.gradient;

            EventManager.OnRemoveTile -= OnRemoveTile;

        }
        SetTurnText(waitTurn);
    }
    private void SetTurnText(int turn) => turnText.text = turn.ToString();
}

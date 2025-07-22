using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI movesText;

    private LevelEndPanel levelEndPanel;
    protected override void Awake()
    {
        base.Awake();
        levelEndPanel = GetComponentInChildren<LevelEndPanel>();
    }

    private void OnEnable()
    {
        EventManager.OnLevelChanged += SetLevelText;
        EventManager.OnMovesChanged += SetMovesText;
    }
    private void OnDisable()
    {
        EventManager.OnLevelChanged -= SetLevelText;
        EventManager.OnMovesChanged -= SetMovesText;
    }

    private void SetLevelText(int level) => levelText.text = $"Level: {level}";
    private void SetMovesText(int moves) => movesText.text = $"{moves} Moves";
}

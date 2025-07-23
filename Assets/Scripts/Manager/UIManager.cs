using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI movesText;
    [SerializeField] private Button settingButton;

    private LevelEndPanel levelEndPanel;
    private SettingPanel settingPanel;
    protected override void Awake()
    {
        base.Awake();
        levelEndPanel = GetComponentInChildren<LevelEndPanel>();
        settingPanel = GetComponentInChildren<SettingPanel>();

        settingButton.onClick.AddListener(OnSettingButtonClick);
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

    private void OnSettingButtonClick()
    {
        settingPanel.ShowPanel();
    }

    private void SetLevelText(int level) => levelText.text = $"Level: {level}";
    private void SetMovesText(int moves) => movesText.text = $"{moves} Moves";
}

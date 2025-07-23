using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : PanelBase
{
    [SerializeField] Button menuButton;
    protected override void Awake()
    {
        base.Awake();
        menuButton.GetComponent<Button>().onClick.AddListener(() => HidePanel());
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        AudioManager.Instance.PlaySFX(AudioType.ButtonClick);
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] private ToggleType toggleType;
    [SerializeField] private RectTransform handleRectTransform;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;

    private Toggle toggle;
    private Vector2 handlePosDefault;
    public void Awake()
    {
        handlePosDefault = handleRectTransform.anchoredPosition;
        
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnSwitch);
    }
    protected virtual void OnSwitch(bool isOn)
    {
        toggle.isOn = isOn;
        handleRectTransform.DOAnchorPos(isOn ? handlePosDefault : -handlePosDefault , 0.25f).SetEase(Ease.InOutBack);
        backgroundImage.DOColor(isOn ? onColor : offColor, 0.25f);
        AudioManager.Instance.PlaySFX(AudioType.ButtonClick);

        switch (toggleType)
        {
            case ToggleType.BGM:
                AudioManager.Instance.ToggleBGMState(isOn);
                break;
            case ToggleType.SFX:
                AudioManager.Instance.ToggleSFXState(isOn);
                break;
        }
    }
}

using DG.Tweening;
using UnityEngine;

public class PanelBase : MonoBehaviour
{
    [SerializeField] private CanvasGroup bgCanvasGroup;
    [SerializeField] private protected GameObject menu;

    protected virtual void Awake()
    {
        if (bgCanvasGroup) bgCanvasGroup.gameObject.SetActive(false);
        if (menu) menu.SetActive(false);
    }

    public virtual void ShowPanel()
    {
        menu.SetActive(true);
        bgCanvasGroup.gameObject.SetActive(true);
        bgCanvasGroup.alpha = 0;
        bgCanvasGroup.DOFade(1, 0.5f).SetUpdate(true);
        //AudioManager.Instance.PlaySFX(AudioType.ButtonClick);
    }
    public virtual void HidePanel()
    {
        menu.SetActive(false);
        bgCanvasGroup.DOFade(0, 0.5f).SetUpdate(true);
        bgCanvasGroup.gameObject.SetActive(false);
        //AudioManager.Instance.PlaySFX(AudioType.CloseClick);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndPanel : PanelBase
{
    [SerializeField] private Button playAgainButton;
    [SerializeField] private TextMeshProUGUI resultText;
    private Coroutine wonCoroutine;
    private Coroutine lostCoroutine;

    protected override void Awake()
    {
        base.Awake();
        playAgainButton.onClick.AddListener(OnPlayAgainClick);
    }
    private void OnEnable()
    {
        EventManager.OnWon += OnWon;
        EventManager.OnLost += OnLost;
    }

    private void OnDisable()
    {
        EventManager.OnWon -= OnWon;
        EventManager.OnLost -= OnLost;
    }

    private void OnPlayAgainClick()
    {
        HidePanel();
        GameManager.Instance.InitCurrentLevel();
    }

    private void OnWon()
    {
        if (wonCoroutine != null) StopCoroutine(wonCoroutine);
        wonCoroutine = StartCoroutine(WonCoroutine());

    }


    private void OnLost()
    {
        if (lostCoroutine != null) StopCoroutine(lostCoroutine);
        lostCoroutine = StartCoroutine(LostCoroutine());

    }

    private void SetResultText(string result) => resultText.text = result;

    private IEnumerator WonCoroutine()
    {
        yield return new WaitForSeconds(1f);

        ShowPanel();
        SetResultText("You Won!");
        playAgainButton.gameObject.SetActive(false);

        yield return new WaitForSeconds(2f);

        HidePanel();
        GameManager.Instance.InitNewLevel();
    }
    private IEnumerator LostCoroutine()
    {
        yield return new WaitForSeconds(1f);

        ShowPanel();
        SetResultText("You Lost!");

        yield return new WaitForSeconds(2f);

        playAgainButton.gameObject.SetActive(true);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiGame : MonoBehaviour
{
    public Animator TipsAnimator;
    public TMP_Text TipsLabel;
    public TMP_Text TipsDeltaLabel;
    public TMP_Text CustomersServedLabel;
    public GameObject WantsImage;

    public GameObject GameOverObj;

    private void Start()
    {
        GameEvents.CustomersServed += OnCustomersServed;
        GameEvents.TimesUp += OnTimesUP;
        GameEvents.PhaseChanged += OnPhaseChanged;
        GameEvents.GameOver += OnGameOver;

        if (GameOverObj)
        {
            GameOverObj.SetActive(false);
        }

        if (WantsImage)
        {
            WantsImage.SetActive(false);
        }

        if (TipsDeltaLabel)
        {
            TipsDeltaLabel.gameObject.SetActive(false);
        }
    }

    private void OnGameOver(GameOverReason reason)
    {
        if (GameOverObj)
        {
            GameOverObj.SetActive(true);
        }
    }

    private void OnPhaseChanged(CustomerLoop.Phase obj)
    {
        if (WantsImage)
        {
            WantsImage.SetActive(obj == CustomerLoop.Phase.WaitingForOrder);
        }
    }

    private void OnTimesUP(CustomerLoop obj)
    {
    }

    private void OnCustomersServed(CustomerServedArgs tips)
    {
        if (TipsLabel != null)
        {
            TipsLabel.text = $"${tips.TipsTotal}!";
        }

        if( CustomersServedLabel != null)
        {
            CustomersServedLabel.text = $"x{tips.CustomersServedSoFar}";
        }

        StartCoroutine(ShowTipDelta(tips.TipsDelta));
    }

    IEnumerator ShowTipDelta(int tipsDelta)
    {
        if (TipsDeltaLabel)
        {
            TipsDeltaLabel.gameObject.SetActive(true);
            var sign = tipsDelta > 0 ? "+" : string.Empty;
            TipsDeltaLabel.text = $"{sign}{tipsDelta}";
        }

        if(TipsAnimator && tipsDelta > 0)
        {
            TipsAnimator.SetTrigger("Ding");
        }

        yield return new WaitForSeconds(1.0f);

        if (TipsDeltaLabel)
        {
            TipsDeltaLabel.gameObject.SetActive(false);
        }
    }
}

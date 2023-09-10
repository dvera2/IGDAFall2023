using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiGame : MonoBehaviour
{
    public TMP_Text TipsLabel;
    public TMP_Text TipsDeltaLabel;
    public TMP_Text CustomersServedLabel;
    public GameObject WantsImage;

    private void Start()
    {
        GameEvents.CustomersServed += OnCustomersServed;
        GameEvents.TimesUp += OnTimesUP;
        GameEvents.PhaseChanged += OnPhaseChanged;
        
        if (TipsDeltaLabel)
        {
            TipsDeltaLabel.gameObject.SetActive(false);
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

        yield return new WaitForSeconds(1.0f);

        if (TipsDeltaLabel)
        {
            TipsDeltaLabel.gameObject.SetActive(false);
        }
    }
}

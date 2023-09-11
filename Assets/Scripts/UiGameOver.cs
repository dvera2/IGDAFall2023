using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiGameOver : MonoBehaviour
{
    public TMP_Text Msg;
    public string LoseMsg = "Customers Mad!";
    public string TimesUpMsg = "Out of Time!";
    public AudioSource Audio;
    public TMP_Text Tips;
    public Animator Animator;

    int tipAmount = 0;

    private void Awake()
    {
        GameEvents.CustomersServed += OnCustomerServed;
        GameEvents.GameOver += OnGameOver;

        gameObject.SetActive(false);
    }

    private void OnGameOver(GameOverReason obj)
    {
        if(Animator)
            Animator.enabled = true;

        if (Msg)
        {
            Msg.text = obj == GameOverReason.Time ? TimesUpMsg : LoseMsg;
        }

        if(Audio)
            Audio.Play();


        if (Tips)
        {
            if (tipAmount > 0)
                Tips.text = $"Earned {tipAmount}!";
            else
                Tips.text = "You earned <i>NOTHING</i>.";
        }
    }

    private void OnCustomerServed(CustomerServedArgs obj)
    {
        tipAmount = obj.TipsTotal;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Start");
    }
}

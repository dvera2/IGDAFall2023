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

    private void Awake()
    {
        GameEvents.CustomersServed += OnCustomerServed;
        GameEvents.GameOver += OnGameOver;

        gameObject.SetActive(false);
    }

    private void OnGameOver(GameOverReason obj)
    {
        if (Msg)
        {
            Msg.text = obj == GameOverReason.Time ? TimesUpMsg : LoseMsg;
        }
    }

    private void OnCustomerServed(CustomerServedArgs obj)
    {
        if (Tips)
            Tips.text = $"Earned {obj.TipsTotal}!";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene(2);
    }

    public void OnEnable()
    {
        Audio.Play();    
    }
}

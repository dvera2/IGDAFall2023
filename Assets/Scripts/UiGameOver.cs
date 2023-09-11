using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiGameOver : MonoBehaviour
{
    public GameObject TimesUp;
    public GameObject LoseGame;
    public AudioSource Audio;
    public TMP_Text Tips;

    private void Awake()
    {
        GameEvents.CustomersServed += OnCustomerServed;
        GameEvents.GameOver += OnGameOver;
    }

    private void OnGameOver(GameOverReason obj)
    {
        if(obj == GameOverReason.Lose)
        {
            if(TimesUp != null)
                TimesUp.SetActive(false);
            
            if(LoseGame != null)
                LoseGame.SetActive(true);
        }
        else
        {
            if (LoseGame != null)
                LoseGame.SetActive(false);

            if (TimesUp != null)
                TimesUp.SetActive(true);
        }
    }

    private void OnCustomerServed(CustomerServedArgs obj)
    {
        if (Tips)
            Tips.text = $"Earned {obj.TipsTotal}!";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Start");
    }

    public void OnEnable()
    {
        Audio.Play();    
    }
}

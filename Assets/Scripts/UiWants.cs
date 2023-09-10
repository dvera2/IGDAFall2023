using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiWants : MonoBehaviour
{

    public Image A; 
    public Image B; 
    public Image C;


    private void Start()
    {
        GameEvents.WantsDetermined += OnWantsDetermined;
    }

    private void OnWantsDetermined(List<IngredientType> obj)
    {
        SetIngredients(obj);
    }

    public void SetIngredients(List<IngredientType> ingredients)
    {
        A.enabled = false;
        B.enabled = false;
        C.enabled = false;

        var images = new Image[]
        {
            A,B,C
        };

        for(int i = 0; i < Mathf.Min(3, ingredients.Count); i++)
        {
            images[i].sprite = ingredients[i].GetComponent<SpriteRenderer>().sprite;
        }
    }
}

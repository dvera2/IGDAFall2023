using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiWants : MonoBehaviour
{

    public Image A; 
    public Image B; 
    public Image C;

    [System.Serializable]
    public class Entry
    {
        public IngredientType Type;
        public Sprite Sprite;
    }

    public Entry[] Entries;

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
            images[i].sprite = GetSprite(ingredients[i]);
            images[i].rectTransform.Rotate(new Vector3(0, 0, Random.Range(-90, 90f)));
            images[i].enabled = true;
        }
    }

    private Sprite GetSprite(IngredientType type)
    {
        foreach(var item in Entries)
        {
            if (item.Type == type)
                return item.Sprite;
        }
        return null;
    }
}

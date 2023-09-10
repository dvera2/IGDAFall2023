using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SandwichCreator: MonoBehaviour
{

    public int ingredientsStackIndex;
    public Transform[] spawnPoints;
    public List<Ingredient> currentSandwhich;
   

    // Start is called before the first frame update
    void Start()
    {

        ingredientsStackIndex = 0;
    }


    public void DecrementIndex()
    {   
        if( ingredientsStackIndex > 0)
        {
            ingredientsStackIndex--;
            Debug.Log("Decremented Index should now be " + ingredientsStackIndex);
            Destroy(currentSandwhich[ingredientsStackIndex].gameObject);
            currentSandwhich.RemoveAt(ingredientsStackIndex);
        }
    }

    public void ClearSandwich()
    {
        foreach (var item in currentSandwhich)
        {
            Destroy(item.gameObject);
        }
        currentSandwhich.Clear();
        ingredientsStackIndex = 0;
    }

    public void SpawnIngredientAtIndex(Ingredient originalPrefab)
    {
        if ( ingredientsStackIndex < 10)
        {
            Ingredient spawnedIngredient = Instantiate(originalPrefab, spawnPoints[ingredientsStackIndex].position, Quaternion.identity);
            currentSandwhich.Add(spawnedIngredient);
            Debug.Log("the index is " + ingredientsStackIndex);

            if(ingredientsStackIndex < 10)
            {
                ingredientsStackIndex++;
            }
            
        }

    }





}

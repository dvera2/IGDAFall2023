using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SandwichCreator: MonoBehaviour
{

    public int ingredientsStackIndex;
    public Transform[] spawnPoints;
    public List<Ingredient> currentSandwhich;

    public AudioSource audioSource;

    public AudioClip SandwhichFull;
    public AudioClip ButtonClick;
   

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
            audioSource.PlayOneShot(ButtonClick);
        }
    }

    public void SubmitSandwich()
    {
        GameEvents.TriggerSandwichSubmitted(new SandwichSubmitArgs()
        {
            Sandwich = currentSandwhich
        });
        audioSource.PlayOneShot(ButtonClick);
        ClearSandwich();
    }

    public void ClearSandwich()
    {
        foreach (var item in currentSandwhich)
        {
            Destroy(item.gameObject);
        }
        currentSandwhich.Clear();
        ingredientsStackIndex = 0;
        audioSource.PlayOneShot(ButtonClick);

    }

    public void SpawnIngredientAtIndex(Ingredient originalPrefab)
    {
        if ( ingredientsStackIndex < 10)
        {
            Ingredient spawnedIngredient = Instantiate(originalPrefab, spawnPoints[ingredientsStackIndex].position, Quaternion.identity);
            spawnedIngredient.GetComponent<SpriteRenderer>().sortingOrder = ingredientsStackIndex + 100;
            if (spawnedIngredient.IngredientType != IngredientType.Bread)
            {
                spawnedIngredient.transform.Rotate(new Vector3(0, 0, Random.Range(-35f, 35f)));
            }
            currentSandwhich.Add(spawnedIngredient);
            //Debug.Log("the index is " + ingredientsStackIndex);

            if(ingredientsStackIndex < 10)
            {
                ingredientsStackIndex++;
            }

            GameEvents.TriggerSandwichIngredientChanged(new SandwichUpdateArgs()
            {
                Ingredient = spawnedIngredient,
            });
        }
        else
        {
            audioSource.PlayOneShot(SandwhichFull);
        }

    }





}

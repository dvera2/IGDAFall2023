using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{
    
    public Ingredient ingredient;
    public SandwichCreator sandwichCreator;
     


public void AddIngredientToSandwhich()
    {
        sandwichCreator.SpawnIngredientAtIndex(ingredient);
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public enum CustomerExpression
{
    Neutral,
    Happy,
    Mad,
    OnFire,
    Sick,
}

public class CustomerCharacter : MonoBehaviour
{
    public SpriteRenderer Head;
    public SpriteRenderer Torso;

    public CharacterFaceData CurrentFace;

    private List<IngredientType> _validIngredients;
    private Dictionary<IngredientType, int> _customerIngredientRanking;

    private void Awake()
    {
        _customerIngredientRanking = new Dictionary<IngredientType, int>();

        _validIngredients = new List<IngredientType>((IngredientType[])System.Enum.GetValues(typeof(IngredientType)));
        _validIngredients.Remove(IngredientType.Bread);

        GameEvents.SatisfactionChanged += SetFaceExpression;
    }

    private void SetFaceExpression(CustomerExpression expression)
    {
        Debug.Assert(Head, "Torso sprite is missing!");
        if (CurrentFace && Head)
        {
            Head.sprite = CurrentFace.GetFace(expression);
        }
    }

    public void SetCustomerTorso(Sprite sprite)
    {
        Debug.Assert(Torso, "Torso sprite is missing!");
        if (Torso && sprite)
        {
            Torso.sprite = sprite;
        }
    }

    public void SetCustomerHead(CharacterFaceData data)
    {
        CurrentFace = data;
        SetFaceExpression(CustomerExpression.Neutral);
    }

    public int GetSandwichScore(SandwichSubmitArgs args)
    {
        if (args.Sandwich.Count < 2)
            return -10;

        int lastIdx = args.Sandwich.Count - 1;
        if (args.Sandwich[0].IngredientType != IngredientType.Bread || args.Sandwich[lastIdx].IngredientType != IngredientType.Bread)
            return -10;

        int score = 0;
        HashSet<IngredientType> Hash = new HashSet<IngredientType>();
        for(int i = 1; i < args.Sandwich.Count - 1; i++)
        {
            if (Hash.Add(args.Sandwich[i].IngredientType))
            {
                score += GetMatchScore(args.Sandwich[i].IngredientType);
            }
        }

        return score;
    }

    public int GetMatchScore(IngredientType type)
    {
        if (_customerIngredientRanking.TryGetValue(type, out int score))
        {
            return score;
        }
        return 0;
    }

    public void RandomizeWants()
    {
        _customerIngredientRanking.Clear();
        var list = new List<IngredientType>(_validIngredients);
        for (int i = 0; i < _validIngredients.Count; i++)
        {
            int idx = Random.Range(0, list.Count);
            _customerIngredientRanking.Add(list[idx], i - 3);
            list.RemoveAt(idx);
        }
    }
}

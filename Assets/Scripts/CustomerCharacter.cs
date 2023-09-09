using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CustomerExpression
{
    Neutral,
    Happy,
    Mad,
    OnFire,
    Sick
}

public class CustomerCharacter : MonoBehaviour
{
    public SpriteRenderer Head;
    public SpriteRenderer Torso;

    public CharacterFaceData CurrentFace;

    private void Awake()
    {
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
        SetFaceExpression(CustomerExpression.Neutral);
    }
}

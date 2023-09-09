using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Face Data", menuName = "Data")]
public class CharacterFaceData : ScriptableObject
{
    public Sprite Neutral;
    public Sprite Happy;
    public Sprite Mad;

    
    public Sprite GetFace(CustomerExpression cs)
    {
        switch(cs)
        {
            case CustomerExpression.Happy: return Happy;
            case CustomerExpression.Mad: return Mad;
            case CustomerExpression.Neutral: return Neutral;
            default:
                return Neutral;
        }
    }
}

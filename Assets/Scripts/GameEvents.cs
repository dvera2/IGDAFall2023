using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static event System.Action<CustomerExpression> SatisfactionChanged;
    public static void TriggerSatisfactionChanged(CustomerExpression cs) => SatisfactionChanged?.Invoke(cs);

    private void OnDestroy()
    {
        SatisfactionChanged = null;
    }
}

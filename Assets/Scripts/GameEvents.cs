using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandwichUpdateArgs
{

}

public class SandwichSubmitArgs
{

}

public struct TimeArgs
{
    public float Time;
    public float Duration;
}

public class GameEvents : MonoBehaviour
{
    public static event System.Action<CustomerExpression> SatisfactionChanged;
    public static void TriggerSatisfactionChanged(CustomerExpression cs) => SatisfactionChanged?.Invoke(cs);

    public delegate void OnSandwichUpdated(SandwichUpdateArgs args);
    public static event OnSandwichUpdated SandwichIngredientChanged;
    public static void TriggerSandwichIngredientChanged(SandwichUpdateArgs args) => SandwichIngredientChanged?.Invoke(args);

    public delegate void OnSandwichSubmit(SandwichSubmitArgs args);
    public static event OnSandwichSubmit SandwichSubmitted;
    public static void TriggerSandwichSubmitted(SandwichSubmitArgs args) => SandwichSubmitted?.Invoke(args);

    public static event System.Action<int> TipsAmount;
    public static void TriggerTipsUpdated(int tipsAmount) => TipsAmount?.Invoke(tipsAmount);

    public static event System.Action<TimeArgs> TimeUpdated;
    public static void TriggerTimeUpdated(TimeArgs args) => TimeUpdated?.Invoke(args);

    public static event System.Action<CustomerLoop> TimesUp;
    internal static void TriggerTimeUp(CustomerLoop customerLoop) => TimesUp?.Invoke(customerLoop);

    private void OnDestroy()
    {
        SandwichIngredientChanged = null;
        SandwichSubmitted = null;
        SatisfactionChanged = null;
        TipsAmount = null;
        TimeUpdated = null;
        TimesUp = null;
    }

}

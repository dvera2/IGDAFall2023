using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandwichUpdateArgs
{
    public Ingredient Ingredient;
}

public class SandwichSubmitArgs
{
    public List<Ingredient> Sandwich;

    public bool IsVerySpicy()
    {
        int spice = 0;
        foreach(var i in Sandwich)
            if(i.IngredientType == IngredientType.Spicy) spice++;

        return spice > 2;
    }
}

public struct TimeArgs
{
    public float Time;
    public float Duration;
}

public struct CustomerServedArgs
{
    public int TipsTotal;
    public int TipsDelta;
    public int CustomersServedSoFar;
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

    public static event System.Action<CustomerServedArgs> CustomersServed;
    public static void TriggerTipsUpdated(CustomerServedArgs args) => CustomersServed?.Invoke(args);

    public static event System.Action<TimeArgs> TimeUpdated;
    public static void TriggerTimeUpdated(TimeArgs args) => TimeUpdated?.Invoke(args);

    public static event System.Action<CustomerLoop> TimesUp;
    public static void TriggerTimeUp(CustomerLoop customerLoop) => TimesUp?.Invoke(customerLoop);

    public static event System.Action GameOver;
    public static void TriggerGameOver() => GameOver?.Invoke();

    public static event System.Action<CustomerLoop.Phase> PhaseChanged;
    public static void TriggerPhaseChange(CustomerLoop.Phase phase) => PhaseChanged?.Invoke(phase);

    public static event System.Action<List<IngredientType>> WantsDetermined;
    public static void TriggerWantsDetermined(List<IngredientType> ingredients) => WantsDetermined?.Invoke(ingredients);

    private void OnDestroy()
    {
        SandwichIngredientChanged = null;
        SandwichSubmitted = null;
        SatisfactionChanged = null;
        CustomersServed = null;
        TimeUpdated = null;
        TimesUp = null;
        GameOver = null;
        PhaseChanged = null;
        WantsDetermined = null;
    }

}

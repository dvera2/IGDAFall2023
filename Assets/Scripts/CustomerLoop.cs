using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerLoop : MonoBehaviour
{
    public Animator CustomerAnimator;
    public CustomerCharacter Customer;

    public float StartingDuration = 30f;
    public float NumBeforeReduction = 5;
    public float MinDuration = 5.0f;
    public float DurationReduction = 0.95f;

    public float CurrentTime;
    public float CurrentDuration;

    public int NumberOfCustomersServed = 0;
    public int TipsAmount = 0;

    private bool _isEnabled = false;
    private float _tempTimer = 0;

    public enum Phase
    {
        BetweenCustomers,
        Arriving,
        WaitingForOrder,
        Leaving,
        Tasting,
    }

    public Phase CurrentPhase;

    private void Awake()
    {
        GameEvents.SandwichIngredientChanged += GameEvents_SandwichIngredientChanged;
        GameEvents.SandwichSubmitted += GameEvents_SandwichSubmitted;
    }

    void Start()
    {
        CurrentPhase = Phase.BetweenCustomers;
        StartNewCustomer();
    }

    public void StartNewCustomer()
    {
        if (NumberOfCustomersServed >= 5)
        {
            CurrentTime = 0;
            CurrentDuration = Mathf.Max(MinDuration, CurrentDuration * DurationReduction);
        }

        _isEnabled = true;
        _tempTimer = 0;

        CurrentPhase = Phase.BetweenCustomers;
        CustomerAnimator.SetTrigger("Reset");
    }

    private void Update()
    {
        if (!_isEnabled)
            return;

        if (Input.GetKeyDown(KeyCode.V))
        {
            GameEvents.TriggerSandwichIngredientChanged(new SandwichUpdateArgs());
            return;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            GameEvents.TriggerSandwichSubmitted(new SandwichSubmitArgs());
            return;
        }

        switch (CurrentPhase)
        {
            case Phase.BetweenCustomers:
                {
                    _tempTimer += Time.deltaTime;
                    if (_tempTimer >= 1.0f)
                    {
                        _tempTimer = 0;
                        CurrentPhase = Phase.Arriving;
                        CustomerAnimator.SetTrigger("Arrive");
                    }
                }
                break;

            case Phase.Arriving:
                {
                    _tempTimer += Time.deltaTime;
                    if (_tempTimer >= 1.0f)
                    {
                        _tempTimer = 0;
                        CurrentPhase = Phase.WaitingForOrder;
                    }
                }
                break;

            case Phase.WaitingForOrder:
                {
                    CurrentTime += Time.deltaTime;
                    GameEvents.TriggerTimeUpdated(new TimeArgs()
                    {
                        Time = CurrentTime,
                        Duration = CurrentDuration,
                    });

                    if (CurrentTime >= CurrentDuration)
                    {
                        _isEnabled = false;
                        GameEvents.TriggerTimeUp(this);
                    }
                }
                break;

            case Phase.Tasting:
                {
                    _tempTimer += Time.deltaTime;
                    if (_tempTimer >= 3.0f)
                    {
                        _tempTimer = 0;
                        TellCustomerToLeave();
                    }
                }
                break;

            case Phase.Leaving:
                {
                    _tempTimer += Time.deltaTime;
                    if (_tempTimer >= 1.0f)
                    {
                        _tempTimer = 0;
                        StartNewCustomer();
                    }
                }
                break;

            default:
                break;
        }
    }

    private void GameEvents_SandwichSubmitted(SandwichSubmitArgs args)
    {
        TipsAmount += Customer.GetSandwichScore(args);
        GameEvents.TriggerTipsUpdated(TipsAmount);

        // Go to tasting...
        _tempTimer = 0;
        CurrentPhase = Phase.Tasting;
    }

    private void GameEvents_SandwichIngredientChanged(SandwichUpdateArgs args)
    {
        int matchScore = Customer.GetMatchScore(args);

        CustomerExpression expression = CustomerExpression.Neutral;
        if( matchScore > 0 )
        {
            expression = CustomerExpression.Happy;
        }
        else
        {
            expression = CustomerExpression.Mad;
        }

        GameEvents.TriggerSatisfactionChanged(expression);
    }

    private void TellCustomerToLeave()
    {
        _tempTimer = 0;
        CustomerAnimator.SetTrigger("Leave");
        CurrentPhase = Phase.Leaving;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerLoop : MonoBehaviour
{
    public CharacterFaceData[] Faces;
    public Animator CustomerAnimator;
    public CustomerCharacter Customer;

    public float StartingDuration = 30f;
    public float NumBeforeReduction = 5;
    public float MinDuration = 5.0f;
    public float DurationReduction = 0.95f;

    public float CurrentTime;
    public float CurrentDuration;
    public int FailureAttempts = 3;

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
        CurrentDuration = StartingDuration;
        SetPhase(Phase.BetweenCustomers);
        StartNewCustomer();
    }

    public void StartNewCustomer()
    {
        if (NumberOfCustomersServed >= 5)
        {
            CurrentDuration = Mathf.Max(MinDuration, CurrentDuration * DurationReduction);
        }

        _isEnabled = true;
        _tempTimer = 0;
        CurrentTime = 0;

        Customer.RandomizeWants();
        Customer.SetCustomerHead(Faces[Random.Range(0, Faces.Length)]);
        Customer.SetCustomerTorso(Customer.Bodies[Random.Range(0, Customer.Bodies.Length)]);

        SetPhase(Phase.BetweenCustomers);
        CustomerAnimator.SetTrigger("Reset");
    }

    private void Update()
    {
        if (!_isEnabled)
            return;

        switch (CurrentPhase)
        {
            case Phase.BetweenCustomers:
                {
                    _tempTimer += Time.deltaTime;
                    if (_tempTimer >= 1.0f)
                    {
                        _tempTimer = 0;
                        SetPhase(Phase.Arriving);
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
                        SetPhase(Phase.WaitingForOrder);
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
                        GameEvents.TriggerGameOver(GameOverReason.Time);
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
        int tipsDelta = Customer.GetSandwichScore(args);
        CustomerExpression expression = CustomerExpression.Neutral;
        if (tipsDelta > 1)
        {
            expression = CustomerExpression.Happy;
        }
        else if(tipsDelta >= 0)
        {
            expression = CustomerExpression.Neutral;
        }
        else if (tipsDelta > -2)
        {
            expression = CustomerExpression.Mad;
        }
        else
        {
            expression = CustomerExpression.Sick;
        }

        if(args.IsVerySpicy())
        {
            expression = CustomerExpression.OnFire;
        }


        GameEvents.TriggerSatisfactionChanged(expression);

        int state = 0;
        switch (expression)
        {
            case CustomerExpression.Neutral:
                state = 0; break;

            case CustomerExpression.Mad:
                state = 1; break;

            case CustomerExpression.Sick:
                state = 2; break;

            case CustomerExpression.OnFire:
                state = 3; break;

            case CustomerExpression.Happy:
                state = 4; break;

            default:
                break;
        }


        // Go to tasting...
        _tempTimer = 0;
        SetPhase(Phase.Tasting);
        CustomerAnimator.SetInteger("TasteState", state);
        CustomerAnimator.SetTrigger("Taste");

        if (tipsDelta < 0)
        {
            FailureAttempts--;

            if(FailureAttempts < 0)
            {
                _isEnabled = false;
                GameEvents.TriggerGameOver(GameOverReason.Lose);
                return;
            }
        }
        else
        {
            TipsAmount += tipsDelta;
        }

        NumberOfCustomersServed++;
        GameEvents.TriggerTipsUpdated( new CustomerServedArgs()
        {
            TipsDelta = tipsDelta,
            TipsTotal = TipsAmount,
            CustomersServedSoFar = NumberOfCustomersServed,
        });
    }

    private void GameEvents_SandwichIngredientChanged(SandwichUpdateArgs args)
    {
        int matchScore = Customer.GetMatchScore(args.Ingredient.IngredientType);
        Debug.Log($"Ingredient evaluated: {args.Ingredient.IngredientType}, score= {matchScore}");

        CustomerExpression expression = CustomerExpression.Neutral;
        if( matchScore > 0 )
        {
            expression = CustomerExpression.Happy;
        }
        else if( matchScore < 0 )
        {
            expression = CustomerExpression.Mad;
        }

        GameEvents.TriggerSatisfactionChanged(expression);
    }

    private void TellCustomerToLeave()
    {
        _tempTimer = 0;
        CustomerAnimator.SetTrigger("Leave");
        SetPhase(Phase.Leaving);
    }

    public void SetPhase(Phase newPhase)
    {
        CurrentPhase = newPhase;
        GameEvents.TriggerPhaseChange(CurrentPhase);
    }
}

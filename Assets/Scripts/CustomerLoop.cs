using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerLoop : MonoBehaviour
{
    public Animator CustomerAnimator;
    public CustomerCharacter Customer;
    
    void Start()
    {
        StartCoroutine(TestSequence());
    }

    IEnumerator TestSequence()
    {
        CustomerAnimator.SetTrigger("Reset");
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return null;

        CustomerAnimator.SetTrigger("Arrive");
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        CustomerAnimator.SetTrigger("Leave");
        yield break;
    }
}

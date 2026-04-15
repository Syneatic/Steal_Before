using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AIScript : MonoBehaviour
{
    public PlayerControllerScript player;

    private bool isMimicking = false;
    private int stepsTaken = 0;

    //Definition of the number of steps
    private List<Vector2> pathToFollow;


    public void ActivateMimic(List<Vector2> history)
    {

        if (history == null) return;

        pathToFollow = history;
        isMimicking = true;
        stepsTaken = 0;
        GetComponent<SpriteRenderer>().enabled = true;

        //Start at the oldest point in the list
        transform.position = pathToFollow[0];
        Debug.Log("AI Mimic: True");
        Debug.Log(pathToFollow.Count);
    }

    public void PlayerMove()
    {
        if (!isMimicking || pathToFollow == null) return;

        ++stepsTaken;

        if (stepsTaken < pathToFollow.Count)
        {
            transform.position = pathToFollow[stepsTaken];
            Debug.Log($"AI moving. Step: {stepsTaken}");

            Collider2D hit = Physics2D.OverlapPoint(transform.position, LayerMask.GetMask("Triggers"));

            if (hit != null && hit.TryGetComponent(out TriggerScript button))
            {
                button.PushButton();
                Debug.Log("AI pressed a button!");
            }
        }
        
        if (stepsTaken >= pathToFollow.Count)
        {
            EndMimic();
        }
    }

    public void EndMimic()
    {
        isMimicking = false;
        GetComponent<SpriteRenderer>().enabled = false;
        Debug.Log("AI finished mimic sequence.");
    }

    private void OnEnable()
    {
        GameStepManager.Instance.OnPlayerStep += PlayerMove;
    }

    private void OnDisable()
    {
        GameStepManager.Instance.OnPlayerStep -= PlayerMove;
    }

    public bool GetIsMimic()
    {
        return isMimicking;
    }
}

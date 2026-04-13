using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AIScript : MonoBehaviour
{
    public PlayerControllerScript player;

    private bool isMimicking = false;
    private int stepsTaken = 0;

    public void ActivateMimic()
    {
        isMimicking = true;
        stepsTaken = 0;
        GetComponent<SpriteRenderer>().enabled = true;
        var history = player.GetMovementHistory();
        transform.position = history.First.Value;
        Debug.Log("AI Mimic: True");
    }

    public void PlayerMove()
    {
        if (!isMimicking) return;

        var history = player.GetMovementHistory();

        if (history.Count == 0)
        {
            isMimicking = false;
            return;
        }

        transform.position = history.First.Value;

        stepsTaken++;
        if (stepsTaken >= 5 || history.Count == 0)
        {
            isMimicking = false;
            Debug.Log("AI finished mimic sequence.");

            //Set the AI to invisble
            GetComponent<SpriteRenderer>().enabled = false;
        }
        Debug.Log("AI moving");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AIScript : MonoBehaviour
{
    public PlayerControllerScript player;

    public void PlayerMove()
    {
        var history = player.GetMovementHistory();

        if (history.Count >= 6)
        {
            transform.position = history.First.Value;
        }
        Debug.Log("AI moving");
    }
}

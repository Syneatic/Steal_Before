using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum InteractType {Door, Laser }

public class DoorV2Script : MonoBehaviour
{
    [SerializeField] private List<TriggerScript> Triggers = new List<TriggerScript>();

    public InteractType interactable;
    SpriteRenderer render;
    private Collider2D col;

    public void Start()
    {
        render = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        CheckButtonLogic();
    }

    public void CheckButtonLogic()
    {
        foreach (var trigger in Triggers)
        {
            // The Boolean AND logic
            if (!trigger.IsActivated)
            {
                render.enabled = true;
                col.enabled = true;
                return;
            }
        }

        render.enabled = false;
        col.enabled = false;
    }
}


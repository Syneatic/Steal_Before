using UnityEngine;

public class DoorV2Script : MonoBehaviour
{
    [SerializeField] private TriggerScript buttonA;
    [SerializeField] private TriggerScript buttonB;

    SpriteRenderer render;
    private Collider2D col;

    public void Start()
    {
        render = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    public void CheckButtonLogic()
    {
        // The Boolean AND logic
        if (buttonA.IsActivated && buttonB.IsActivated)
        {
            render.enabled = false;
            col.enabled = false;
        }
        else
        {
            render.enabled = true;
            col.enabled = true;
        }
    }

    //private void OpenDoor()
    //{
    //    Debug.Log("Both buttons active. Door opening!");

    //    // For a simple puzzle: just make the door disappear
    //    gameObject.SetActive(false);

    //    // Or, if you want to keep the object but walk through it:
    //    // GetComponent<Collider2D>().enabled = false;
    //    // GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f); // Make transparent
    //}
}


using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Switch buttonA;
    [SerializeField] private Switch buttonB;

    public void CheckButtonLogic()
    {
        // The Boolean AND logic
        if (buttonA.IsActivated && buttonB.IsActivated)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        Debug.Log("Both buttons active. Door opening!");

        // For a simple puzzle: just make the door disappear
        gameObject.SetActive(false);

        // Or, if you want to keep the object but walk through it:
        // GetComponent<Collider2D>().enabled = false;
        // GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f); // Make transparent
    }
}

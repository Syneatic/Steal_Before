using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool IsActivated { get; private set; } = false;

    [SerializeField] private Color activatedColor = Color.red;
    [SerializeField] private Color idleColor = Color.green;
    [SerializeField] private Door targetDoor;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = idleColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            IsActivated = true;
            spriteRenderer.color = activatedColor;

            // Notify the door to check its conditions
            if (targetDoor != null)
            {
                targetDoor.CheckButtonLogic();
            }
        }
    }
}

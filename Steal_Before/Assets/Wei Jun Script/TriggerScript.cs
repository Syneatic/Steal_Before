using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    public bool IsActivated { get; private set; } = false;

    [SerializeField] private Color activatedColor = Color.red;
    [SerializeField] private Color idleColor = Color.green;
    [SerializeField] private DoorV2Script targetDoor;

    public int stepToStayActive = 5;
    public int currentStepsleft = 0;
    private bool isActive = false;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = idleColor;
    }

    private void OnEnable()
    {
        // If the manager is already ready, subscribe now
        if (GameStepManager.Instance != null)
        {
            GameStepManager.Instance.OnPlayerStep += HandleStep;
            GameStepManager.Instance.OnRewind += DeactivateButton;
        }
        else
        {
            Debug.LogError($"{gameObject.name} Failed to subscribe to manager");
        }
    }

    private void OnDisable()
    {
        if (GameStepManager.Instance != null)
        {
            GameStepManager.Instance.OnPlayerStep -= HandleStep;
            GameStepManager.Instance.OnRewind -= DeactivateButton;
        }
    }

    private void HandleStep()
    {
        if (!isActive) return;

        Debug.Log($"Steps left before its turns back on: {currentStepsleft}");
        currentStepsleft--;

        if (currentStepsleft < 0)
        {
            DeactivateButton();
        }
    }
    public void PushButton()
    {
        // If the button failed to subscribe on start, try one more time now!
        if (GameStepManager.Instance != null)
        {
            // Always unsubscribe first to prevent double-subscription
            GameStepManager.Instance.OnPlayerStep -= HandleStep;
            GameStepManager.Instance.OnPlayerStep += HandleStep;

            GameStepManager.Instance.OnRewind -= DeactivateButton;
            GameStepManager.Instance.OnRewind += DeactivateButton;
        }

        IsActivated = true;
        spriteRenderer.color = activatedColor;
        currentStepsleft = stepToStayActive;

        Debug.Log("Button is pressed");
        isActive = true;

        targetDoor.CheckButtonLogic();
    }

    public void DeactivateButton()
    {
        isActive = false;
        IsActivated = false;
        currentStepsleft = 0;

        if (spriteRenderer != null) spriteRenderer.color = idleColor;

        // Notify the door to check its conditions
        if (targetDoor != null)
        {
            targetDoor.CheckButtonLogic();
        }

        Debug.Log("Button has turn back on");
    }
}

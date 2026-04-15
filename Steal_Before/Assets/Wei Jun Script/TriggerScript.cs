using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    public bool IsActivated { get; private set; } = false;

    [SerializeField] private Color activatedColor = Color.red;
    [SerializeField] private Color idleColor = Color.green;
    [SerializeField] private DoorV2Script ObjectInteract;

    public List<bool> buttonHistory = new List<bool>();

    private int stepToStayActive => GameStepManager.Instance.MaxHistory - 1;
    private int currentStepsleft = 0;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = idleColor;
    }

    private void Start()
    {
        Subscribe();
    }

    private void OnEnable()
    {
        // If the manager is already ready, subscribe now
        Subscribe();
    }

    private void OnDisable()
    {
        if (GameStepManager.Instance != null)
        {
            GameStepManager.Instance.OnPlayerStep -= HandleStep;
            GameStepManager.Instance.OnPlayerStep -= OnSaveState;

            GameStepManager.Instance.OnRewind -= RefreshButtonState;
        }
    }

    public void Subscribe()
    {
        if (GameStepManager.Instance != null)
        {
            GameStepManager.Instance.OnPlayerStep += HandleStep;
            GameStepManager.Instance.OnPlayerStep += OnSaveState;

            GameStepManager.Instance.OnRewind += RefreshButtonState;
        }
    }


    private void HandleStep()
    {
        if (!IsActivated) return;

        Debug.Log($"Steps left before its turns back on: {currentStepsleft}");
        currentStepsleft--;

        if (currentStepsleft < 0)
        {
            DeactivateButton();
        }
    }
    public void PushButton()
    {
        IsActivated = true;
        spriteRenderer.color = activatedColor;
        currentStepsleft = stepToStayActive;

        Debug.Log("Button is pressed");

        ObjectInteract.CheckButtonLogic();
    }

    public void DeactivateButton()
    {
        IsActivated = false;
        currentStepsleft = 0;

        if (spriteRenderer != null) spriteRenderer.color = idleColor;

        // Notify the door to check its conditions
        if (ObjectInteract != null)
        {
            ObjectInteract.CheckButtonLogic();
        }

        Debug.Log("Button has turn back on");
    }

    private void OnSaveState() // Call this when RegisterStep happens
    {
        buttonHistory.Add(IsActivated);

        if (buttonHistory.Count > stepToStayActive) // Use your maxNode variable here
        {
            buttonHistory.RemoveAt(0);
        }
    }

    private void RefreshButtonState()
    {
        if (buttonHistory[buttonHistory.Count - 1] == false)
        {
            return;
        }

        PushButton();

        // Now update the color based on the new isPressed value
        spriteRenderer.color = IsActivated ? Color.red : Color.green;
    }
}

using UnityEngine;


public class GameplayUIManager : MonoBehaviour
{
    [SerializeField] private Animator pendantAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void UpdatePendant(int currentSteps, int maxStepsForThisLevel)
    {
        if (pendantAnimator != null)
        {
            float percentage = (float)currentSteps / maxStepsForThisLevel;
            int uiStage = Mathf.CeilToInt(percentage * 5); // This turns 0.0-1.0 into 1-5

            pendantAnimator.SetInteger("EnergyLevel", uiStage);
        }

    }
}

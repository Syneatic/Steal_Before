using UnityEngine;

public class TimeDebugger : MonoBehaviour
{
    void Update()
    {
        // Time.time stops when timeScale is 0
        // Time.realtimeSinceStartup keeps counting regardless
        Debug.Log($"Scale: {Time.timeScale} | Game Time: {Time.time}");
    }
}

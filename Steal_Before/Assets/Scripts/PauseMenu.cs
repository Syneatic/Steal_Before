using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI; // Required if you want to change the button icon

//Audio
public class VolumeControl : MonoBehaviour
{
    private bool isMuted = false;

    public void ToggleMute()
    {
        isMuted = !isMuted;

        // This silences all AudioSources in the entire game
        AudioListener.pause = isMuted;

        // Debug to verify in console
        Debug.Log(isMuted ? "Game Muted" : "Game Unmuted");
    }
}

public class PauseMenu : MonoBehaviour
{
    static bool GamePaused = false;

    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (GamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Debug.Log("Click Registered");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // Resumes normal game speed
        GamePaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Freezes physics and timers
        GamePaused = true;
    }

    public void RestartLevel()
    {
        Debug.Log("Click Registered");
        Time.timeScale = 1f; // Always reset time before reloading!
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMenu()
    {
        Debug.Log("Click Registered");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu"); // Ensure this matches your Build Settings name
    }

}

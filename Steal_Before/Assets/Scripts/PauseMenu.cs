using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;

    static bool GamePaused = false;

    public GameObject pauseMenuUI;
    public PlayerControllerScript player;

    void Start()
    {
        player = Object.FindAnyObjectByType<PlayerControllerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) return;
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (GamePaused)
            {
                Resume();
                player.controls.Enable();
            }
            else
            {
                Pause();
                player.controls.Disable();
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

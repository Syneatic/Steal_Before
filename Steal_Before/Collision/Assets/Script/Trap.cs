using UnityEngine;
using UnityEngine.SceneManagement;

public class Trap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Trap activated");
            RestartLevel();
        }

    /*/////////////////////////////////////////////////
        @brief - Restarts level when collide with trap
    
    *//////////////////////////////////////////////////
    void RestartLevel()
    {
        // Get the index of the currently active scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reload it
        SceneManager.LoadScene(currentSceneIndex);
    }
}
}

using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonScript : MonoBehaviour
{
    public GameObject SettingsPage;
    public GameObject CreditsPage;
    public GameObject MainMenu;

    void Start()
    {
        MainMenu.SetActive(true);
        CreditsPage.SetActive(false);
        SettingsPage.SetActive(false);
    }

    public void StartGame()
    {
        MainMenu.SetActive(false);
        SceneManager.LoadScene("SampleScene");
    }

    public void OpenSettings()
    {
        MainMenu.SetActive(false);
        SettingsPage.SetActive(true);
    }

    public void ReturnFromSettings()
    {
        MainMenu.SetActive(true);
        SettingsPage.SetActive(false);
    }

    public void ShowCredits()
    {
        MainMenu.SetActive(false);
        CreditsPage.SetActive(true);
    }

    public void ReturnFromCredits()
    {
        MainMenu.SetActive(true);
        CreditsPage.SetActive(false);
    }

    public void QuitGame()
    {
        MainMenu.SetActive(false);
        Application.Quit();
    }
}

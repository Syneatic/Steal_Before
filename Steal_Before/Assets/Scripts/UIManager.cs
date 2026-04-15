using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject SettingsPage;
    public GameObject CreditsPage;
    public GameObject MainMenu;
    //public GameObject LevelSelect;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        MainMenu.SetActive(true);
        CreditsPage.SetActive(false);
        SettingsPage.SetActive(false);
    }

    public void StartGame()
    {
        MainMenu.SetActive(false);
        SceneManager.LoadScene("Lvl1");
    }

    public void ToggleSettings()
    {
        ToggleUIPair(ref MainMenu, ref SettingsPage);
    }

    public void ToggleCredits()
    {
        ToggleUIPair(ref MainMenu, ref CreditsPage);
    }

    public void QuitGame()
    {
        MainMenu.SetActive(false);
        Application.Quit();
    }

    public void ToggleUIPair(ref GameObject lhs, ref GameObject rhs)
    {
        lhs.SetActive(!lhs.activeSelf);
        rhs.SetActive(!rhs.activeSelf);
    }
}

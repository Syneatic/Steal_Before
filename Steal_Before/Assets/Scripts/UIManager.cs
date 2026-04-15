using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject SettingsPage;
    public GameObject CreditsPage;
    public GameObject MainMenu;
    public GameObject LevelSelect;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        MainMenu.SetActive(true);
        CreditsPage.SetActive(false);
        SettingsPage.SetActive(false);
        LevelSelect.SetActive(false);
    }

    public void ToggleStartGame()
    {
        ToggleUIPair(ref MainMenu, ref LevelSelect);
    }

    public void ToggleSettings()
    {
        ToggleUIPair(ref MainMenu, ref SettingsPage);
    }

    public void ToggleCredits()
    {
        ToggleUIPair(ref MainMenu, ref CreditsPage);
    }

    public void LevelSelect1()
    {
        MainMenu.SetActive(false);
        SceneManager.LoadScene("Lvl1");
    }

    public void LevelSelect2()
    {
        MainMenu.SetActive(false);
        SceneManager.LoadScene("Lvl2");
    }

    public void LevelSelect3()
    {
        MainMenu.SetActive(false);
        SceneManager.LoadScene("Lvl3");
    }

    public void LevelSelect4()
    {
        MainMenu.SetActive(false);
        SceneManager.LoadScene("Lvl4");
    }

    public void LevelSelect5()
    {
        MainMenu.SetActive(false);
        SceneManager.LoadScene("Lvl5");
    }

    public void LevelSelect6()
    {
        MainMenu.SetActive(false);
        SceneManager.LoadScene("Lvl6");
    }

    public void LevelSelect7()
    {
        MainMenu.SetActive(false);
        SceneManager.LoadScene("Lvl7");
    }

    public void LevelSelect8()
    {
        MainMenu.SetActive(false);
        SceneManager.LoadScene("Lvl8");
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

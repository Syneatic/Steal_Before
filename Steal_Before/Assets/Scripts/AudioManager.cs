using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Speakers")]
    public AudioSource menuSource;
    public AudioSource gameplaySource;

    [Header("Menu Sounds")]
    public AudioClip selectSound;
    public AudioClip startButtonSound;

    [Header("Gameplay Sounds")]
    public AudioClip moveSound;
    public AudioClip rewindSound;
    public AudioClip deathSound;
    public AudioClip platePress;
    public AudioClip plateRelease;

    void Awake()
    {
        Instance = this;
    }

    // A simple function to play any sound through any source
    public void PlaySound(AudioClip clip, bool isMenuSound)
    {
        if (clip == null) return;

        if (isMenuSound)
            menuSource.PlayOneShot(clip);
        else
            gameplaySource.PlayOneShot(clip);

    }

    public void PlayMenuSelect()
    {
        PlaySound(selectSound, true);
    }

    // Easy access for UI "Start" Buttons
    public void PlayStartButton()
    {
        PlaySound(startButtonSound, true);
    }

 
       
    
}
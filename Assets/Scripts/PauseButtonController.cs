using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButtonController : MonoBehaviour, IInteractiveButton
{
    private bool isPaused = false;
    public Sprite pauseIcon;
    public Sprite playIcon;
    private Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ButtonClick()
    {
        Time.timeScale = isPaused ? 1.0f : 0.0f;
        isPaused = !isPaused;
        if (isPaused)
        {
            image.sprite = playIcon;
            // Pause all audio sources
            foreach (var audio in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
            {
                audio.Pause();
            }
        }
        else
        {
            image.sprite = pauseIcon;
            // Unpause all audio sources
            foreach (var audio in FindObjectsByType<AudioSource>(FindObjectsSortMode.None))
            {
                audio.UnPause();
            }
        }
    }
}

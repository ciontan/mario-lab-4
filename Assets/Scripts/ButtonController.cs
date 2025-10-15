using System.Collections;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public void ButtonClick()
    {
        // Hide the game over panel
        HUDManager hudManager = FindObjectOfType<HUDManager>();
        if (hudManager != null)
        {
            if (hudManager.gameOverPanel != null)
            {
                hudManager.gameOverPanel.SetActive(false);
            }

            // Explicitly call GameStart to reposition UI elements
            hudManager.GameStart();
        }

        // Make sure time scale is set to 1 (unfrozen)
        Time.timeScale = 1.0f;
        Debug.Log("Button clicked, set Time.timeScale = " + Time.timeScale);

        // Then restart the game
        GameManager.instance.GameRestart();
    }
}

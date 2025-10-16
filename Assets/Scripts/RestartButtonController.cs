using System.Collections;
using UnityEngine;

public class RestartButtonController : MonoBehaviour, IInteractiveButton
{
    public void ButtonClick()
    {
        GameManager.instance.GameRestart();
        Debug.Log("Restart button pressed, called GameRestart.");
    }
}

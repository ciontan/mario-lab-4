using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }

    void Awake()
    {
        // No need to call base.Awake() as we're no longer a Singleton
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameRestart()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<EnemyMovement>().ResetGame();
        }
    }
}

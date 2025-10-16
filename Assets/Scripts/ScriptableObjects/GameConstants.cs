using UnityEngine;

[CreateAssetMenu(fileName =  "GameConstants", menuName =  "ScriptableObjects/GameConstants", order =  1)]
public  class GameConstants : ScriptableObject
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // lives
    public int maxLives;

    // Mario's movement
    public int speed;
    public int maxSpeed;
    public int upSpeed;
    public int deathImpulse;
    public Vector3 marioStartingPosition;

    // Goomba's movement
    public float goombaPatrolTime;
    public float goombaMaxOffset;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

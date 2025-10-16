using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMushroomPowerup : BasePowerup
{
    private bool moveStarted = false;

    // setup this object's type
    // instantiate variables
    protected override void Start()
    {
        base.Start(); // call base class Start()
        this.type = PowerupType.MagicMushroom;
    }

    // interface implementation
    public override void SpawnPowerup()
    {
        if (!moveStarted)
        {
            spawned = true;
            moveStarted = true;
            rigidBody.AddForce(Vector2.right * 3, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && spawned)
        {
            // TODO: do something when colliding with Player

            // then destroy powerup (optional)
            DestroyPowerup();

        }
        else if (col.gameObject.layer == 10 && moveStarted) // only change direction after movement has started
        {
            goRight = !goRight;
            rigidBody.AddForce(Vector2.right * 3 * (goRight ? 1 : -1), ForceMode2D.Impulse);

        }
    }

    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO: do something with the object

    }
}
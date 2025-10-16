using System.Collections;
using UnityEngine;

public class BoxBrick : MonoBehaviour
{
    [SerializeField] private AudioSource sfx;       // coin SFX
    [SerializeField] private LayerMask playerMask;  // layer for Mario

    [SerializeField] private Sprite usedSprite;     // sprite for after box is used
    public BasePowerup powerup; // reference to the coin powerup
    public GameObject coinPrefab; // reference to the coin prefab

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private bool used = false;
    public GameManager gameManager;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (used) return;

        // Must be the player (by layer)
        if (((1 << col.collider.gameObject.layer) & playerMask) == 0) return;

        // Accept hit only from below.
        bool fromBelow = false;

        // Primary: contact normal on THIS box tends to be ~Vector2.down when hit on its bottom.
        for (int i = 0; i < col.contactCount; i++)
        {
            var n = col.GetContact(i).normal;
            if (n.y < -0.5f) { fromBelow = true; break; }
        }

        // Fallback: player moving upward, but only if below the box center
        if (!fromBelow && col.relativeVelocity.y > 0.1f)
        {
            if (col.transform.position.y < transform.position.y)
                fromBelow = true;
        }

        if (!fromBelow) return;

        StartCoroutine(TriggerBox());
    }

    private IEnumerator TriggerBox()
    {
        used = true;

        if (sfx) sfx.Play();

        // Change to "used" sprite
        if (usedSprite != null)
            sr.sprite = usedSprite;

        // Prefer powerup system
        if (powerup != null && powerup.powerupType == PowerupType.Coin && !powerup.hasSpawned)
        {
            // Set the GameManager reference
            CoinPowerup coinPowerup = powerup as CoinPowerup;
            if (coinPowerup != null)
            {
                coinPowerup.gameManager = gameManager;
            }

            // Enable and spawn the powerup
            if (!powerup.gameObject.activeSelf)
            {
                powerup.gameObject.SetActive(true);
            }
            powerup.SpawnPowerup();
        }
        // Fallback to prefab system if no powerup
        else if (coinPrefab != null)
        {
            Vector3 localSpawnPos = new Vector3(0, 1f, 0);
            Vector3 spawnPosition = transform.TransformPoint(localSpawnPos);
            Debug.Log($"Box position: {transform.position}, Spawn position: {spawnPosition}");
            GameObject coinObj = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
            Coin coinScript = coinObj.GetComponent<Coin>();
            if (coinScript != null)
                coinScript.Animate();
            // Increase score and specify it's from a coin (true)
            gameManager.IncreaseScore(1, true);
            Destroy(coinObj, 1f); // coin disappears after 1s
        }

        yield return null;
    }

    public void ResetBox()
    {
        used = false;
        if (sr != null && usedSprite != null)
            sr.sprite = usedSprite;
        if (rb != null)
            rb.bodyType = RigidbodyType2D.Dynamic;

        // Reset the powerup if it exists
        if (powerup != null)
        {
            powerup.ResetPowerup();
        }
    }
}
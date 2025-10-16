using System.Collections;
using UnityEngine;

public class QuestionBoxPowerupController : MonoBehaviour, IPowerupController
{
    [SerializeField] private AudioSource sfx;       // SFX
    [SerializeField] private LayerMask playerMask;  // layer for Mario
    [SerializeField] private Sprite usedSprite;     // sprite for after box is used
    [SerializeField] private Sprite unusedSprite;   // sprite for before box is used

    public BasePowerup powerup; // reference to this question box's powerup
    public Animator powerupAnimator;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private bool used = false;

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

        // First trigger the box animation
        this.GetComponent<Animator>().SetTrigger("spawned");

        // Enable the powerup GameObject but don't spawn it yet
        if (powerup != null && !powerup.hasSpawned)
        {
            if (!powerup.gameObject.activeSelf)
                powerup.gameObject.SetActive(true);

            // Trigger powerup animation
            powerupAnimator.SetTrigger("spawned");

            // // Wait for animation to finish (adjust time based on your animation length)
            // yield return new WaitForSeconds(0.5f);

            // Now spawn the powerup (this will make it move)
            powerup.SpawnPowerup();
        }

        // After a short moment, disable the joint
        yield return new WaitForSeconds(0.3f);
        rb.bodyType = RigidbodyType2D.Static;
        transform.localPosition = new Vector3(0, 0, 0);
    }

    public void ResetBox()
    {
        used = false;
        if (sr != null && unusedSprite != null)
            sr.sprite = unusedSprite;
        if (rb != null)
            rb.bodyType = RigidbodyType2D.Dynamic;
    }

    // Implementation of IPowerupController
    public void Disable()
    {
        rb.bodyType = RigidbodyType2D.Static;
        transform.localPosition = new Vector3(0, 0, 0);
    }
}
using UnityEngine;

public class CoinPowerup : BasePowerup
{
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float jumpDuration = 0.5f;
    private Vector3 startPosition;
    private SpriteRenderer sr;
    public GameManager gameManager;

    protected override void Start()
    {
        base.Start();
        this.type = PowerupType.Coin;
        sr = GetComponent<SpriteRenderer>();
    }

    public override void SpawnPowerup()
    {
        if (!spawned)
        {
            spawned = true;
            startPosition = transform.position;
            Animate();
        }
    }

    private void Animate()
    {
        StartCoroutine(JumpAnimation());
    }

    private System.Collections.IEnumerator JumpAnimation()
    {
        float elapsedTime = 0;

        while (elapsedTime < jumpDuration)
        {
            float jumpProgress = elapsedTime / jumpDuration;
            float heightCurve = Mathf.Sin(jumpProgress * Mathf.PI);
            transform.position = startPosition + new Vector3(0, heightCurve * jumpHeight, 0);

            if (jumpProgress > 0.7f)
            {
                Color coinColor = sr.color;
                coinColor.a = 1 - ((jumpProgress - 0.7f) / 0.3f);
                sr.color = coinColor;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        DestroyPowerup();
    }

    public override void ApplyPowerup(MonoBehaviour i)
    {
        if (gameManager != null)
        {
            gameManager.IncreaseScore(1, true);
        }
    }
}
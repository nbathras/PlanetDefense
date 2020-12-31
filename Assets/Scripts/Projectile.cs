using UnityEngine;

public class Projectile : MonoBehaviour {
    public static Projectile Create(Vector3 spawnPosition, Vector3 targetPosition) {
        Transform pfProjectile = Resources.Load<Transform>("pfProjectile");
        Transform projectileTransform = Instantiate(pfProjectile, spawnPosition, Quaternion.identity);

        Projectile projectile = projectileTransform.GetComponent<Projectile>();
        Vector3 normalizedDirection = targetPosition - spawnPosition;

        projectile.normalizedDirection = new Vector3(normalizedDirection.x, normalizedDirection.y, 0f).normalized;
        projectile.targetPostion = new Vector3(targetPosition.x, targetPosition.y, 0f);

        return projectile;
    }

    /* Timers */
    [SerializeField] private float despawnTimerMax;
    private float despawnTimer;
    [SerializeField] private float flashTimerMax;
    private float flashTimer;
    [SerializeField] private float fadeTimerMax;
    private float fadeTimer;

    /* Refrences */
    [SerializeField] private Transform explosionTransform;
    [SerializeField] private Transform explosionFlashTransform;

    /* Parameters */
    [SerializeField] private float speed;

    private SpriteRenderer sr;

    private Vector3 targetPostion;
    private Vector3 normalizedDirection;
    private bool isExploded;
    private bool isFading;

    private void Awake() {
        despawnTimer = despawnTimerMax;
        flashTimer = flashTimerMax;
        fadeTimer = fadeTimerMax;

        isExploded = false;
        isFading = false;

        explosionTransform.gameObject.SetActive(false);
        explosionFlashTransform.gameObject.SetActive(false);

        sr = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (!isExploded) {
            HandleMovement();
            HandleFuse();
        } else {
            if (!isFading) {
                HandleFlashTimer();
                HandleExplosion();
                HandleDespawnTimer();
            } else {
                HandleFade();
            }
        }
    }

    private void HandleFade() {
        fadeTimer -= Time.deltaTime;

        float size = (fadeTimer / fadeTimerMax);

        transform.localScale = new Vector3(.75f * size + .25f, .75f * size + .25f, 0);
        SpriteRenderer[] srList = explosionTransform.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < srList.Length; i++) {
            Color currentColor = srList[i].color;
            srList[i].color = new Color(currentColor.r, currentColor.g, currentColor.b, size);
        }

        if (fadeTimer < 0f) {
            Destroy(gameObject);
        }
    }

    private void HandleMovement() {
        transform.position += normalizedDirection * Time.deltaTime * speed;
    }

    private void HandleFuse() {
        if (Vector3.Distance(transform.position, targetPostion) < 0.1f) {
            // Debug.Log("Projectile " + name + " exploded");
            isExploded = true;
            sr.color = new Color(0, 0, 0, 0);
            explosionTransform.gameObject.SetActive(true);
        }
    }

    private void HandleFlashTimer() {
        flashTimer -= Time.deltaTime;
        if (flashTimer >= 0) {
            explosionFlashTransform.gameObject.SetActive(true);
        } else {
            explosionFlashTransform.gameObject.SetActive(false);
        }
    }

    private void HandleExplosion() {
        float size = (1 - despawnTimer / despawnTimerMax);

        transform.localScale = new Vector3(size, size, 0);
    }

    private void HandleDespawnTimer() {
        despawnTimer -= Time.deltaTime;
        if (despawnTimer < 0f) {
            // Debug.Log("Projectile " + name + " was destoryed");
            // Destroy(gameObject);
            isFading = true;
        }
    }
}

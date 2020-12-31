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

    /* Refrences */
    [SerializeField] private Transform explosionTransform;
    [SerializeField] private Transform explosionFlashTransform;

    /* Parameters */
    [SerializeField] private float speed;

    private Vector3 targetPostion;
    private Vector3 normalizedDirection;
    private bool isExploded;

    private void Awake() {
        despawnTimer = despawnTimerMax;
        flashTimer = flashTimerMax;
        isExploded = false;

        explosionTransform.gameObject.SetActive(false);
        explosionFlashTransform.gameObject.SetActive(false);
    }

    private void Update() {
        if (!isExploded) {
            HandleMovement();
            HandleFuse();
        } else {
            HandleFlashTimer();
            HandleExplosion();
            HandleDespawnTimer();
        }
    }

    private void HandleMovement() {
        transform.position += normalizedDirection * Time.deltaTime * speed;
    }

    private void HandleFuse() {
        if (Vector3.Distance(transform.position, targetPostion) < 0.1f) {
            // Debug.Log("Projectile " + name + " exploded");
            isExploded = true;
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
            Destroy(gameObject);
        }
    }
}

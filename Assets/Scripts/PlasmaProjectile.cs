using UnityEngine;

public class PlasmaProjectile : MonoBehaviour {
    public static PlasmaProjectile Create(Vector3 spawnPosition, Vector3 targetPosition) {
        Transform pfPlasmaProjectile = Resources.Load<Transform>("pfPlasmaProjectile");
        Transform plasmaProjectileTransform = Instantiate(pfPlasmaProjectile, spawnPosition, Quaternion.identity);

        PlasmaProjectile plasmaProjectile = plasmaProjectileTransform.GetComponent<PlasmaProjectile>();
        Vector3 normalizedDirection = targetPosition - spawnPosition;

        plasmaProjectile.normalizedDirection = new Vector3(normalizedDirection.x, normalizedDirection.y, 0f).normalized;

        return plasmaProjectile;
    }

    /* Other Parameters */
    [Header("Other Parameters")]
    [SerializeField] private float projectileSpeed;

    private Vector3 normalizedDirection;

    private void Update() {
        if (!PauseMenuUI.IsGamePaused) {
            // Moves projectile to taget at constant speed;
            transform.position += normalizedDirection * Time.deltaTime * projectileSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Projectile") || other.CompareTag("Laser")) {
            ScoreController.Instance.AddScore(ScoreController.ScoreCategories.AsteroidsDestroyed, 20);
            Destroy(gameObject);
        } else if (other.CompareTag("Ground")) {
            Destroy(gameObject);
        } else if (other.CompareTag("City")) {
            // Destory city
            Destroy(other.gameObject);
            // Destory plasama
            Destroy(gameObject);
        }
    }
}

using UnityEngine;

public class AsteroidSplinter : Enemy {

    public static AsteroidSplinter Create(string name) {
        // Corner locations in world coordinates
        Vector2 upperLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height));
        Vector2 upperRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 lowerLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 lowerRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0));

        Vector2 spawnPosition = new Vector2(Random.Range(upperLeft.x, upperRight.x), upperRight.y + .075f);
        Vector2 direction = (new Vector2(Random.Range(lowerLeft.x, lowerRight.x), lowerRight.y) - spawnPosition).normalized;

        Transform pfAsteroidSplinter = Resources.Load<Transform>("pfAsteroidSplinter");
        Transform asteroidSplinterTransform = Instantiate(pfAsteroidSplinter, spawnPosition, Quaternion.identity);

        AsteroidSplinter asteroidSplinter = asteroidSplinterTransform.GetComponent<AsteroidSplinter>();
        Vector3 normalizedDirection = direction.normalized;

        asteroidSplinter.name = name;
        asteroidSplinter.normalizedDirection = new Vector3(normalizedDirection.x, normalizedDirection.y, 0f);
        asteroidSplinter.spawnPosition = new Vector3(spawnPosition.x, spawnPosition.y, 0f);

        return asteroidSplinter;
    }

    [SerializeField] private float despawnTimerMax;
    private float despawnTimer;

    private float splinterTimer;
    private int numberOfSplinters;

    [SerializeField] private float speed;

    private Vector3 spawnPosition;
    private Vector3 normalizedDirection;
    private LineRenderer lineRenderer;

    private void Awake() {
        despawnTimer = despawnTimerMax;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = .02f;
        lineRenderer.endWidth = .02f;

        numberOfSplinters = Random.Range(2, 4);
        splinterTimer = Random.Range(10, 50) / 10f;
    }

    private void Update() {
        if (!PauseMenuUI.IsGamePaused) {
            transform.position += normalizedDirection * speed * Time.deltaTime;
            lineRenderer.SetPosition(0, spawnPosition);
            lineRenderer.SetPosition(1, transform.position);

            despawnTimer -= Time.deltaTime;
            if (despawnTimer < 0f) {
                Destroy(gameObject);
            }

            splinterTimer -= Time.deltaTime;
            if (splinterTimer < 0f) {
                for (int i = 0; i < numberOfSplinters; i++) {
                    EnemySpawnerController.Instance.SpawnAsteroid(transform.position);
                }
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Projectile") || other.CompareTag("Laser")) {
            ScoreController.Instance.AddScore(ScoreController.ScoreCategories.AsteroidsDestroyed, 30);
            Destroy(gameObject);
        } else if (other.CompareTag("Ground")) {
            Destroy(gameObject);
        } else if (other.CompareTag("City")) {
            // Destory city
            Destroy(other.gameObject);
            // Destory asteroid
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        EnemySpawnerController.Instance.RemoveEnemy(this);
    }
}

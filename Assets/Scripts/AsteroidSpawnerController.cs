using UnityEngine;

public class AsteroidSpawnerController : MonoBehaviour {

    public static AsteroidSpawnerController Instance;

    private Camera mainCamera;

    [SerializeField] private float spawnTimerMax;
    private float spawnTimer;

    private void Awake() {
        if (Instance == null) { Instance = this; }

        mainCamera = Camera.main;

        spawnTimer = spawnTimerMax;
    }

    private void Update() {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0f) {
            SpawnAsteroid();
            spawnTimer = spawnTimerMax;
        }
    }

    private void SpawnAsteroid() {
        // Corner screen coordinates 
        Vector2 upperLeftScreen  = new Vector2(0, Screen.height);
        Vector2 upperRightScreen = new Vector2(Screen.width, Screen.height);
        Vector2 lowerLeftScreen  = new Vector2(0, 0);
        Vector2 lowerRightScreen = new Vector2(Screen.width, 0);
    
        // Corner locations in world coordinates
        Vector2 upperLeft = mainCamera.ScreenToWorldPoint(upperLeftScreen);
        Vector2 upperRight = mainCamera.ScreenToWorldPoint(upperRightScreen);
        Vector2 lowerLeft = mainCamera.ScreenToWorldPoint(lowerLeftScreen);
        Vector2 lowerRight = mainCamera.ScreenToWorldPoint(lowerRightScreen);
        
        Vector2 spawnPosition = new Vector2(Random.Range(upperLeft.x, upperRight.x), upperRight.y + .075f);
        Vector2 direction = (new Vector2(Random.Range(lowerLeft.x, lowerRight.x), lowerRight.y) - spawnPosition).normalized;

        Asteroid.Create(spawnPosition, direction);
    }
}

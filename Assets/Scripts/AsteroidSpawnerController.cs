using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawnerController : MonoBehaviour {

    public static AsteroidSpawnerController Instance;

    private Camera mainCamera;

    [SerializeField] private float spawnTimerMax;
    private float spawnTimer;

    private List<Asteroid> asteroidList;

    private void Awake() {
        if (Instance == null) { Instance = this; }

        mainCamera = Camera.main;
        Pause(true);
    }

    private void Update() {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer < 0f) {
            SpawnAsteroid();
            spawnTimer = spawnTimerMax / (1 + GameManager.Instance.GetLevel());
        }
    }
    
    public void Setup() {
        Cleanup();
        Pause(false);

        spawnTimer = spawnTimerMax / (1 + GameManager.Instance.GetLevel());

        asteroidList = new List<Asteroid>();
    }

    public void Cleanup() {
        if (asteroidList != null) {
            for (int i = 0; i < asteroidList.Count; i++) {
                if (asteroidList[i] != null) {
                    Destroy(asteroidList[i].gameObject);
                }
            }

            asteroidList = null;
        }
        Pause(true);
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

        asteroidList.Add(Asteroid.Create(spawnPosition, direction, "Asteroid " + asteroidList.ToString()));
    }

    public void Pause(bool isPaused) {
        Instance.enabled = !isPaused;
        if (asteroidList != null) {
            for (int i = 0; i < asteroidList.Count; i++) {
                if (asteroidList[i]) {
                    asteroidList[i].enabled = !isPaused;
                }
            }
        }
    }

    public bool DestroyAsteroid(Asteroid asteroid) {
        if (asteroid == null) {
            throw new System.Exception("Error: Attempted to destory an asteroid with a null references");
        }
        if (!asteroidList.Remove(asteroid)) {
            throw new System.Exception("Error: Attempted to destory an asteroid not in asteroid list");
        }

        Destroy(asteroid.gameObject);

        return true;
    }
}

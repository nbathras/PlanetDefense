using System;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawnerController : MonoBehaviour {

    public static AsteroidSpawnerController Instance;

    private Camera mainCamera;

    [SerializeField] private float levelLengthTimerMax;
    private float levelLengthTimer;

    private int[] asteroidSpawnTimes;
    private List<Asteroid> asteroidList;

    public event EventHandler OnAsteroidDestory;

    private void ValidateSerializeField() {
        if (levelLengthTimerMax < 1f) {
            throw new Exception("Serialize Field Validation Error!\nField: levelLengthTimerMax\nReason: Field must be greater than or equal to 1");
        }
    }

    private void Awake() {
        if (Instance == null) { 
            Instance = this; 
        }

        ValidateSerializeField();

        mainCamera = Camera.main;
        Pause(true);
    }

    private void Update() {
        levelLengthTimer -= Time.deltaTime;
        SpawnAsteroidWave();
        if (levelLengthTimer < -1f && asteroidList.Count == 0) {
            GameManager.Instance.EndLevel();
        }
    }
    
    public void Setup() {
        Pause(true);
    }

    public void SetupLevel() {
        Pause(true);
        Cleanup();
        asteroidList = new List<Asteroid>();
        GenerateAsteroidSpawnTimes();
    }

    public void StartLevel() {
        levelLengthTimer = levelLengthTimerMax;
        Pause(false);
    }

    public void EndLevel() {
        Pause(true);
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

        asteroidSpawnTimes = null;

        Pause(true);
    }

    private void GenerateAsteroidSpawnTimes() {
        // Current level
        int level = LevelController.Instance.GetLevel();
        // Provides half second granularity
        int levelTime = (int)(levelLengthTimerMax);
        // Array of spawn times for asteroids
        asteroidSpawnTimes = new int[levelTime * 2];
        // Number of asteroids to spawn
        int numberOfAsteroids =  (level + 2) * (levelTime / 6);

        int f = asteroidSpawnTimes.Length / (2 * numberOfAsteroids / 3);
        for (int i = 0; i < asteroidSpawnTimes.Length; i += f) {
            asteroidSpawnTimes[i] += 1;
            numberOfAsteroids--;
        }

        for (int i = 0; i < numberOfAsteroids; i++) {
            int index = UnityEngine.Random.Range(0, asteroidSpawnTimes.Length);
            asteroidSpawnTimes[index] += 1;
        }
    }

    private void SpawnAsteroidWave() {
        int index = Mathf.RoundToInt(levelLengthTimer * 2 - 1);
        if (index >= 0 && index < asteroidSpawnTimes.Length) {
            for (int i = 0; i < asteroidSpawnTimes[index]; i++) {
                SpawnAsteroid();
            }
            asteroidSpawnTimes[index] = 0;
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
        
        Vector2 spawnPosition = new Vector2(UnityEngine.Random.Range(upperLeft.x, upperRight.x), upperRight.y + .075f);
        Vector2 direction = (new Vector2(UnityEngine.Random.Range(lowerLeft.x, lowerRight.x), lowerRight.y) - spawnPosition).normalized;

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

    public void RemoveAsteroid(Asteroid asteroid) {
        if (!asteroidList.Remove(asteroid)) {
            throw new Exception("Error: Attempted to destory an asteroid not in asteroid list");
        }

        OnAsteroidDestory?.Invoke(this, EventArgs.Empty);
    }

    /*
    public bool DestroyAsteroid(Asteroid asteroid) {
        if (asteroid == null) {
            throw new Exception("Error: Attempted to destory an asteroid with a null references");
        }
        if (!asteroidList.Remove(asteroid)) {
            throw new Exception("Error: Attempted to destory an asteroid not in asteroid list");
        }

        Destroy(asteroid.gameObject);

        OnAsteroidDestory?.Invoke(this, EventArgs.Empty);

        return true;
    }
    */

    public int GetRemainingAsteroids() {
        int r = 0;
        if (asteroidSpawnTimes != null) {
            for (int i = 0; i < asteroidSpawnTimes.Length; i++) {
                r += asteroidSpawnTimes[i];
            }
        }
        return r;
    }
}

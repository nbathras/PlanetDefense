﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour {

    private class Spawns {
        private int asteroids;
        private int aliens;
        private int splinter;

        public Spawns() {
            asteroids = 0;
            aliens = 0;
        }

        public void AddAsteroids(int a) {
            asteroids += a;
        }

        public int GetAsteroids() {
            return asteroids;
        }

        public void AddAliens(int a) {
            aliens += a;
        }

        public int GetAliens() {
            return aliens;
        }

        public void AddSplinter(int a) {
            splinter += a; 
        }

        public int GetSplinters() {
            return splinter;
        }

        public void Zero() {
            asteroids = 0;
            aliens = 0;
            splinter = 0;
        }

        public int GetRemaining() {
            return asteroids + aliens + splinter;
        }
    }

    public static EnemySpawnerController Instance;

    [SerializeField] private float levelLengthTimerMax;
    private float levelLengthTimer;

    private Spawns[] enemySpawnTimes;
    private List<Enemy> enemyList; 

    public event EventHandler OnEnemyDestory;

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
    }

    private void Update() {
        if (!PauseMenuUI.IsGamePaused) {
            levelLengthTimer -= Time.deltaTime;
            SpawnWave();
            if (levelLengthTimer < -1f && enemyList.Count == 0) {
                GameManager.Instance.EndLevel();
            }
        }
    }
    
    public void Setup() {
        // Do nothing
    }

    public void SetupLevel() {
        Cleanup();
        enemyList = new List<Enemy>();
        GenerateAsteroidSpawnTimes();
    }

    public void StartLevel() {
        levelLengthTimer = levelLengthTimerMax;
    }

    public void EndLevel() {
        // Do nthing
    }

    public void Cleanup() {
        if (enemyList != null) {
            for (int i = 0; i < enemyList.Count; i++) {
                if (enemyList[i] != null) {
                    Destroy(enemyList[i].gameObject);
                }
            }
            enemyList = null;
        }

        enemySpawnTimes = null;
    }

    private void GenerateAsteroidSpawnTimes() {
        // Current level
        int level = LevelController.Instance.GetLevel();
        // Provides half second granularity
        int levelTime = (int)(levelLengthTimerMax);
        // Array of spawn times for asteroids
        enemySpawnTimes = new Spawns[levelTime * 2];
        // Number of asteroids to spawn
        int numberOfAsteroids =  (level + 2) * (levelTime / 6);

        for (int i = 0; i < enemySpawnTimes.Length; i++) {
            enemySpawnTimes[i] = new Spawns();
        }

        int f = enemySpawnTimes.Length / (2 * numberOfAsteroids / 3);
        for (int i = 0; i < enemySpawnTimes.Length; i += f) {
            enemySpawnTimes[i].AddAsteroids(1);
            numberOfAsteroids--;
        }

        for (int i = 0; i < numberOfAsteroids; i++) {
            int index = UnityEngine.Random.Range(0, enemySpawnTimes.Length);
            enemySpawnTimes[index].AddAsteroids(1);
        }

        int numberOfAliens = Mathf.FloorToInt(level / 3);
        for (int i = 0; i < numberOfAliens; i++) {
            int index = UnityEngine.Random.Range(0, enemySpawnTimes.Length);
            enemySpawnTimes[index].AddAliens(1);
        }

        int numberOfSplinters = Mathf.FloorToInt(level / 2) * 2;
        for (int i = 0; i < numberOfSplinters; i++) {
            int index = UnityEngine.Random.Range(0, enemySpawnTimes.Length);
            enemySpawnTimes[index].AddSplinter(1);
        }
    }

    private void SpawnWave() {
        int index = Mathf.RoundToInt(levelLengthTimer * 2 - 1);
        if (index >= 0 && index < enemySpawnTimes.Length) {
            for (int i = 0; i < enemySpawnTimes[index].GetAsteroids(); i++) {
                SpawnAsteroid();
            }
            for (int i = 0; i < enemySpawnTimes[index].GetAliens(); i++) {
                SpawnAlien();
            }
            for (int i = 0; i < enemySpawnTimes[index].GetSplinters(); i++) {
                SpawnSpliner();
            }
            enemySpawnTimes[index].Zero();
        }
    }

    public void SpawnAlien() {
        enemyList.Add(Alien.Create("Alien " + enemyList.Count.ToString()));
    }

    public void SpawnAsteroid() {
        enemyList.Add(Asteroid.Create("Asteroid " + enemyList.Count.ToString()));
    }

    public void SpawnAsteroid(Vector2 spawnPosition) {
        enemyList.Add(Asteroid.Create("Asteroid " + enemyList.Count.ToString(), spawnPosition));
    }

    public void SpawnSpliner() {
        enemyList.Add(AsteroidSplinter.Create("AsteroidSplinter " + enemyList.Count.ToString()));
    }

    public void RemoveEnemy(Enemy enemy) {
        if (!enemyList.Remove(enemy)) {
            Debug.LogWarning("Warn: Attempted to destory an enemy [" + enemy.name + "] not in enemy list");
        }

        OnEnemyDestory?.Invoke(this, EventArgs.Empty);
    }

    public int GetRemainingEnemies() {
        int r = 0;
        if (enemySpawnTimes != null) {
            for (int i = 0; i < enemySpawnTimes.Length; i++) {
                r += enemySpawnTimes[i].GetRemaining();
            }
        }
        return r;
    }
}

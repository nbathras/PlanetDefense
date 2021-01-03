using System;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    /* Game Lifecycle
     * 1. Pre event (All starts and awake run)
     * 2. OnGameSetupEvent (Game is setup and started)
     * 3. OnGameOver (Game Over screen comes up and wait for setup or quit)
     * 4. OnGameQuit (Call cleanup and same state as pre event)
     */
    public event EventHandler OnGameSetupEvent;
    public event EventHandler OnScoreChangedEvent;
    public event EventHandler OnLevelChangedEvent;
    public event EventHandler OnGameOverEvent;
    public event EventHandler OnGameQuitEvent;

    private int score;

    [SerializeField] private float levelIncreaseTimerMax;
    private float levelIncreaseTimer;

    private int level;

    private bool isPaused;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    private void Start() {
        CityController.Instance.OnDestoryCityEvent += GameManager_OnDestoryCityEvent;
        Pause(true);
    }

    private void Update() {
        if (!IsPaused()) {
            levelIncreaseTimer -= Time.deltaTime;
            if (levelIncreaseTimer < 0f) {

                level += 1;
                levelIncreaseTimer = levelIncreaseTimerMax;

                AddScore(CityController.Instance.GetScore() + CannonController.Instance.GetScore() + level * 100);

                CityController.Instance.Setup();
                AsteroidSpawnerController.Instance.Setup();
                CannonController.Instance.Setup();

                OnLevelChangedEvent?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void SetUpGame() {
        score = 0;
        level = 0;
        levelIncreaseTimer = levelIncreaseTimerMax;

        CityController.Instance.Setup();
        AsteroidSpawnerController.Instance.Setup();
        CannonController.Instance.Setup();

        Pause(false);

        OnGameSetupEvent?.Invoke(this, EventArgs.Empty);
    }

    public void QuitGame() {
        Pause(true);

        CityController.Instance.Cleanup();
        AsteroidSpawnerController.Instance.Cleanup();
        CannonController.Instance.Cleanup();

        OnGameQuitEvent?.Invoke(this, EventArgs.Empty);
    }

    public void AddScore(int s) {
        score += s;

        OnScoreChangedEvent?.Invoke(this, EventArgs.Empty);
    }

    public int GetLevel() {
        return level;
    }

    public float GetLeveLTimer() {
        return levelIncreaseTimer;
    }

    private void GameManager_OnDestoryCityEvent(object sender, System.EventArgs e) {
        if (CityController.Instance.GetNumberAliveCities() <= 0) {
            Pause(true);
            OnGameOverEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    public int GetScore() {
        return score;
    }

    public bool IsPaused() {
        return isPaused;
    }

    public void Pause(bool isPaused) {
        this.isPaused = isPaused;
        AsteroidSpawnerController.Instance.Pause(isPaused);
        CannonController.Instance.Pause(isPaused);
        CityController.Instance.Pause(isPaused);
    }
}

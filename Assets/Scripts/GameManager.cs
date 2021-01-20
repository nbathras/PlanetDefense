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
    public event EventHandler OnLevelSetupEvent;
    public event EventHandler OnLevelStartEvent;
    public event EventHandler OnLevelEndEvent;
    public event EventHandler OnGameOverEvent;
    public event EventHandler OnGameCleanupEvent;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    private void Start() {
        CityController.Instance.OnDestoryCityEvent += GameManager_OnDestoryCityEvent;
    }

    public void SetupGame() {
        Debug.Log("GameManager SetupGame()");

        ScoreController.Instance.Setup();
        LevelController.Instance.Setup();
        CityController.Instance.Setup();
        EnemySpawnerController.Instance.Setup();
        CannonController.Instance.Setup();

        OnGameSetupEvent?.Invoke(this, EventArgs.Empty);

        SetupLevel();
    }

    public void SetupLevel() {
        Debug.Log("GameManager SetupLevel()");

        ScoreController.Instance.SetupLevel();
        LevelController.Instance.SetupLevel();
        CityController.Instance.SetupLevel();
        EnemySpawnerController.Instance.SetupLevel();
        CannonController.Instance.SetupLevel();

        OnLevelSetupEvent?.Invoke(this, EventArgs.Empty);
    }

    public void StartLevel() {
        Debug.Log("GameManager StartLevel()");

        PauseMenuUI.UnPauseGame();

        ScoreController.Instance.StartLevel();
        LevelController.Instance.StartLevel();
        CityController.Instance.StartLevel();
        EnemySpawnerController.Instance.StartLevel();
        CannonController.Instance.StartLevel();

        OnLevelStartEvent?.Invoke(this, EventArgs.Empty);
    }

    public void EndLevel() {
        Debug.Log("GameManager EndLevel()");

        PauseMenuUI.PauseGame();

        ScoreController.Instance.AddScore(ScoreController.ScoreCategories.CitiesSaved, CityController.Instance.GetNumberAliveCities() * 100);
        ScoreController.Instance.AddScore(ScoreController.ScoreCategories.LevelPassed, 400 + LevelController.Instance.GetLevel() * 200);

        OnLevelEndEvent?.Invoke(this, EventArgs.Empty);
    }

    public void EndGame() {
        Debug.Log("GameManager EndGame()");

        PauseMenuUI.PauseGame();

        OnGameOverEvent?.Invoke(this, EventArgs.Empty);
    }

    public void CleanupGame() {
        Debug.Log("GameManager CleanupGame()");

        ScoreController.Instance.Cleanup();
        LevelController.Instance.Cleanup();
        CityController.Instance.Cleanup();
        EnemySpawnerController.Instance.Cleanup();
        CannonController.Instance.Cleanup();

        OnGameCleanupEvent?.Invoke(this, EventArgs.Empty);
    }

    private void GameManager_OnDestoryCityEvent(object sender, System.EventArgs e) {
        if (CityController.Instance.GetNumberAliveCities() <= 0) {
            EndGame();
        }
    }
}

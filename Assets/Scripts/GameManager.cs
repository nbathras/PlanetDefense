using System;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public event EventHandler OnGameSetupEvent;
    public event EventHandler OnScoreChangedEvent;
    public event EventHandler OnGameOverEvent;

    private int score;

    private bool isPaused;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    private void Start() {
        SetUpGame();

        CityController.Instance.OnDestoryCityEvent += GameManager_OnDestoryCityEvent;
    }

    public void SetUpGame() {
        score = 0;

        CityController.Instance.Setup();
        AsteroidSpawnerController.Instance.Setup();
        CannonController.Instance.Setup();

        Pause(false);

        OnGameSetupEvent?.Invoke(this, EventArgs.Empty);
    }

    public void AddScore(int s) {
        score += s;

        OnScoreChangedEvent?.Invoke(this, EventArgs.Empty);
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

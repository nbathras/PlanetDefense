using System;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public event EventHandler OnGameSetupEvent;
    public event EventHandler OnScoreChangedEvent;
    public event EventHandler OnGameOverEvent;

    private int score;
    private int cities;

    private bool isPaused;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }

        score = 0;

        Pause(false);
    }

    private void Start() {
        SetUpGame();
    }

    public void SetUpGame() {
        score = 0;
        cities = 6;

        CityController.Instance.SetupCities();

        OnGameSetupEvent?.Invoke(this, EventArgs.Empty);
    }

    public void AddScore(int s) {
        score += s;

        OnScoreChangedEvent?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveCity(int c) {
        cities -= c;

        if (cities <= 0) {
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
        Debug.Log(isPaused);
        AsteroidSpawnerController.Instance.Pause(isPaused);
        CannonController.Instance.Pause(isPaused);
    }
}

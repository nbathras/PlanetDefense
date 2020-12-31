using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    public event EventHandler OnScoreChanged; 

    private int score;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }

        score = 0;
    }

    public void AddScore(int s) {
        score += s;

        OnScoreChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetScore() {
        return score;
    }
}

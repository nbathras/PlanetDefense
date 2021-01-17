using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour {

    public static ScoreController Instance;

    public enum ScoreCategories {
        AsteroidsDestoryed,
        CitiesSaved,
        LevelPassed
    }

    // Index represents the level #
    //   The categories are the score accumlated during that level
    private List<Dictionary<ScoreCategories, int>> scoreHistory;

    public event EventHandler OnScoreChangedEvent;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    public void Setup() {
        Cleanup();
        scoreHistory = new List<Dictionary<ScoreCategories, int>>();
    }

    public void SetupLevel() {
        scoreHistory.Add(new Dictionary<ScoreCategories, int>());
        int i = scoreHistory.Count - 1;
        scoreHistory[i].Add(ScoreCategories.AsteroidsDestoryed, 0);
        scoreHistory[i].Add(ScoreCategories.CitiesSaved, 0);
        scoreHistory[i].Add(ScoreCategories.LevelPassed, 0);
    }

    public void StartLevel() {
        // Do nothing
    }

    public void Cleanup() {
        scoreHistory = null;
    }

    public int GetTotalScore() {
        int total = 0;
        for (int i = 0; i < scoreHistory.Count; i++) {
            foreach (ScoreCategories sc in scoreHistory[i].Keys) {
                total += scoreHistory[i][sc];
            }
        }

        return total;
    }

    public int GetPreviousLevelsScore() {
        int total = 0;
        for (int i = 0; i < scoreHistory.Count - 1; i++) {
            foreach (ScoreCategories sc in scoreHistory[i].Keys) {
                total += scoreHistory[i][sc];
            }
        }

        return total;
    }

    public Dictionary<ScoreCategories, int> GetLevelScores(int level) {
        return scoreHistory[level - 1];
    }

    public void AddScore(ScoreCategories category, int score) {
        AddScore(LevelController.Instance.GetLevel(), category, score);
    } 

    public void AddScore(int level, ScoreCategories category, int score) {
        scoreHistory[level - 1][category] += score;

        OnScoreChangedEvent?.Invoke(this, EventArgs.Empty);
    }
}

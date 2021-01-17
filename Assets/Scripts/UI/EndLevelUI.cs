using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndLevelUI : MonoBehaviour {

    [Header("Transform Holder")]
    [SerializeField] private Transform holder;

    [Header("Static Text")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI previousScoreText;

    [Header("Dynmaic Score Text")]
    [SerializeField] private int minFontSize;
    [SerializeField] private int maxFontSize;
    [SerializeField] private TextMeshProUGUI asteroidDestoryedScoreText;
    [SerializeField] private TextMeshProUGUI citiesSavedScoreText;
    [SerializeField] private TextMeshProUGUI levelPassedScoreText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private float timerMax;
    private float timer;
    [SerializeField] private List<TextMeshProUGUI> displayOrderList;
    private int index;

    private void Awake() {
    }

    private void Start() {
        GameManager.Instance.OnLevelSetupEvent += EndLevelUI_OnLevelSetupEvent;
        GameManager.Instance.OnLevelEndEvent += EndLevelUI_OnLevelEndEvent;
        Disable();
    }

    private void Update() {
        timer -= Time.deltaTime;
        if (index >= 0 && index < displayOrderList.Count) {
            displayOrderList[index].fontSize = CalculateFontSize();
        }

        if (timer < 0f) {
            index++;
            if (index == displayOrderList.Count) {
                // Do nothing
            } else if (index > displayOrderList.Count) {
                LevelController.Instance.NextLevel();
            } else {
                displayOrderList[index].gameObject.SetActive(true);
            }
            timer = timerMax;
        }
    }

    private float CalculateFontSize() {
        float maxMinDiff = maxFontSize - minFontSize;
        float percentage = (1 - (timer / timerMax)) * 2;
        float newFontSize = Mathf.Clamp(maxFontSize - maxMinDiff * percentage, minFontSize, maxFontSize);

        return newFontSize;
    }

    private void SetTitleText(int level) {
        titleText.SetText("Level " + level.ToString() + " Complete!");
    }

    private void SetPreviousScoreText(int previousScore) {
        previousScoreText.SetText("Previous Score: " + previousScore.ToString());
    }

    private void SetScoreTexts(int asteroidDestroyedScore, int citiesSavedScore, int levelPassedScore, int totalScore) {
        asteroidDestoryedScoreText.fontSize = maxFontSize;
        asteroidDestoryedScoreText.SetText("Asteroid Destroyed: +" + asteroidDestroyedScore.ToString());
        asteroidDestoryedScoreText.gameObject.SetActive(false);
        citiesSavedScoreText.fontSize = maxFontSize;
        citiesSavedScoreText.SetText("Cities Saved: +" + citiesSavedScore.ToString());
        citiesSavedScoreText.gameObject.SetActive(false);
        levelPassedScoreText.fontSize = maxFontSize;
        levelPassedScoreText.SetText("Level Passed: +" + levelPassedScore.ToString());
        levelPassedScoreText.gameObject.SetActive(false);
        totalScoreText.fontSize = maxFontSize;
        totalScoreText.SetText("Score: " + totalScore.ToString());
        totalScoreText.gameObject.SetActive(false);
    }

    private void EndLevelUI_OnLevelEndEvent(object sender, EventArgs e) {
        Enable();
        index = -1;
        timer = timerMax;
        SetTitleText(LevelController.Instance.GetLevel());
        SetPreviousScoreText(ScoreController.Instance.GetPreviousLevelsScore());
        Dictionary<ScoreController.ScoreCategories, int> levelScoreHistory = ScoreController.Instance.GetLevelScores(LevelController.Instance.GetLevel());
        SetScoreTexts(
            levelScoreHistory[ScoreController.ScoreCategories.AsteroidsDestoryed], 
            levelScoreHistory[ScoreController.ScoreCategories.CitiesSaved], 
            levelScoreHistory[ScoreController.ScoreCategories.LevelPassed], 
            ScoreController.Instance.GetTotalScore()
        );
    }

    private void EndLevelUI_OnLevelSetupEvent(object sender, EventArgs e) {
        Disable();
    }

    private void Disable() {
        holder.gameObject.SetActive(false);
        enabled = false;
    }

    private void Enable() {
        holder.gameObject.SetActive(true);
        timer = timerMax;
        enabled = true;
    }
}

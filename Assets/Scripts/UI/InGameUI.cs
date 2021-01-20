using UnityEngine;
using TMPro;
using System;

/* UI systems have the following structure
 * 
 * GameObject Structure 
 *
 * ParentUIObject (UI script component)
 * ==> HolderObject (used to activate and deactivate element without turning off script)
 * ====> All Remaining UI elements
 * 
 * Script Structure
 * 
 * Awake() {
 *   1. Set active or inactive at start of game
 *   2. Add event listeners for buttons
 * }
 * 
 * Start () {
 *   1. Add event handlers for unity events that rely on singeltons instances from controllers
 * }
 */
public class InGameUI : MonoBehaviour {
    [Header("Transform Holder")]
    [SerializeField] private Transform inGameUIHolder;

    [Header("Buttons")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI remainingAsteroidText;

    private void Awake() {
        inGameUIHolder.gameObject.SetActive(false);
    }

    private void Start() {
        ScoreController.Instance.OnScoreChangedEvent += InGameUI_OnScoreChangedEvent;
        EnemySpawnerController.Instance.OnEnemyDestory += InGameUI_OnEnemyDestory;
        GameManager.Instance.OnLevelSetupEvent += InGameUI_OnLevelSetupEvent;
        GameManager.Instance.OnLevelStartEvent += InGameUI_OnLevelStartEvent;
        GameManager.Instance.OnGameSetupEvent += InGameUI_OnGameSetupEvent;
        GameManager.Instance.OnGameCleanupEvent += InGameUI_OnGameCleanupEvent;
    }

    private void InGameUI_OnLevelStartEvent(object sender, EventArgs e) {
        UpdateRemainingAsteroidText();
    }

    private void InGameUI_OnEnemyDestory(object sender, EventArgs e) {
        UpdateRemainingAsteroidText();
    }

    private void InGameUI_OnLevelSetupEvent(object sender, EventArgs e) {
        UpdateLevelText();
    }

    private void InGameUI_OnGameCleanupEvent(object sender, EventArgs e) {
        inGameUIHolder.gameObject.SetActive(false);
    }

    private void InGameUI_OnScoreChangedEvent(object sender, EventArgs e) {
        UpdateScoreText();
    }

    private void InGameUI_OnGameSetupEvent(object sender, EventArgs e) {
        inGameUIHolder.gameObject.SetActive(true);

        UpdateScoreText();
        UpdateLevelText();
        UpdateRemainingAsteroidText();
    }

    private void UpdateScoreText() {
        scoreText.SetText("Score: " + ScoreController.Instance.GetTotalScore().ToString());
    }

    private void UpdateLevelText() {
        levelText.SetText("Level: " + LevelController.Instance.GetLevel().ToString());
    }

    private void UpdateRemainingAsteroidText() {
        remainingAsteroidText.SetText("Enemies Left: " + EnemySpawnerController.Instance.GetRemainingEnemies());
    }
}

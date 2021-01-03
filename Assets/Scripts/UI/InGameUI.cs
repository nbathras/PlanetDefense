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
    [SerializeField] private TextMeshProUGUI nextLeveLTimerText;

    private void Awake() {
        inGameUIHolder.gameObject.SetActive(false);
    }

    private void Start() {
        GameManager.Instance.OnScoreChangedEvent += InGameUI_OnScoreChangedEvent;
        GameManager.Instance.OnLevelChangedEvent += InGameUI_OnLevelChangedEvent;
        GameManager.Instance.OnGameSetupEvent += InGameUI_OnGameSetupEvent;
        GameManager.Instance.OnGameQuitEvent += InGameUI_OnGameQuitEvent;
    }

    private void Update() {
        if (!GameManager.Instance.IsPaused()) {
            nextLeveLTimerText.SetText("Next Level In: " + String.Format("{0:0.##}", GameManager.Instance.GetLeveLTimer()));
        }
    }

    private void InGameUI_OnLevelChangedEvent(object sender, EventArgs e) {
        UpdateLevelText();
    }

    private void InGameUI_OnGameQuitEvent(object sender, EventArgs e) {
        inGameUIHolder.gameObject.SetActive(false);
    }

    private void InGameUI_OnScoreChangedEvent(object sender, System.EventArgs e) {
        UpdateScoreText();
    }

    private void InGameUI_OnGameSetupEvent(object sender, System.EventArgs e) {
        inGameUIHolder.gameObject.SetActive(true);

        UpdateScoreText();
        UpdateLevelText();
    }

    private void UpdateScoreText() {
        scoreText.SetText("Score: " + GameManager.Instance.GetScore().ToString());
    }

    private void UpdateLevelText() {
        levelText.SetText("Level: " + (GameManager.Instance.GetLevel() + 1).ToString());
    }
}
